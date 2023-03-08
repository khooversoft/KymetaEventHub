using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public static class WorkflowTools
{
    public static void LogDetails(this OrchestrationContext context, string methodName, ILogger logger)
    {
        logger.LogInformation(
            "Starting orchestration for {methodName}, replaying={replaying}, InstanceId={instanceId}, ExceutionId={executionId}",
            methodName,
            context.IsReplaying,
            context.OrchestrationInstance.InstanceId,
            context.OrchestrationInstance.ExecutionId
            );
    }

    public static RetryOptions GetRetryOptions(ILogger logger)
    {
        logger.NotNull();

        var firstRetryInterval = TimeSpan.FromSeconds(1);
        var maxNumberOfAttempts = 5;
        var backoffCoefficient = 1.1;

        return new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
        {
            BackoffCoefficient = backoffCoefficient,
            Handle = ex => true.Action(_ => logger.LogError(ex, "Orchestration failed")),
        };
    }
}
