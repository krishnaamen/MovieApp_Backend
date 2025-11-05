namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class UserNoteDto
    {
        public int noteId { get; set; }
        public int userId { get; set; }
        public string? tconst { get; set; }
        public string? nconst { get; set; }
        public string noteText { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public string? movieTitle { get; set; }
        public string? personName { get; set; }
    }
}
