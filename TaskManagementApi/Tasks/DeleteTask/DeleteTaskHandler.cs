
namespace TaskManagementApi.Tasks.DeleteTask;
public record DeleteTaskCommand(Guid Id, Guid UserId) : ICommand<DeleteTaskResult>;
public record DeleteTaskResult(bool IsSuccess, string? Error = null);

public class DeleteTaskHandler(TasksRepository tasksRepo)
    : ICommandHandler<DeleteTaskCommand, DeleteTaskResult>
{
    public async Task<DeleteTaskResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await tasksRepo.Get(command.Id);
        if (task.UserId != command.UserId)
            return new DeleteTaskResult(false, "Forbiden");
        await tasksRepo.Delete(task);
        return new DeleteTaskResult(true);
    }
}
