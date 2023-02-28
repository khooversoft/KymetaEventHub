using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record OracleCreateInvoiceResponseModel
{
    public string TransactionNumber { get; init; } = null!;
}
