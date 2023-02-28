using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record SalesforceUpdateInvoiceRequestModel
{
    public string? OracleInvoiceNumber { get; init; }
    public string? NEO_Integration_Error { get; init; }
    public string? NEO_Integration_Status { get; init; }
}
