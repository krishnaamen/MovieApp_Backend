namespace MovieAppPortfolio.DataServiceLayer
{
    public interface IDataService
    {
        IList<TitleBasic> GetTitleBasics();
        TitleBasic? GetTitleBasicById(string tconst);
        IList<BestMatchResult> BestMatchSearch(string[] keywords);

    }
}
