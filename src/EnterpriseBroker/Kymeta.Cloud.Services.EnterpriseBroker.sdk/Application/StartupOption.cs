using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;

public record StartupOption
{
    public string ServiceHealthUrl { get; init; } = null!;
    public GrapvineOption Configuration { get; init; } = null!;
}

public record GrapvineOption
{
    public string ClientId { get; init; } = null!;
    public string Secret { get; init; } = null!;
}


public static class StartupOptionExtensions
{
    public static StartupOption Verify(this StartupOption subject)
    {
        const string msg = "is required";

        subject.NotNull(message: msg);
        subject.ServiceHealthUrl.NotEmpty(message: msg);
        subject.Configuration.NotNull(message: msg);
        subject.Configuration.ClientId.NotEmpty(message: msg);
        subject.Configuration.Secret.NotEmpty(message: msg);

        return subject;
    }
}
