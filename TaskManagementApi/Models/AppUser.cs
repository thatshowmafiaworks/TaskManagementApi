namespace TaskManagementApi.Models;

public class AppUser : IdentityUser<Guid>
{
    public List<Task> Tasks { get; set; } = new List<Task>();
}
