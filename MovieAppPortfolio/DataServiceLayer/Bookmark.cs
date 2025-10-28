namespace MovieAppPortfolio.DataServiceLayer
{
    /// <summary>
    /// Represents a user's bookmarked movie
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// Unique identifier for the bookmark
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the bookmarked movie (references TitleBasic.tconst)
        /// </summary>
        public string? TitleId { get; set; }

        /// <summary>
        /// When the bookmark was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// The associated movie title
        /// </summary>
        public TitleBasic? Title { get; set; }
    }
}
