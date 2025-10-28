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

        public async Task<Bookmark> AddBookmarkAsync(string titleId)
        {
            var title = await _context.Title_Basics.FindAsync(titleId);
            if (title == null)
                throw new KeyNotFoundException($"Movie with ID {titleId} not found");

            var bookmark = new Bookmark
            {
                TitleId = titleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            return bookmark;
        }

        public async Task RemoveBookmarkAsync(int bookmarkId)
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkId);
            if (bookmark == null)
                throw new KeyNotFoundException($"Bookmark with ID {bookmarkId} not found");

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
        }

        public List<Bookmark> GetBookmarks()
        {
            return _context.Bookmarks
                .OrderByDescending(b => b.CreatedAt)
                .ToList();
        }
    }
}
