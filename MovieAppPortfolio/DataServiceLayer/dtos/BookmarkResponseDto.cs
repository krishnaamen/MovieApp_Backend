namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class BookmarkResponseDto
    {
        public int BookmarkId { get; set; }
        public string TConst { get; set; } = string.Empty;
        public string? NConst { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type => NConst != null ? "person" : "movie";
    }
}
