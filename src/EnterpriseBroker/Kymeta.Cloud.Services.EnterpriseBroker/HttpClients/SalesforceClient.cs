using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Newtonsoft.Json;
using System.Text;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

public interface ISalesforceClient
{
    #region Account-related
    Task<SalesforceAccountObjectModel> GetAccountFromSalesforce(string accountId);
    Task<List<SalesforceAccountObjectModel>> GetAccountsFromSalesforce(bool syncEnabledOnly = true);
    Task UpdateAccountInSalesforce(SalesforceAccountObjectModel account);
    #endregion

    Task<Tuple<string, string>?> GetTokenAndUrl();
    Task<SalesforceAddressObjectModel> GetAddressFromSalesforce(string addressId);
    Task<SalesforceContactObjectModel> GetContactFromSalesforce(string contactId);
    Task<List<SalesforceContactObjectModel>> GetContactsFromSalesforce(string? accountId = null);
    Task<SalesforceUserObjectModel> GetUserFromSalesforce(string userId);
    Task<T?> GetReport<T>(string reportId);
    Task<IEnumerable<SalesforceProductObjectModelV2>> GetProductsByManyIds(IEnumerable<string> productIds);
    Task<SalesforceQueryObjectModel<SalesforceQueryRelatedFilesModel>> GetRelatedFiles(IEnumerable<string> salesforceIds);
    Task<SalesforceFileResponseModel?> GetFileMetadataByManyIds(IEnumerable<string> fileIds);
    Task<Stream> DownloadFileContent(string downloadUrl);
}

public class SalesforceClient : ISalesforceClient
{
    private HttpClient _client;
    private IConfiguration _config;
    private ILogger<UsersClient> _logger;
    private IRedisClient _redis;

    public SalesforceClient(HttpClient client, IConfiguration config, ILogger<UsersClient> logger, IRedisClient redis)
    {
        _client = client;
        _config = config;
        _logger = logger;
        _redis = redis;
    }

