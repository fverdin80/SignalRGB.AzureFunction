using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRGB.LocalWorker.Configuration;
using SignalRGB.LocalWorker.Services;
using System.Reflection;

namespace SignalRGB.LocalWorker.Extensions;
public static class HostBuilderExtensions
{
    public static IHostBuilder SetupConfigurations(this IHostBuilder builder, Assembly assembly)
    {
        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var environment = hostingContext.HostingEnvironment;

            if (environment.IsDevelopment())
                config.AddUserSecrets(assembly, optional: true);

            config.AddEnvironmentVariables();
        });

        return builder;
    }

    public static IHostBuilder ConfigureAppLogging(this IHostBuilder builder)
    {
        return builder.ConfigureLogging(logging =>
        {
            logging.AddConsole();
        });
    }

    public static IHostBuilder ConfigureAppServices(this IHostBuilder builder)
    {
        return builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IAppConfiguration>();
                return new ServiceBusClient(configuration.ServiceBusConnectionString);
            });
            services.AddSingleton(provider =>
            {
                var client = provider.GetRequiredService<ServiceBusClient>();
                var configuration = provider.GetRequiredService<IAppConfiguration>();

                return client.CreateProcessor(configuration.QueueName, new ServiceBusProcessorOptions());
            });

            services.AddHostedService<SignalRgbService>();
        });
    }
}

