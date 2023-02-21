//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DurableTask.AzureStorage;
//using DurableTask.Core;
//using DurableTask.Emulator;
//using Microsoft.Extensions.DependencyInjection;
//using System.Xml.Linq;
//using System.Diagnostics;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Kymeta.Cloud.Services.Toolbox.Tools;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

//public class MessageOrchestrationService : IBackgroundHost
//{
//    private readonly ILogger<MessageOrchestrationService> _logger;
//    private readonly IServiceProvider _serviceProvider;

//    public MessageOrchestrationService(IServiceProvider serviceProvider, ILogger<MessageOrchestrationService> logger)
//    {
//        _serviceProvider = serviceProvider.NotNull();
//        _logger = logger.NotNull();
//    }

//    public async Task StartAsync(CancellationToken token)
//    {
//        var service = new LocalOrchestrationService();

//        TaskHubWorker hubWorker = new TaskHubWorker(service);

//        hubWorker.AddTaskOrchestrations(new ActivityCreator<TaskOrchestration>(typeof(SalesOrderOrchestration), _serviceProvider));

//        hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(GetSalesOrderLinesActivity), _serviceProvider));
//        hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(SetSalesOrderWithOracleActivity), _serviceProvider));
//        hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(UpdateOracleSalesOrderActivity), _serviceProvider));

//        await service.CreateIfNotExistsAsync();
//        await hubWorker.StartAsync();

//        var taskHubClient = new TaskHubClient(service);

//        string instanceId = Guid.NewGuid().ToString();
//        OrchestrationInstance instance = taskHubClient.CreateOrchestrationInstanceAsync(typeof(SalesOrderOrchestration), instanceId, "start orchestration").Result;

//        OrchestrationState result = await taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromSeconds(60));
//    }

//    public Task StopAsync(CancellationToken token)
//    {
//        throw new NotImplementedException();
//    }

//    //public class TestActivity1 : BaseActivity<string, int> { public TestActivity1() : base("Task #1", 1) { } }
//    //public class TestActivity2 : BaseActivity<string, int> { public TestActivity2() : base("Task #2", 20) { } }
//    //public class TestActivity3 : BaseActivity<string, int> { public TestActivity3() : base("Task #3", 100) { } }
//    //public class TestActivity4 : BaseActivity<string, int> { public TestActivity4() : base("Task #4", 200) { } }



//    //public class TestOrchestration : TaskOrchestration<bool, string>
//    //{
//    //    public override async Task<bool> RunTask(OrchestrationContext context, string input)
//    //    {
//    //        string prefix = context.IsReplaying ? "**" : string.Empty;

//    //        Debug.WriteLine($"{prefix} Is Replaying =" + context.IsReplaying + " InstanceId =" + context.OrchestrationInstance.InstanceId + " Execution ID =" + context.OrchestrationInstance.ExecutionId);
//    //        Debug.WriteLine($"{prefix} Running Orchestration, Input={input}");
//    //        var firstRetryInterval = TimeSpan.FromSeconds(1);
//    //        var maxNumberOfAttempts = 5;
//    //        var backoffCoefficient = 1.1;

//    //        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
//    //        {
//    //            BackoffCoefficient = backoffCoefficient,
//    //            Handle = HandleError
//    //        };
//    //        bool result = false;
//    //        try
//    //        {
//    //            Debug.WriteLine($"{prefix} Schedule with Retry ");

//    //            var task1 = context.ScheduleWithRetry<int>(typeof(TestActivity1), options, "Test Input1");
//    //            Debug.WriteLine($"{prefix} Schedule with Retry Complete - Task 1");

//    //            var task2 = context.ScheduleWithRetry<int>(typeof(TestActivity2), options, "Test Input2");
//    //            Debug.WriteLine($"{prefix} Schedule with Retry Complete - Task 2");

//    //            int[] results = await Task.WhenAll(task1, task2);

//    //            if (Enumerable.SequenceEqual(results, new int[] { 1, 20 }) == false) throw new ArgumentException("failed response");

//    //            int subTaskResult = await context.CreateSubOrchestrationInstanceWithRetry<int>(typeof(SubTestOrchestration), options, results.Sum());
//    //            Debug.WriteLine($"{prefix} Sub Orchestration's result={subTaskResult}");
//    //        }
//    //        catch (Exception e)
//    //        {
//    //            Debug.WriteLine($"{prefix} Exception in Orchestration" + e.ToString());
//    //        }

