using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRGB.AzureFunction;
using SignalRGB.AzureFunction.Configuration;
using SignalRGB.AzureFunction.Middleware;
using SignalRGB.AzureFunction.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(worker =>
    {
        worker.UseMiddleware<LoggingMiddleware>();
    })
    .SetupConfigurations(ProjectInfo.Assembly)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IAppConfiguration, AppConfiguration>();
        services.AddScoped<IServiceBusSenderService, ServiceBusSenderService>();
        services.AddScoped<ISteamHelperService, SteamHelperService>();
        services.AddSingleton<ServiceBusClient>(provider =>
        {
            var configuration = provider.GetRequiredService<IAppConfiguration>();
            return new ServiceBusClient(configuration.ServiceBusConnectionString);
        });
    })
    .Build();

host.Run();