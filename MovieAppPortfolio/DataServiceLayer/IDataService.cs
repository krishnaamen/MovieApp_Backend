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


        Task<bool> RateMovieAsync(int userId, string tconst, int rating);
        Task<bool> AddBookmarkAsync(int userId, string tconst, string? nconst = null);
        Task<bool> RemoveBookmarkAsync(int userId, string tconst, string? nconst = null);
        Task<List<Bookmark>> GetUserBookmarksAsync(int userId);
        Task<List<UserRating>> GetUserRatingsAsync(int userId);
        Task AddSearchHistoryAsync(int userId, string searchQuery);
        Task<List<SearchHistory>> GetUserSearchHistoryAsync(int userId);
        Task<bool> IsMovieBookmarkedAsync(int userId, string tconst);
        Task<int?> GetUserRatingForMovieAsync(int userId, string tconst);

    }
}
