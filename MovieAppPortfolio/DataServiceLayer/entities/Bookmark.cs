using MovieAppPortfolio.DataServiceLayer.user;

namespace MovieAppPortfolio.DataServiceLayer.entities
{
    public class Bookmark
    {
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        public string? TConst { get; set; }
        public string? NConst { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual TitleBasic? TitleBasic { get; set; }
        public virtual NameBasic? NameBasic { get; set; }

    }
}
