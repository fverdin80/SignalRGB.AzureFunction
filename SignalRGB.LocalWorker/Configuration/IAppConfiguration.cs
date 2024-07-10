namespace SignalRGB.LocalWorker.Configuration;
public interface IAppConfiguration
{
    string ServiceBusConnectionString { get; }
    string QueueName { get; }
}

