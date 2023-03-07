using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using CsvHelper.Configuration.Attributes;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Serialization;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;

public class OracleReportApi
{
    private readonly HttpClient _client;
    private readonly ILogger<OracleReportApi> _logger;

    public OracleReportApi(HttpClient client, ILogger<OracleReportApi> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ReportRequestResponse> Get(DateTime date1, CancellationToken token = default)
    {
        using var ls = _logger.LogEntryExit();
        _logger.LogInformation("Getting Oracle report for date1={date1}", date1);

        string xmlPayoad = CreateXML(date1);

        var response = await new RestClient(_client)
            .SetPath("xmlpserver/services/ExternalReportWSSService")
            .SetLogger(_logger)
            .SetContent(xmlPayoad, isXml: true)
            .PostAsync(token);

        if (response.Content == null) throw new HttpRequestException("Empty content");

        return ReportRequestResponseTool.Parse(response.Content);
    }

    private string CreateXML(DateTime date1)
    {
        XNamespace ns = "http://www.w3.org/2003/05/soap-envelope";
        XNamespace pub = "http://xmlns.oracle.com/oxp/service/PublicReportService";

        XDocument soapRequest = new XDocument(
            new XDeclaration("1.0", "UTF-8", "no"),
            new XElement(ns + "Envelope",
                new XAttribute(XNamespace.Xmlns + "pub", pub),
                new XAttribute(XNamespace.Xmlns + "soap", ns),
                new XElement(ns + "Header"),
                new XElement(ns + "Body",
                    new XElement(pub + "runReport",
                        new XElement(pub + "reportRequest",
                            new XElement(pub + "parameterNameValues",
                                new XElement(pub + "item",
                                    new XElement(pub + "name", "DATE1"),
                                    new XElement(pub + "values",
                                        new XElement(pub + "item", date1.ToString("MM-dd-yyyy"))
                                    )
                                )
                            ),
                        new XElement(pub + "reportAbsolutePath", "/Custom/Kymeta_Shipping_transactions_by_date_Report.xdo"),
                        new XElement(pub + "sizeOfDataChunkDownload", "-1")
                        ),
                    new XElement(pub + "appParams", "?")
                    )
                )
            )
        );

        return soapRequest.ToString();
    }

    //private 
}

