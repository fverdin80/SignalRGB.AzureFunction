using Azure.Messaging.ServiceBus;
using SignalRGB.AzureFunction.Configuration;

namespace SignalRGB.AzureFunction.Services;
public class ServiceBusSenderService : IServiceBusSenderService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly IAppConfiguration _appConfiguration;

    public ServiceBusSenderService(ServiceBusClient serviceBusClient, IAppConfiguration appConfiguration)
    {
        _serviceBusClient = serviceBusClient;
        _appConfiguration = appConfiguration;
    }

    public async Task SendMessageAsync(string messageContent)
    {
        ServiceBusSender sender = _serviceBusClient.CreateSender(_appConfiguration.ServiceBusQueue);
        ServiceBusMessage message = new ServiceBusMessage(messageContent);

        await sender.SendMessageAsync(message);
    }
}