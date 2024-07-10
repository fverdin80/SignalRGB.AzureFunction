namespace SignalRGB.AzureFunction.Configuration;

public interface IAppConfiguration
{
    string ServiceBusConnectionString { get; }
    string ServiceBusQueue { get; }
    string SteamApiKey { get; }
    string SteamId { get; }
    string GameModeMessage { get; }
}

