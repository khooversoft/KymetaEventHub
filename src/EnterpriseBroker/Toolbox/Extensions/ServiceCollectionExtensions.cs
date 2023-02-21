using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Kymeta.Cloud.Services.Toolbox.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<T>(this IServiceCollection services)
    {
        services.TryRemove<T>().Assert(message: $"Cannot remove service type={typeof(T).FullName}");
        return services;
    }

    public static bool TryRemove<T>(this IServiceCollection services)
    {
        if (services.IsReadOnly)
        {
            throw new ReadOnlyException($"{nameof(services)} is read only");
        }

        var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
            return true;
        }

        serviceDescriptor = services.FirstOrDefault(description => description.ImplementationType == typeof(T));
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
            return true;
        }

        return false;
    }
}

