using Microsoft.AspNetCore.Mvc;
using MovieAppPortfolio.DataServiceLayer;

namespace MovieAppPortfolio.WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/titlebasics")]
    public class TitleBasicsController : ControllerBase
    {
        private readonly IDataService _dataService;

        //Use IDataService, not DataService
        public TitleBasicsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult GetTitleBasics()
        {
            var titles = _dataService.GetTitleBasics();
            return Ok(titles);
        }

        [HttpGet("{tconst}")]
        public IActionResult GetTitleBasicById(string tconst)
        {
            var title = _dataService.GetTitleBasicById(tconst);
            if (title == null)
                return NotFound();

            return Ok(title);
        }
    }
}
