

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using Dapper;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class DataService: IDataService
    {
        private readonly MyDbContext _context;

        public DataService(MyDbContext context)
        {
            _context = context;
        }

     
        
        public TitleBasic? GetTitleBasicById(string tconst)
        {
            return _context.Title_Basics
                .FirstOrDefault(tb => tb.tconst == tconst);
        }

     

        public IList<BestMatchResult> BestMatchSearch(string[] keywords)
        {
            try
            {
                
                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Let Npgsql handle the array conversion
                    var parameters = new { p_keywords = keywords };

                    var results = connection.Query<BestMatchResult>(
                        "SELECT * FROM movie_app.best_match_search(@p_keywords)",
                        parameters
                    ).ToList();

                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
        }

        public List<TitleBasic> GetTitleBasicsPaginated(int page, int pageSize)
        {
        
            return _context.Title_Basics
                .OrderBy(tb => tb.tconst)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }
        

        public int GetTotalTitleBasicsCount()
        {
            return _context.Title_Basics.Count();
        }
        public List<TitleBasic> GetTitleBasics()
        {
            return _context.Title_Basics
               .OrderBy(tb => tb.tconst)
               .Take(50)
               .ToList();
        }
    }
}
