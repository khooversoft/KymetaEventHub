//using FluentAssertions;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests
//{
//    public class SalesforceClientTests : TestApplicationFixture
//    {
//        private readonly SalesforceRestApi _client;
//        public SalesforceClientTests(EnterpriseBrokerFactory factory) : base(factory)
//        {
//            _client = Resolve<SalesforceClient2>().Rest;
//        }


//        //[Fact]
//        //public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
//        //{
//        //    //var result = await _client.GetOrderProducts("0047355", CancellationToken.None);
//        //    //result.Should().NotBeEmpty();
//        //}

//    }
//}