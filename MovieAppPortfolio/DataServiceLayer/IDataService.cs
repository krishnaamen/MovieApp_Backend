using MovieAppPortfolio.DataServiceLayer.dtos;
using MovieAppPortfolio.DataServiceLayer.entities;

namespace MovieAppPortfolio.DataServiceLayer
{
    public interface IDataService
    {
        List<TitleBasic> GetTitleBasics();
        List<TitleBasic> GetTitleBasicsPaginated(int page, int pageSize);
        int GetTotalTitleBasicsCount();
        TitleBasic? GetTitleBasicById(string tconst);
        IList<BestMatchResult> BestMatchSearch(string[] keywords);
        MovieDto? GetMovieDetails(string tconst);
        Task AddSearchHistoryAsync(int userId, string searchQuery);
        Task<List<SearchHistory>> GetUserSearchHistoryAsync(int userId);


        Task<bool> AddBookmarkAsync(int userId, string tconst, string? nconst = null);
        Task<bool> RemoveBookmarkAsync(int userId, string tconst, string? nconst = null);
        Task<List<Bookmark>> GetUserBookmarksAsync(int userId);
        Task<bool> IsMovieBookmarkedAsync(int userId, string tconst);






        Task<bool> RateMovieAsync(int userId, string tconst, int rating);
        Task<List<UserRating>> GetUserRatingsAsync(int userId);
        Task<int?> GetUserRatingForMovieAsync(int userId, string tconst);
        Task<bool> UpdateMovieRatingAsync(int userId, string tconst, int newRating);
        Task<bool> RemoveRatingAsync(int userId, string tconst);
        Task<UserRating?> GetUserRatingByIdAsync(int ratingId, int userId);
        Task<bool> HasUserRatedMovieAsync(int userId, string tconst);




        Task<List<NameBasic>> GetNameBasicsPaginated(int page, int pageSize);
        Task<int> GetTotalNameBasicsCount();
        Task<NameBasic?> GetNameBasicByIdAsync(string nconst);
        Task<NameDto?> GetNameDetails(string nconst);



        Task<UserNote?> AddUserNoteAsync(int userId, string noteText, string? tconst = null, string? nconst = null);
        Task<List<UserNoteDto>> GetUserNotesAsync(int userId);
        Task<UserNote?> GetUserNoteByIdAsync(int noteId, int userId);
        Task<bool> UpdateUserNoteAsync(int noteId, int userId, string newNoteText);
        Task<bool> DeleteUserNoteAsync(int noteId, int userId);
        Task<bool> UserHasNoteForAsync(int userId, string? tconst = null, string? nconst = null);


    }
}
