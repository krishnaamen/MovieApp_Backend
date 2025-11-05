namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class NameDto
    {
        public string? nconst { get; set; } = string.Empty;
        public string? primaryName { get; set; } = string.Empty;
        public int? birthYear { get; set; }
        public int? deathYear { get; set; }

        
        public string? knownForTitles { get; set; }
        public decimal? weightedRating { get; set; }

    }
}
