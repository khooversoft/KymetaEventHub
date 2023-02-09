using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class FaultEnvelope
{
    /// <remarks/>
    public object Header { get; set; }

    /// <remarks/>
    public FaultEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FaultEnvelopeBody
{
    /// <remarks/>
    public FaultEnvelopeBodyFault Fault { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FaultEnvelopeBodyFault
{
    /// <remarks/>
    [XmlElement(Namespace = "")]
    public string faultcode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "")]
    public string faultstring { get; set; }

    /// <remarks/>
    [XmlElement("detail", Namespace = "")]
    public detail detail { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot("ServiceErrorMessage", Namespace = "", IsNullable = false)]
public class detail
{
    /// <remarks/>
    [XmlElement("ServiceErrorMessage", Namespace = "http://xmlns.oracle.com/adf/svc/errors/")]
    public FaultServiceErrorMessage ServiceErrorMessage { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/adf/svc/errors/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/adf/svc/errors/", IsNullable = false)]
public class FaultServiceErrorMessage
{
    /// <remarks/>
    public string code { get; set; }

    /// <remarks/>
    public string message { get; set; }

    /// <remarks/>
    public string severity { get; set; }

    /// <remarks/>
    public string exceptionClassName { get; set; }
}