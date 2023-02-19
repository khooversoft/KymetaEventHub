using DurableTask.AzureStorage;
using DurableTask.Core;
using DurableTask.Emulator;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class EventOrchestrationService
{
    private readonly ServiceOption _option;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventOrchestrationService> _logger;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 1);
    private readonly Dictionary<string, Type> _channelLookup;
    private IOrchestrationService? _orchestrationService;
    private IOrchestrationServiceClient? _orchestrationServiceClient;
    private TaskHubClient _taskHubClient;

    public EventOrchestrationService(ServiceOption option, IServiceProvider serviceProvider, ILogger<EventOrchestrationService> logger)
    {
        _option = option.NotNull();
        _serviceProvider = serviceProvider.NotNull();
        _logger = logger.NotNull();

        _channelLookup = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            [_option.Salesforce.PlatformEvents.Channels.NeoApproveOrder] = typeof(SalesOrderOrchestration),
        };
    }

    public async Task<bool> Start()
    {
        await _semaphore.WaitAsync();

        try
        {
            if (_orchestrationService != null) return true;

            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageConnectionString = _option.ConnectionStrings.DurableTask,
            };

            object obj = _option.UseDurableTaskEmulator switch
            {
                true => new LocalOrchestrationService(),
                false => new AzureStorageOrchestrationService(settings),
            };
            _orchestrationService = (IOrchestrationService)obj;
            _orchestrationServiceClient = (IOrchestrationServiceClient)obj;

            TaskHubWorker hubWorker = new TaskHubWorker(_orchestrationService);

            hubWorker.RegisterSalesOrderActivities(_serviceProvider);

            await _orchestrationService.CreateIfNotExistsAsync();
            await hubWorker.StartAsync();

            _taskHubClient = new TaskHubClient(_orchestrationServiceClient);

            //string instanceId = Guid.NewGuid().ToString();
            //OrchestrationInstance instance = taskHubClient.CreateOrchestrationInstanceAsync(typeof(SalesOrderOrchestration), instanceId, "start orchestration").Result;

            //OrchestrationState result = await _taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromSeconds(60));

            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task Stop()
    {
        IOrchestrationService? current = Interlocked.Exchange(ref _orchestrationService, null);
        if (current != null) await current.StopAsync();
    }

    public async Task RunOrchestration(string channel, string data)
    {
        if (!_channelLookup.TryGetValue(channel, out Type? orchestrationType))
        {
            _logger.LogCritical("Channel={channel} is not registered", channel);
            return;
        }

        string instanceId = Guid.NewGuid().ToString();
        OrchestrationInstance instance = await _taskHubClient.CreateOrchestrationInstanceAsync(orchestrationType, instanceId, data);
    }
}

