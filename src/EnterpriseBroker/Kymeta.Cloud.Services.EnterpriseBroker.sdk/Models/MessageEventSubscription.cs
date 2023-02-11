using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;

public record MessageEventSubscription
{
    public string Channel { get; init; } = null!;
    public Action<MessageEventContent> Forward { get; init; } = null!;
    public long? ReplayId { get; init; }
}
