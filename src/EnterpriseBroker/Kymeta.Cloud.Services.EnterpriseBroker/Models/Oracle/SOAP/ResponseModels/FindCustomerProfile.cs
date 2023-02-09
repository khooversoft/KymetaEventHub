using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class FindCustomerProfileEnvelope
{
    /// <remarks/>
    public FindCustomerProfileEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public FindCustomerProfileEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FindCustomerProfileEnvelopeHeader
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
public class FindCustomerProfileEnvelopeBody
{
    /// <remarks/>
    [XmlElement("getActiveCustomerProfileResponse", Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/")]
    public getActiveCustomerProfileResponse getActiveCustomerProfileResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/", IsNullable = false)]
public class getActiveCustomerProfileResponse
{
    /// <remarks/>
    public getActiveCustomerProfileResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/", TypeName = "CustomerProfileResult")]
public class getActiveCustomerProfileResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/")]
    public FindCustomerProfileValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/", IsNullable = false)]
public class FindCustomerProfileValue
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

    /// <remarks/>
    public FindCustomerProfileValueCustomerProfileFLEX CustomerProfileFLEX { get; set; }

    /// <remarks/>
    public FindCustomerProfileValueCustomerProfileGdf CustomerProfileGdf { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/")]
public class FindCustomerProfileValueCustomerProfileFLEX
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/")]
    public ulong CustAccountProfileId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/", DataType = "date")]
    public System.DateTime EffectiveStartDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/", DataType = "date")]
    public System.DateTime EffectiveEndDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/", IsNullable = true)]
    public object @__FLEX_Context { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/", IsNullable = true)]
    public object @__FLEX_Context_DisplayValue { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/")]
    public byte _FLEX_NumOfSegments { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/")]
public class FindCustomerProfileValueCustomerProfileGdf
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/")]
    public ulong CustAccountProfileId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/", DataType = "date")]
    public System.DateTime EffectiveStartDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/", DataType = "date")]
    public System.DateTime EffectiveEndDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/", IsNullable = true)]
    public object FLEX_PARAM_GLOBAL_COUNTRY_CODE { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/", IsNullable = true)]
    public object @__FLEX_Context { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/", IsNullable = true)]
    public object @__FLEX_Context_DisplayValue { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/")]
    public byte _FLEX_NumOfSegments { get; set; }
}


