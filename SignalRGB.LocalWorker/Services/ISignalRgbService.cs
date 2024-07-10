
namespace SignalRGB.LocalWorker.Services
{
    public interface ISignalRgbService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}