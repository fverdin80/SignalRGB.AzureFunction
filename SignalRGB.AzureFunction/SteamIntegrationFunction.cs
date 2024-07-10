using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalRGB.AzureFunction.Configuration;
using SignalRGB.AzureFunction.Models;
using SignalRGB.AzureFunction.Services;
using TimerInfo = SignalRGB.AzureFunction.Models.TimerInfo;

namespace SignalRGB.AzureFunction
{
    public class SteamIntegrationFunction
    {
        private readonly ILogger<SteamIntegrationFunction> _logger;
        private readonly IAppConfiguration _appConfiguration;
        private readonly ISteamHelperService _steamHelperService;

        public SteamIntegrationFunction(ILogger<SteamIntegrationFunction> logger, IAppConfiguration appConfiguration, ISteamHelperService steamHelperService)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
            _steamHelperService = steamHelperService;
        }

        // Timer trigger function that runs every 15 seconds
        [Function("SteamIntegration")]
        [ServiceBusOutput("%ServiceBusQueue%", Connection = "ServiceBusConnection")]
        public async Task<string?> Run([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Checking Steam status: {DateTime.Now}");

            var queueMessage = _appConfiguration.GameModeMessage;
            var status = await _steamHelperService.GetSteamStatus();

            _logger.LogInformation($"Steam status: {status}");

            //Send message using Service Bus binding
            if (status == OnlineStatus.Online) return queueMessage;

            return null;
        }
    }
}
