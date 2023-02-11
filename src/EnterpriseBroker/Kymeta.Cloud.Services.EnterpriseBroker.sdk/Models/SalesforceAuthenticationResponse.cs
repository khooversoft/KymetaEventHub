using System.Text.Json.Serialization;
using Kymeta.Cloud.Services.Toolbox.Extensions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;

public record SalesforceAuthenticationResponse
{
    /// <summary>
    /// This is the token we need to use to send requests to the API
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = null!;
    /// <summary>
    /// This is the base URL for the API
    /// </summary>
    [JsonPropertyName("instance_url")]
    public string InstanceUrl { get; init; } = null!;
    /// <summary>
    /// This is when the token was issued
    /// </summary>
    [JsonPropertyName("issued_at")]
    public string? IssuedAt { get; init; } = null!;
}


public static class SalesforceAuthenticationResponseExtensions
{
    public static bool IsValid(this SalesforceAuthenticationResponse subject) =>
        subject is not null &&
        subject.AccessToken.IsNotEmpty() &&
        subject.InstanceUrl.IsNotEmpty();
}