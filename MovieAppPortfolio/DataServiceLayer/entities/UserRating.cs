using MovieAppPortfolio.DataServiceLayer.user;

namespace MovieAppPortfolio.DataServiceLayer.entities
{
    public class UserRating
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string TConst { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime RatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual TitleBasic TitleBasic { get; set; } = null!;
    }
}
