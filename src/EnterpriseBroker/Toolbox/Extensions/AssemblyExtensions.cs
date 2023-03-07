using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.Toolbox.Extensions;

public static class AssemblyExtensions
{
    public static string ReadAssemblyResource(this Assembly? subject, string resourcePath)
    {
        using var stream = subject
            .GetManifestResourceStream(resourcePath)
            .NotNull();

        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static T ReadAssemblyResource<T>(this Assembly? subject, string resourcePath)
    {
        subject.NotNull();

        using var stream = subject
            .GetManifestResourceStream(resourcePath)
            .NotNull();

        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd().ToObject<T>().NotNull(message: "Deserialzation failed");
    }
}
