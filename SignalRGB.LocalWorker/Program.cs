using Microsoft.Extensions.Hosting;
using SignalRGB.LocalWorker;
using SignalRGB.LocalWorker.Extensions;

var host = Host.CreateDefaultBuilder(args)
            .SetupConfigurations(ProjectInfo.Assembly)
            .ConfigureAppServices()
            .ConfigureAppLogging()
            .Build();

await host.RunAsync();