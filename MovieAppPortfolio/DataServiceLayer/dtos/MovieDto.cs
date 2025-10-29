namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class MovieDto
    {
        public string? tconst { get; set; }
        public string? titleType { get; set; }
        public string? primaryTitle { get; set; }
        public string? originalTitle { get; set; }
        public bool? isAdult { get; set; }
        public int? startYear { get; set; }
        public int? endYear { get; set; }
        public int? runtimeMinutes { get; set; }

        public decimal? AverageRating { get; set; }
        public int? NumVotes { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public List<string>? Genres { get; set; }
        public string? Url { get; set; }

    }
}
