using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public static class WorkflowExtensions
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
}
