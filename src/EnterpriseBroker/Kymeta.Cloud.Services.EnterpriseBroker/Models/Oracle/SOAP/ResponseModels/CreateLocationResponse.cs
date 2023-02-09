using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class CreateLocationEnvelope
{
    /// <remarks/>
    public CreateLocationEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public CreateLocationEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class CreateLocationEnvelopeHeader
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
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class CreateLocationEnvelopeBody
{
    /// <remarks/>
    [XmlElement("createLocationResponse", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/")]
    public createLocationResponse createLocationResponse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/", IsNullable = false)]
public class createLocationResponse
{
    /// <remarks/>
    public createLocationResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/", TypeName = "LocationResult")]
public class createLocationResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/")]
    public CreateLocationValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/", IsNullable = false)]
public class CreateLocationValue
{
    /// <remarks/>
    public ulong LocationId { get; set; }

    /// <remarks/>
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public System.DateTime CreationDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public string OrigSystem { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string Country { get; set; }

    /// <remarks/>
    public string Address1 { get; set; }

    /// <remarks/>
    public string Address2 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address3 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address4 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object City { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PostalCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object State { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Province { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object County { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressStyle { get; set; }

    /// <remarks/>
    public bool ValidatedFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressLinesPhonetic { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PostalPlus4Code { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Position { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LocationDirections { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressEffectiveDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressExpirationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ClliCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Language { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ShortDescription { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Description { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SalesTaxGeocode { get; set; }

    /// <remarks/>
    public byte SalesTaxInsideCityLimits { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FaLocationId { get; set; }

    /// <remarks/>
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public string GeometryStatusCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ValidationStatusCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DateValidated { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DoNotValidateFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object HouseType { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public System.DateTime EffectiveDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddrElementAttribute1 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddrElementAttribute2 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddrElementAttribute3 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddrElementAttribute4 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddrElementAttribute5 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Building { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FloorNumber { get; set; }

    /// <remarks/>
    public bool StatusFlag { get; set; }

    /// <remarks/>
    public bool InternalFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TimezoneCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Latitude { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Longitude { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Distance { get; set; }
}