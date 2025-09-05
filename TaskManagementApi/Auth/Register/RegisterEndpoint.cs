namespace TaskManagementApi.Auth.Register;

public record RegisterRequest(string Username, string Email, string Password);
public record RegisterResponse(bool IsSuccess, string? Error = null);

public class RegisterEndpoint :
    ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/register", async (RegisterRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RegisterResponse>();
            return Results.Ok(response);
        });
    }
}
