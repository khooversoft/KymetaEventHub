namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceAddressModel : SalesforceActionObject
{
    /// <summary>
    /// OraclePartyId__c
    /// </summary>
    public ulong? OraclePartyId { get; set; }
    /// <summary>
    /// OracleLocationId__c
    /// The Oracle Location object Id
    /// </summary>
    public ulong? OracleLocationId { get; set; }
    /// <summary>
    /// Account__c
    /// </summary>
    public string? ParentAccountId { get; set; }
    /// <summary>
    /// Parent Account Business Unit
    /// </summary>
    public string? ParentAccountBusinessUnit { get; set; }
    /// <summary>
    /// This is required to link the site to the customer account?
    /// </summary>
    public string? ParentOracleAccountId { get; set; }
    /// <summary>
    /// This is required to link legacy objects to the correct items in Oracle
    /// </summary>
    public ulong? ParentOraclePartyId { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string? SiteName { get; set; }
    /// <summary>
    /// Address__c
    /// This field is two lines and needs to be split on newline
    /// </summary>
    public string? Address { get; set; }
    public string? Address1 { get; set; }
    /// <summary>
    /// Split the Address value on newline/return to extract the relevant value
    /// </summary>
    public string? Address2 {
        get
        {
            // define newline/return identifiers
            var newlines = new string[] { "\r\n", "\r", "\n" };
            // if the Address value contains any matches, then split the address to acquire the 2nd (or last) line
            return !string.IsNullOrEmpty(Address) && newlines.Any(Address.Contains) 
                ? Address?.Split(newlines, StringSplitOptions.None).LastOrDefault() // 2nd or last line
                : null; // no newlines/returns - nothing to assign to this property
        }
    }
    /// <summary>
    /// City__c
    /// </summary>
    public string? City { get; set; }
    /// <summary>
    /// Country__c
    /// </summary>
    public string? Country { get; set; }
    /// <summary>
    /// State_Province__c
    /// </summary>
    public string? StateProvince { get; set; }
    /// <summary>
    /// Postal_Code__c
    /// </summary>
    public string? PostalCode { get; set; }
    /// <summary>
    /// Type__c (picklist -> 'Billing & Shipping' || 'Shipping')
    /// </summary>
    public string? Type { get; set; }
}