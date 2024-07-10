using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace SignalRGB.AzureFunction;
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
}