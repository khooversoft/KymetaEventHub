using Microsoft.Azure.Cosmos;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface IActionsRepository
{
    Task<SalesforceActionTransaction> InsertActionRecord(SalesforceActionTransaction model);
    Task<IEnumerable<SalesforceActionTransaction>> GetActionRecords();
    Task<SalesforceActionTransaction> UpdateActionRecord(SalesforceActionTransaction model);
    Task<SalesforceActionTransaction> GetActionRecord(Guid id, string partitionKey);
    Task AddTransactionRecord(Guid id, string partitionKey, SalesforceActionRecord model);
}

public class ActionsRepository : IActionsRepository
{
    public Container Container { get; }
    public PartitionKey ResolvePartitionKey(string objectType) => new PartitionKey(objectType);

    public ActionsRepository(
            IConfiguration config,
            CosmosClient cosmosClient)
    {
        string databaseName = config["AzureDocumentDB:KCSDatabase"] ?? "KymetaCloudServices";
        string containerName = config["AzureDocumentDB:EnterpriseActionsCollection"] ?? "EnterpriseActions";
        Container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<SalesforceActionTransaction> GetActionRecord(Guid id, string partitionKey)
    {
        var response = await Container.ReadItemAsync<SalesforceActionTransaction>(id.ToString(), ResolvePartitionKey(partitionKey));
        return response.Resource;
    }

    public async Task<IEnumerable<SalesforceActionTransaction>> GetActionRecords()
    {
        var sqlQueryText = "SELECT * FROM c ORDER BY c.createdOn DESC";

        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        FeedIterator<SalesforceActionTransaction> queryResultSetIterator = this.Container.GetItemQueryIterator<SalesforceActionTransaction>(queryDefinition);

        List<SalesforceActionTransaction> records = new List<SalesforceActionTransaction>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<SalesforceActionTransaction> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (SalesforceActionTransaction record in currentResultSet)
            {
                records.Add(record);
            }
        }

        return records;
    }

    public async Task<SalesforceActionTransaction> InsertActionRecord(SalesforceActionTransaction model)
    {
        var response = await Container.UpsertItemAsync(model, ResolvePartitionKey(model.Object.ToString() ?? "Unset"));
        return response.Resource;
    }

    public async Task<SalesforceActionTransaction> UpdateActionRecord(SalesforceActionTransaction model)
    {
        if (!model.Object.HasValue) return null;
        var existingRecord = await GetActionRecord(model.Id, model.Object.ToString());
        if (existingRecord == null) return null;
        existingRecord.LastUpdatedOn = DateTime.UtcNow;
        var response = await Container.UpsertItemAsync(model, ResolvePartitionKey(model.Object.ToString() ?? "Unset"));
        return response.Resource;
    }

    public async Task AddTransactionRecord(Guid id, string partitionKey, SalesforceActionRecord model)
    {
        var existingRecord = await GetActionRecord(id, partitionKey);
        if (existingRecord == null) return;
        if (existingRecord.TransactionLog == null) existingRecord.TransactionLog = new List<SalesforceActionRecord>();
        existingRecord.TransactionLog.Add(model);
        await UpdateActionRecord(existingRecord);
    }
}

public static class ServiceCollectionCosmosDbExtensions
{
    public static IServiceCollection AddCosmosDb(this IServiceCollection services, string connectionString)
    {
        var cosmosClient = new CosmosClient(connectionString, new CosmosClientOptions { 
            ConnectionMode = ConnectionMode.Direct, 
            RequestTimeout = TimeSpan.FromSeconds(10),
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                IgnoreNullValues = false,
                Indented = true
            }
        });
        services.AddSingleton<CosmosClient>(cosmosClient);
        return services;
    }
}