    public async Task<SalesforceAddressObjectModel> GetAddressFromSalesforce(string addressId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/Address__c/{addressId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceAddressObjectModel>(stringResponse);

            return accountObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Address from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<SalesforceContactObjectModel> GetContactFromSalesforce(string contactId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/Contact/{contactId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceContactObjectModel>(stringResponse);

            return accountObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Contact from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<List<SalesforceContactObjectModel>> GetContactsFromSalesforce(string? accountId = null)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            string queryBase = $"{url}/services/data/v53.0/query/?q=";
            string querySelectStatement = $"SELECT c.Id, c.FirstName, c.LastName, c.Name, c.Email, c.Account.Name, c.Description, c.Primary_Contact__c, c.Technical_Contact__c FROM Contact c";
            string fullUrl = queryBase + querySelectStatement;
            if (accountId != null) fullUrl += $" WHERE c.AccountId = '{accountId}'";

            var response = await _client.GetAsync(fullUrl);
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceQueryObjectModel<SalesforceContactObjectModel>>(stringResponse);

            return accountObject?.Records;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Contact from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<SalesforceAccountObjectModel> GetAccountFromSalesforce(string accountId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/Account/{accountId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceAccountObjectModel>(stringResponse);

            return accountObject;

        } catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Account from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<List<SalesforceAccountObjectModel>> GetAccountsFromSalesforce(bool syncEnabledOnly = true)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            string queryBase = $"{url}/services/data/v53.0/query/?q=";
            string querySelectStatement = $"SELECT a.Id,a.Name,a.IsPartner,a.ParentId,a.AccountType__c,a.Sub_Type__c,a.Vertical__c,a.KSN_Acct_ID__c,a.Oracle_Acct__c,a.Master_Agreement__c,a.Account_Manager__c,a.Marketing_Representative__c,a.Sync_Instructions__c,a.Type_of_Company__c,a.Business_Unit__c,a.Pricebook__c,a.Volume_Tier__c,a.EB_Configurator_Contact__c,a.EB_Configurator_Contact_Override__c,a.EB_Configurator_PB_C_Visible__c,a.EB_Configurator_Discount_Tier__c,a.EB_Configurator_PB_M_Visible__c,a.EB_Configurator_Pricing_MSRP_Visible__c,a.EB_Configurator_Visible__c,a.EB_Configurator_Pricing_WS_Visible__c FROM Account a";
            if (syncEnabledOnly)
            {
                querySelectStatement += $" WHERE a.Sync_Instructions__c = 'Sync to All'";
            }
            string fullUrl = queryBase + querySelectStatement;

            var response = await _client.GetAsync(fullUrl);
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accounts = JsonConvert.DeserializeObject<SalesforceQueryObjectModel<SalesforceAccountObjectModel>>(stringResponse);

            return accounts?.Records;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Contact from Salesforce: {ex.Message}");
            throw;
        }
    }

    public async Task<SalesforceUserObjectModel> GetUserFromSalesforce(string userId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/User/{userId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceUserObjectModel>(stringResponse);

            return accountObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching User from Salesforce: {ex.Message}");
            return null;
        }
    }
    public async Task<T?> GetReport<T>(string reportId)
    {
        try
        {
            if (string.IsNullOrEmpty(reportId)) return default(T);
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            if (!_client.DefaultRequestHeaders.Any(drh => drh.Key == "Authorization")) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            //_client.DefaultRequestHeaders.Add("Accept", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // receive data as Excel file stream
            var response = await _client.GetAsync($"{url}/services/data/v53.0/analytics/reports/{reportId}");
            var stringResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return default(T);

            var productObject = JsonConvert.DeserializeObject<T>(stringResponse);

            return productObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Report '{reportId}' from Salesforce: {ex.Message}");
            return default(T);
        }
    }

    #region Product-related Methods
    public async Task<IEnumerable<SalesforceProductObjectModelV2>> GetProductsByManyIds(IEnumerable<string> productIds)
    {
        try
        {
            // if no productIds were provided, return empty list
            if (productIds == null || !productIds.Any()) return new List<SalesforceProductObjectModelV2>();

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // define the payload the Salesforce endpoint accepts
            var payload = new SalesforceProductCompositeObjectModel()
            {
                Ids = productIds.ToList(),
                Fields = new List<string>()
                {
                    "Id",
                    "Name__c",
                    "cpqProductDescription__c"
                }
            };

            // append auth token
            if (!_client.DefaultRequestHeaders.Any(drh => drh.Key == "Authorization")) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            // send the request
            var response = await _client.PostAsJsonAsync($"{url}/services/data/v53.0/composite/sobjects/Product2", payload);
            var stringResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // the request failed
                _logger.LogError($"The attempt to fetch Products from Salesforce failed: {stringResponse}", productIds);
                return null;
            }

            var products = JsonConvert.DeserializeObject<List<SalesforceProductObjectModelV2>>(stringResponse);
            return products;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Products from Salesforce: {ex.Message}");
            return null;
        }
    }
    public async Task<SalesforceQueryObjectModel<SalesforceQueryRelatedFilesModel>> GetRelatedFiles(IEnumerable<string> salesforceIds)
    {
        try
        {
            // if no objectIds were provided, return empty list
            if (salesforceIds == null || !salesforceIds.Any()) return null;

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // Salesforce can only accommodate batches of 100 ids in each query
            // https://developer.salesforce.com/docs/atlas.en-us.chatterapi.meta/chatterapi/connect_resources_files_information_batch.htm
            // slice salesforceIds into counts of 85
            IEnumerable<string[]> batches = salesforceIds.Chunk(85);

            // build list of requests to fetch related files
            var batchTasks = new List<Task<HttpResponseMessage>>();
            foreach (var batch in batches)
            {
                var formattedIds = batch.Select(id => $"'{id}'");
                var batchPayload = string.Join(",", formattedIds);
                batchTasks.Add(_client.GetAsync($"{url}/services/data/v43.0/query?q=select id, LinkedEntityId,ContentDocumentId from ContentDocumentLink where LinkedEntityId IN ({batchPayload})"));
            }

            // process all batches
            await Task.WhenAll(batchTasks);

            // get the results
            var batchResponses = batchTasks.Select(t => t.Result).ToList();

            // check for errors for all responses
            var hasErrors = batchResponses.Any(br => !br.IsSuccessStatusCode);
            if (hasErrors)
            {
                _logger.LogError($"The attempt to fetch Files from Salesforce failed.", batchResponses.Where(br => !br.IsSuccessStatusCode));
                return null;
            }

            // isolate the response data for each request
            var result = new SalesforceQueryObjectModel<SalesforceQueryRelatedFilesModel>();
            foreach (var res in batchResponses)
            {
                // extract the data from the response
                var data = await res.Content.ReadAsStringAsync();
                var queryResult = JsonConvert.DeserializeObject<SalesforceQueryObjectModel<SalesforceQueryRelatedFilesModel>>(data);

                // add all of the file records to the return object
                if (result.Records == null)
                {
                    result.Records = queryResult.Records;
                }
                else
                {
                    result.Records.AddRange(queryResult.Records);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Related Files from Salesforce: {ex.Message}", salesforceIds);
            throw;
        }
    }
    public async Task<SalesforceFileResponseModel?> GetFileMetadataByManyIds(IEnumerable<string> fileIds)
    {
        try
        {
            // if no objectIds were provided, return empty list
            if (fileIds == null || !fileIds.Any()) return null;

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // Salesforce can only accommodate batches of 100 file Ids
            // https://developer.salesforce.com/docs/atlas.en-us.chatterapi.meta/chatterapi/connect_resources_files_information_batch.htm
            // slice fileIds into counts of 99
            IEnumerable<string[]> batches = fileIds.Chunk(99);

            // build list of requests to fetch metadata for all files
            var batchFileTasks = new List<Task<HttpResponseMessage>>();
            foreach (var batch in batches)
            {
                var batchPayload = string.Join(",", batch);
                batchFileTasks.Add(_client.GetAsync($"{url}/services/data/v43.0/connect/files/batch/{batchPayload}"));
            }

            // process all batches
            await Task.WhenAll(batchFileTasks);

            // get the results
            var batchResponses = batchFileTasks.Select(t => t.Result).ToList();

            // check for errors for all responses
            var hasErrors = batchResponses.Any(br => !br.IsSuccessStatusCode);
            if (hasErrors)
            {
                _logger.LogError($"The attempt to fetch Files from Salesforce failed.", batchResponses.Where(br => !br.IsSuccessStatusCode));
                return null;
            }

            // isolate the response data for each request
            var fileResponse = new SalesforceFileResponseModel();
            foreach (var res in batchResponses)
            {
                // extract the data from the response
                var data = await res.Content.ReadAsStringAsync();
                var fileResults = JsonConvert.DeserializeObject<SalesforceFileResponseModel>(data);
                // if this resul had an error, make sure our response object indicates this as well
                if (fileResults.HasErrors) fileResponse.HasErrors = true;

                // add all of the file results to the return object
                if (fileResponse.Results == null)
                {
                    fileResponse.Results = fileResults.Results;
                }
                else
                {
                    fileResponse.Results.AddRange(fileResults.Results);
                }
            }

            return fileResponse;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Files from Salesforce: {ex.Message}");
            return null;
        }
    }
    public async Task<Stream> DownloadFileContent(string downloadUrl)
    {
        try
        {
            // if no URL was provided, return null
            if (string.IsNullOrEmpty(downloadUrl)) return null;

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // append auth token
            if (!_client.DefaultRequestHeaders.Any(drh => drh.Key == "Authorization")) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            // send the request
            var response = await _client.GetAsync($"{url}/{downloadUrl}");
            if (!response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                // the request failed
                _logger.LogError($"The attempt to fetch File content from Salesforce failed: {stringResponse}", downloadUrl);
                return null;
            }
            var streamResponse = await response.Content.ReadAsStreamAsync();
            return streamResponse;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Files from Salesforce: {ex.Message}");
            return null;
        }
    }
    #endregion

    public async Task<Tuple<string, string>?> GetTokenAndUrl()
    {
        var token = _redis.StringGet<string>("EB:SFToken");
        var url = _redis.StringGet<string>("EB:SFApiRoot");

        // authenticate
        if (/*true ||*/ string.IsNullOrEmpty(token))
        {
            var authObject = await Authenticate();
            if (authObject != null)
            {
                token = authObject.AccessToken;
                url = authObject.InstanceUrl;
            }
            else
            {
                _logger.LogError($"[EB] Attempted to log into Salesforce, but failed to get token. First 6 of client id: {_config["Salesforce:ConnectedApp:ClientId"]?.Substring(0, 6)}, First 6 of client secret: {_config["Salesforce:ConnectedApp:ClientSecret"]?.Substring(0, 6)}");
                return null;
            }
        }

        return new Tuple<string, string>(token, url);
    }

    private async Task<SalesforceAuthenticationResponse> Authenticate()
    {
        HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", _config["Salesforce:ConnectedApp:ClientId"] ?? "defaultclientid" },
            { "client_secret", _config["Salesforce:ConnectedApp:ClientSecret"] ?? "defaultclientsecret" },
            { "username", _config["Salesforce:Username"] ?? "defaultusername" },
            { "password", _config["Salesforce:Password"] ?? "defaultpassword" }
        });

        var response = await _client.PostAsync($"{_config["Salesforce:LoginEndpoint"]}/services/oauth2/token", content);
        var stringResponse = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) return null;

        var authenticationObject = JsonConvert.DeserializeObject<SalesforceAuthenticationResponse>(stringResponse);
        if (authenticationObject != null)
        {
            _redis.StringSet("EB:SFToken", authenticationObject?.AccessToken, TimeSpan.FromHours(1));
            _redis.StringSet("EB:SFApiRoot", authenticationObject?.InstanceUrl, TimeSpan.FromHours(1));
        }

        return authenticationObject;
    }

    public async Task UpdateAccountInSalesforce(SalesforceAccountObjectModel account)
    {
        try
        {
            var json = JsonConvert.SerializeObject(account);
            var content = new StringContent(json, Encoding.UTF8, "application/jsom");

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.PatchAsync($"{url}/services/data/v53.0/sobjects/Account/{account.Id}", content);

            if (!response.IsSuccessStatusCode) throw new Exception($"Response was not successful. Code {response.StatusCode}");
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating account in SF: {ex.Message}");
            ex.Data.Add("ErrorData", $"Error updating account in SF: {ex.Message}");
            throw;
        }
    }
}

