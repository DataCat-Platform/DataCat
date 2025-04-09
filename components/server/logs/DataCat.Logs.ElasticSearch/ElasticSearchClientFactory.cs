namespace DataCat.Logs.ElasticSearch;

public sealed class ElasticSearchClientFactory : ISearchLogsClientFactory
{
    public ISearchLogsClient CreateClient()
    {
        throw new NotImplementedException();
    }

    public ISearchLogsClient CreateClient(string indexPattern)
    {
        throw new NotImplementedException();
    }
}