namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

public class SalesforceAddressObjectModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Account__c { get; set; } // AccountId
    public string? Address_Line_1__c { get; set; }
    public string? Address__c { get; set; }
    public string? City__c { get; set; }
    public string? Country__c { get; set; }
    public string? Notes__c { get; set; }
    public string? Oracle_Address_ID__c { get; set; }
    public string? Postal_Code__c { get; set; }
    public string? State_Province__c { get; set; }
    public string? Type__c { get; set; }
    public string? Country_Code__c { get; set; }
}
