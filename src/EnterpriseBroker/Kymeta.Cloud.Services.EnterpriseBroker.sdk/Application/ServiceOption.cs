using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;

public record ServiceOption
{
    public ConnectionString ConnectionStrings { get; init; } = null!;
    public SalesforceOption Salesforce { get; init; } = null!;
    public bool UseDurableTaskEmulator { get; init; } = false;
}

public record ConnectionString
{
    public string AzureCosmosDB { get; init; } = null!;
    public string RedisCache { get; init; } = null!;
    public string ActivityQueue { get; init; } = null!;
    public string DurableTask { get; init; } = null!;
}

public record SalesforceOption
{
    public string BasePath { get; init; } = null!;
    public ConnectedAppOption ConnectedApp { get; init; } = null!;
    public string Username { get; init; } = "defaultusername";
    public string Password { get; init; } = "defaultpassword";
    public string LoginEndpoint { get; init; } = null!;
    public PlatformEventsOption PlatformEvents { get; init; } = null!;
}

public record ConnectedAppOption
{
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
}

public record PlatformEventsOption
{
    public ChannelsOption Channels { get; init; } = null!;
}

public record ChannelsOption
{
    public string Asset { get; init; } = null!;
    public string NeoApproveOrder { get; init; } = null!;
}


public static class ServiceOptionExtensions
{
    public static ServiceOption Verify(this ServiceOption subject)
    {
        const string msg = "is required";

        subject.NotNull(message: msg);

        subject.ConnectionStrings.NotNull(message: msg);
        subject.ConnectionStrings.AzureCosmosDB.NotEmpty(message: msg);
        subject.ConnectionStrings.RedisCache.NotEmpty(message: msg);
        subject.ConnectionStrings.ActivityQueue.NotEmpty(message: msg);
        if (!subject.UseDurableTaskEmulator) subject.ConnectionStrings.DurableTask.NotEmpty(message: msg);

        subject.Salesforce.NotNull(message: msg);
        subject.Salesforce.BasePath.NotNull(message: msg);
        subject.Salesforce.ConnectedApp.NotNull(message: msg);
        subject.Salesforce.ConnectedApp.ClientId.NotEmpty(message: msg);
        subject.Salesforce.ConnectedApp.ClientSecret.NotEmpty(message: msg);

        subject.Salesforce.Username.NotEmpty(message: msg);
        subject.Salesforce.Password.NotEmpty(message: msg);
        subject.Salesforce.LoginEndpoint.NotEmpty(message: msg);

        subject.Salesforce.PlatformEvents.NotNull(message: msg);
        subject.Salesforce.PlatformEvents.Channels.NotNull(message: msg);
        subject.Salesforce.PlatformEvents.Channels.Asset.NotEmpty(message: msg);
        subject.Salesforce.PlatformEvents.Channels.NeoApproveOrder.NotEmpty(message: msg);

        return subject;
    }
}