using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface ITerminalSerialCacheRepository
{
    Task<IEnumerable<SalesOrderResponse>> GetSalesOrdersByOrderNumbers(IEnumerable<string> orderNumbers);
}

public class TerminalSerialCacheRepository : ITerminalSerialCacheRepository
{
    private IManufacturingProxyClient _mfgProxyClient;

    public TerminalSerialCacheRepository(IManufacturingProxyClient manufacturingProxyClient)
    {
        _mfgProxyClient = manufacturingProxyClient;
    }

    public async Task<IEnumerable<SalesOrderResponse>> GetSalesOrdersByOrderNumbers(IEnumerable<string> orderNumbers)
    {
        var stringResponse = await _mfgProxyClient.GetSalesOrdersByNumbers(orderNumbers);
        return JsonConvert.DeserializeObject<IEnumerable<SalesOrderResponse>>(stringResponse);
    }
}
