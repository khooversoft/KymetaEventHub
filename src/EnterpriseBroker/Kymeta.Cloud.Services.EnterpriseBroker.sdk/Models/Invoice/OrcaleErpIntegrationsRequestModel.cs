using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record OrcaleErpIntegrationsRequestModel
{
    public string OperationName { get; init; } = "submitESSJobRequest";
    public string JobPackageName { get; init; } = "/oracle/apps/ess/financials/receivables/transactions/autoInvoices";
    public string JobDefName { get; init; } = "AutoInvoiceImportEss";
    public string ESSParameters { get; init; } = null!;
}
