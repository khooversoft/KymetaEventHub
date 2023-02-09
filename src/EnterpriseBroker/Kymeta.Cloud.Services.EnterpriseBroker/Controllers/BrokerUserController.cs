using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/user")]
[ExcludeFromCodeCoverage]
public class BrokerUserController : ControllerBase
{
    private readonly ILogger<BrokerContactController> _logger;
    private readonly ISalesforceClient _salesforceClient;

    public BrokerUserController(ILogger<BrokerContactController> logger, ISalesforceClient salesforceClient)
    {
        _logger = logger;
        _salesforceClient = salesforceClient;
    }

    [HttpGet("{userId}"), AllowAnonymous]
    public async Task<ActionResult<SalesforceUserObjectModel>> GetSalesforceUser(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return new BadRequestObjectResult($"You must provide an userId to query.");

        try
        {
            var result = await _salesforceClient.GetUserFromSalesforce(userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching user from Salesforce REST API due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}
