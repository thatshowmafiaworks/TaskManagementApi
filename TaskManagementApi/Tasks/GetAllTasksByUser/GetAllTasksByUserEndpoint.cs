namespace TaskManagementApi.Tasks.GetAllTasksByUser;
public record GetAllTasksByUserRequest(
    int? PageNumber = 1,
    int? PageSize = 10,
    Status? Status = null,
    Priority? Priority = null);
public record GetAllTasksByUserResponse(IEnumerable<Models.Task> Tasks);
public class GetAllTasksByUserEndpoint
: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks/my",
            async ([AsParameters] GetAllTasksByUserRequest request,
            ISender sender,
            HttpContext context,
            UserManager<AppUser> userManager) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var query = new GetAllTasksByUserQuery(
                    Guid.Parse(userId),
                    request.PageNumber,
                    request.PageSize,
                    request.Status,
                    request.Priority);

                var result = await sender.Send(query);
                var response = result.Adapt<GetAllTasksByUserResponse>();

                return Results.Ok(response);
            })
           .RequireAuthorization();
    }
}
