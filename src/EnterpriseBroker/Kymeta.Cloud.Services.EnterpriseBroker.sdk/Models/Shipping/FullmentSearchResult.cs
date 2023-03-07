using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Shipping;

public record FullmentSearchResult
{
    [JsonPropertyName("ID")]
    public string Id { get; init; } = null!;
}
