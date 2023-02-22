using DurableTask.AzureStorage;
using DurableTask.Core;
using DurableTask.Emulator;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class OrchestrationService : IBackgroundHost
{
    private readonly ServiceOption _option;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrchestrationService> _logger;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 1);
    private readonly OrchestrationConfiguration _config;

    private TaskHubClient? _taskHubClient;
    private IOrchestrationService? _orchestrationService;

    public OrchestrationService(ServiceOption option, IServiceProvider serviceProvider, OrchestrationConfiguration config, ILogger<OrchestrationService> logger)
    {
        _option = option.NotNull();
        _serviceProvider = serviceProvider.NotNull();
        _config = config.NotNull();
        _logger = logger.NotNull();
    }

    public async Task StartAsync(CancellationToken token)
    {
        await _semaphore.WaitAsync();

        try
        {
            if (_orchestrationService != null) return;

            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageConnectionString = _option.ConnectionStrings.DurableTask,
            };

            _orchestrationService = _option.UseDurableTaskEmulator switch
            {
                true => new LocalOrchestrationService(),
                false => new AzureStorageOrchestrationService(settings),
            };

            TaskHubWorker hubWorker = new TaskHubWorker(_orchestrationService)
                .AddTaskOrchestrations(_config.TaskOrchestrations.Select(x => x.ToOrchestrationObjectCreator(_serviceProvider)).ToArray())
                .AddTaskActivities(_config.TaskActivities.Select(x => x.ToActivitiesObjectCreator(_serviceProvider)).ToArray());

            await _orchestrationService.CreateIfNotExistsAsync();
            await hubWorker.StartAsync();

            _taskHubClient = new TaskHubClient((IOrchestrationServiceClient)_orchestrationService);

            return;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        IOrchestrationService? current = Interlocked.Exchange(ref _orchestrationService, null);
        if (current != null) await current.StopAsync();
    }

    public async Task<(bool success, string? instanceId)> RunOrchestration(MessageEventContent messageEvent)
    {
        messageEvent.NotNull();
        _taskHubClient.NotNull(message: "Orchestration not started");

        if (!_config.ChannelMapToOrchestrations.TryGetValue(messageEvent.Channel, out Type? orchestrationType))
        {
            _logger.LogCritical("Channel={channel} is not registered", messageEvent.Channel);
            return (false, null);
        }

        string instanceId = Guid.NewGuid().ToString();

        try
        {
            OrchestrationInstance instance = await _taskHubClient.CreateOrchestrationInstanceAsync(orchestrationType, instanceId, messageEvent.Json);
            _logger.LogInformation("Orchestration started - instanceId={instanceId}, ExecutionId={executionId}", instanceId, instance.ExecutionId);

            OrchestrationState result = await _taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromSeconds(60));
            _logger.LogInformation("Orchestration completed - instanceId={instanceId}, OrchestrationStatus={orchestrationStatus}", instanceId, result.OrchestrationStatus);

            return (true, instanceId);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Orchestration timed out for {messageEvent}", messageEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Orchestration failed for {messageEvent}", messageEvent);
        }

        return (false, instanceId);
    }
}

