using MovieAppPortfolio.DataServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace MovieAppPortfolio.DataServiceLayer.Data
{
    public class UserRating
    {
        [Key]
        public int user_id { get; set; }  // ✅ Now EF recognizes it as primary key

        public string? tconst { get; set; }
        public int rating { get; set; }
        public DateTime rated_date { get; set; }
        public TitleBasic Title_Basics { get; set; }
    }
}
