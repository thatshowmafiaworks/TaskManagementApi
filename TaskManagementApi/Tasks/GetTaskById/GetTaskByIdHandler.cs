
namespace TaskManagementApi.Tasks.GetTaskById;

public record GetTaskByIdQuery(Guid Id) : IQuery<GetTaskByIdResult>;
public record GetTaskByIdResult(Models.Task Task);

public class GetTaskByIdHandler(TasksRepository tasksRepo)
    : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResult>
{
    public async Task<GetTaskByIdResult> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var task = await tasksRepo.Get(query.Id);
        return new GetTaskByIdResult(task);
    }
}
