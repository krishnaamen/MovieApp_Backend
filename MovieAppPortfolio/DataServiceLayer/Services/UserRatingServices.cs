using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.Data;

namespace DataServiceLayer.Services.UserRatingServices
{
    public class UserRatingServices : IRatingRepository
    {
        private readonly MyDbContext _mydbcontext;

        public UserRatingServices(MyDbContext mydbcontext)
        {
            _mydbcontext = mydbcontext;
        }

        // Check if user has rated a movie
        public async Task<bool> CheckUserHasRatedMovie(int userId, string tconst)
        {
            return await _mydbcontext.UserRatings
                .AnyAsync(r => r.user_id == userId && r.tconst == tconst);
        }

        //Add or update rating
        public async Task AddOrUpdateRating(UserRating rating)
        {
            var existing = await _mydbcontext.UserRatings
                .FirstOrDefaultAsync(r => r.user_id == rating.user_id && r.tconst == rating.tconst);

            if (existing != null)
            {
                existing.rating = rating.rating;
                existing.rated_date = DateTime.UtcNow;
                _mydbcontext.UserRatings.Update(existing);
            }
            else
            {
                rating.rated_date = DateTime.UtcNow;
                await _mydbcontext.UserRatings.AddAsync(rating);
            }

            await _mydbcontext.SaveChangesAsync();
        }

        //  Update only existing rating
        public async Task<bool> UpdateRating(int userId, string tconst, int newRating)
        {
            var rating = await _mydbcontext.UserRatings
                .FirstOrDefaultAsync(r => r.user_id == userId && r.tconst == tconst);

            if (rating == null)
                return false;

            rating.rating = newRating;
            rating.rated_date = DateTime.UtcNow;
            _mydbcontext.UserRatings.Update(rating);
            await _mydbcontext.SaveChangesAsync();
            return true;
        }

        //  Delete rating
        public async Task<bool> DeleteRating(int userId, string tconst)
        {
            var rating = await _mydbcontext.UserRatings
                .FirstOrDefaultAsync(r => r.user_id == userId && r.tconst == tconst);

            if (rating == null)
                return false;

            _mydbcontext.UserRatings.Remove(rating);
            await _mydbcontext.SaveChangesAsync();
            return true;
        }

        //  Get one rating by user and movie
        public async Task<UserRating?> GetRatingForUserAndMovie(int userId, string tconst)
        {
            return await _mydbcontext.UserRatings
                .Include(r => r.TitleBasics)
                .FirstOrDefaultAsync(r => r.user_id == userId && r.tconst == tconst);
        }

        // Get all ratings
        public async Task<IEnumerable<UserRating>> GetAllRatings()
        {
            return await _mydbcontext.UserRatings
                .Include(r => r.TitleBasics)
                .ToListAsync();
        }
    }
}
