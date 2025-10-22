using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/titlebasics")]
    public class TitleBasicsController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly MyDbContext _context; // Add this

        // Inject both services
        public TitleBasicsController(DataService dataService, MyDbContext context)
        {
            _dataService = dataService;
            _context = context; // Initialize context
        }

        [HttpGet]
        public IActionResult GetTitleBasics()
        {
            var categories = _dataService.GetTitleBasics();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{tconst}")]
        public IActionResult GetTitleBasicById(string tconst)
        {
            var category = _dataService.GetTitleBasicById(tconst);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

      [HttpGet]
       [Route("testdbconnection")]
        public IActionResult TestDatabaseConnection()
      {
          try
          {
              // Simple query to test the connection
              var canConnect = _context.Database.CanConnect();
              if (canConnect)
              {
                  return Ok("Database connection successful.");
              }
              else
              {
                  return StatusCode(500, "Database connection failed.");
              }
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Database connection error: {ex.Message}");
          }
        }

    }
}