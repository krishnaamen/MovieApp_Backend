using DataAccessLayer;
using DataServiceLayer.Services.RatingRepository;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services.UserRatingServices
{
    public class UserRatingServices : IRatingRepository
    {
        private readonly MyDbContext _mydbcontext;

  
             // ✅ Get all user ratings
        public async Task<List<UserRating>> GetAllUserRatingsAsync()
        {
            return await _mydbcontext.UserRatings
                .Include(r => r.Title_Basics)
                .ToListAsync();
        }

        // ✅ Get a single rating by user ID and movie ID
        public async Task<UserRating?> GetRatingAsync(int userId, string tconst)
        {
            return await _mydbcontext.UserRatings
                .Include(r => r.Title_Basics)
                .FirstOrDefaultAsync(r => r.user_id == userId && r.tconst == tconst);
        }

        // ✅ Add a new rating
        public async Task<bool> AddRatingAsync(UserRating userRating)
        {
            // check if user already rated
            var existing = await GetRatingAsync(userRating.user_id, userRating.tconst);
            if (existing != null)
                return false; // already exists

            userRating.rated_date = DateTime.UtcNow;
            await _mydbcontext.UserRatings.AddAsync(userRating);
            await _mydbcontext.SaveChangesAsync();
            return true;
        }

        // ✅ Update an existing rating
        public async Task<bool> UpdateRatingAsync(int userId, string tconst, int newRating)
        {
            var rating = await GetRatingAsync(userId, tconst);
            if (rating == null)
                return false;

            rating.rating = newRating;
            rating.rated_date = DateTime.UtcNow;
            _mydbcontext.UserRatings.Update(rating);
            await _mydbcontext.SaveChangesAsync();
            return true;
        }

        // ✅ Delete a rating
        public async Task<bool> DeleteRatingAsync(int userId, string tconst)
        {
            var rating = await GetRatingAsync(userId, tconst);
            if (rating == null)
                return false;

            _mydbcontext.UserRatings.Remove(rating);
            await _mydbcontext.SaveChangesAsync();
            return true;
        }

        public Task<bool> CheckUserHasRatedMovie(int userId, string tconst)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateRating(UserRating rating)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRating(int userId, string tconst, int newRating)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRating(int userId, string tconst)
        {
            throw new NotImplementedException();
        }

        public Task<UserRating?> GetRatingForUserAndMovie(int userId, string tconst)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRating>> GetAllRatings()
        {
            throw new NotImplementedException();
        }
    }
}
