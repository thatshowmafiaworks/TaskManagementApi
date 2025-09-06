namespace TaskManagementApi.Behaviors;

public class CustomExceptionHandling(
    ILogger<CustomExceptionHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError($"Error Message: {exception.Message}, TimeAt:{DateTime.UtcNow}");
        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Title = exception.GetType().Name,
            Status = StatusCodes.Status500InternalServerError,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }
}
