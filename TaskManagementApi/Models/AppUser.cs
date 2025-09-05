using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

public class AppUser : IdentityUser<Guid>
{
    [JsonIgnore]
    public List<Task> Tasks { get; set; } = new List<Task>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
