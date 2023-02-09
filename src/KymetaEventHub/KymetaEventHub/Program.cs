// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Xml.Linq;
using DurableTask.AzureStorage;
using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

const string storageHubConnectionString = "DefaultEndpointsProtocol=https;AccountName=kymetaeventhubstorage;AccountKey=sSWJ7oHIwKS4XQlAICBpV3eOHR7R2Eo0vhPpY1rMsh+D0yv4DSqIij9WPcxGUl87tyw7Sl8cLtuL+AStWxlFaQ==;EndpointSuffix=core.windows.net";

Console.WriteLine("Hello, World!");

var serviceProvider = new ServiceCollection()
    .AddSingleton<TestActivity1>()
    .AddSingleton<TestActivity2>()
    .AddSingleton<TestActivity3>()
    .AddSingleton<TestActivity4>()
    .BuildServiceProvider();

await startAsync(serviceProvider);

Console.WriteLine("Press <Enter> to quit");
Console.ReadLine();


async Task startAsync(IServiceProvider serviceProvider)
{
    CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1;");

    var settings = new AzureStorageOrchestrationServiceSettings
    {
        StorageConnectionString = storageHubConnectionString,
    };

    var service = new AzureStorageOrchestrationService(settings);

    TaskHubWorker hubWorker = new TaskHubWorker(service);
    hubWorker.AddTaskOrchestrations(typeof(TestOrchestration));
    hubWorker.AddTaskOrchestrations(typeof(SubTestOrchestration));
    hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(TestActivity1), serviceProvider));
    hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(TestActivity2), serviceProvider));
    hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(TestActivity3), serviceProvider));
    hubWorker.AddTaskActivities(new ActivityCreator<TaskActivity>(typeof(TestActivity4), serviceProvider));
    await service.CreateIfNotExistsAsync();
    await hubWorker.StartAsync();

    var taskHubClient = new TaskHubClient(service);

    string instanceId = Guid.NewGuid().ToString();
    OrchestrationInstance instance = taskHubClient.CreateOrchestrationInstanceAsync(typeof(TestOrchestration), instanceId, "start orchestration").Result;

    OrchestrationState result = await taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromSeconds(60));
}

public class TestActivity1 : BaseActivity<string, int> { public TestActivity1() : base("Task #1", 1) { } }
public class TestActivity2 : BaseActivity<string, int> { public TestActivity2() : base("Task #2", 20) { } }
public class TestActivity3 : BaseActivity<string, int> { public TestActivity3() : base("Task #3", 100) { } }
public class TestActivity4 : BaseActivity<string, int> { public TestActivity4() : base("Task #4", 200) { } }



public class TestOrchestration : TaskOrchestration<bool, string>
{
    public override async Task<bool> RunTask(OrchestrationContext context, string input)
    {
        string prefix = context.IsReplaying ? "**" : string.Empty;

        Console.WriteLine($"{prefix} Is Replaying =" + context.IsReplaying + " InstanceId =" + context.OrchestrationInstance.InstanceId + " Execution ID =" + context.OrchestrationInstance.ExecutionId);
        Console.WriteLine($"{prefix} Running Orchestration, Input={input}");
        var firstRetryInterval = TimeSpan.FromSeconds(1);
        var maxNumberOfAttempts = 5;
        var backoffCoefficient = 1.1;

        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
        {
            BackoffCoefficient = backoffCoefficient,
            Handle = HandleError
        };
        bool result = false;
        try
        {
            Console.WriteLine($"{prefix} Schedule with Retry ");

            var task1 = context.ScheduleWithRetry<int>(typeof(TestActivity1), options, "Test Input1");
            Console.WriteLine($"{prefix} Schedule with Retry Complete - Task 1");

            var task2 = context.ScheduleWithRetry<int>(typeof(TestActivity2), options, "Test Input2");
            Console.WriteLine($"{prefix} Schedule with Retry Complete - Task 2");

            int[] results = await Task.WhenAll(task1, task2);

            if (Enumerable.SequenceEqual(results, new int[] { 1, 20 }) == false) throw new ArgumentException("failed response");

            int subTaskResult = await context.CreateSubOrchestrationInstanceWithRetry<int>(typeof(SubTestOrchestration), options, results.Sum());
            Console.WriteLine($"{prefix} Sub Orchestration's result={subTaskResult}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{prefix} Exception in Orchestration" + e.ToString());
        }

        Console.WriteLine($"{prefix} Orchestration Finished");
        return result;
    }

