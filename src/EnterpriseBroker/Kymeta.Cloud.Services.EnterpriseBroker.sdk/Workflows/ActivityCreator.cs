using DurableTask.Core;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class ActivityCreator<T> : ObjectCreator<T> where T : notnull, TaskActivity
{
    private readonly Type _activityType;
    private readonly IServiceProvider _serviceProvider;

    public ActivityCreator(Type activityType, IServiceProvider serviceProvider)
    {
        _activityType = activityType.NotNull();
        _serviceProvider = serviceProvider.NotNull();

        Name = NameVersionHelper.GetDefaultName(_activityType);
        Version = NameVersionHelper.GetDefaultVersion(_activityType);
    }

    public override T Create()
    {
        return (T)_serviceProvider.GetRequiredService(_activityType);
    }
}
