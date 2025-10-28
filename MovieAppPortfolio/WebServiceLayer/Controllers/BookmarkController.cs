using Microsoft.AspNetCore.Mvc;
using MovieAppPortfolio.DataServiceLayer;

using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    /// <summary>
    /// Manages movie bookmarks
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BookmarkController : ControllerBase
    {
        private readonly DataService _dataService;

        public BookmarkController(DataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Retrieves all bookmarked movies
        /// </summary>
        /// <returns>List of bookmarks with associated movie details</returns>
        /// <response code="200">Returns the list of bookmarks</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        
        public ActionResult<IEnumerable<Bookmark>> GetBookmarks()
        {
            return _dataService.GetBookmarks();
        }

        /// <summary>
        /// Bookmarks a movie
        /// </summary>
        /// <param name="titleId">The ID of the movie to bookmark</param>
        /// <returns>The created bookmark</returns>
        /// <response code="201">Returns the newly created bookmark</response>
        /// <response code="404">If the movie is not found</response>
        [HttpPost("{titleId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        
        public async Task<ActionResult<Bookmark>> AddBookmark(string titleId)
        {
            try
            {
                var bookmark = await _dataService.AddBookmarkAsync(titleId);
                return CreatedAtAction(nameof(GetBookmarks), new { id = bookmark.Id }, bookmark);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Removes a bookmark
        /// </summary>
        /// <param name="id">The ID of the bookmark to remove</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the bookmark was successfully deleted</response>
        /// <response code="404">If the bookmark was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> DeleteBookmark(int id)
        {
            try
            {
                await _dataService.RemoveBookmarkAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}