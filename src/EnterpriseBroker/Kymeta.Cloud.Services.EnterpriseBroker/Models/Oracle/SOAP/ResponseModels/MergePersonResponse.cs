using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class MergePersonEnvelope
{
    /// <remarks/>
    public MergePersonEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public MergePersonEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class MergePersonEnvelopeHeader
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
public class MergePersonEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
    public mergePersonResponse mergePersonResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public class mergePersonResponse
{
    /// <remarks/>
    public mergePersonResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", TypeName = "PersonResult")]
public class mergePersonResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
    public MergePersonValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public class MergePersonValue
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
    public MergePersonValueEmail Email { get; set; }

    /// <remarks/>
    //public MergePersonValuePersonProfile PersonProfile { get; set; }

    /// <remarks/>
    public MergePersonValuePhone Phone { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public class MergePersonValueEmail
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
public class MergePersonValuePhone
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

