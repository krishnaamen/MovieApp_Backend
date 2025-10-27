using Microsoft.AspNetCore.Mvc;
using DataServiceLayer.Services.UserRatingServices;
using MovieAppPortfolio.DataServiceLayer.Data;

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

        [HttpGet("{userId}/{tconst}")]
        public async Task<IActionResult> GetUserRating(int userId, string tconst)
        {
            var rating = await _ratingRepository.GetRatingForUserAndMovie(userId, tconst);
            return rating == null ? NotFound("Rating not found.") : Ok(rating);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRating([FromBody] UserRating rating)
        {
            await _ratingRepository.AddOrUpdateRating(rating);
            return Ok("Rating added or updated successfully.");
        }

        [HttpDelete("{userId}/{tconst}")]
        public async Task<IActionResult> DeleteRating(int userId, string tconst)
        {
            var deleted = await _ratingRepository.DeleteRating(userId, tconst);
            return deleted ? Ok("Rating deleted successfully.") : NotFound("Rating not found.");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingRepository.GetAllRatings();
            return Ok(ratings);
        }
    }
}
