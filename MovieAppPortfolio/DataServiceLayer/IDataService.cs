namespace MovieAppPortfolio.DataServiceLayer
{
    public interface IDataService
    {
        List<TitleBasic> GetTitleBasics();
        List<TitleBasic> GetTitleBasicsPaginated(int page, int pageSize);
        int GetTotalTitleBasicsCount();
        TitleBasic? GetTitleBasicById(string tconst);
        IList<BestMatchResult> BestMatchSearch(string[] keywords);

    }
}
