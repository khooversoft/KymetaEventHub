//using FluentAssertions;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests;

//public class OracleClientTests : TestApplicationFixture
//{
//    private readonly IOracleRestClient _client;
//    public OracleClientTests(EnterpriseBrokerFactory factory) : base(factory)
//    {
//        _client = Resolve<IOracleRestClient>();
//    }

//    [Fact]
//    public async Task CreateOrder_InvalidOrder_ReturnsCreatedWithPayloadIssues()
//    {
//        Random random = new Random();

//        var order = new OracleCreateOrder
//        {
//            SourceTransactionNumber = random.Next(1000000, 9999999).ToString(),
//            SourceTransactionId = random.Next(1000, 9999).ToString(),
//            OrderKey = "OPS:0047355",
//            SourceTransactionSystem = "OPS",
//            BusinessUnitName = "Kymeta Corporation BU",
//            BuyingPartyNumber = "423011",
//            BuyingPartyContactNumber = "423012",
//            TransactionType = "Standard Orders",
//            RequestedShipDate = "2023-10-19T20:49:12+00:00",
//            PaymentTerms = "Net 60",
//            TransactionalCurrencyCode = "USD",
//            RequestingBusinessUnitName = "Kymeta Corporation BU",
//            FreezePriceFlag = false,
//            FreezeShippingChargeFlag = false,
//            FreezeTaxFlag = false,
//            SubmittedFlag = true,
//            SourceTransactionRevisionNumber = "1",
//        };

//        var result = await _client.CreateOrder(order, CancellationToken.None);
//        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
//        result.Content.Should().NotBeNull();
//        result.Content!.ToString().Should().Contain("\"MessageText\" : \"The application didn't submit the sales order because it failed validation.\"");
//    }

//    [Fact]
//    public async Task CreateOrder_ValidOrder_Retrunsuccess()
//    {
//        Random random = new Random();

//        var order = new OracleCreateOrder
//        {
//            SourceTransactionNumber = random.Next(1000000, 9999999).ToString(),
//            SourceTransactionId = random.Next(1000, 9999).ToString(),
//            OrderKey = "OPS:0047355",
//            SourceTransactionSystem = "OPS",
//            BusinessUnitName = "Kymeta Corporation BU",
//            BuyingPartyNumber = "423011",
//            BuyingPartyContactNumber = "423012",
//            TransactionType = "Standard Orders",
//            RequestedShipDate = "2023-10-19T20:49:12+00:00",
//            PaymentTerms = "Net 60",
//            TransactionalCurrencyCode = "USD",
//            RequestingBusinessUnitName = "Kymeta Corporation BU",
//            FreezePriceFlag = false,
//            FreezeShippingChargeFlag = false,
//            FreezeTaxFlag = false,
//            SubmittedFlag = true,
//            SourceTransactionRevisionNumber = "1",
//            Lines = new[]
//            {
//                new OrderLines
//                {
//                    SourceTransactionLineId = "1",
//                    SourceTransactionLineNumber = "1",
//                    SourceTransactionScheduleId = "1",
//                    SourceScheduleNumber = "1",
//                    TransactionCategoryCode = "ORDER",
//                    TransactionLineType = "Buy",
//                    ProductNumber = "U8911-11113-0",
//                    OrderedQuantity = "80",
//                    OrderedUOM = "EA"
//                }
//            }
//        };

//        var result = await _client.CreateOrder(order, CancellationToken.None);
//        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
//        result.Content.Should().NotBeNull();
//        result.Content.ToString().Should().Contain("\"MessageText\" : \"The application successfully imported the sales order.\"");
//    }

//    [Fact]
//    public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
//    {
//        var order = new OracleCreateOrder
//        {
//            SourceTransactionNumber = "0047355",
//            SourceTransactionId = "0047355",
//            OrderKey = "OPS:0047355",
//            SourceTransactionSystem = "OPS",
//            BusinessUnitName = "Kymeta Corporation BU",
//            BuyingPartyNumber = "423011",
//            BuyingPartyContactNumber = "423012",
//            TransactionType = "Standard Orders",
//            RequestedShipDate = "2023-10-19T20:49:12+00:00",
//            PaymentTerms = "Net 60",
//            TransactionalCurrencyCode = "USD",
//            RequestingBusinessUnitName = "Kymeta Corporation BU",
//            FreezePriceFlag = false,
//            FreezeShippingChargeFlag = false,
//            FreezeTaxFlag = false,
//            SubmittedFlag = true,
//            SourceTransactionRevisionNumber = "1",
//            BillToCustomer = new[] { new CustomerBill { AccountNumber = "77001" } },
//            ShipToCustomer = new[] { new CustomerShip { PartyNumber = "423011" } },
//            Lines = new[]
//            {
//                new OrderLines
//                {
//                    SourceTransactionLineId = "1",
//                    SourceTransactionLineNumber = "1",
//                    SourceTransactionScheduleId = "1",
//                    SourceScheduleNumber = "1",
//                    TransactionCategoryCode = "ORDER",
//                    TransactionLineType = "Buy",
//                    ProductNumber = "U8911-11113-0",
//                    OrderedQuantity = "80",
//                    OrderedUOM = "EA"
//                },
//                new OrderLines
//                {
//                    SourceTransactionLineId = "2",
//                    SourceTransactionLineNumber = "2",
//                    SourceTransactionScheduleId = "1",
//                    SourceScheduleNumber = "1",
//                    TransactionCategoryCode = "ORDER",
//                    TransactionLineType = "Buy",
//                    ProductNumber = "U8911-11113-0",
//                    OrderedQuantity = "1",
//                    OrderedUOM = "EA"
//                }
//            }
//        };

//        var result = await _client.CreateOrder(order, CancellationToken.None);
//        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
//        result.Message.Should().Be("Bad Request");
//        result.Content.Should().Be("The request failed because a sales order with transaction 0047355 from source system OPS already exists.");
//    }

//}