namespace TaskManagementApi.Tasks.DeleteTask;

//public record DeleteTaskRequest();
public record DeleteTaskResponse(bool IsSuccess, string? Error = null);

public class DeleteTaskEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tasks/{Id}", async (Guid Id, ISender sender, HttpContext context) =>
        {
            var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var command = new DeleteTaskCommand(Id, userId);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteTaskResult>();
            return Results.Ok(response);
        })
            .RequireAuthorization();
    }
}
