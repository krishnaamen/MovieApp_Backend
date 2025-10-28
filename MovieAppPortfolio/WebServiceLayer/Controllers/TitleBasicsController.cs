using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.WebServiceLayer.Models;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/titlebasics")]
    public class TitleBasicsController : BaseController
    {
        
        private new readonly IDataService _dataService;
        private readonly MyDbContext _context;
        

       
        public TitleBasicsController(
            IDataService dataService,
            LinkGenerator generator,
            MyDbContext context
            
        ) : base(dataService, generator)
        {
            _dataService = dataService;
            _context = context;
           
        }

        [HttpGet]
        public IActionResult GetTitleBasics()
        {
            // Explicitly cast to resolve ambiguity between IList<TitleBasic> and List<TitleBasic>
            var categories = ((IDataService)_dataService).GetTitleBasics();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{tconst}", Name = nameof(GetTitleBasicById))]
        public IActionResult GetTitleBasicById(string tconst)
        {
            var movie = _dataService.GetTitleBasicById(tconst);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        private TitleBasicsModel CreateTitleBasicsModel(TitleBasic titleBasic)
        {
            var model = new TitleBasicsModel
            {
                tconst = titleBasic.tconst,
                titleType = titleBasic.titleType,
                primaryTitle = titleBasic.primaryTitle,
                originalTitle = titleBasic.originalTitle,
                isAdult = titleBasic.isAdult,
                startYear = titleBasic.startYear,
                endYear = titleBasic.endYear,
                runtimeMinutes = titleBasic.runtimeMinutes,
                Url = GetUrl(nameof(GetTitleBasicById), new { tconst = titleBasic.tconst })
            };
            return model;
        }

        [HttpGet("search/{keyword}")]
        public IActionResult BestMatchSearchSingle(string keyword)
        {
            try
            {
                var keywords = new[] { keyword };
                var results = _dataService.BestMatchSearch(keywords);
                return Ok(results);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during search: {ex.Message}");
            }
        }

        [HttpGet("paginated", Name = nameof(GetTitleBasicsPaginated))]
        public IActionResult GetTitleBasicsPaginated([FromQuery] QueryParams queryParams)
        {
            try
            {
                var items = _dataService.GetTitleBasicsPaginated(queryParams.Page, queryParams.PageSize);
                var totalCount = _dataService.GetTotalTitleBasicsCount();

                var models = items.Select(CreateTitleBasicsModel).ToList();

                var pagingResult = CreatePaging(nameof(GetTitleBasicsPaginated), models, totalCount, queryParams);
                return Ok(pagingResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
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