using System.ComponentModel.DataAnnotations;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS
{
    public class AddRoleModel
    {
        [Required(AllowEmptyStrings = false), MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public Guid? AccountId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
