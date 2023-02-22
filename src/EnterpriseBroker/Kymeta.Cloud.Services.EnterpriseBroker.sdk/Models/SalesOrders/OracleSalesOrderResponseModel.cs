using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record OracleSalesOrderResponseModel
{
    public string? OracleSalesOrderId { get; init; }
    public string? IntegrationStatus { get; init; }
    public string? IntergrationError { get; init; }
}
