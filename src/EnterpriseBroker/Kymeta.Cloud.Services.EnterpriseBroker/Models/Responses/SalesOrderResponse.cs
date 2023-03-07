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
    public string? GeoModemSerial { get; set; }
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
    public string? LeoModemSerial { get; set; }
    public string? LeoModemPart { get; set; }
    public string? LeoModemMACAddress { get; set; }
    public string? LeoModemIMEI { get; set; }
    public string? LeoModemIMSI { get; set; }
    public string? LeoModemSWVersion { get; set; }
    public string? CartridgeSerial { get; set; }
    public string? HybridRouterPart { get; set; }
    public string? HybridRouterMAC { get; set; }
    public string? KrcmPart { get; set; }
    public string? KrcmSerial { get; set; }
    public string? McrcmPart { get; set; }
    public string? McrcmSerial { get; set; }
    public string? EgrPart { get; set; }
    public string? EgrMfr { get; set; }
    public string? EgrSerial { get; set; }
    public string? OimPart { get; set; }
    public string? OimSerial { get; set; }
    public string? OimSWVersion { get; set; }
}
