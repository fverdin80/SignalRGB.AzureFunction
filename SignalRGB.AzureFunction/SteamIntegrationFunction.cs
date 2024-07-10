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
        private readonly IServiceBusSenderService _serviceBusSenderService;
        private readonly ISteamHelperService _steamHelperService;

        public SteamIntegrationFunction(ILogger<SteamIntegrationFunction> logger, IAppConfiguration appConfiguration, IServiceBusSenderService serviceBusSenderService, ISteamHelperService steamHelperService)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
            _serviceBusSenderService = serviceBusSenderService;
            _steamHelperService = steamHelperService;
        }

        // Timer trigger function that runs every 15 seconds
        [Function("SteamIntegration")]
        public async Task Run([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Checking Steam status: {DateTime.Now}");

            var status = await _steamHelperService.GetSteamStatus();

            _logger.LogInformation($"Steam status: {status}");

            if (status == OnlineStatus.Online)
            {
                await _serviceBusSenderService.SendMessageAsync(_appConfiguration.GameModeMessage);
            }
        }
    }
}
