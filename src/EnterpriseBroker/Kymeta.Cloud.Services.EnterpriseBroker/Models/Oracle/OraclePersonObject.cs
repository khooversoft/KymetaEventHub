namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
public class OraclePersonObject
{
    public ulong? PartyId { get; set; }
    public ulong? RelationshipId { get; set; }
    public ulong? ContactNumber { get; set; }
    public ulong? PartyNumber { get; set; }
    public string? OrigSystemReference { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    //public string PhoneNumber { get; set; }
    public bool? IsPrimary { get; set; }
    public string? Title { get; set; }
    public List<OraclePersonEmailModel>? EmailAddresses{ get; set; }
    public List<OraclePersonPhoneModel>? PhoneNumbers{ get; set; }
}
public class OracleContactPointModel
{
    public ulong? ContactPointId { get; set; }
}

public class OraclePersonEmailModel : OracleContactPointModel
{
    public string? EmailAddress { get; set; }
}

public class OraclePersonPhoneModel : OracleContactPointModel
{
    public string? PhoneNumber { get; set; }
    public string? PhoneLineType { get; set; }
}