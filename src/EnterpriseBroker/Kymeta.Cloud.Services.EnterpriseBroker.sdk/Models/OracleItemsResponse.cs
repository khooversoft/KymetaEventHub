using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;

public record OracleItemsResponse<T>
{
    [JsonPropertyName("items")]
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int? Count { get; init; }
    public bool? HasMore { get; init; }
    public int? Offset { get; init; }
}
