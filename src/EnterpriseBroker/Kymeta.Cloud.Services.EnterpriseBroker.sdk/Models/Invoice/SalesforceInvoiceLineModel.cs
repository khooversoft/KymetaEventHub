using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public class SalesforceInvoiceLineModel
{
    public IReadOnlyList<string> Lines { get; init; } = Array.Empty<string>();
}
