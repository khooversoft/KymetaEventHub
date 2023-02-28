using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;

public record SalesforceSearchResult<T>
{
    [JsonPropertyName("totalSize")]
    public int TotalSize { get; init; }

    [JsonPropertyName("done")]
    public string Done { get; init; } = null!;

    [JsonPropertyName("records")]
    public IReadOnlyList<T> Records { get; init; } = Array.Empty<T>();
}
