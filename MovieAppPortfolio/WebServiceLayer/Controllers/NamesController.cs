using Microsoft.AspNetCore.Mvc;
using MovieAppPortfolio.DataServiceLayer;
using MovieAppPortfolio.DataServiceLayer.entities;
using MovieAppPortfolio.WebServiceLayer.Models;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NamesController : BaseController
    {
         private new readonly IDataService _dataService;
        private readonly MyDbContext _context;

        public NamesController(
            IDataService dataService,
            LinkGenerator generator,
            MyDbContext context
        ) : base(dataService, generator)
        {
             _dataService = dataService;
             _context = context;
        }

        [HttpGet("paginated", Name = nameof(GetNamesPaginated))]
        public async Task<IActionResult> GetNamesPaginated([FromQuery] QueryParams queryParams)
        {
            try
            {
                // Use the base _dataService instead of local _dataService
                var items = await _dataService.GetNameBasicsPaginated(queryParams.Page, queryParams.PageSize);
                var totalCount = await _dataService.GetTotalNameBasicsCount();

                var models = items.Select(CreateNameBasicModel).ToList();

                var pagingResult = CreatePaging(nameof(GetNamesPaginated), models, totalCount, queryParams);
                return Ok(pagingResult);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{nconst}", Name = nameof(GetNameBasicById))]
        public async Task<IActionResult> GetNameBasicById(string nconst)
        {
            try
            {
                var name = await _dataService.GetNameBasicByIdAsync(nconst);
                if (name == null)
                {
                    return NotFound();
                }
                return Ok(name);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Error retrieving name: {ex.Message}");
            }
        }

        private object? CreateNameBasicModel(NameBasic name)
        {
            if (name == null) return null;

            return new
            {
                nconst = name.nconst,
                primaryName = name.primaryName,
                birthYear = name.birthYear,
                deathYear = name.deathYear,
                url = GetUrl(nameof(GetNameBasicById), new { nconst = name.nconst })
            };
        }
    }
}