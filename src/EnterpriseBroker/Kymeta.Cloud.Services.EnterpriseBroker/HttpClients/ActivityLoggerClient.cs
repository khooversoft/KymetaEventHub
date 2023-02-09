using Kymeta.Cloud.Logging.Activity;
using Newtonsoft.Json;
using System.Net;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients
{
    public interface IActivityLoggerClient
    {
        Task<List<ActivityServiceModel>> GetActivities(string entityId);
        Task<List<ActivityServiceModel>> GetActivitiesForUser(Guid userId, int limit);
        Task AddActivity(string entityType, Guid userId, string userName, Guid? entityId, string entityName, string action, string fieldName = "", string oldValue = "", string newValue = "");
        Task<Tuple<HttpStatusCode, string>> HealthCheck();
    }

    public class ActivityLoggerClient : IActivityLoggerClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly IActivityLogger _activityLogger;

        public ActivityLoggerClient(HttpClient client, IConfiguration config, IActivityLogger activityLogger)
        {
            client.BaseAddress = new Uri(config["Api:ActivityLogger"]);
            client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

            _client = client;
            _config = config;
            _activityLogger = activityLogger;
        }

        public async Task AddActivity(string entityType, Guid userId, string userName, Guid? entityId, string entityName, string action, string fieldName = "", string oldValue = "", string newValue = "")
        {
            string eid = null;
            if (entityId.HasValue) eid = entityId.Value.ToString();
            await _activityLogger.AddActivity(entityType, userId.ToString(), userName, eid, entityName, action, fieldName, oldValue, newValue);
        }

        public async Task<List<ActivityServiceModel>> GetActivities(string entityId)
        {
            var results = await _client.GetAsync($"v1?entityId={entityId}&suppressGet=true");
            if (!results.IsSuccessStatusCode) return null;

            var data = await results.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ActivityServiceModel>>(data);
        }

        public async Task<List<ActivityServiceModel>> GetActivitiesForUser(Guid userId, int limit)
        {
            var response = await _client.GetAsync($"v1/user/{userId}?limit={limit}");

            if (!response.IsSuccessStatusCode) return null;

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ActivityServiceModel>>(data);
        }

        public async Task<Tuple<HttpStatusCode, string>> HealthCheck()
        {
            try
            {
                var response = await _client.GetAsync($"version");
                string data = await response.Content?.ReadAsStringAsync();
                return new Tuple<HttpStatusCode, string>(response.StatusCode, data);
            }
            catch (Exception ex)
            {
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.RequestTimeout, ex.Message);
            }
        }
    }
}
