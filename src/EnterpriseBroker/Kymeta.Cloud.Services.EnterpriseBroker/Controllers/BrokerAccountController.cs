using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/account")]
[ExcludeFromCodeCoverage]
public class BrokerAccountController : ControllerBase
{
    private readonly ILogger<BrokerAccountController> _logger;
    private readonly IAccountBrokerService _accountBrokerService;

    public BrokerAccountController(ILogger<BrokerAccountController> logger, IAccountBrokerService accountBrokerService)
    {
        _logger = logger;
        _accountBrokerService = accountBrokerService;
    }

    /// <summary>
    /// This endpoint accepts an Account payload including Contacts and Addresses
    /// </summary>
    /// <param name="model">Incoming Payload</param>
    /// <returns>UnifiedResponse model with context about the created/updated objects.</returns>
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<UnifiedResponse>> ProcessAccount([FromBody] SalesforceAccountModel model)
    {
        try
        {
            var result = await _accountBrokerService.ProcessAccountAction(model);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing create account action due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Fetch Salesforce Account metadata directly from Salesforce
    /// </summary>
    /// <param name="accountId">The unique identifier for the Account to fetch.</param>
    /// <returns>Account metadata pertaining to the Account specified.</returns>
    [HttpGet("{accountId}"), AllowAnonymous]
    public async Task<ActionResult<SalesforceAccountObjectModel>> GetSalesforceAccount(string accountId)
    {
        if (string.IsNullOrEmpty(accountId)) return new BadRequestObjectResult($"You must provide an accountId to query.");

        try
        {
            var result = await _accountBrokerService.GetSalesforceAccountById(accountId);
            return result;
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching account from Salesforce REST API due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Fetch all salesforce accounts from Salesforce
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<List<SalesforceAccountObjectModel>>> GetSalesforceAccounts()
    {
        try
        {
            var result = await _accountBrokerService.GetSalesforceAccounts();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching accounts from Salesforce REST API due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("syncToOss"), AllowAnonymous]
    public async Task<ActionResult<int>> SyncSalesforceAccountsToOss()
    {
        try
        {
            var result = await _accountBrokerService.SyncSalesforceAccountsToOss();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching accounts from Salesforce REST API due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}