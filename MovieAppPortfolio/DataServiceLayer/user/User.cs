using  MovieAppPortfolio.DataServiceLayer.entities;
using System.ComponentModel.DataAnnotations;

namespace MovieAppPortfolio.DataServiceLayer.user
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? Token { get; set; }


        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();
        public virtual ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
        public virtual ICollection<UserNote> UserNotes { get; set; } = new List<UserNote>();

    }
}
