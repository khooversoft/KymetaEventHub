using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record Step3_TestModel
{
    public string? OracleSalesOrderId { get; init; }
    public string? IntegrationStatus { get; init; }
    public string? IntergrationError { get; init; }
}
