using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: This code was generated from XML (Oracle Response payload)

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class CreateCustomerAccountResponseEnvelope
{

    [XmlElement("Header")]
    public CreateCustomerAccountResponseEnvelopeHeader Header { get; set; }

    [XmlElement("Body")]
    public CreateCustomerAccountResponseEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class CreateCustomerAccountResponseEnvelopeHeader
{
    [XmlElement("Action", Namespace = "http://www.w3.org/2005/08/addressing")]
    public string Action { get; set; }

    [XmlElement("MessageID", Namespace = "http://www.w3.org/2005/08/addressing")]
    public string MessageID { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class CreateCustomerAccountResponseEnvelopeBody
{
    [XmlElement("createCustomerAccountResponse", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/")]
    public CreateCustomerAccountResponse createCustomerAccountResponse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/", IsNullable = false)]
public class CreateCustomerAccountResponse
{
    /// <remarks/>
    public createCustomerAccountResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/", TypeName = "CustomerAccountResult")]
public class createCustomerAccountResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
    public CreateCustomerAccountValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/", IsNullable = false)]
public class CreateCustomerAccountValue
{
    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public uint AccountNumber { get; set; }

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
    [XmlElement(IsNullable = true)]
    public object OrigSystem { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CustomerType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CustomerClassCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxHeaderLevelFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxRoundingRule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CoterminateDayMonth { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime AccountEstablishedDate { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime AccountTerminationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object HeldBillExpirationDate { get; set; }

    /// <remarks/>
    public bool HoldBillFlag { get; set; }

    /// <remarks/>
    public string AccountName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DepositRefundMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object NpaNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DateTypePreference { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ArrivalsetsIncludeLinesFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object StatusUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AutopayFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LastBatchId { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SellingPartyId { get; set; }

    /// <remarks/>
    public UpdateCustomerAccountValueCustAcctInformation CustAcctInformation { get; set; }

    /// <remarks/>
    public CreateCustomerAccountValueOriginalSystemReference OriginalSystemReference { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustAcctInformation
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

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class CreateCustomerAccountValueOriginalSystemReference
{

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong OrigSystemReferenceId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string OrigSystem { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string OwnerTableName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong OwnerTableId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string Status { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object ReasonCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object OldOrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
    public System.DateTime StartDateActive { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
    public System.DateTime EndDateActive { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CreatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public System.DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartyId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object RequestId { get; set; }
}