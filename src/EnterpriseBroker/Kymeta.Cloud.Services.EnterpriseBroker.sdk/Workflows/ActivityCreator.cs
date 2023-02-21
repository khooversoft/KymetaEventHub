using System;
using DurableTask.Core;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class ActivityCreator<T> : ObjectCreator<T> where T : notnull
{
    private readonly Type _activityType;
    private readonly IServiceProvider _serviceProvider;

    public ActivityCreator(Type activityType, IServiceProvider serviceProvider)
    {
        _activityType = activityType.NotNull();
        _serviceProvider = serviceProvider.NotNull();

        _activityType.IsAssignableTo(typeof(T)).Assert(message: $"activityType is not of type {typeof(T).FullName}");

        Name = NameVersionHelper.GetDefaultName(_activityType);
        Version = NameVersionHelper.GetDefaultVersion(_activityType);
    }

    public override T Create()
    {
        return (T)_serviceProvider.GetRequiredService(_activityType);
    }
}


public static class ActivityCreatorExtensions
{
    public static TaskHubWorker AddTaskOrchestrations(this TaskHubWorker worker, Type orchestrationType, IServiceProvider serviceProvider)
    {
        orchestrationType.IsAssignableTo(typeof(TaskOrchestration)).Assert(message: $"orchestrationType is not of type {typeof(TaskOrchestration).FullName}");

        worker.AddTaskOrchestrations(new ActivityCreator<TaskOrchestration>(orchestrationType, serviceProvider));
        return worker;
    }

    public static ObjectCreator<TaskOrchestration> ToOrchestrationObjectCreator(this Type orchestrationType, IServiceProvider serviceProvider) =>
        new ActivityCreator<TaskOrchestration>(orchestrationType, serviceProvider);

    public static ObjectCreator<TaskActivity> ToActivitiesObjectCreator(this Type activityType, IServiceProvider serviceProvider) =>
        new ActivityCreator<TaskActivity>(activityType, serviceProvider);

    public static TaskHubWorker AddTaskActivities(this TaskHubWorker worker, Type activityType, IServiceProvider serviceProvider)
    {        
        activityType.IsAssignableTo(typeof(TaskActivity)).Assert(message: $"activityType is not of type {typeof(TaskActivity).FullName}");

        worker.AddTaskActivities(new ActivityCreator<TaskActivity>(activityType, serviceProvider));
        return worker;
    }
}