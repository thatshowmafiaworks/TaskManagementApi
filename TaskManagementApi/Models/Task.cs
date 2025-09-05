namespace TaskManagementApi.Models;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;
}

public enum Status
{
    Pending = 0,
    InProgress,
    Completed
}
public enum Priority
{
    Low = 0,
    Medium,
    High
}
