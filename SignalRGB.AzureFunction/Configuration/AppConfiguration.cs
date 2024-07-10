using Microsoft.Extensions.Configuration;

namespace SignalRGB.AzureFunction.Configuration;
public class AppConfiguration : IAppConfiguration
{
    private readonly IConfiguration _configuration;

    public AppConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ServiceBusConnectionString => _configuration["ConnectionStrings:ServiceBusConnection"] ?? string.Empty;
    public string ServiceBusQueue => _configuration["ServiceBusQueue"] ?? string.Empty;
    public string SteamApiKey => _configuration["SteamApiKey"] ?? string.Empty;
    public string SteamId => _configuration["SteamId"] ?? string.Empty;
    public string GameModeMessage => _configuration["GameModeMessage"] ?? string.Empty;
}

