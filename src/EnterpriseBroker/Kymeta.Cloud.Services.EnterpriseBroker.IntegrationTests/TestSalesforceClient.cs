//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests;

//internal class TestSalesforceClient : ISalesforceClient2
//{
//    public Queue<object> Queue { get; } = new Queue<object>();

//    public ISalesforceEventApi Events => throw new NotImplementedException();

//    public ISalesforceRestApi Rest => throw new NotImplementedException();

//}


//internal class TestSalesforceEventApi : ISalesforceEventApi
//{
//    private readonly TestSalesforceClient _parent;
//    public TestSalesforceEventApi(TestSalesforceClient parent) => _parent = parent;

//    public Task<bool> SendEvent<T>(string eventName, T value, CancellationToken token)
//    {
//        var payload = new
//        {
//            EventName = eventName,
//            Value = value,
//        };

//        _parent.Queue.Enqueue(payload);

//        return Task.FromResult(true);
//    }
//}

