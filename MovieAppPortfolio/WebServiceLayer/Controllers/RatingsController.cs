using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.dtos;
using System.Security.Claims;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private readonly IDataService _dataService;

        public RatingsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public async Task<IActionResult> RateMovie([FromBody] RateMovieDto rateDto)
        {
            var userId = GetCurrentUserId();
            var success = await _dataService.RateMovieAsync(userId, rateDto.TConst, rateDto.Rating);

            if (success)
                return Ok(new { message = "Movie rated successfully" });
            else
                return BadRequest(new { message = "Failed to rate movie" });
        }

        [HttpGet("my-ratings")]
        public async Task<IActionResult> GetMyRatings()
        {
            var userId = GetCurrentUserId();
            var ratings = await _dataService.GetUserRatingsAsync(userId);

            var response = ratings.Select(r => new UserRatingResponseDto
            {
                RatingId = r.RatingId,
                TConst = r.TConst,
                Title = r.TitleBasic?.primaryTitle ?? "Unknown Title",
                Rating = r.Rating,
                RatedAt = r.RatedAt
            });

            return Ok(response);
        }

        [HttpGet("movie/{tconst}")]
        public async Task<IActionResult> GetMyRatingForMovie(string tconst)
        {
            var userId = GetCurrentUserId();
            var rating = await _dataService.GetUserRatingForMovieAsync(userId, tconst);

            return Ok(new { rating });
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
