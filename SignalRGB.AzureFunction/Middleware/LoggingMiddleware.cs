using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace SignalRGB.AzureFunction.Middleware;

public class LoggingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger _logger;

    public LoggingMiddleware(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var functionName = context.FunctionDefinition.Name;
        try
        {
            _logger.LogInformation("Executing function {FunctionName}", functionName);

            await next(context);

            _logger.LogInformation("Executed function {FunctionName}", functionName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was an error while executing function {FunctionName}", functionName);
            throw;
        }
    }
}

