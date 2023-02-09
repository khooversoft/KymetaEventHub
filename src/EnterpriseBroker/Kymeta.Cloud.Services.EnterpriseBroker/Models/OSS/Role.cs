namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class Role
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Name { get; set; }
    public string Shortcode { get; set; }
    public string Description { get; set; }
    public int UserCount { get; set; }
    public bool Test { get; set; }

    public List<string> Permissions { get; set; }
}
