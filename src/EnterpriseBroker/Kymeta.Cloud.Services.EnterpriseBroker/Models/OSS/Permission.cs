namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class Permission
{
    public Guid Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string Name { get; set; }
    public string Shortcode { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public bool Test { get; set; }
    public bool ExcludeFromDefaultRoles { get; set; }
}