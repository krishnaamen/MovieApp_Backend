namespace MovieAppPortfolio.WebServiceLayer.Models
{
    public class NoteResponseModel
    {

        public int NoteId { get; set; }
        public string TConst { get; set; } = string.Empty;
        public string? NConst { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
