using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.dtos;
using MovieAppPortfolio.DataServiceLayer.entities;
using MovieAppPortfolio.DataServiceLayer.TitlePrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieAppPortfolio.Tests
{
    public class DataServiceTests : IDisposable
    {
        private readonly MyDbContext _context;
        private readonly DataService _dataService;

        public DataServiceTests()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MyDbContext(options);
            _dataService = new DataService(_context);

            SeedTestData();
        }

        // TitleBasic Tests
        [Fact]
        public void GetTitleBasicById_ValidId_ReturnsTitleBasic()
        {
            // Act
            var result = _dataService.GetTitleBasicById("tt001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("tt001", result.tconst);
            Assert.Equal("Test Movie 1", result.primaryTitle);
        }

        [Fact]
        public void GetTitleBasicById_InvalidId_ReturnsNull()
        {
            // Act
            var result = _dataService.GetTitleBasicById("tt999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetTitleBasics_ReturnsFirst50Titles()
        {
            // Act
            var result = _dataService.GetTitleBasics();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetTitleBasicsPaginated_ValidPage_ReturnsCorrectPage()
        {
            // Act
            var result = _dataService.GetTitleBasicsPaginated(0, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("tt001", result[0].tconst);
        }

        [Fact]
        public void GetTotalTitleBasicsCount_ReturnsCorrectCount()
        {
            // Act
            var result = _dataService.GetTotalTitleBasicsCount();

            // Assert
            Assert.Equal(3, result);
        }

        // Movie Details Tests
        

        [Fact]
        public void GetMovieDetails_InvalidId_ReturnsNull()
        {
            // Act
            var result = _dataService.GetMovieDetails("tt999");

            // Assert
            Assert.Null(result);
        }

        // Bookmark Tests
        [Fact]
        public async Task AddBookmarkAsync_NewBookmark_ReturnsTrue()
        {
            // Act
            var result = await _dataService.AddBookmarkAsync(1, "tt001");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddBookmarkAsync_DuplicateBookmark_ReturnsTrue()
        {
            // Arrange
            await _dataService.AddBookmarkAsync(1, "tt001");

            // Act
            var result = await _dataService.AddBookmarkAsync(1, "tt001");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveBookmarkAsync_ExistingBookmark_ReturnsTrue()
        {
            // Arrange
            await _dataService.AddBookmarkAsync(1, "tt001");

            // Act
            var result = await _dataService.RemoveBookmarkAsync(1, "tt001");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveBookmarkAsync_NonExistingBookmark_ReturnsFalse()
        {
            // Act
            var result = await _dataService.RemoveBookmarkAsync(1, "tt999");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUserBookmarksAsync_ReturnsUserBookmarks()
        {
            // Arrange
            await _dataService.AddBookmarkAsync(1, "tt001");
            await _dataService.AddBookmarkAsync(1, "tt002");
            await _dataService.AddBookmarkAsync(2, "tt001");

            // Act
            var result = await _dataService.GetUserBookmarksAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, b => Assert.Equal(1, b.UserId));
        }

        [Fact]
        public async Task IsMovieBookmarkedAsync_Bookmarked_ReturnsTrue()
        {
            // Arrange
            await _dataService.AddBookmarkAsync(1, "tt001");

            // Act
            var result = await _dataService.IsMovieBookmarkedAsync(1, "tt001");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsMovieBookmarkedAsync_NotBookmarked_ReturnsFalse()
        {
            // Act
            var result = await _dataService.IsMovieBookmarkedAsync(1, "tt999");

            // Assert
            Assert.False(result);
        }

        // Search History Tests
        [Fact]
        public async Task AddSearchHistoryAsync_AddsSearchHistory()
        {
            // Act
            await _dataService.AddSearchHistoryAsync(1, "test search");

            // Assert
            var history = await _context.Search_History.ToListAsync();
            Assert.Single(history);
            Assert.Equal("test search", history[0].SearchQuery);
        }

        [Fact]
        public async Task GetUserSearchHistoryAsync_ReturnsUserHistory()
        {
            // Arrange
            await _dataService.AddSearchHistoryAsync(1, "search 1");
            await _dataService.AddSearchHistoryAsync(1, "search 2");
            await _dataService.AddSearchHistoryAsync(2, "search 3");

            // Act
            var result = await _dataService.GetUserSearchHistoryAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, h => Assert.Equal(1, h.UserId));
        }

        // User Rating Tests
        [Fact]
        public async Task RateMovieAsync_ValidRating_ReturnsTrue()
        {
            // Act
            var result = await _dataService.RateMovieAsync(1, "tt001", 8);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RateMovieAsync_InvalidRating_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(await _dataService.RateMovieAsync(1, "tt001", 0));
            Assert.False(await _dataService.RateMovieAsync(1, "tt001", 11));
        }

        [Fact]
        public async Task GetUserRatingForMovieAsync_HasRating_ReturnsRating()
        {
            // Arrange
            await _dataService.RateMovieAsync(1, "tt001", 8);

            // Act
            var result = await _dataService.GetUserRatingForMovieAsync(1, "tt001");

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public async Task GetUserRatingForMovieAsync_NoRating_ReturnsNull()
        {
            // Act
            var result = await _dataService.GetUserRatingForMovieAsync(1, "tt999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateMovieRatingAsync_ValidRating_ReturnsTrue()
        {
            // Arrange
            await _dataService.RateMovieAsync(1, "tt001", 5);

            // Act
            var result = await _dataService.UpdateMovieRatingAsync(1, "tt001", 9);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveRatingAsync_ExistingRating_ReturnsTrue()
        {
            // Arrange
            await _dataService.RateMovieAsync(1, "tt001", 5);

            // Act
            var result = await _dataService.RemoveRatingAsync(1, "tt001");

            // Assert
            Assert.True(result);
        }

        // User Notes Tests
        [Fact]
        public async Task AddUserNoteAsync_ValidNote_ReturnsNote()
        {
            // Act
            var result = await _dataService.AddUserNoteAsync(1, "Great movie!", "tt001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Great movie!", result.NoteText);
            Assert.Equal("tt001", result.tconst);
        }

        [Fact]
        public async Task AddUserNoteAsync_InvalidMovie_ReturnsNull()
        {
            // Act
            var result = await _dataService.AddUserNoteAsync(1, "Note", "tt999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserNotesAsync_ReturnsUserNotes()
        {
            // Arrange
            await _dataService.AddUserNoteAsync(1, "Note 1", "tt001");
            await _dataService.AddUserNoteAsync(1, "Note 2", "tt002");
            await _dataService.AddUserNoteAsync(2, "Note 3", "tt001");

            // Act
            var result = await _dataService.GetUserNotesAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, n => Assert.Equal(1, n.userId));
        }

     

        [Fact]
        public async Task DeleteUserNoteAsync_ValidNote_ReturnsTrue()
        {
            // Arrange
            var note = await _dataService.AddUserNoteAsync(1, "Note to delete", "tt001");

            // Act
            var result = await _dataService.DeleteUserNoteAsync(note.NoteId, 1);

            // Assert
            Assert.True(result);
        }

        // NameBasic Tests
        [Fact]
        public async Task GetNameBasicByIdAsync_ValidId_ReturnsNameBasic()
        {
            // Act
            var result = await _dataService.GetNameBasicByIdAsync("nm001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("nm001", result.nconst);
            Assert.Equal("Actor One", result.primaryName);
        }

        [Fact]
        public async Task GetNameDetails_ValidId_ReturnsNameDto()
        {
            // Act
            var result = await _dataService.GetNameDetails("nm001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("nm001", result.nconst);
            Assert.Equal("Actor One", result.primaryName);
        }

        [Fact]
        public async Task GetNameBasicsPaginated_ReturnsPaginatedResults()
        {
            // Act
            var result = await _dataService.GetNameBasicsPaginated(0, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetTotalNameBasicsCount_ReturnsCount()
        {
            // Act
            var result = await _dataService.GetTotalNameBasicsCount();

            // Assert
            Assert.Equal(2, result);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        private void SeedTestData()
        {
            // Title Basics
            var titleBasics = new List<TitleBasic>
            {
                new TitleBasic { tconst = "tt001", primaryTitle = "Test Movie 1", titleType = "movie", isAdult = false, startYear = 2020, runtimeMinutes = 120 },
                new TitleBasic { tconst = "tt002", primaryTitle = "Test Movie 2", titleType = "movie", isAdult = false, startYear = 2021, runtimeMinutes = 110 },
                new TitleBasic { tconst = "tt003", primaryTitle = "Test Movie 3", titleType = "movie", isAdult = false, startYear = 2022, runtimeMinutes = 130 }
            };
            _context.Title_Basics.AddRange(titleBasics);

            // Title Ratings
            var titleRatings = new List<TitleRating>
            {
                new TitleRating { tconst = "tt001", averageRating = 8.5m, numVotes = 1000 },
                new TitleRating { tconst = "tt002", averageRating = 7.8m, numVotes = 500 },
                new TitleRating { tconst = "tt003", averageRating = 9.1m, numVotes = 1500 }
            };
            _context.Title_Ratings.AddRange(titleRatings);

            // OMDB Data
            var omdbData = new List<OmdbData>
            {
                new OmdbData { tconst = "tt001", plot = "Test plot 1", poster = "poster1.jpg" },
                new OmdbData { tconst = "tt002", plot = "Test plot 2", poster = "poster2.jpg" },
                new OmdbData { tconst = "tt003", plot = "Test plot 3", poster = "poster3.jpg" }
            };
            _context.Omdb_Data.AddRange(omdbData);

            // Genres
            var genres = new List<Genre>
            {
                new Genre { genreId = 1, genreName = "Action" },
                new Genre { genreId = 2, genreName = "Drama" },
                new Genre { genreId = 3, genreName = "Comedy" }
            };
            _context.Genres.AddRange(genres);

            // Genre Titles
            var genreTitles = new List<GenreTitle>
            {
                new GenreTitle { genreTitleId = 1, tconst = "tt001", genreId = 1 },
                new GenreTitle { genreTitleId = 2, tconst = "tt001", genreId = 2 },
                new GenreTitle { genreTitleId = 3, tconst = "tt002", genreId = 3 }
            };
            _context.Genre_Titles.AddRange(genreTitles);

            // Name Basics
            var nameBasics = new List<NameBasic>
            {
                new NameBasic { nconst = "nm001", primaryName = "Actor One", birthYear = 1990 },
                new NameBasic { nconst = "nm002", primaryName = "Actor Two", birthYear = 1985 }
            };
            _context.Name_Basics.AddRange(nameBasics);

            // Title Principals
            var titlePrincipals = new List<TitlePrincipals>
            {
                new TitlePrincipals { tconst = "tt001", nconst = "nm001", ordering = 1, category = "actor" },
                new TitlePrincipals { tconst = "tt002", nconst = "nm002", ordering = 1, category = "actress" }
            };
            _context.Title_Principals.AddRange(titlePrincipals);

            _context.SaveChanges();
        }
    }
}