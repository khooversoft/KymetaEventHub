using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;

public class SalesforceOrder
{
    public string? Id { get; set; }
    public string? OrderNumber { get; set; }
    public string? BusinessUnit { get; set; }
    public string? OrderType { get; set; }
    public string? AccountName { get; set; }
    public string? PrimaryContact { get; set; }
    public string? PreferredContactMethod { get; set; }
    public DateOnly? PoDate { get; set; }
    public string? PoNumber { get; set; }
    public string? BillToName { get; set; }
    public string? OracleAccountId { get; set; }
    public string? ShipToName { get; set; }
    public string? ShippingAddress { get; set; }
    public string? SalesRepresentative { get; set; }

}

public class OrderProduct
{
    public string? OrderId { get; set; }
    public string? BillingFrequency { get; set; }
    public string? NumberOfBillingPeriods { get; set; }
    public string? LineType { get; set; }
}
