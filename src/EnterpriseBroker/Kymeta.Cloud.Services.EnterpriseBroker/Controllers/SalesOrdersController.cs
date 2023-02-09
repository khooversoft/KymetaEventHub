using Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/salesorders")]
[ExcludeFromCodeCoverage]
public class SalesOrdersController : ControllerBase
{
    private readonly ILogger<SalesOrdersController> _logger;
    private readonly ITerminalSerialCacheRepository _tscr;

    public SalesOrdersController(ILogger<SalesOrdersController> logger, ITerminalSerialCacheRepository terminalSerialCacheRepository)
    {
        _logger = logger;
        _tscr = terminalSerialCacheRepository;
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SalesOrderResponse>>> GetSalesOrders([FromBody] IEnumerable<string> salesOrders)
    {
        try
        {
            var result = await _tscr.GetSalesOrdersByOrderNumbers(salesOrders);
            if (result == null) return new BadRequestObjectResult($"Sales order query returned no results due to an error.");
            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error querying the SERIALCACHE database for sales order serial records, due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}