using MovieAppPortfolio.DataServiceLayer.user;

namespace MovieAppPortfolio.DataServiceLayer.entities
{
    public class UserNote
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public string? tconst { get; set; }
        public string? nconst { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual TitleBasic? TitleBasic { get; set; }
        public virtual NameBasic? NameBasic { get; set; }
    }
}
