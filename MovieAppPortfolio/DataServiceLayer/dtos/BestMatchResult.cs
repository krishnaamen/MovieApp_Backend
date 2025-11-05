namespace MovieAppPortfolio.DataServiceLayer.dtos
{
    public class BestMatchResult
    {
       
            public string tconst { get; set; } = string.Empty;
            public string primary_title { get; set; } = string.Empty;
            public string? title_type { get; set; } 
            public int? start_year { get; set; } 
            public decimal? average_rating { get; set; }
            public int? num_votes { get; set; } 
            public long match_score { get; set; } 
        
    }
}
