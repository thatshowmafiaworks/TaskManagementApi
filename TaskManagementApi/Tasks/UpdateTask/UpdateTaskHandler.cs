namespace TaskManagementApi.Tasks.UpdateTask;

public record UpdateTaskCommand(
    Guid UserId,
    Guid Id,
    string? Title = null,
    string? Description = null,
    DateTime? DueDate = null,
    Status? Status = null,
    Priority? Priority = null
    ) : ICommand<UpdateTaskResult>;
public record UpdateTaskResult(bool IsSuccess, string? Error = null);

public class UpdateTaskHandler(TasksRepository tasksRepo
    )
    : ICommandHandler<UpdateTaskCommand, UpdateTaskResult>
{
    public async Task<UpdateTaskResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await tasksRepo.Get(command.Id);
        if (command.UserId != task.UserId)
            return new UpdateTaskResult(false, "Forbiden");
        if (command.Title != null)
            task.Title = command.Title;
        if (command.Description != null)
            task.Description = command.Description;
        if (command.DueDate != null)
            task.DueDate = command.DueDate;
        if (command.Status.HasValue)
            task.Status = command.Status.Value;
        if (command.Priority.HasValue)
            task.Priority = command.Priority.Value;
        task.UpdatedAt = DateTime.UtcNow;
        await tasksRepo.Update(task);
        return new UpdateTaskResult(true);
    }
}
