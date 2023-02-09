using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class CreateCustomerProfileEnvelope
{
    /// <remarks/>
    public CreateCustomerProfileEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public CreateCustomerProfileEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class CreateCustomerProfileEnvelopeHeader
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
public class CreateCustomerProfileEnvelopeBody
{
    /// <remarks/>
    [XmlElement("createCustomerProfileResponse", Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/")]
    public createCustomerProfileResponse createCustomerProfileResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/", IsNullable = false)]
public class createCustomerProfileResponse
{
    /// <remarks/>
    public createCustomerProfileResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/", TypeName = "CustomerProfileResult")]
public class createCustomerProfileResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/")]
    public CreateCustomerProfileValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/", IsNullable = false)]
public class CreateCustomerProfileValue
{
    /// <remarks/>
    public uint AccountNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SiteNumber { get; set; }

    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public string ProfileClassName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ClearingDays { get; set; }

    /// <remarks/>
    public string CreditBalanceStatements { get; set; }

    /// <remarks/>
    public string CreditChecking { get; set; }

    /// <remarks/>
    public string CreditHold { get; set; }

    /// <remarks/>
    public ulong CustomerAccountProfileId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DiscountGraceDays { get; set; }

    /// <remarks/>
    public string DiscountTerms { get; set; }

    /// <remarks/>
    public string DunningLetters { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public System.DateTime EffectiveEndDate { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public System.DateTime EffectiveStartDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LastCreditReviewDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object NextCreditReviewDate { get; set; }

    /// <remarks/>
    public string OverrideTerms { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PercentCollectable { get; set; }

    /// <remarks/>
    public string SendStatements { get; set; }

    /// <remarks/>
    public byte Tolerance { get; set; }

    /// <remarks/>
    public string CollectorName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ConversionRateType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PaymentTerms { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AutoCashRuleSet { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ApplicationExceptionRuleSet { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AutoMatchRuleSet { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ReminderRuleSet { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object StatementCycle { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object GroupingRule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CreditClassificationValue { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AccountStatusValue { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RiskCodeValue { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CreditRatingValue { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object BillLevel { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object BillType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object MatchReceiptsBy { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredDeliveryMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object GenerateBill { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object MatchByAutoupdate { get; set; }

    /// <remarks/>
    public string AutoReceiptsIncludeDisputedItems { get; set; }

    /// <remarks/>
    public string ConsolidatedInvoice { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CreditReviewCycleName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CreditAnalystName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CreditLimit { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CreditCurrencyCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object StatementDeliveryMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object OrderAmountLimit { get; set; }
}

