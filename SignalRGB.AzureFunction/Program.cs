using Microsoft.Extensions.Hosting;
using SignalRGB.AzureFunction;
using SignalRGB.AzureFunction.Extensions;

var host = new HostBuilder()
    .ConfigureMiddleware()
    .SetupConfigurations(ProjectInfo.Assembly)
    .ConfigureAppServices()
    .Build();

host.Run();