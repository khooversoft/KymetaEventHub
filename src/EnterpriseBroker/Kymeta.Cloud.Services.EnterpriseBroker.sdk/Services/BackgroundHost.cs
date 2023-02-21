using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public interface IBackgroundHost
{
    Task StartAsync(CancellationToken token);
    Task StopAsync(CancellationToken token);
}

public class BackgroundHost<T> : BackgroundService where T : IBackgroundHost
{
    private string _serviceName;
    private readonly ILogger _logger;
    private readonly T _backgroundService;

    public BackgroundHost(T backgroundService, IServiceProvider service)
    {
        _serviceName = typeof(BackgroundHost<T>).GetTypeName();
        ILoggerFactory loggerFactory = service.GetRequiredService<ILoggerFactory>();

        _backgroundService = backgroundService.NotNull();
        _logger = loggerFactory.NotNull().CreateLogger(_serviceName);
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        _logger.LogInformation("Starting {serviceName} service", _serviceName);

        try
        {
            await _backgroundService.StartAsync(token);
        }
        catch (Exception ex) when (token.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "Service {serviceName} execution has been cancelled.", _serviceName);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Service {serviceName} has aborted", _serviceName);
        }

        _logger.LogInformation("Started {serviceName} service", _serviceName);
    }

    public override async Task StopAsync(CancellationToken token)
    {
        _logger.LogInformation("Stopping {serviceName} service", _serviceName);

        await _backgroundService.StopAsync(token);
        await base.StopAsync(token);
    }
}
