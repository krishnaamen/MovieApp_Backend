using System.ComponentModel.DataAnnotations;

namespace MovieAppPortfolio.DataServiceLayer.Data
{
    public class UserRating
    {
        [Key] //defines the primary key
        public int rating_id { get; set; }//primary key

        public int user_id { get; set; }

        public string tconst { get; set; } = string.Empty;


        public int rating { get; set; }

        public DateTime rated_at { get; set; } 

        public required TitleBasic TitleBasics { get; set; }
    }
}
