using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Configuration;

namespace Kymeta.Cloud.Services.Toolbox.Extensions;

public static class ConfigurationExtensions
{
    public static T BindToOption<T>(this IConfiguration configuration) where T : new()
    {
        configuration.NotNull();

        var option = new T();
        configuration.Bind(option, x => x.BindNonPublicProperties = true);
        return option;
    }
}