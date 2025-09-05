namespace TaskManagementApi.Tasks.CreateTask;

public record CreateTaskRequest(
    string Title,
    string? Description,
    DateTime? DueDate,
    Status Status,
    Priority Priority);
public record CreateTaskResponse(
    Guid Id,
    bool IsSuccess,
    string? Error = null);

public class CreateTaskEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tasks",
            async (
                CreateTaskRequest request,
                ISender sender,
                HttpContext context,
                UserManager<AppUser> userManager
                ) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userManager.FindByIdAsync(userId);

                var command = new CreateTaskCommand(
                    request.Title,
                    request.Description,
                    request.DueDate,
                    request.Status,
                    request.Priority,
                    user);

                var result = await sender.Send(command);
                var response = result.Adapt<CreateTaskResponse>();
                return response;
            })
            .RequireAuthorization();
    }
}
