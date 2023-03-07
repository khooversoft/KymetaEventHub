using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceAuthenticationResponse
    {
        /// <summary>
        /// This is the token we need to use to send requests to the API
        /// </summary>
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        /// <summary>
        /// This is the base URL for the API
        /// </summary>
        [JsonProperty("instance_url")]
        public string? InstanceUrl { get; set; }
        /// <summary>
        /// This is when the token was issued
        /// </summary>
        [JsonProperty("issued_at")]
        public string? IssuedAt { get; set; }
    }
}
