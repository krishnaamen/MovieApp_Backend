namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class UserRatingResponseDto
    {
        public int RatingId { get; set; }
        public string TConst { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
