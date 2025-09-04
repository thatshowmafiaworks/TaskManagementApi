
namespace TaskManagementApi.Auth.Login;

public record LoginQuery(string Username, string Password) : IQuery<LoginResult>;
public record LoginResult(string Token, bool IsSuccess, string? Error = null);

public class LoginHandler(
    JwtTokenGenerator tokenGenerator,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager)
    : IQueryHandler<LoginQuery, LoginResult>
{
    public async Task<LoginResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(query.Username)
            ?? await userManager.FindByNameAsync(query.Username);
        if (user is null)
        {
            return new LoginResult("", false, "Cant find user with Username/Email!");
        }
        var result = await signInManager.CheckPasswordSignInAsync(user, query.Password, false);
        if (!result.Succeeded)
        {
            return new LoginResult("", false, "Wrong Username/Email/Password");
        }
        var token = tokenGenerator.GenerateToken(user);
        return new LoginResult(token, true);
    }
}
