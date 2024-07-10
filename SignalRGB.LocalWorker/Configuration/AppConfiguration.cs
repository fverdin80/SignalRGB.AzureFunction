using Microsoft.Extensions.Configuration;

namespace SignalRGB.LocalWorker.Configuration;
public class AppConfiguration : IAppConfiguration
{
    private readonly IConfiguration _configuration;

    public AppConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
    }

    public string ServiceBusConnectionString => _configuration.GetConnectionString("ServiceBusConnection") ?? string.Empty;
    public string QueueName => _configuration["ServiceBusQueue"] ?? string.Empty;
}

