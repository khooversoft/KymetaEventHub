using System.Linq;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Application;

internal static class TestApplication
{
    private static bool _initialized = false;
    private static WebApplicationFactory<Program> _host = null!;
    private static object _lock = new object();

    public static void StartHost()
    {
        lock (_lock)
        {
            if (_initialized) return;

            ILogger logger = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
                builder.AddFilter(x => true);
            }).CreateLogger<Program>();

            _host = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Development");
                    builder.UseSetting("UseDurableTaskEmulator", "true");

                    builder.ConfigureServices(services =>
                    {
                        ConfigureModelBindingExceptionHandling(services, logger);
                    });

                    builder.ConfigureTestServices(services =>
                    {
                        services.Remove<SalesforceBackgroundOperationService>();
                        services.Remove<ISalesforceProcessingService>();
                        services.Remove<OracleBackgroundOperationService>();
                        services.Remove<IOracleProcessingService>();
                        services.Remove<SalesforcePlatformEventsBackgroundOperationService>();
                        services.Remove<SalesforcePlatformEventsProcessingService>();

                        services.Remove<BackgroundHost<MessageListenerService>>();
                    });
                });

            _initialized = true;
        }
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        StartHost();
        return _host.Services.GetRequiredService<T>();
    }

    private static void ConfigureModelBindingExceptionHandling(IServiceCollection services, ILogger logger)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                ValidationProblemDetails? error = actionContext.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => new ValidationProblemDetails(actionContext.ModelState))
                    .FirstOrDefault();

                logger.LogError("ApiBehaviorOption error");

                // Here you can add logging to you log file or to your Application Insights.
                // For example, using Serilog:
                // Log.Error($"{{@RequestPath}} received invalid message format: {{@Exception}}", 
                //   actionContext.HttpContext.Request.Path.Value, 
                //   error.Errors.Values);
                return new BadRequestObjectResult(error);
            };
        });
    }
}