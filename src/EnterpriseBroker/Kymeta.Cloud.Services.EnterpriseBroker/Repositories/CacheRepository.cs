using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories
{
    public interface ICacheRepository
    {
        void AddProduct(SalesforceProductObjectModelV2 model);
        void UpdateProduct(SalesforceProductObjectModelV2 model);
        SalesforceProductObjectModelV2 GetProduct(string productId);
        IEnumerable<SalesforceProductObjectModelV2> GetProducts();
        void DeleteProducts(IEnumerable<string> productIds);
        void ClearProductsCacheCompletely();
        void SetProducts(IEnumerable<SalesforceProductObjectModelV2> products);
        void SetSalesforceEventReplayId(string channel, string replayId);
        int GetSalesforceEventReplayId(string channel);
    }
    public class CacheRepository : ICacheRepository
    {
        private IRedisClient _redisClient;
        private const string _SALESFORCE_PRODUCTS_KEY = "EB:SalesforceProducts";
        private const string _SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS = "EB:PlatformEvents:ReplayIds";

        public CacheRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public void AddProduct(SalesforceProductObjectModelV2 model)
        {
            _redisClient.HashSetField(_SALESFORCE_PRODUCTS_KEY, model.Id, model);
        }

        public void ClearProductsCacheCompletely()
        {
            _redisClient.KeyRemove(_SALESFORCE_PRODUCTS_KEY);
        }

        public void DeleteProducts(IEnumerable<string> productIds)
        {
            _redisClient.HashRemoveFields(_SALESFORCE_PRODUCTS_KEY, productIds.ToArray());
        }

        public SalesforceProductObjectModelV2 GetProduct(string productId)
        {
            return _redisClient.HashGet<SalesforceProductObjectModelV2>(_SALESFORCE_PRODUCTS_KEY, productId);
        }

        public IEnumerable<SalesforceProductObjectModelV2> GetProducts()
        {
            var dictionary = _redisClient.HashGetAll<string, SalesforceProductObjectModelV2>(_SALESFORCE_PRODUCTS_KEY);
            return dictionary?.Values;
        }

        public void SetProducts(IEnumerable<SalesforceProductObjectModelV2> products)
        {
            var dictionary = products.ToDictionary(x => x.Id, y => y);
            _redisClient.HashSet(_SALESFORCE_PRODUCTS_KEY, dictionary);
        }

        public void UpdateProduct(SalesforceProductObjectModelV2 model)
        {
            _redisClient.HashSetField(_SALESFORCE_PRODUCTS_KEY, model.Id, model);
        }

        /// <summary>
        /// Add the ReplayId for the given 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="replayId"></param>
        public void SetSalesforceEventReplayId(string channel, string replayId)
        {
            _redisClient.HashSetField(_SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS, channel, replayId);
        }
        /// <summary>
        /// Fetch cached ReplayId value from Redis cache
        /// replayId -1: (Default if no replay option is specified.) Subscriber receives new events that are broadcast after the client subscribes.
        /// replayId -2: Subscriber receives all events, including past events that are within the retention window and new events.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public int GetSalesforceEventReplayId(string channel)
        {
            // fetch the replay Id from redis
            var replayIdCached = _redisClient.HashGet<string>(_SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS, channel);
            // if not found, return default value of -1
            if (replayIdCached == null) return -1;
            // attempt to parse the value into an integer
            var isParsed = int.TryParse(replayIdCached, out int replayId);
            // return the parsed value or default value
            return isParsed ? replayId : -1;
        }
    }
}
