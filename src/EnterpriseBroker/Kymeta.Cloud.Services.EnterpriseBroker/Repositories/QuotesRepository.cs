using Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator;
using Microsoft.Azure.Cosmos;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface IQuotesRepository
{
    Task<QuoteRequestViewModel> InsertQuoteRecord(QuoteRequestViewModel model);
    Task<IEnumerable<QuoteRequestViewModel>> GetQuoteRecords();
    Task<QuoteRequestViewModel> GetQuoteRecord(Guid id, string partitionKey);
}

public class QuotesRepository : IQuotesRepository
{
    public Container Container { get; }
    public PartitionKey ResolvePartitionKey(string objectType) => new PartitionKey(objectType);

    public QuotesRepository(
            IConfiguration config,
            CosmosClient cosmosClient)
    {
        string databaseName = config["AzureDocumentDB:KCSDatabase"] ?? "KymetaCloudServices";
        string containerName = config["AzureDocumentDB:Quotes"] ?? "Quotes";
        Container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<QuoteRequestViewModel> GetQuoteRecord(Guid id, string partitionKey)
    {
        var response = await Container.ReadItemAsync<QuoteRequestViewModel>(id.ToString(), ResolvePartitionKey(partitionKey));
        return response.Resource;
    }

    public async Task<IEnumerable<QuoteRequestViewModel>> GetQuoteRecords()
    {
        var sqlQueryText = "SELECT * FROM c ORDER BY c.createdOn DESC";

        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        FeedIterator<QuoteRequestViewModel> queryResultSetIterator = this.Container.GetItemQueryIterator<QuoteRequestViewModel>(queryDefinition);

        List<QuoteRequestViewModel> records = new();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<QuoteRequestViewModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (QuoteRequestViewModel record in currentResultSet)
            {
                records.Add(record);
            }
        }

        return records;
    }

    public async Task<QuoteRequestViewModel> InsertQuoteRecord(QuoteRequestViewModel model)
    {
        var response = await Container.UpsertItemAsync(model, ResolvePartitionKey(model.Id.ToString() ?? "Unset"));
        return response.Resource;
    }

}