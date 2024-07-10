using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRGB.AzureFunction.Configuration;
using SignalRGB.AzureFunction.Middleware;
using SignalRGB.AzureFunction.Services;
using System.Reflection;

namespace SignalRGB.AzureFunction.Extensions;
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

    public static IHostBuilder ConfigureMiddleware(this IHostBuilder builder)
    {
        return builder.ConfigureFunctionsWebApplication(worker =>
        {
            worker.UseMiddleware<LoggingMiddleware>();
        });
    }

    public static IHostBuilder ConfigureAppServices(this IHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddScoped<ISteamHelperService, SteamHelperService>();
        });
    }
}