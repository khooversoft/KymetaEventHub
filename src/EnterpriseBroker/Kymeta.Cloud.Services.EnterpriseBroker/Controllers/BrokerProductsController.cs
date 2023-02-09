using Kymeta.Cloud.Services.EnterpriseBroker.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
[ExcludeFromCodeCoverage]
public class BrokerProductsController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<BrokerProductsController> _logger;
    private readonly ISalesforceProductsRepository _sfProductsRepo;
    private readonly IProductsBrokerService _sfProductBrokerService;
    private readonly ICacheRepository _cacheRepo;

    public BrokerProductsController(IConfiguration config, ILogger<BrokerProductsController> logger, ISalesforceProductsRepository sfProductsRepo, IProductsBrokerService sfProductsBrokerService, ICacheRepository cacheRepository)
    {
        _config = config;
        _logger = logger;
        _sfProductsRepo = sfProductsRepo;
        _sfProductBrokerService = sfProductsBrokerService;
        _cacheRepo = cacheRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<SalesforceProductObjectModelV2>>> GetProducts()
    {
        try
        {
            var result = await _sfProductsRepo.GetProducts();
            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching products from Cosmos DB due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [MapToApiVersion("2")]
    [HttpGet]
    public async Task<ActionResult<List<SalesforceProductObjectModelV2>>> GetProductsV2()
    {
        try
        {
            // check redis for hash key, if not present, then trigger sync
            var cachedProducts = _cacheRepo.GetProducts();
            // attempt to re-hydrate cache if SalesforceProducts HashKey is missing or null
            if (cachedProducts == null)
            {
                await SynchronizeProductsFromSalesforce();
                // attempt from cache again
                cachedProducts = _cacheRepo.GetProducts();
                // if still no Products, we were unable to re-hydrate
                if (cachedProducts == null) return new BadRequestObjectResult($"Unable to fetch Products.");
            }

            // return cached Products
            return new JsonResult(cachedProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching Products due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Synchronize Products (including file assets) with OSS persistence
    /// </summary>
    /// <returns></returns>
    [HttpGet("sync")]
    public async Task<ActionResult<List<SalesforceProductObjectModelV2>>> SynchronizeProductsFromSalesforce()
    {
        try
        {
            var productsSynchronized = await _sfProductBrokerService.SynchronizeProducts();
            if (productsSynchronized == null || !productsSynchronized.Any()) return new BadRequestObjectResult($"Encountered an error while attempting to synchronize Products from Salesforce.");
            return productsSynchronized.ToList();
        }
        catch (SynchronizeProductsException spex)
        {
            // catch errors specific to the Synchronization operation
            _logger.LogError(spex.Message);
            return new BadRequestObjectResult(spex.Message);
        }
        catch (Exception ex)
        {
            // unknown error/exception
            _logger.LogError(ex, $"Error fetching Product files from Salesforce due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("report")]
    public async Task<ActionResult<List<SalesforceProductObjectModelV2>>> GetProductReport()
    {
        try
        {
            var result = await _sfProductBrokerService.GetSalesforceProductReport();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching Products Report from SF due to an exception: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}