using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[System.Xml.Serialization.XmlRootAttribute("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class CreatePersonEnvelope
{

    private CreatePersonEnvelopeHeader headerField;

    private CreatePersonEnvelopeBody bodyField;

    /// <remarks/>
    public CreatePersonEnvelopeHeader Header
    {
        get
        {
            return this.headerField;
        }
        set
        {
            this.headerField = value;
        }
    }

    /// <remarks/>
    public CreatePersonEnvelopeBody Body
    {
        get
        {
            return this.bodyField;
        }
        set
        {
            this.bodyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class CreatePersonEnvelopeHeader
{

    private string actionField;

    private string messageIDField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string Action
    {
        get
        {
            return this.actionField;
        }
        set
        {
            this.actionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string MessageID
    {
        get
        {
            return this.messageIDField;
        }
        set
        {
            this.messageIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class CreatePersonEnvelopeBody
{

    private createPersonResponse createPersonResponseField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
    public createPersonResponse createPersonResponse
    {
        get
        {
            return this.createPersonResponseField;
        }
        set
        {
            this.createPersonResponseField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[System.Xml.Serialization.XmlRootAttribute("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public partial class createPersonResponse
{

    private createPersonResponseResult resultField;

    /// <remarks/>
    public createPersonResponseResult result
    {
        get
        {
            return this.resultField;
        }
        set
        {
            this.resultField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", TypeName = "PersonResult")]
public partial class createPersonResponseResult
{

    private CreatePersonValue valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
    public CreatePersonValue Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public partial class CreatePersonValue
{

    private ulong partyIdField;

    private uint partyNumberField;

    private string partyNameField;

    private string partyTypeField;

    private object validatedFlagField;

    private string lastUpdatedByField;

    private string lastUpdateLoginField;

    private System.DateTime creationDateField;

    private object requestIdField;

    private System.DateTime lastUpdateDateField;

    private string createdByField;

    private string origSystemReferenceField;

    private object sICCodeField;

    private object jgzzFiscalCodeField;

    private string personFirstNameField;

    private object personPreNameAdjunctField;

    private string personLastNameField;

    private object personMiddleNameField;

    private object personTitleField;

    private object personNameSuffixField;

    private object personPreviousLastNameField;

    private object personAcademicTitleField;

    private object countryField;

    private object address2Field;

    private object address1Field;

    private object address4Field;

    private object address3Field;

    private object postalCodeField;

    private object cityField;

    private object provinceField;

    private object stateField;

    private object countyField;

    private string statusField;

    private object uRLField;

    private object sICCodeTypeField;

    private string emailAddressField;

    private object gSAIndicatorFlagField;

    private object languageNameField;

    private object missionStatementField;

    private object categoryCodeField;

    private object thirdPartyFlagField;

    private object salutationField;

    private string createdByModuleField;

    private object certReasonCodeField;

    private object certificationLevelField;

    private object primaryPhonePurposeField;

    private ulong primaryPhoneContactPTIdField;

    private object primaryPhoneCountryCodeField;

    private string primaryPhoneLineTypeField;

    private string primaryPhoneNumberField;

    private object primaryPhoneAreaCodeField;

    private object preferredContactMethodField;

    private object primaryPhoneExtensionField;

    private object idenAddrLocationIdField;

    private ulong primaryEmailContactPTIdField;

    private object idenAddrPartySiteIdField;

    private object personLastNamePrefixField;

    private object primaryURLContactPTIdField;

    private object preferredNameField;

    private object personSecondLastNameField;

    private object preferredNameIdField;

    private object preferredContactPersonIdField;

    private bool internalFlagField;

    private object preferredFunctionalCurrencyField;

    private object genderField;

    private object maritalStatusField;

    private object commentsField;

    private object dateOfBirthField;

    private object userGUIDField;

    private string partyUniqueNameField;

    private string sourceSystemField;

    private string sourceSystemReferenceValueField;

    private object sourceSystemUpdateDateField;

    private CreatePersonValuePersonProfile personProfileField;

    private CreatePersonValueRelationship relationshipField;

    /// <remarks/>
    public ulong PartyId
    {
        get
        {
            return this.partyIdField;
        }
        set
        {
            this.partyIdField = value;
        }
    }

    /// <remarks/>
    public uint PartyNumber
    {
        get
        {
            return this.partyNumberField;
        }
        set
        {
            this.partyNumberField = value;
        }
    }

    /// <remarks/>
    public string OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    public string PersonFirstName
    {
        get
        {
            return this.personFirstNameField;
        }
        set
        {
            this.personFirstNameField = value;
        }
    }

    /// <remarks/>
    public string PersonLastName
    {
        get
        {
            return this.personLastNameField;
        }
        set
        {
            this.personLastNameField = value;
        }
    }

    public CreatePersonPhone Phone { get; set; }
    public CreatePersonEmail Email { get; set; }

    /// <remarks/>
    public CreatePersonValueRelationship Relationship
    {
        get
        {
            return this.relationshipField;
        }
        set
        {
            this.relationshipField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class CreatePersonValuePersonProfile
{

    private ulong personProfileIdField;

    private ulong partyIdField;

    private string personNameField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private object personPreNameAdjunctField;

    private string personFirstNameField;

    private object personMiddleNameField;

    private string personLastNameField;

    private object personNameSuffixField;

    private object personTitleField;

    private object personAcademicTitleField;

    private object personPreviousLastNameField;

    private object personInitialsField;

    private object jgzzFiscalCodeField;

    private object dateOfBirthField;

    private object placeOfBirthField;

    private object dateOfDeathField;

    private object genderField;

    private object declaredEthnicityField;

    private object maritalStatusField;

    private object maritalStatusEffectiveDateField;

    private object personalIncomeAmountField;

    private object rentOwnIndField;

    private object lastKnownGPSField;

    private System.DateTime effectiveStartDateField;

    private System.DateTime effectiveEndDateField;

    private bool internalFlagField;

    private string statusField;

    private string createdByModuleField;

    private bool deceasedFlagField;

    private object commentsField;

    private object personLastNamePrefixField;

    private object personSecondLastNameField;

    private object preferredFunctionalCurrencyField;

    private object origSystemField;

    private string origSystemReferenceField;

    private byte effectiveSequenceField;

    private object headOfHouseholdFlagField;

    private object householdIncomeAmountField;

    private object householdSizeField;

    private string effectiveLatestChangeField;

    private bool suffixOverriddenFlagField;

    private object uniqueNameSuffixField;

    private string corpCurrencyCodeField;

    private string curcyConvRateTypeField;

    private string currencyCodeField;

    private ulong partyNumberField;

    private object salutationField;

    private object certReasonCodeField;

    private object certificationLevelField;

    private object preferredContactMethodField;

    private object preferredContactPersonIdField;

    private object primaryAddressLine1Field;

    private object primaryAddressLine2Field;

    private object primaryAddressLine3Field;

    private object primaryAddressLine4Field;

    private object aliasField;

    private object primaryAddressCityField;

    private object primaryAddressCountryField;

    private object primaryAddressCountyField;

    private string primaryEmailAddressField;

    private object primaryFormattedAddressField;

    private ulong primaryFormattedPhoneNumberField;

    private object primaryLanguageField;

    private string partyUniqueNameField;

    private object primaryAddressPostalCodeField;

    private object preferredContactEmailField;

    private object preferredContactNameField;

    private object preferredContactPhoneField;

    private object preferredContactURLField;

    private object preferredNameField;

    private object preferredNameIdField;

    private ulong primaryEmailIdField;

    private object primaryPhoneAreaCodeField;

    private ulong primaryPhoneIdField;

    private object primaryPhoneCountryCodeField;

    private object primaryPhoneExtensionField;

    private string primaryPhoneLineTypeField;

    private string primaryPhoneNumberField;

    private object primaryPhonePurposeField;

    private object primaryWebIdField;

    private object pronunciationField;

    private object primaryAddressProvinceField;

    private object primaryAddressStateField;

    private object primaryURLField;

    private object validatedFlagField;

    private object primaryAddressLatitudeField;

    private object primaryAddressLongitudeField;

    private object primaryAddressLocationIdField;

    private bool favoriteContactFlagField;

    private object distanceField;

    private object salesAffinityCodeField;

    private object salesBuyingRoleCodeField;

    private object departmentCodeField;

    private object departmentField;

    private object jobTitleCodeField;

    private object jobTitleField;

    private bool doNotCallFlagField;

    private bool doNotContactFlagField;

    private bool doNotEmailFlagField;

    private bool doNotMailFlagField;

    private object lastContactDateField;

    private ulong primaryCustomerIdField;

    private ulong primaryCustomerRelationshipIdField;

    private string primaryCustomerNameField;

    private System.DateTime lastSourceUpdateDateField;

    private string lastUpdateSourceSystemField;

    private object dataCloudStatusField;

    private object lastEnrichmentDateField;

}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class CreatePersonValueRelationship
{

    private ulong relationshipRecIdField;

    private ulong relationshipIdField;

    private ulong subjectIdField;

    private string subjectTypeField;

    private string subjectTableNameField;

    private ulong objectIdField;

    private string objectTypeField;

    private string objectTableNameField;

    private string relationshipCodeField;

    private string relationshipTypeField;

    private string roleField;

    private object commentsField;

    private System.DateTime startDateField;

    private System.DateTime endDateField;

    private string statusField;

    private string createdByField;

    private System.DateTime creationDateField;

    private string lastUpdatedByField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    private object additionalInformation1Field;

    private object additionalInformation2Field;

    private object additionalInformation3Field;

    private object additionalInformation4Field;

    private object additionalInformation5Field;

    private object additionalInformation6Field;

    private object additionalInformation7Field;

    private object additionalInformation8Field;

    private object additionalInformation9Field;

    private object additionalInformation10Field;

    private object additionalInformation11Field;

    private object additionalInformation12Field;

    private object additionalInformation13Field;

    private object additionalInformation14Field;

    private object additionalInformation15Field;

    private object additionalInformation16Field;

    private object additionalInformation17Field;

    private object additionalInformation18Field;

    private object additionalInformation19Field;

    private object additionalInformation20Field;

    private object additionalInformation21Field;

    private object additionalInformation22Field;

    private object additionalInformation23Field;

    private object additionalInformation24Field;

    private object additionalInformation25Field;

    private object additionalInformation26Field;

    private object additionalInformation27Field;

    private object additionalInformation28Field;

    private object additionalInformation29Field;

    private object additionalInformation30Field;

    private string directionCodeField;

    private object percentageOwnershipField;

    private object objectUsageCodeField;

    private object subjectUsageCodeField;

    private bool preferredContactFlagField;

    private string objectPartyNameField;

    private string partyNameField;

    private string currencyCodeField;

    private string curcyConvRateTypeField;

    private string corpCurrencyCodeField;

    private bool primaryCustomerFlagField;

    private string subjectEmailAddressField;

    private object objectEmailAddressField;

    private CreatePersonOrganizationContact organizationContactField;

    private CreatePersonPhone phoneField;

    private CreatePersonEmail emailField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong RelationshipId
    {
        get
        {
            return this.relationshipIdField;
        }
        set
        {
            this.relationshipIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public CreatePersonPhone Phone
    {
        get
        {
            return this.phoneField;
        }
        set
        {
            this.phoneField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public CreatePersonEmail Email
    {
        get
        {
            return this.emailField;
        }
        set
        {
            this.emailField = value;
        }
    }


    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public CreatePersonOrganizationContact OrganizationContact { get; set; }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class CreatePersonOrganizationContact
{

    private ulong orgContactIdField;

    private ulong partyRelationshipIdField;

    private ulong contactPartyIdField;

    private string personFirstNameField;

    private string personLastNameField;

    private string contactNameField;

    private object personPreNameAdjunctField;

    private object personMiddleNameField;

    private object personNameSuffixField;

    private object personPreviousLastNameField;

    private object personAcademicTitleField;

    private object salutationField;

    private object personLastNamePrefixField;

    private object preferredNameField;

    private object personSecondLastNameField;

    private object personLanguageNameField;

    private object personTitleField;

    private object personCertificationLevelField;

    private object personCertReasonCodeField;

    private ulong customerPartyIdField;

    private string customerUniqueNameField;

    private string customerNameField;

    private ulong customerPartyNumberField;

    private object formattedPhoneNumberField;

    private object emailAddressField;

    private object webUrlField;

    private object commentsField;

    private ulong contactNumberField;

    private object departmentCodeField;

    private object departmentField;

    private object jobTitleField;

    private bool decisionMakerFlagField;

    private object jobTitleCodeField;

    private bool referenceUseFlagField;

    private object rankField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private object partySiteIdField;

    private ulong origSystemReferenceField;

    private string createdByModuleField;

    private byte objectVersionNumberField;

    private object partySiteNameField;

    private object salesAffinityCodeField;

    private object salesAffinityCommentsField;

    private object salesBuyingRoleCodeField;

    private object salesInfluenceLevelCodeField;

    private object formattedAddressField;

    private object preferredContactMethodField;

    private string currencyCodeField;

    private string curcyConvRateTypeField;

    private string corpCurrencyCodeField;

    private bool preferredContactFlagField;

    private object contactFormattedAddressField;

    private object contactFormattedMultilineAddressField;

    private object customerEmailAddressField;

    private bool primaryCustomerFlagField;

    private CreatePersonOrganizationContactOrganizationContactRole organizationContactRoleField;

    public ulong? ContactNumber { get; set; }

}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
public partial class CreatePersonOrganizationContactOrganizationContactRole
{

    private ulong orgContactRoleIdField;

    private ulong origSystemReferenceField;

    private string createdByField;

    private string roleTypeField;

    private ulong orgContactIdField;

    private System.DateTime creationDateField;

    private object roleLevelField;

    private bool primaryFlagField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private string lastUpdateLoginField;

    private string primaryContactPerRoleTypeField;

    private object requestIdField;

    private string statusField;

    private byte objectVersionNumberField;

    private string createdByModuleField;
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class CreatePersonPhone
{

    private ulong contactPointIdField;

    private string contactPointTypeField;

    private string statusField;

    private string ownerTableNameField;

    private ulong ownerTableIdField;

    private bool primaryFlagField;

    private ulong origSystemReferenceField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    private string contactPointPurposeField;

    private string primaryByPurposeField;

    private System.DateTime startDateField;

    private System.DateTime endDateField;

    private ulong relationshipIdField;

    private object partyUsageCodeField;

    private object origSystemField;

    private object phoneCallingCalendarField;

    private object lastContactDtTimeField;

    private object phoneAreaCodeField;

    private object phoneCountryCodeField;

    private string phoneNumberField;

    private object phoneExtensionField;

    private string phoneLineTypeField;

    private string rawPhoneNumberField;

    private object pagerTypeCodeField;

    private ulong formattedPhoneNumberField;

    private ulong transposedPhoneNumberField;

    private string partyNameField;

    private object timezoneCodeField;

    private bool overallPrimaryFlagField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId
    {
        get
        {
            return this.contactPointIdField;
        }
        set
        {
            this.contactPointIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PhoneNumber
    {
        get
        {
            return this.phoneNumberField;
        }
        set
        {
            this.phoneNumberField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class CreatePersonEmail
{

    private ulong contactPointIdField;

    private string contactPointTypeField;

    private string statusField;

    private string ownerTableNameField;

    private ulong ownerTableIdField;

    private bool primaryFlagField;

    private string origSystemReferenceField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    private string contactPointPurposeField;

    private string primaryByPurposeField;

    private System.DateTime startDateField;

    private System.DateTime endDateField;

    private ulong relationshipIdField;

    private object partyUsageCodeField;

    private object origSystemField;

    private object emailFormatField;

    private string emailAddressField;

    private string partyNameField;

    private bool overallPrimaryFlagField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId
    {
        get
        {
            return this.contactPointIdField;
        }
        set
        {
            this.contactPointIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string EmailAddress
    {
        get
        {
            return this.emailAddressField;
        }
        set
        {
            this.emailAddressField = value;
        }
    }
}

