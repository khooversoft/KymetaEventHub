using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record OracleErpIntegrationResponseModel
{
    public string OperationaName { get; init; } = null!;
    public string ReqstId { get; init; } = null!;
}
