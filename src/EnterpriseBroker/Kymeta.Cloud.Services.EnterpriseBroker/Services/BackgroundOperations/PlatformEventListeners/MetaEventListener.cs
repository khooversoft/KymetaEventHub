using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners
{
    class MetaEventListener : IMessageListener
    {
        /// <summary>
        /// Listen for messages from Salesforce Platform Events for Meta events (errors)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            // fetch the JSON
            var convertedJson = message.Json;
            Console.WriteLine(convertedJson);
            // TODO: if message indicates auth failed, renew the token and refresh/store in redis?
        }
    }
}