//    //        Debug.WriteLine($"{prefix} Orchestration Finished");
//    //        return result;
//    //    }

//    //    private bool HandleError(Exception e)
//    //    {
//    //        Debug.WriteLine(e.ToString());
//    //        return true;
//    //    }
//    //}

//    //public class SubTestOrchestration : TaskOrchestration<int, int>
//    //{
//    //    public async override Task<int> RunTask(OrchestrationContext context, int input)
//    //    {
//    //        string prefix = context.IsReplaying ? "**" : string.Empty;

//    //        Debug.WriteLine($"{prefix} Sub - Orchestration - Is Replaying =" + context.IsReplaying + " InstanceId =" + context.OrchestrationInstance.InstanceId + " Execution ID =" + context.OrchestrationInstance.ExecutionId);
//    //        Debug.WriteLine($"{prefix} Sub - Running Orchestration, Input={input}");

//    //        var firstRetryInterval = TimeSpan.FromSeconds(1);
//    //        var maxNumberOfAttempts = 5;
//    //        var backoffCoefficient = 1.1;

//    //        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
//    //        {
//    //            BackoffCoefficient = backoffCoefficient,
//    //            Handle = HandleError
//    //        };

//    //        try
//    //        {
//    //            Debug.WriteLine($"{prefix} Schedule with Retry ");

//    //            var task1 = context.ScheduleWithRetry<int>(typeof(TestActivity3), options, "Test Input3");
//    //            Debug.WriteLine($"{prefix} Schedule with Retry Complete - Task 3");

//    //            var task2 = context.ScheduleWithRetry<int>(typeof(TestActivity4), options, "Test Input4");
//    //            Debug.WriteLine($"{prefix} Schedule with Retry Complete - Task 4");

//    //            int[] results = await Task.WhenAll(task1, task2);

//    //            if (Enumerable.SequenceEqual(results, new int[] { 100, 200 }) == false) throw new ArgumentException("failed response");

//    //            Debug.WriteLine($"{prefix} Sub - Orchestration Finished, sum={results.Sum()}");
//    //            return results.Append(input).Sum();
//    //        }
//    //        catch (Exception e)
//    //        {
//    //            Debug.WriteLine($"{prefix} sub exception in Orchestration" + e.ToString());
//    //            return -1;
//    //        }
//    //    }
//    //    private bool HandleError(Exception e)
//    //    {
//    //        Debug.WriteLine(e.ToString());
//    //        return true;
//    //    }
//    //}

//    //public class ActivityCreator<T> : ObjectCreator<T> where T : notnull, TaskActivity
//    //{
//    //    private readonly Type _activityType;
//    //    private readonly IServiceProvider _serviceProvider;

//    //    public ActivityCreator(Type activityType, IServiceProvider serviceProvider)
//    //    {
//    //        _activityType = activityType;
//    //        _serviceProvider = serviceProvider;

//    //        Name = NameVersionHelper.GetDefaultName(_activityType);
//    //        Version = NameVersionHelper.GetDefaultVersion(_activityType);
//    //    }

//    //    public override T Create()
//    //    {
//    //        return (T)_serviceProvider.GetRequiredService(_activityType);
//    //    }
//    //}

//    //public abstract class BaseActivity<TInput, TResult> : TaskActivity<TInput, TResult>
//    //{
//    //    private readonly string _taskName;
//    //    private readonly TResult _resultValue;

//    //    public BaseActivity(string taskName, TResult resultValue)
//    //    {
//    //        _taskName = taskName;
//    //        _resultValue = resultValue;
//    //    }

//    //    protected override TResult Execute(TaskContext context, TInput input)
//    //    {
//    //        Debug.WriteLine($"Starting {_taskName}, Input={input}, Execution Id={context.OrchestrationInstance.ExecutionId}, Instance id={context.OrchestrationInstance.InstanceId}");
//    //        Task.Delay(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
//    //        Debug.WriteLine($"Ending {_taskName}, Execution Id={context.OrchestrationInstance.ExecutionId}, Instance id={context.OrchestrationInstance.InstanceId}");
//    //        return _resultValue;
//    //    }
//    //}

//}
