
namespace TaskManagementApi.Tasks.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    DateTime? DueDate,
    Status Status,
    Priority Priority,
    AppUser User) : ICommand<CreateTaskResult>;
public record CreateTaskResult(
    Guid Id,
    bool IsSuccess,
    string? Error = null);

public class CreateTaskHandler(
    TasksRepository repo
    )
    : ICommandHandler<CreateTaskCommand, CreateTaskResult>
{
    public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken ct = default)
    {
        var task = new Models.Task
        {
            Title = command.Title,
            Description = command.Description,
            DueDate = command.DueDate,
            Status = command.Status,
            Priority = command.Priority,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            User = command.User
        };

        await repo.Create(task);

        return new CreateTaskResult(task.Id, true);
    }
}