    private bool HandleError(Exception e)
    {
        Console.WriteLine(e.ToString());
        return true;
    }
}

public class SubTestOrchestration : TaskOrchestration<int, int>
{
    public async override Task<int> RunTask(OrchestrationContext context, int input)
    {
        string prefix = context.IsReplaying ? "**" : string.Empty;

        Console.WriteLine($"{prefix} Sub - Orchestration - Is Replaying =" + context.IsReplaying + " InstanceId =" + context.OrchestrationInstance.InstanceId + " Execution ID =" + context.OrchestrationInstance.ExecutionId);
        Console.WriteLine($"{prefix} Sub - Running Orchestration, Input={input}");

        var firstRetryInterval = TimeSpan.FromSeconds(1);
        var maxNumberOfAttempts = 5;
        var backoffCoefficient = 1.1;

        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
        {
            BackoffCoefficient = backoffCoefficient,
            Handle = HandleError
        };

        try
        {
            Console.WriteLine($"{prefix} Schedule with Retry ");

            var task1 = context.ScheduleWithRetry<int>(typeof(TestActivity3), options, "Test Input3");
            Console.WriteLine($"{prefix} Schedule with Retry Complete - Task 3");

            var task2 = context.ScheduleWithRetry<int>(typeof(TestActivity4), options, "Test Input4");
            Console.WriteLine($"{prefix} Schedule with Retry Complete - Task 4");

            int[] results = await Task.WhenAll(task1, task2);

            if (Enumerable.SequenceEqual(results, new int[] { 100, 200 }) == false) throw new ArgumentException("failed response");

            Console.WriteLine($"{prefix} Sub - Orchestration Finished, sum={results.Sum()}");
            return results.Append(input).Sum();
        }
        catch (Exception e)
        {
            Console.WriteLine($"{prefix} sub exception in Orchestration" + e.ToString());
            return -1;
        }
    }
    private bool HandleError(Exception e)
    {
        Console.WriteLine(e.ToString());
        return true;
    }
}

public class ActivityCreator<T> : ObjectCreator<T> where T : notnull, TaskActivity
{
    private readonly Type _activityType;
    private readonly IServiceProvider _serviceProvider;

    public ActivityCreator(Type activityType, IServiceProvider serviceProvider)
    {
        _activityType = activityType;
        _serviceProvider = serviceProvider;

        Name = NameVersionHelper.GetDefaultName(_activityType);
        Version = NameVersionHelper.GetDefaultVersion(_activityType);
    }

    public override T Create()
    {
        return (T)_serviceProvider.GetRequiredService(_activityType);
    }
}

public abstract class BaseActivity<TInput, TResult> : TaskActivity<TInput, TResult>
{
    private readonly string _taskName;
    private readonly TResult _resultValue;

    public BaseActivity(string taskName, TResult resultValue)
    {
        _taskName = taskName;
        _resultValue = resultValue;
    }

    protected override TResult Execute(TaskContext context, TInput input)
    {
        Console.WriteLine($"Starting {_taskName}, Input={input}, Execution Id={context.OrchestrationInstance.ExecutionId}, Instance id={context.OrchestrationInstance.InstanceId}");
        Task.Delay(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
        Console.WriteLine($"Ending {_taskName}, Execution Id={context.OrchestrationInstance.ExecutionId}, Instance id={context.OrchestrationInstance.InstanceId}");
        return _resultValue;
    }
}
