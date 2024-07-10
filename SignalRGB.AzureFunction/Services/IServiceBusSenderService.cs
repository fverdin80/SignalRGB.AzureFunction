namespace SignalRGB.AzureFunction.Services;
public interface IServiceBusSenderService
{
    Task SendMessageAsync(string messageContent);
}