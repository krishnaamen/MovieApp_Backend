namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class TitlePrincipalDto
    {
        public string tconst { get; set; } = string.Empty;
        public string nconst { get; set; } = string.Empty;
        public int ordering { get; set; }
        public string? category { get; set; }
        public string? job { get; set; }
        public string? characters { get; set; }
    }
}
