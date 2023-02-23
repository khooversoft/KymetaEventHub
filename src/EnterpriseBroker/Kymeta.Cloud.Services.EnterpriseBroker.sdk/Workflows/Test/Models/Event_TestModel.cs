using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record Event_TestModel
{
    public string? NEO_Oracle_Bill_to_Address_ID__c { get; init; }
    public string? NEO_Id__c { get; init; }
    public string? NEO_Preferred_Contract_Method__c { get; init; }
    public string? NEO_Account_Name__c { get; init; }
    public string? NEO_Ship_to_Name__c { get; init; }
    public string Channel { get; init; } = null!;
}
