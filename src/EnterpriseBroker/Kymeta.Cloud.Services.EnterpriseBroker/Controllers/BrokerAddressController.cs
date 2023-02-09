using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/address")]
[ExcludeFromCodeCoverage]
public class BrokerAddressController : ControllerBase
{
    private readonly ILogger<BrokerAddressController> _logger;
    private readonly IAddressBrokerService _addressService;

    public BrokerAddressController(ILogger<BrokerAddressController> logger, IAddressBrokerService addressBrokerService)
    {
        _logger = logger;
        _addressService = addressBrokerService;
    }

    /// <summary>
    /// This endpoint accepts an address Payload
    /// </summary>
    /// <param name="model">Incoming Payload</param>
    /// <returns>Response model</returns>
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<UnifiedResponse>> ProcessAddress([FromBody] SalesforceAddressModel model)
    {
        try
        {
            var result = await _addressService.ProcessAddressAction(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing address action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}