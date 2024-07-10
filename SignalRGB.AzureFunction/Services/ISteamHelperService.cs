using SignalRGB.AzureFunction.Models;

namespace SignalRGB.AzureFunction.Services;
public interface ISteamHelperService
{
    Task<OnlineStatus> GetSteamStatus();
}

