using SignalRGB.AzureFunction.Configuration;
using SignalRGB.AzureFunction.Models;
using System.Text.Json;

namespace SignalRGB.AzureFunction.Services;
public class SteamHelperService : ISteamHelperService
{
    private readonly IAppConfiguration _appConfiguration;

    public SteamHelperService(IAppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }

    public async Task<OnlineStatus> GetSteamStatus()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_appConfiguration.SteamApiKey}&steamids={_appConfiguration.SteamId}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response using System.Text.Json
            var steamResponse = JsonSerializer.Deserialize<SteamResponse>(responseBody);
            var onlineStatus = steamResponse?.Response?.Players?.FirstOrDefault()?.OnlineStatus;

            return onlineStatus ?? OnlineStatus.Offline;
        }
    }
}

