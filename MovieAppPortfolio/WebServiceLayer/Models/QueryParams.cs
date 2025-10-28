namespace MovieAppPortfolio.WebServiceLayer.Models
{
    public class QueryParams
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 20;

        public int Page { get; set; } = 0;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}




