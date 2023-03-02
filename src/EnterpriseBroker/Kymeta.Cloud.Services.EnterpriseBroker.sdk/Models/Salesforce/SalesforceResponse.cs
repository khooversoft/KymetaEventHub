using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;

public record SalesforceResponse<T>
{
    public string Channel { get; init; } = null!;

    [JsonPropertyName("data")]
    public SalesforceResponsePayload<T> Data { get; init; } = default!;
}

public record SalesforceResponsePayload<T>
{
    [JsonPropertyName("payload")]
    public T Payload { get; init; } = default!;
}


