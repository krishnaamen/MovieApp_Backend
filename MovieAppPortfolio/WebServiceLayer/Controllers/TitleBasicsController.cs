using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.entities;
using MovieAppPortfolio.WebServiceLayer.Models;
using System.Security.Claims;

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
            var movies = _dataService.GetTitleBasics();
            return Ok(movies);
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
        [Authorize]
        public async Task<IActionResult> BestMatchSearchSingle(string keyword)
        {
            try
            {
                var userId = GetCurrentUserId();
                var keywords = new[] { keyword };
                var results = _dataService.BestMatchSearch(keywords);

                // Track search history
                await _dataService.AddSearchHistoryAsync(userId, keyword);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during search: {ex.Message}");
            }
        }

        [HttpGet("search/history")]
        [Authorize]
        public async Task<IActionResult> GetSearchHistory()
        {
            var userId = GetCurrentUserId();
            var history = await _dataService.GetUserSearchHistoryAsync(userId);
            return Ok(history);
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
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
        [Route("{tconst}/details")]
        public IActionResult GetMovieDetails(string tconst)
        {
            var movieDetails = _dataService.GetMovieDetails(tconst);
            if (movieDetails == null)
            {
                return NotFound();
            }

            movieDetails.Url = GetUrl(nameof(GetTitleBasicById), new { tconst = movieDetails.tconst });

            return Ok(movieDetails);
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