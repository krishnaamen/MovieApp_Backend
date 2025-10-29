using Dapper;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer.dtos;
using MovieAppPortfolio.DataServiceLayer.entities;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class DataService: IDataService
    {
        private readonly MyDbContext _context;

        public DataService(MyDbContext context)
        {
            _context = context;
        }

     
        
        public TitleBasic? GetTitleBasicById(string tconst)
        {
            return _context.Title_Basics
                .FirstOrDefault(tb => tb.tconst == tconst);
        }

        public IList<BestMatchResult> BestMatchSearch(string[] keywords)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Converts keywords array to PostgreSQL array format
                    var parameters = new { p_keywords = keywords };

                    
                    var results = connection.Query<BestMatchResult>(
                        "SELECT * FROM movie_app.best_match_search(@p_keywords)",
                        parameters
                    ).ToList();

                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error during search: {ex.Message}");
                throw; 
            }
        }



        public List<TitleBasic> GetTitleBasicsPaginated(int page, int pageSize)
        {
        
            return _context.Title_Basics
                .OrderBy(tb => tb.tconst)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }
        

        public int GetTotalTitleBasicsCount()
        {
            return _context.Title_Basics.Count();
        }
        public List<TitleBasic> GetTitleBasics()
        {
            return _context.Title_Basics
               .OrderBy(tb => tb.tconst)
               .Take(50)
               .ToList();
        }



        public MovieDto? GetMovieDetails(string tconst)
        {
            
            var movie = _context.Title_Basics
                .FirstOrDefault(tb => tb.tconst == tconst);
            if (movie == null) return null;

            
            var rating = _context.Title_Ratings
                .FirstOrDefault(tr => tr.tconst == tconst);

            
            var omdbData = _context.Omdb_Data
                .FirstOrDefault(od => od.tconst == tconst);

            
            var genres = GetMovieGenres(tconst);

           
            return new MovieDto
            {
                tconst = movie.tconst,
                titleType = movie.titleType,
                primaryTitle = movie.primaryTitle,
                originalTitle = movie.originalTitle,
                isAdult = movie.isAdult,
                startYear = movie.startYear,
                endYear = movie.endYear,
                runtimeMinutes = movie.runtimeMinutes,
                AverageRating = rating?.averageRating,
                NumVotes = rating?.numVotes,
                Plot = omdbData?.plot,
                Poster = omdbData?.poster,
                Genres = genres
            };
        }

        private List<string> GetMovieGenres(string tconst)
        {
            return _context.Genre_Titles
        .Where(gt => gt.tconst == tconst)
        .Join(_context.Genres,
            gt => gt.genreId,
            g => g.genreId,
            (gt, g) => g.genreName)
        .Where(genreName => genreName != null)
        .Select(genreName => genreName!)
        .Distinct()
        .ToList();
        }

        public async Task<bool> RateMovieAsync(int userId, string tconst, int rating)
        {
            try
            {
                // Validate rating range
                if (rating < 1 || rating > 10)
                    return false;

                // Check if user already rated this movie
                var existingRating = await _context.User_Ratings
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.TConst == tconst);

                if (existingRating != null)
                {
                    // Update existing rating
                    existingRating.Rating = rating;
                    existingRating.RatedAt = DateTime.UtcNow;
                }
                else
                {
                    // Create new rating
                    var userRating = new UserRating
                    {
                        UserId = userId,
                        TConst = tconst,
                        Rating = rating,
                        RatedAt = DateTime.UtcNow
                    };
                    _context.User_Ratings.Add(userRating);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddBookmarkAsync(int userId, string tconst, string? nconst = null)
        {
            try
            {
                // Check if bookmark already exists
                var existingBookmark = await _context.Bookmarks
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.TConst == tconst && b.NConst == nconst);

                if (existingBookmark != null)
                    return true; // Already bookmarked

                var bookmark = new Bookmark
                {
                    UserId = userId,
                    TConst = tconst,
                    NConst = nconst,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookmarks.Add(bookmark);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveBookmarkAsync(int userId, string tconst, string? nconst = null)
        {
            try
            {
                var bookmark = await _context.Bookmarks
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.TConst == tconst && b.NConst == nconst);

                if (bookmark != null)
                {
                    _context.Bookmarks.Remove(bookmark);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Bookmark>> GetUserBookmarksAsync(int userId)
        {
            return await _context.Bookmarks
             .Where(b => b.UserId == userId)
             .Include(b => b.TitleBasic)
             .Include(b => b.NameBasic)
             .OrderByDescending(b => b.CreatedAt)
             .ToListAsync();
        }

        public async Task<List<UserRating>> GetUserRatingsAsync(int userId)
        {
            return await _context.User_Ratings
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.TitleBasic)
            .OrderByDescending(ur => ur.RatedAt)
            .ToListAsync();
        }

        public async Task AddSearchHistoryAsync(int userId, string searchQuery)
        {
            try
            {
                var searchHistory = new SearchHistory
                {
                    UserId = userId,
                    SearchQuery = searchQuery,
                    SearchTime = DateTime.UtcNow
                };

                _context.Search_History.Add(searchHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Log error but don't break the search functionality
            }
        }

        public async Task<List<SearchHistory>> GetUserSearchHistoryAsync(int userId)
        {
            return await _context.Search_History
            .Where(sh => sh.UserId == userId)
            .OrderByDescending(sh => sh.SearchTime)
            .Take(20) // Last 20 searches
            .ToListAsync();
        }

        public async Task<bool> IsMovieBookmarkedAsync(int userId, string tconst)
        {
            return await _context.Bookmarks
            .AnyAsync(b => b.UserId == userId && b.TConst == tconst);
        }

        public async Task<int?> GetUserRatingForMovieAsync(int userId, string tconst)
        {
            var rating = await _context.User_Ratings
             .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.TConst == tconst);

            return rating?.Rating;
        }
    }


    

    }
