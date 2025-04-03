namespace InforceApplicationTask.Server.Models.Entities
{
    public class About
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? LastUpdatedBy { get; set; } = string.Empty;
        public DateTime? LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
