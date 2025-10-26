using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Services.UserRatingServices;
using MovieAppPortfolio.DataServiceLayer.Data;
using DataServiceLayer.Services.RatingRepository; // Where IRatingRepository is
using DataAccessLayer;
using Microsoft.AspNetCore.Routing;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        // ✅ POST: api/rating/rate
        [HttpPost("rate")]
        public async Task<IActionResult> RateMovie([FromQuery] int userId, [FromQuery] string tconst, [FromQuery] int rating)
        {
            if (rating < 1 || rating > 10)
                return BadRequest("Rating must be between 1 and 10.");

            // Check if user has already rated this movie
            bool hasRated = await _ratingRepository.CheckUserHasRatedMovie(userId, tconst);
            if (hasRated)
                return BadRequest("User has already rated this movie.");

            var userRating = new UserRating
            {
                user_id = userId,
                tconst = tconst,
                rating = rating,
                rated_date = DateTime.UtcNow
            };

            await _ratingRepository.AddOrUpdateRating(userRating);

            return Ok("Rating submitted successfully.");
        }

        // ✅ PUT: api/rating/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRating([FromQuery] int userId, [FromQuery] string tconst, [FromQuery] int newRating)
        {
            if (newRating < 1 || newRating > 10)
                return BadRequest("Rating must be between 1 and 10.");

            var updated = await _ratingRepository.UpdateRating(userId, tconst, newRating);
            if (!updated)
                return NotFound("Rating not found to update.");

            return Ok("Rating updated successfully.");
        }

        // ✅ DELETE: api/rating/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRating([FromQuery] int userId, [FromQuery] string tconst)
        {
            var isDeleted = await _ratingRepository.DeleteRating(userId, tconst);
            if (!isDeleted)
                return NotFound("Rating not found for this user or movie.");

            return Ok("Rating deleted successfully.");
        }

        // ✅ GET: api/rating/{userId}/{tconst}
        [HttpGet("{userId}/{tconst}")]
        public async Task<IActionResult> GetRatingForUserAndMovie(int userId, string tconst)
        {
            if (string.IsNullOrWhiteSpace(tconst))
                return BadRequest("Movie ID is missing.");

            var rating = await _ratingRepository.GetRatingForUserAndMovie(userId, tconst);
            if (rating == null)
                return NotFound("No rating found for this user and movie.");

            return Ok(rating);
        }

        // ✅ GET: api/rating/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingRepository.GetAllRatings();

            if (ratings == null || !ratings.Any())
                return NotFound("No ratings found.");

            return Ok(ratings);
        }
    }
}
