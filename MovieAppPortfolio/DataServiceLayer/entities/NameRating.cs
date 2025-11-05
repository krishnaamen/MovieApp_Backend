namespace MovieAppPortfolio.DataServiceLayer.entities
{
    public class NameRating
    {
        public string nconst { get; set; } = string.Empty;
        public decimal? weightedRating { get; set; }
        public DateTime lastUpdated { get; set; }

        public NameBasic? NameBasic { get; set; }


    }
}
