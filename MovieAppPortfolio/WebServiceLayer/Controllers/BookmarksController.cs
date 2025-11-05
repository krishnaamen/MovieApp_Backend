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
    public class BookmarksController : ControllerBase
    {
        private readonly IDataService _dataService;

        public BookmarksController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("debug/token")]
        public IActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }



        [HttpPost]
        public async Task<IActionResult> AddBookmark([FromBody] BookmarkDto bookmarkDto)
        {
            var userId = GetCurrentUserId();
            var success = await _dataService.AddBookmarkAsync(userId, bookmarkDto.TConst, bookmarkDto.NConst);

            if (success)
                return Ok(new { message = "Bookmark added successfully" });
            else
                return BadRequest(new { message = "Failed to add bookmark" });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBookmark([FromBody] BookmarkDto bookmarkDto)
        {
            var userId = GetCurrentUserId();
            var success = await _dataService.RemoveBookmarkAsync(userId, bookmarkDto.TConst, bookmarkDto.NConst);

            if (success)
                return Ok(new { message = "Bookmark removed successfully" });
            else
                return BadRequest(new { message = "Failed to remove bookmark" });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyBookmarks()
        {
            var userId = GetCurrentUserId();
            var bookmarks = await _dataService.GetUserBookmarksAsync(userId);

            var response = bookmarks.Select(b => new BookmarkResponseDto
            {
                BookmarkId = b.BookmarkId,
                TConst = b.TConst ?? "",
                NConst = b.NConst,
                Title = b.TitleBasic?.primaryTitle,
                Name = b.NameBasic?.primaryName,
                CreatedAt = b.CreatedAt
            });

            return Ok(response);
        }

        [HttpGet("check/{tconst}")]
        public async Task<IActionResult> IsBookmarked(string tconst)
        {
            var userId = GetCurrentUserId();
            var isBookmarked = await _dataService.IsMovieBookmarkedAsync(userId, tconst);

            return Ok(new { isBookmarked });
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
