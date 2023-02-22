using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public class CreateOrderResponse : IOracleResponsePayload
    {
        private const string SucecssfulMessageText = "The application successfully imported the sales order.";

        public long HeaderId { get; set; }
        public string OrderNumber { get; set; }
        public string PricingSegmentExplanation { get; set; }
        public object StrategyExplanationMessageName { get; set; }
        public string PricingStrategyExplanation { get; set; }
        public string StatusCode { get; set; }
        public DateTime CreationDate { get; set; }
        public string MessageText { get; set; }

        public bool IsSuccessfulResponse()
        {
            return MessageText.Equals(SucecssfulMessageText, StringComparison.InvariantCulture);
        }
    }
}




