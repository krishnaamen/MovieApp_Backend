using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MovieAppPortfolio.DataServiceLayer.entities
{
    public class TitleRating
    {
        public string? tconst { get; set; }
        public decimal? averageRating { get; set; }
        public int? numVotes { get; set; }
    }
}
