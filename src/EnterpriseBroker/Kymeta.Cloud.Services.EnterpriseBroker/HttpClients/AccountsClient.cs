using System.Text.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

/// <summary>
/// Client used to communicate with the Accounts microservice
/// </summary>
public interface IAccountsClient
{
    Task<Tuple<AccountV2, string>> AddAccount(AccountV2 model);
    Task<Tuple<AccountV2, string>> UpdateAccount(Guid id, AccountV2 model);
    Task<List<AccountV2>> GetAccountsByManySalesforceIds(List<string> salesforceIds);
    Task<string> SyncAccountsFromSalesforce(List<AccountV2> accounts);
}
public class AccountsClient : IAccountsClient
{
    private readonly HttpClient _client;
    private readonly ILogger<AccountsClient> _logger;

    public AccountsClient(HttpClient client, IConfiguration config, ILogger<AccountsClient> logger)
    {
        client.BaseAddress = new Uri(config["Api:Accounts"]);
        client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

        _client = client;
        _logger = logger;
    }

    public async Task<Tuple<AccountV2, string>> AddAccount(AccountV2 model)
    {
        var response = await _client.PostAsJsonAsync($"v2", model);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed AddAccount HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<AccountV2, string>(null, data);
        }
        
        return new Tuple<AccountV2, string>(JsonSerializer.Deserialize<AccountV2>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), null);

    }

    public async Task<Tuple<AccountV2, string>> UpdateAccount(Guid id, AccountV2 model)
    {
        var response = await _client.PutAsJsonAsync($"v2/{id}", model);
        string data = await response.Content?.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed UpdateAccount HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<AccountV2, string>(null, data);
        }

        return new Tuple<AccountV2, string>(JsonSerializer.Deserialize<AccountV2>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), null);

    }

    public async Task<List<AccountV2>> GetAccountsByManySalesforceIds(List<string> salesforceIds)
    {
        var response = await _client.PostAsJsonAsync($"v2/sfid", salesforceIds);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) return null;

        return JsonSerializer.Deserialize<List<AccountV2>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<string> SyncAccountsFromSalesforce(List<AccountV2> accounts)
    {
        var response = await _client.PostAsJsonAsync($"v2/trigger/sync", accounts);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed SyncAccountsToOSS HTTP call: {(int)response.StatusCode} | {data}");
            return $"Error syncing to OSS: {data}";
        }

        return data;
    }
}