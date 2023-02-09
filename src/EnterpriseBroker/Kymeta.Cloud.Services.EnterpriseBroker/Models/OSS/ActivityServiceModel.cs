namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS
{
    public class ActivityServiceModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserName { get; set; }
    }
}
