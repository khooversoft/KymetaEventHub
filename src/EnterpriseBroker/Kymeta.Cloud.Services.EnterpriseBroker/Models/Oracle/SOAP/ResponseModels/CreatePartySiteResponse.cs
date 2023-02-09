using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class PartySiteEnvelope
{
    /// <remarks/>
    public PartySiteEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public PartySiteEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class PartySiteEnvelopeHeader
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
[XmlType("mergeOrganizationResponse", AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class PartySiteEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/")]
    public mergeOrganizationResponse mergeOrganizationResponse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/", IsNullable = false)]
public class mergeOrganizationResponse
{
    /// <remarks/>
    public mergeOrganizationResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/", TypeName = "OrganizationPartyResult")]
public class mergeOrganizationResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
    public PartySiteValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/", IsNullable = false)]
public class PartySiteValue
{
    /// <remarks/>
    public uint PartyNumber { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public string PartyType { get; set; }

    /// <remarks/>
    public string PartyName { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement("PartySite")]
    public PartySiteValuePartySite[] PartySite { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
public class PartySiteValuePartySite
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong PartySiteId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong PartyId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong LocationId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public uint PartySiteNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string? PartySiteName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public PartySitePartySiteUse[] PartySiteUse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = false)]
public class PartySitePartySiteUse
{
    /// <remarks/>
    public ulong PartySiteUseId { get; set; }

    /// <remarks/>
    public ulong PartySiteId { get; set; }

    /// <remarks/>
    public string SiteUseType { get; set; }
}