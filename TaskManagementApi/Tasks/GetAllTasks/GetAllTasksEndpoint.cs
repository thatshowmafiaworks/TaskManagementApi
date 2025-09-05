
namespace TaskManagementApi.Tasks.GetAllTasks;

public record GetAllTasksRequest(
    int? PageNumber = 1,
    int? PageSize = 10,
    Status? Status = null,
    Priority? Priority = null);
public record GetAllTasksResponse(IEnumerable<Models.Task> Tasks);

public class GetAllTasksEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks",
            async ([AsParameters] GetAllTasksRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetAllTasksQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetAllTasksResponse>();
            return Results.Ok(response);
        })
           .RequireAuthorization();
    }
}
