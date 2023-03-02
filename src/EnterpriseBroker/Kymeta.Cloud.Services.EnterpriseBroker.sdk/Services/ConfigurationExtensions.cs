using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddOrchestrationServices(this IServiceCollection serviceCollection, Action<OrchestrationConfigurationBuilder> config)
    {
        var builder = new OrchestrationConfigurationBuilder();
        config(builder);

        //serviceCollection.AddSingleton<ITransactionLoggingService, TransactionLoggingService>();
        serviceCollection.AddSingleton<IMessageRouter, MessageRouter>();

        serviceCollection.AddTransient<MessageChannelListener>();
        serviceCollection.AddSingleton<MessageListenerService>();
        serviceCollection.AddSingleton<OrchestrationService>();

        serviceCollection.AddHostedService<BackgroundHost<OrchestrationService>>();
        serviceCollection.AddHostedService<BackgroundHost<MessageListenerService>>();

        builder.TaskOrchestrations.ForEach(x => serviceCollection.AddSingleton(x));
        builder.TaskActivities.ForEach(x => serviceCollection.AddSingleton(x));

        serviceCollection.AddSingleton<OrchestrationConfiguration>(service =>
        {
            var channelBuilder = new MapChannelBuilder();

            if (builder.MapBuilder != null)
            {
                builder.MapBuilder(service, channelBuilder);
            }

            return new OrchestrationConfiguration
            {
                TaskOrchestrations = builder.TaskOrchestrations,
                TaskActivities = builder.TaskActivities,
                ChannelMapToOrchestrations = channelBuilder.ChannelMap,
            };
        });

        return serviceCollection;
    }

    public static IServiceCollection AddTransactionLogging(this IServiceCollection serviceCollection, Action<IServiceProvider, ITransactionLoggingService> config)
    {
        serviceCollection.AddSingleton<TransactionLoggerBuffer>();

        serviceCollection.AddSingleton<ITransactionLoggingService, TransactionLoggingService>(service =>
        {
            var logService = new TransactionLoggingService();
            logService.AddProvider(service.GetRequiredService<TransactionLoggerBuffer>());

            config(service, logService);
            return logService;
        });

        return serviceCollection;
    }

    public class OrchestrationConfigurationBuilder
    {
        public List<Type> TaskOrchestrations { get; } = new List<Type>();
        public List<Type> TaskActivities { get; } = new List<Type>();
        public Action<IServiceProvider, MapChannelBuilder>? MapBuilder { get; set; }
        public OrchestrationConfigurationBuilder AddTaskOrchestrations<T>() where T : TaskOrchestration => this.Action(x => x.TaskOrchestrations.Add(typeof(T)));
        public OrchestrationConfigurationBuilder AddTaskActivities<T>() where T : TaskActivity => this.Action(x => x.TaskActivities.Add(typeof(T)));
        public OrchestrationConfigurationBuilder MapChannel(Action<IServiceProvider, MapChannelBuilder> builder) => this.Action(x => x.MapBuilder = builder);
    }

    public class MapChannelBuilder
    {
        public IDictionary<string, Type> ChannelMap { get; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        public MapChannelBuilder Map<T>(string channel) => this.Action(x => ChannelMap[channel] = typeof(T));
    }
}

