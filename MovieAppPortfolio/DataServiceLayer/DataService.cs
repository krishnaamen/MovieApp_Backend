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

     // This method return movie(Titlebasics) with id tconst
        
        public TitleBasic? GetTitleBasicById(string tconst)
        {
            return _context.Title_Basics
                .FirstOrDefault(tb => tb.tconst == tconst);
        }

        // This method return the best match movies with keywords we are using it for the search movie in controller

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


        // This method is used to fecth the movie list with page number and page size,this is paging requirement implementation.
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

        List<MovieDto> IDataService.GetTitleBasics()
        {
        
        // Pull the base title list (limit as needed)
        var titleBasics = _context.Title_Basics
                .OrderBy(tb => tb.tconst)
                .Take(50)
                .ToList();

            var result = new List<MovieDto>();

            foreach (var movie in titleBasics)
            {
                var rating = _context.Title_Ratings
                    .FirstOrDefault(tr => tr.tconst == movie.tconst);

                var omdbData = _context.Omdb_Data
                    .FirstOrDefault(od => od.tconst == movie.tconst);

                // Fix: Only call GetMovieGenres if movie.tconst is not null, otherwise use an empty list
                var genres = movie.tconst != null ? GetMovieGenres(movie.tconst) : new List<string>();

                result.Add(new MovieDto
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
                });
            }

            return result;
        }



        


        // This method is returns the details of one particular movie, This contains the plot and posters and genres which will be useful for the frontend for the single movie details page
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


        // This method returns the genre of the movie with id tconst
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


        //this method rate the movie and userId will be retrieved from jwt token using get user method
       
        // This method is responsible to add the bookmark 
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


        // Removing the bookmark 
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
         // This method list out all the book marks for particular user with id 
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


        // this method is responsible for adding the search history 


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

     






        // namebasic methods 


        public async Task<List<NameBasic>> GetNameBasicsPaginated(int page, int pageSize)
        {
            return await _context.Name_Basics
                .OrderBy(nb => nb.primaryName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<NameDto?> GetNameDetails(string nconst)
        {
            var name = await _context.Name_Basics
                .FirstOrDefaultAsync(nb => nb.nconst == nconst);

            if (name == null) return null;


            var nameRating = await _context.Name_Ratings
        .FirstOrDefaultAsync(nr => nr.nconst == nconst);


            var knownForTitles = await _context.Title_Principals
                .Where(tp => tp.nconst == nconst)
                .Join(_context.Title_Basics,
                    tp => tp.tconst,
                    tb => tb.tconst,
                    (tp, tb) => tb.primaryTitle)
                .Take(5)
                .ToListAsync();

            return new NameDto
            {
                nconst = name.nconst,
                primaryName = name.primaryName,
                birthYear = name.birthYear,
                deathYear = name.deathYear,
                weightedRating = nameRating?.weightedRating,
                knownForTitles = knownForTitles.Any() ? string.Join(", ", knownForTitles) : null
            };
        }




        // User Notes methods from here 
        public async Task<UserNote?> AddUserNoteAsync(int userId, string noteText, string? tconst = null, string? nconst = null)
        {
            try
            {
                // Validate that at least one of tconst or nconst is provided
                if (string.IsNullOrEmpty(tconst) && string.IsNullOrEmpty(nconst))
                {
                    throw new ArgumentException("Either tconst or nconst must be provided");
                }

                // Validate that the movie or person exists
                if (!string.IsNullOrEmpty(tconst))
                {
                    var movieExists = await _context.Title_Basics.AnyAsync(tb => tb.tconst == tconst);
                    if (!movieExists) return null;
                }

                if (!string.IsNullOrEmpty(nconst))
                {
                    var personExists = await _context.Name_Basics.AnyAsync(nb => nb.nconst == nconst);
                    if (!personExists) return null;
                }

                var userNote = new UserNote
                {
                    UserId = userId,
                    tconst = tconst,
                    nconst = nconst,
                    NoteText = noteText,
                    CreatedAt = DateTime.UtcNow
                };

                _context.User_Notes.Add(userNote);
                await _context.SaveChangesAsync();

                // Reload the entity with related data
                return await _context.User_Notes
                    .Include(un => un.TitleBasic)
                    .Include(un => un.NameBasic)
                    .FirstOrDefaultAsync(un => un.NoteId == userNote.NoteId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user note: {ex.Message}");
                return null;
            }
        }

        public async Task<List<UserNoteDto>> GetUserNotesAsync(int userId)
        {
            return await _context.User_Notes
                .Where(un => un.UserId == userId)
                .Include(un => un.TitleBasic)
                .Include(un => un.NameBasic)
                .OrderByDescending(un => un.CreatedAt)
                .Select(un => new UserNoteDto
                {
                    noteId = un.NoteId,
                    userId = un.UserId,
                    tconst = un.tconst,
                    nconst = un.nconst,
                    noteText = un.NoteText,
                    createdAt = un.CreatedAt,
                    movieTitle = un.TitleBasic != null ? un.TitleBasic.primaryTitle : null,
                    personName = un.NameBasic != null ? un.NameBasic.primaryName : null
                })
                .ToListAsync();
        }

        public async Task<UserNote?> GetUserNoteByIdAsync(int noteId, int userId)
        {
            return await _context.User_Notes
                .Include(un => un.TitleBasic)
                .Include(un => un.NameBasic)
                .FirstOrDefaultAsync(un => un.NoteId == noteId && un.UserId == userId);
        }

        public async Task<bool> UpdateUserNoteAsync(int noteId, int userId, string newNoteText)
        {
            try
            {
                var userNote = await _context.User_Notes
                    .FirstOrDefaultAsync(un => un.NoteId == noteId && un.UserId == userId);

                if (userNote == null) return false;

                userNote.NoteText = newNoteText;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserNoteAsync(int noteId, int userId)
        {
            try
            {
                var userNote = await _context.User_Notes
                    .FirstOrDefaultAsync(un => un.NoteId == noteId && un.UserId == userId);

                if (userNote == null) return false;

                _context.User_Notes.Remove(userNote);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Helper method to check if user has note for specific movie/person
        public async Task<bool> UserHasNoteForAsync(int userId, string? tconst = null, string? nconst = null)
        {
            return await _context.User_Notes
                .AnyAsync(un => un.UserId == userId &&
                               ((tconst != null && un.tconst == tconst) ||
                                (nconst != null && un.nconst == nconst)));
        }

        public async Task<int> GetTotalNameBasicsCount()
        {
            return await _context.Name_Basics.CountAsync();
        }

        public async Task<NameBasic?> GetNameBasicByIdAsync(string nconst)
        {
            return await _context.Name_Basics
                .FirstOrDefaultAsync(nb => nb.nconst == nconst);
        }



        // User Ratings methods from here


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


        public async Task<int?> GetUserRatingForMovieAsync(int userId, string tconst)
        {
            var rating = await _context.User_Ratings
             .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.TConst == tconst);

            return rating?.Rating;
        }


        public async Task<bool> UpdateMovieRatingAsync(int userId, string tconst, int newRating)
        {
            try
            {
                if (newRating < 1 || newRating > 10)
                    return false;

                var existingRating = await _context.User_Ratings
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.TConst == tconst);

                if (existingRating == null)
                    return false;

                existingRating.Rating = newRating;
                existingRating.RatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveRatingAsync(int userId, string tconst)
        {
            try
            {
                var rating = await _context.User_Ratings
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.TConst == tconst);

                if (rating != null)
                {
                    _context.User_Ratings.Remove(rating);
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


        public async Task<UserRating?> GetUserRatingByIdAsync(int ratingId, int userId)
        {
            return await _context.User_Ratings
                .Include(ur => ur.TitleBasic)
                .FirstOrDefaultAsync(ur => ur.RatingId == ratingId && ur.UserId == userId);
        }

        public async Task<bool> HasUserRatedMovieAsync(int userId, string tconst)
        {
            return await _context.User_Ratings
                .AnyAsync(ur => ur.UserId == userId && ur.TConst == tconst);
        }

     
    }








}
