namespace TaskManagementApi.Auth.Register;

public record RegisterCommand(string Username, string Email, string Password) : ICommand<RegisterResult>;
public record RegisterResult(bool IsSuccess, string? Error = null);

public class RegisterHandler(
    UserManager<AppUser> userManager
    )
    : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // check for existing user
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user != null)
            return new RegisterResult(false, "This email already registered!");

        var newUser = new AppUser
        {
            UserName = command.Username,
            Email = command.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var identityResult = await userManager.CreateAsync(newUser, command.Password);
        if (identityResult.Errors.Any())
        {
            return new RegisterResult(false, identityResult.Errors.First().Description);
        }
        return new RegisterResult(true);
    }
}
