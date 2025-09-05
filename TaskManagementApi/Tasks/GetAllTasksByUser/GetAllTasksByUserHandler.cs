namespace TaskManagementApi.Tasks.GetAllTasksByUser;

public record GetAllTasksByUserQuery(
    Guid UserId,
    int? PageNumber = 1,
    int? PageSize = 10,
    Status? Status = null,
    Priority? Priority = null) : IQuery<GetAllTasksByUserResult>;
public record GetAllTasksByUserResult(IEnumerable<Models.Task> Tasks);

public class GetAllTasksByUserHandler(
    TasksRepository tasksRepo
    )
    : IQueryHandler<GetAllTasksByUserQuery, GetAllTasksByUserResult>
{
    public async Task<GetAllTasksByUserResult> Handle(GetAllTasksByUserQuery query, CancellationToken cancellationToken)
    {
        var tasks = await tasksRepo.GetAllByUser(
            query.UserId,
            query.PageNumber ?? 1,
            query.PageSize ?? 10,
            query.Status,
            query.Priority);

        return new GetAllTasksByUserResult(tasks);
    }
}
