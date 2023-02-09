using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class FindLocationsEnvelope
{
    /// <remarks/>
    public FindLocationsEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public FindLocationsEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FindLocationsEnvelopeHeader
{
    /// <remarks/>
    [XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string Action { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string MessageID { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/", IsNullable = false)]
public class FindLocationsValue
{
    /// <remarks/>
    public ulong LocationId { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string Country { get; set; }

    /// <remarks/>
    public string Address1 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public string Address2 { get; set; }

    /// <remarks/>
    public string City { get; set; }

    /// <remarks/>
    public string PostalCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public string State { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FindLocationsEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/")]
    public getLocationByOriginalSystemReferenceResponse getLocationByOriginalSystemReferenceResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/", IsNullable = false)]
public class getLocationByOriginalSystemReferenceResponse
{
    /// <remarks/>
    [XmlArrayItem("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/", IsNullable = false)]
    public FindLocationsValue[] result { get; set; }
}
