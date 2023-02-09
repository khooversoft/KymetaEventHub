using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    [XmlRoot("Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope", IsNullable = false)]
    public class SalesOrderReportResponseModel
    {
        /// <remarks/>
        public object Header { get; set; }

        /// <remarks/>
        public SalesOrderReportBody Body { get; set; }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class SalesOrderReportBody
    {
        /// <remarks/>
        [XmlElement(Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public runReportResponse runReportResponse { get; set; }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
    [XmlRoot(Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService", IsNullable = false)]
    public class runReportResponse
    {
        /// <remarks/>
        public runReportResponseRunReportReturn runReportReturn { get; set; }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
    public class runReportResponseRunReportReturn
    {
        /// <remarks/>
        public string reportBytes { get; set; }

        /// <remarks/>
        public string reportContentType { get; set; }

        /// <remarks/>
        [XmlElement(IsNullable = true)]
        public object reportFileID { get; set; }

        /// <remarks/>
        [XmlElement(IsNullable = true)]
        public object reportLocale { get; set; }

        /// <remarks/>
        [XmlElement(IsNullable = true)]
        public object metaDataList { get; set; }
    }
}
