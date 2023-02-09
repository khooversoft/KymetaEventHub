using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class FindPersonsEnvelope
{
    /// <remarks/>
    public FindPersonsEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public FindPersonsEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class FindPersonsEnvelopeHeader
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
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public partial class FindPersonsValue
{
    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public string PartyName { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string PersonFirstName { get; set; }

    /// <remarks/>
    public string PersonLastName { get; set; }

    /// <remarks/>
    public string EmailAddress { get; set; }

    /// <remarks/>
    public FindPersonsEmailValue Email { get; set; }
    
    [XmlElement("Phone")]
    public FindPersonsPhoneValue[] Phone { get; set; }

    /// <remarks/>
    public FindPersonsValueRelationship Relationship { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class FindPersonsEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
    public findPersonResponse findPersonResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public partial class findPersonResponse
{
    /// <remarks/>
    [XmlArrayItem("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
    public FindPersonsValue[] result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class FindPersonsEmailValue
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string? EmailAddress { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class FindPersonsPhoneValue
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string? PhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string? PhoneLineType { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class FindPersonsValueRelationship
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong ObjectId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public FindPersonOrganizationContact OrganizationContact { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class FindPersonOrganizationContact
{
    public ulong? ContactNumber { get; set; }

}