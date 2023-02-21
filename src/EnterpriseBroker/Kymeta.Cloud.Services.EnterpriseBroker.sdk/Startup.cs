using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        services.AddSingleton<ReplayIdStoreService>();
        services.AddSingleton<SalesforceClient2>();

        services.AddSingleton<SalesOrderOrchestration>();
        services.AddSingleton<GetSalesOrderLinesActivity>();
        services.AddSingleton<SetSalesOrderWithOracleActivity>();
        services.AddSingleton<UpdateOracleSalesOrderActivity>();

        //services.AddSingleton<MessageListenerService>();
        //services.AddSingleton<OrchestrationService>();

        //services.AddHostedService<BackgroundHost<MessageListenerService>>();
        //services.AddHostedService<BackgroundHost<OrchestrationService>>();

        //services.AddMessageListenerService((service, builder) =>
        //{
        //    ServiceOption option = service.GetRequiredService<ServiceOption>();

        //    //builder.AddChannel(option.Salesforce.PlatformEvents.Channels.Asset);
        //    builder.AddChannel(option.Salesforce.PlatformEvents.Channels.NeoApproveOrder);
        //});

        services.AddOrchestrationServices(builder =>
        {
            builder.AddTaskOrchestrations<SalesOrderOrchestration>();
            builder.AddTaskActivities<GetSalesOrderLinesActivity>();
            builder.AddTaskActivities<SetSalesOrderWithOracleActivity>();
            builder.AddTaskActivities<UpdateOracleSalesOrderActivity>();

            builder.MapChannel((services, map) =>
            {
                ServiceOption option = services.GetRequiredService<ServiceOption>();

                map.Map<SalesOrderOrchestration>(option.Salesforce.PlatformEvents.Channels.NeoApproveOrder);
            });
        });


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

            SalesforceAuthenticationResponse authDetails = authClient.GetAuthToken(CancellationToken.None).Result.NotNull();
            httpClient.BaseAddress = new Uri(authDetails.InstanceUrl + option.Salesforce.BasePath);
        })
        .AddPolicyHandler(_retryPolicy)
        .AddHttpMessageHandler<SalesforceAccessTokenHandler>();


        return services;
    }
}
