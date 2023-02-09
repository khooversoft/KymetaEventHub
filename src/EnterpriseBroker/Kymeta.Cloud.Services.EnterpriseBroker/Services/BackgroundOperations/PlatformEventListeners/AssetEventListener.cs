using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;
using Newtonsoft.Json;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;
using CometD.NetCore.Salesforce.Messaging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners
{
    public class AssetEventListener : IMessageListener
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AssetEventListener> _logger;
        private readonly ICacheRepository _cacheRepo;

        public AssetEventListener(IConfiguration config, ILogger<AssetEventListener> logger, ICacheRepository cacheRepo)
        {
            _config = config;
            _logger = logger;
            _cacheRepo = cacheRepo;
        }

        /// <summary>
        /// Listen for messages from Salesforce Platform Events for Assets
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            // fetch the JSON
            var convertedJson = message.Json;
            if (convertedJson == null)
            {
                _logger.LogCritical($"[PLATFORM_EVENTS] Message content not available.");
                return;
            }
            // deserialize JSON into C# model
            var assetEvent = JsonConvert.DeserializeObject<MessageEnvelope<SalesforceAssetEventPayload>>(convertedJson);
            if (assetEvent == null || assetEvent.Data?.Event == null)
            {
                _logger.LogCritical($"[PLATFORM_EVENTS] Unable to deserialize message payload: Asset event not recognized.");
                return;
            }
            // assign replayId to redis cache to establish replay starting point in event of service failure
            _cacheRepo.SetSalesforceEventReplayId(_config["Salesforce:PlatformEvents:Channels:Asset"], assetEvent.Data.Event.ReplayId.ToString());
            // TODO: take action with message data
            _logger.LogInformation($"Message received ({assetEvent.Data.Payload.CreatedDate}) - Name: {assetEvent?.Data.Payload.Name}");
        }
    }
}
