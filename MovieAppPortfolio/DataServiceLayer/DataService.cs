namespace MovieAppPortfolio.DataServiceLayer
{
    public class DataService
    {
        private readonly MyDbContext _context;

        public DataService(MyDbContext context)
        {
            _context = context;
        }

        public List<TitleBasic> GetTitleBasics()
        {
            return _context.Title_Basics
                .OrderBy(tb => tb.tconst)
                .Take(50)
                .ToList();
        }
        
        public TitleBasic? GetTitleBasicById(string tconst)
        {
            return _context.Title_Basics
                .FirstOrDefault(tb => tb.tconst == tconst);
        }
    }
}
