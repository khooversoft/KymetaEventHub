namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;

public class SalesOrderResponse
{
    public string? SalesOrder { get; set; }
    public IEnumerable<SalesOrderTerminal>? Terminals { get; set; }
}

public class SalesOrderTerminal
{
    public Guid? Id { get; set; }
    public string? OracleSalesOrder { get; set; }
    public string? OracleTerminalSerial { get; set; }
    public string? ProductCode { get; set; }
    public string? TerminalKpn { get; set; }
    public string? TerminalSerial { get; set; }
    public string? AntennaSerial { get; set; }
    public string? SatModem { get; set; }
    public string? HybridRouterSerial { get; set; }
    public string? HybridRouterImei { get; set; }
    public string? HybridRouterIccid { get; set; }
    public string? BucSerial { get; set; }
    public string? LnbSerial { get; set; }
    public string? DiplexerSerial { get; set; }
    public string? DescriptionFirstLine { get; set; }
    public string? DescriptionSecondLine { get; set; }
    public string? IpAddress { get; set; }
    public DateTime? LinkTimestamp { get; set; }
}
