using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class UpdatePersonEnvelope
{
    /// <remarks/>
    public UpdatePersonEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public UpdatePersonEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class UpdatePersonEnvelopeHeader
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
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class UpdatePersonEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
    public updatePersonResponse updatePersonResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public class updatePersonResponse
{
    /// <remarks/>
    public updatePersonResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", TypeName = "PersonResult")]
public class updatePersonResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
    public UpdatePersonValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public class UpdatePersonValue
{
    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public uint PartyNumber { get; set; }

    /// <remarks/>
    public string PartyName { get; set; }

    /// <remarks/>
    public string PartyType { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string PersonFirstName { get; set; }

    /// <remarks/>
    public string PersonLastName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonMiddleName { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string EmailAddress { get; set; }

    /// <remarks/>
    public UpdatePersonValueEmail Email { get; set; }

    /// <remarks/>
    //public UpdatePersonValuePersonProfile PersonProfile { get; set; }

    /// <remarks/>
    public UpdatePersonValuePhone Phone { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public class UpdatePersonValueEmail
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string ContactPointType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public ulong? RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string EmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PartyName { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public class UpdatePersonValuePhone
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string ContactPointType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public ulong? RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PhoneNumber { get; set; }
}

