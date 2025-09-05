namespace TaskManagementApi.Tasks.GetTaskById;

//public record GetTaskByIdRequest(Guid Id);
public record GetTaskByIdResponse(Models.Task Task);

public class GetTaskByIdEndpoint :
    ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks/{Id}", async (Guid Id, HttpContext context, ISender sender) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetTaskByIdQuery(Id);
            var result = await sender.Send(query);
            if (Guid.Parse(userId) != result.Task.UserId)
                return Results.Forbid();
            var response = result.Adapt<GetTaskByIdResponse>();
            return Results.Ok(response);
        })
            .RequireAuthorization();
    }
}
