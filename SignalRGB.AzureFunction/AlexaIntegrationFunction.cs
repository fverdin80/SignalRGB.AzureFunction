using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalRGB.AzureFunction.Configuration;

namespace SignalRGB.AzureFunction;
public class AlexaIntegrationFunction
{
    private readonly ILogger<AlexaIntegrationFunction> _logger;
    private readonly IAppConfiguration _appConfiguration;
    public AlexaIntegrationFunction(ILogger<AlexaIntegrationFunction> logger, IAppConfiguration appConfiguration)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
    }

    [Function("AlexaIntegration")]
    [ServiceBusOutput("%ServiceBusQueue%", Connection = "ServiceBusConnection")]
    public string Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        var queueMessage = _appConfiguration.GameModeMessage;

        _logger.LogInformation($"Sending \"{queueMessage}\" message to Azure Service Bus");

        // Send the message to the Azure Bus queue by using the Service Bus Binding
        return queueMessage;
    }
}

