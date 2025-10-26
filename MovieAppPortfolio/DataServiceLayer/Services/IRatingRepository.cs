using DataAccessLayer;
using MovieAppPortfolio.DataServiceLayer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataServiceLayer.Services.RatingRepository
{
    // This interface defines what our Rating Repository (service) must do.
    // It’s like a “contract” that tells the program which methods are available
    // for working with movie ratings in the database.
    public interface IRatingRepository
    {
        // ✅ Check if a user has already rated a specific movie.
        Task<bool> CheckUserHasRatedMovie(int userId, string tconst);

        // ✅ Add a new rating or update an existing one.
        // Example: if a user changes their mind and gives a new score.
        Task AddOrUpdateRating(UserRating rating);

        // ✅ Update the rating value for a movie already rated by the user.
        // Returns true if successful, false if no such rating was found.
        Task<bool> UpdateRating(int userId, string tconst, int newRating);

        // ✅ Delete a user’s rating for a specific movie.
        // Returns true if deleted, false if not found.
        Task<bool> DeleteRating(int userId, string tconst);

        // ✅ Get the rating a user gave to a particular movie.
        // Returns the UserRating object if found, otherwise null.
        Task<UserRating?> GetRatingForUserAndMovie(int userId, string tconst);

        // ✅ Get a list of ALL ratings in the system (for admin or testing).
        Task<IEnumerable<UserRating>> GetAllRatings();
    }
}
