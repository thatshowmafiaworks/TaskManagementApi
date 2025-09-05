
namespace TaskManagementApi.Tasks.GetAllTasks;

public record GetAllTasksQuery(
    int? PageNumber = 1,
    int? PageSize = 10,
    Status? Status = null,
    Priority? Priority = null) : IQuery<GetAllTasksResult>;
public record GetAllTasksResult(IEnumerable<Models.Task> Tasks);

public class GetAllTasksHandler(
    TasksRepository taskRepo)
    : IQueryHandler<GetAllTasksQuery, GetAllTasksResult>
{
    public async Task<GetAllTasksResult> Handle(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await taskRepo.GetAll(
            query.PageNumber ?? 1,
            query.PageSize ?? 10,
            query.Status,
            query.Priority);
        return new GetAllTasksResult(tasks);
    }
}
