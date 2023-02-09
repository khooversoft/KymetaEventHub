using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class FindCustomerAccountEnvelope
{
    /// <remarks/>
    public FindCustomerAccountEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public FindCustomerAccountEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FindCustomerAccountEnvelopeHeader
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
public class FindCustomerAccountEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/")]
    public findCustomerAccountResponse findCustomerAccountResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/", IsNullable = false)]
public class findCustomerAccountResponse
{
    /// <remarks/>
    public findCustomerAccountResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/", TypeName = "CustomerAccountResult")]
public class findCustomerAccountResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
    public FindCustomerAccountValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/", IsNullable = false)]
public class FindCustomerAccountValue
{
    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public uint AccountNumber { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string CustomerType { get; set; }

    /// <remarks/>
    public string CustomerClassCode { get; set; }

    /// <remarks/>
    public string AccountName { get; set; }

    [XmlElement("CustAcctInformation")]
    public FindCustomerAccountValueCustAcctInformation CustAcctInformation { get; set; }

    /// <remarks/>
    [XmlElement("CustomerAccountContact")]
    public FindCustomerAccountValueCustomerAccountContact[]? CustomerAccountContacts { get; set; }

    /// <remarks/>
    [XmlElement("CustomerAccountSite")]
    public FindCustomerAccountValueCustomerAccountSite[]? CustomerAccountSites { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class FindCustomerAccountValueCustomerAccountContact
{
    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public ulong RelationshipId { get; set; }

    /// <remarks/>
    public ulong ContactPersonId { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class FindCustomerAccountValueCustomerAccountSite
{
    /// <remarks/>
    public ulong CustomerAccountSiteId { get; set; }

    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public ulong PartySiteId { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public FindCustomerAccountValueCustomerAccountSiteCustomerAccountSiteUse CustomerAccountSiteUse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class FindCustomerAccountValueCustomerAccountSiteCustomerAccountSiteUse
{
    /// <remarks/>
    public FindCustomerAccountValueCustomerAccountSiteCustomerAccountSiteUseOriginalSystemReference OriginalSystemReference { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class FindCustomerAccountValueCustomerAccountSiteCustomerAccountSiteUseOriginalSystemReference
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string OrigSystemReference { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public partial class FindCustomerAccountValueCustAcctInformation
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public ulong CustAccountId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public string salesforceId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public string ksnId { get; set; }
}