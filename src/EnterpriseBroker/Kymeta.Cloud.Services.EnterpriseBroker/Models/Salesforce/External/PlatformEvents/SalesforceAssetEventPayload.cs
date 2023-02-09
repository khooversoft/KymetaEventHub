using CometD.NetCore.Salesforce.Messaging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents
{
    public class SalesforceAssetEventPayload : MessagePayload
    {
        [JsonProperty("Name__c")]
        public string Name { get; set; }
    }
}
