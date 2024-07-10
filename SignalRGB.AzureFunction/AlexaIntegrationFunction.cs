using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalRGB.AzureFunction.Configuration;
using SignalRGB.AzureFunction.Services;

namespace SignalRGB.AzureFunction;
public class AlexaIntegrationFunction
{
    private readonly ILogger<AlexaIntegrationFunction> _logger;
    private readonly IAppConfiguration _appConfiguration;
    private readonly IServiceBusSenderService _serviceBusSenderService;
    public AlexaIntegrationFunction(ILogger<AlexaIntegrationFunction> logger, IAppConfiguration appConfiguration, IServiceBusSenderService serviceBusSenderService)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
        _serviceBusSenderService = serviceBusSenderService;
    }

    [Function("AlexaIntegration")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        var queueMessage = _appConfiguration.GameModeMessage;

        _logger.LogInformation($"Sending \"{queueMessage}\" message to Azure Service Bus");
        // Send the message to the Azure Bus queue
        await _serviceBusSenderService.SendMessageAsync(queueMessage);

        return new OkObjectResult("Message sent to the Azure Service Bus queue");
    }
}

