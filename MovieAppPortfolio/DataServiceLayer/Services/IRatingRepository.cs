using MovieAppPortfolio.DataServiceLayer.Data;

namespace DataServiceLayer.Services.UserRatingServices
{
    public interface IRatingRepository
    {
        Task<bool> CheckUserHasRatedMovie(int userId, string tconst);
        Task AddOrUpdateRating(UserRating rating);
        Task<bool> UpdateRating(int userId, string tconst, int newRating);
        Task<bool> DeleteRating(int userId, string tconst);
        Task<UserRating?> GetRatingForUserAndMovie(int userId, string tconst);
        Task<IEnumerable<UserRating>> GetAllRatings();
    }
}
