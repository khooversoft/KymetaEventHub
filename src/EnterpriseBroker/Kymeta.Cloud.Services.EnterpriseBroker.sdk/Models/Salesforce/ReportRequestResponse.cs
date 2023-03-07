using System.Xml.Linq;
using CsvHelper.Configuration.Attributes;
using Kymeta.Cloud.Services.Toolbox.Serialization;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using System.Text;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;

public record ReportRequestResponse
{
    public IReadOnlyList<ReportRequestItem> Items { get; init; } = Array.Empty<ReportRequestItem>();
}

public record ReportRequestItem
{
    [Name("DATE1")]
    public DateTime Date1 { get; init; }

    [Name("SOURCE_ORDER_NUMBER")]
    public string SourceOrderNumber { get; init; } = null!;

    [Name("FULFILLED_QTY")]
    public int FulfilledQuantity { get; init; }

    [Name("SOURCE_LINE_NUMBER")]
    public string SourceLineNumber { get; init; } = null!;

    [Name("FULFILL_LINE_ID")]
    public string FulfillLineId { get; init; } = null!;

    [Name("FULFILL_LINE_NUMBER")]
    public string FulfillLineNumber { get; init; } = null!;

    [Name("SHIPPED_DATE_AND_TIME")]
    public DateTime ShippedDateAndTime { get; init; }

    [Name("SPLIT_FROM_FLINE_ID")]
    public string SplitFromFLineId { get; init; } = null!;
}


public static class ReportRequestResponseTool
{
    public static ReportRequestResponse Parse(string xmlData)
    {
        string reportBytes = GetReportBytes(xmlData);

        IReadOnlyList<ReportRequestItem> items = reportBytes
            .StringToBytes()
            .DeserializeCsv<ReportRequestItem>();

        return new ReportRequestResponse
        {
            Items = items,
        };
    }

    public static string GetReportBytes(string xmlData)
    {
        xmlData.NotEmpty();
        var xdoc = XElement.Parse(xmlData);

        XNamespace ns2 = "http://xmlns.oracle.com/oxp/service/PublicReportService";
        string? reportBytes = xdoc.Descendants(ns2 + "reportBytes").FirstOrDefault()?.Value;

        if (reportBytes == null) throw new FormatException("Cannot find 'reportBytes' in Report reponse");

        return reportBytes
            .FromBase64String(Encoding.UTF8)
            .Trim(new char[] { '\uFEFF' });
    }
}