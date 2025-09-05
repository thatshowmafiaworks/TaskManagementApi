using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApi.Tasks.UpdateTask;

public record UpdateTaskRequest(
    string? Title = null,
    string? Description = null,
    DateTime? DueDate = null,
    Status? Status = null,
    Priority? Priority = null
    );
public record UpdateTaskResponse(bool IsSuccess, string? Error = null);

public class UpdateTaskEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tasks/{Id}", async (Guid Id, [FromBody] UpdateTaskRequest request, HttpContext context, ISender sender) =>
        {
            var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var command = new UpdateTaskCommand(
                userId,
                Id,
                request.Title,
                request.Description,
                request.DueDate,
                request.Status,
                request.Priority);

            var result = await sender.Send(command);
            var response = result.Adapt<UpdateTaskResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization();
    }
}
