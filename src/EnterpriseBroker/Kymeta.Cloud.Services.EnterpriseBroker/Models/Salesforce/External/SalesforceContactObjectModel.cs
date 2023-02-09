namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

public class SalesforceContactObjectModel
{
    public string? Id { get; set; }
    public string? AccountId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }

    public string? MailingStreet { get; set; }
    public string? MailingCity { get; set; }
    public string? MailingState { get; set; }
    public string? MailingPostalCode { get; set; }
    public string? MailingCountry { get; set; }
    public string? MailingStateCode { get; set; }
    public string? MailingCountryCode { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? Contact_Role__c { get; set; } // Bill to or Ship to    
}
