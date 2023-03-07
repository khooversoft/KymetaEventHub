using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Shipping;

public record UpdateProductModel
{
    public DateTime Actual_Ship_Date__c { get; init; }
    public int NEO_Shipped_Quantity__c { get; init; }
    public string NEO_Oracle_Back_Order_Fulfillment_Id__c { get; init; } = null!;
    public string NEO_Oracle_Tracking_Number__c { get; init; } = null!;
}


