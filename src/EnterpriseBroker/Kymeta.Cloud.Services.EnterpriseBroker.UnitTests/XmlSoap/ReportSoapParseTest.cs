using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Serialization;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.XmlSoap;

public class ReportSoapParseTest
{
    [Fact]
    public void TestXmlParse()
    {
        string data = Assembly.GetAssembly(this.GetType())
            .ReadAssemblyResource("Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Data.ReportSampleReponse.xml");

        var xdoc = XElement.Parse(data);

        XNamespace ns2 = "http://xmlns.oracle.com/oxp/service/PublicReportService";

        string? v1 = xdoc.Descendants(ns2 + "reportBytes").FirstOrDefault()?.Value;
        v1.Should().NotBeNull();
        v1.Should().Be("77u/REFURTEsU09VUkNFX09SREVSX05VTUJFUixGVUxGSUxMRURfUVRZLFNPVVJDRV9MSU5FX05VTUJFUixGVUxGSUxMX0xJTkVfSUQsRlVMRklMTF9MSU5FX05VTUJFUixTSElQUEVEX0RBVEVfQU5EX1RJTUUsU1BMSVRfRlJPTV9GTElORV9JRAoyMDIzLTAyLTE0VDE2OjAwOjAwLjAwMC0wODowMCwwMDAxOTksNCwwMDAwMDA3MTM1LDMwMDAwMDExMzQ5NjUyMSwxLDIwMjMtMDItMTVUMTc6MTA6MzQuMDAwKzAwOjAwLAoyMDIzLTAyLTE0VDE2OjAwOjAwLjAwMC0wODowMCwyODY0NTcsMSwxLDMwMDAwMDExMzQ5ODIxOSwxLDIwMjMtMDItMTVUMDU6MTY6MzMuMDAwKzAwOjAwLAoyMDIzLTAyLTE0VDE2OjAwOjAwLjAwMC0wODowMCwyODY0NTcsMSwxLDMwMDAwMDExMzQ5Njg2NCwyLDIwMjMtMDItMTVUMDU6NTg6MjMuMDAwKzAwOjAwLDMwMDAwMDExMzQ5ODIxOQo=");
    }

    [Fact]
    public void ParseReportCsv()
    {
        string data = Assembly.GetAssembly(this.GetType())
            .ReadAssemblyResource("Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Data.ReportSampleReponse.xml");

        ReportRequestResponse response = ReportRequestResponseTool.Parse(data);
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Count.Should().Be(3);
    }

    [Fact]
    public void ParseReportCsvFromXml()
    {
        string data = Assembly.GetAssembly(this.GetType())
            .ReadAssemblyResource("Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Data.ReportCsv.txt");

        IReadOnlyList<ReportRequestItem> items = data
            .Trim(new char[] { '\uFEFF' })
            .StringToBytes()
            .DeserializeCsv<ReportRequestItem>();

        items.Should().NotBeNull();
        items.Count.Should().Be(3);
    }
}
