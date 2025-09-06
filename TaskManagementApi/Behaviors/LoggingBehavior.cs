﻿namespace TaskManagementApi.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Log begin of the request
        logger.LogInformation("[BEGIN] Handle request={Request}" +
            " - Response={Response}" +
            " - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request
            );


        var response = await next();

        // Log end of request
        logger.LogInformation("[END] Handled {Request} with {Response}",
            typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}
