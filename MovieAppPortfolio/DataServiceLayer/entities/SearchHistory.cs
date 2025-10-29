using MovieAppPortfolio.DataServiceLayer.user;

namespace MovieAppPortfolio.DataServiceLayer.entities
{
    public class SearchHistory
    {
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string SearchQuery { get; set; } = string.Empty;
        public DateTime SearchTime { get; set; }

       
        public virtual User User { get; set; } = null!;
    }
}
