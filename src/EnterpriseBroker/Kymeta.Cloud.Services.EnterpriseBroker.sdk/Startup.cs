using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk;

public static class Startup
{
    public static readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    static Startup()
    {
        _retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .RetryAsync(5);
    }

    public static IServiceCollection AddEnterpriseBrokerServices(this IServiceCollection services)
    {
        services.NotNull();

        services.AddTransient<SalesforceAccessTokenHandler>();
        services.AddSingleton<MessageEventService>();
        services.AddSingleton<ReplayIdStoreService>();
        services.AddSingleton<SalesforceClient2>();

        services.AddSingleton<EventOrchestrationService>();
        services.AddSingleton<GetSalesOrderLinesActivity>();
        services.AddSingleton<SetSalesOrderWithOracleActivity>();
        services.AddSingleton<UpdateOracleSalesOrderActivity>();

        services.AddHostedService<MessageEventBackgroundService>();

        services.AddHttpClient<SalesforceAuthClient>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();
            httpClient.BaseAddress = new Uri(option.Salesforce.LoginEndpoint);
        })
        .AddPolicyHandler(_retryPolicy);

        services.AddHttpClient<ISalesforceClient2, SalesforceClient2>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();
            var authClient = services.GetRequiredService<SalesforceAuthClient>();

            var authDetails = authClient.GetAuthToken(CancellationToken.None).Result.NotNull();
            httpClient.BaseAddress = new Uri(authDetails.InstanceUrl + "/services/data/v56.0");
        })
        .AddPolicyHandler(_retryPolicy)
        .AddHttpMessageHandler<SalesforceAccessTokenHandler>();


        return services;
    }
}
