using System.Net;
using System.Net.Http.Headers;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Handlers;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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

        services.AddSingleton<ReplayIdStoreService>();


        //  =======================================================================================
        //  Integration orchestration for sales force platform events
        //
        //      Register orchestrationa and all required activities
        //      Map plantfor event to orchestration

        services.AddOrchestrationServices(builder =>
        {
            //builder.AddTaskOrchestrations<SalesOrderOrchestration>();
            //builder.AddTaskActivities<Step2_GetSalesOrderLinesActivity>();
            //builder.AddTaskActivities<Step4_UpdateSalesforceSalesOrderActivity>();
            //builder.AddTaskActivities<Step3_SetOracleSalesOrderActivity>(); 
            
            builder.AddTaskOrchestrations<InvoiceCreateOrchestration>();
            builder.AddTaskActivities<H1_CreateHardwareInvoiceActivity>();
            builder.AddTaskActivities<H2_ScanOracleAndUpdateInvoiceActivity>();
            builder.AddTaskActivities<Step1_GetInvoiceLineItemsActivity>();
            builder.AddTaskActivities<Step2_CreateOtherInvoiceActivity>();

            builder.AddTaskOrchestrations<TestOrchestration>();
            builder.AddTaskActivities<Step2_TestActivity>();
            builder.AddTaskActivities<Step3_TestActivity>();
            builder.AddTaskActivities<Step4_TestActivity>();

            builder.MapChannel((services, map) =>
            {
                ServiceOption option = services.GetRequiredService<ServiceOption>();

                //map.Map<SalesOrderOrchestration>(option.Salesforce.PlatformEvents.Channels.NeoApproveOrder);
                map.Map<InvoiceCreateOrchestration>(option.Salesforce.PlatformEvents.Channels.NeoInvoicePosted);
                map.Map<TestOrchestration>("testChannel");
            });
        });


        //  =======================================================================================
        //  Transaction logging

        services.AddSingleton<TransactionLoggingFileProvider>(service =>
        {
            var option = service.GetRequiredService<ServiceOption>();
            return new TransactionLoggingFileProvider(option.TransactionLogging.LoggingFolder, option.TransactionLogging.BaseLogFileName);
        });

        services.AddTransactionLogging((service, transLog) =>
        {
            var option = service.GetRequiredService<ServiceOption>();

            if (option.TransactionLogging?.Enabled == true)
            {
                transLog.AddProvider(service.GetRequiredService<TransactionLoggingFileProvider>());
            }
        });


        //  =======================================================================================
        //  HTTP Clients, policy, and message handlers

        services.AddTransient<SalesforceAccessTokenHandler>();
        services.AddTransient<TransactionLoggerHandler>();

        services.AddHttpClient<SalesforceAuthClient>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();
            httpClient.BaseAddress = new Uri(option.Salesforce.LoginEndpoint);
        })
        .AddPolicyHandler(_retryPolicy);

        services.AddHttpClient<OracleClient>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();

            string basicAuth = $"{option.Oracle.Username}:{option.Oracle.Password}"
                .StringToBytes()
                .Func(Convert.ToBase64String);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            string basePath = option.Oracle.Endpoint
                .Trim()
                .Func(x => x + option.Oracle.BasePath);

            httpClient.BaseAddress = new Uri(basePath);
        })
        .AddPolicyHandler(_retryPolicy)
        .AddHttpMessageHandler<TransactionLoggerHandler>();

        services.AddHttpClient<SalesforceClient2>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();
            var authClient = services.GetRequiredService<SalesforceAuthClient>();

            SalesforceAuthenticationResponse authDetails = authClient.GetAuthToken(CancellationToken.None).Result.NotNull();

            string basePath = option.Salesforce.BasePath
                .Trim()
                .Func(x => authDetails.InstanceUrl + x + (x.EndsWith("/") ? string.Empty : "/"));

            httpClient.BaseAddress = new Uri(basePath);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authDetails.AccessToken}");
        })
        .AddPolicyHandler(_retryPolicy)
        .AddHttpMessageHandler<SalesforceAccessTokenHandler>()
        .AddHttpMessageHandler<TransactionLoggerHandler>();

        return services;
    }
}
