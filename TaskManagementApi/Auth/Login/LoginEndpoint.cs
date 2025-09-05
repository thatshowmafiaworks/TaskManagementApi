namespace TaskManagementApi.Auth.Login;

public record LoginRequest(string Username, string Password);
public record LoginResponse(string Token, bool IsSuccess, string? Error = null);

public class LoginEndpoint
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login", async (LoginRequest request, ISender sender) =>
        {
            var query = request.Adapt<LoginQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<LoginResponse>();
            return response;
        });
    }
}
