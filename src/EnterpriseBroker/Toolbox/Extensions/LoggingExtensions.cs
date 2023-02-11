using System.Diagnostics;
using System.Runtime.CompilerServices;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.Toolbox.Extensions;

public static class LoggingExtensions
{
    public static IDisposable LogEntryExit(
    this ILogger logger,
    string? message = null,
    [CallerMemberName] string function = "",
    [CallerFilePath] string path = "",
    [CallerLineNumber] int lineNumber = 0
    )
    {
        logger
            .NotNull()
            .LogTrace("Enter: Message={message}, Method={method}, path={path}, line={lineNumber}", message ?? "<no message>", function, path, lineNumber);

        var sw = Stopwatch.StartNew();

        return new FinalizeScope<ILogger>(logger, x =>
            x.LogTrace("Exit: Message={message}, ms={ms} Method={method}, path={path}, line={lineNumber}", message ?? "<no message>", sw.ElapsedMilliseconds, function, path, lineNumber)
            );
    }
}
