using System.Text.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

public interface IUsersClient
{
    Task<Role> AddRole(Role model);
    Task<Role> EditRolePermissions(Guid roleId, List<Guid> permissionIds);
    Task<List<Permission>> GetPermissions(Guid? roleId);
    Task<User> GetUserByEmail(string email);
}
public class UsersClient : IUsersClient
{
    private HttpClient _client;
    private IConfiguration _config;
    private ILogger<UsersClient> _logger;

    public UsersClient(HttpClient client, IConfiguration config, ILogger<UsersClient> logger)
    {
        client.BaseAddress = new Uri(config["Api:Users"]);
        client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

        _client = client;
        _config = config;
        _logger = logger;
    }

    public async Task<Role> AddRole(Role model)
    {
        var response = await _client.PostAsJsonAsync($"v1/roles", model);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed AddRole HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return null;
        }

        return JsonSerializer.Deserialize<Role>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    }

    public async Task<Role> EditRolePermissions(Guid roleId, List<Guid> permissionIds)
    {
        var response = await _client.PutAsJsonAsync($"v1/roles/{roleId}/permissions", permissionIds);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed EditRolePermissions HTTP call: {(int)response.StatusCode} | {data} | RoleId: {roleId}");
            return null;
        }

        return JsonSerializer.Deserialize<Role>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<List<Permission>> GetPermissions(Guid? roleId)
    {
        var response = await _client.GetAsync($"v1/permissions?roleId={roleId}");
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed GetPermissions HTTP call: {(int)response.StatusCode} | {data} | RoleId: {roleId}");
            return null;
        }

        return JsonSerializer.Deserialize<List<Permission>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var response = await _client.GetAsync($"v2/users/email/{email}");
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if ((int)response.StatusCode >= 500 && (int)response.StatusCode <= 599)
            {
                _logger.LogCritical($"Failed GetUserByEmail HTTP call: {(int)response.StatusCode} | {data}");
            } 
            else
            {
                _logger.LogWarning($"Failed GetUserByEmail HTTP call: {(int)response.StatusCode} | {data}");
            }
            return null;
        }

        return JsonSerializer.Deserialize<User>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}