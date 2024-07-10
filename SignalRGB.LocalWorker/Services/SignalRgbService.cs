using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SignalRGB.LocalWorker.Services;
public class SignalRgbService : IHostedService, ISignalRgbService
{
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<SignalRgbService> _logger;

    public SignalRgbService(ServiceBusProcessor processor, ILogger<SignalRgbService> logger)
    {
        _processor = processor;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(cancellationToken);

        _logger.LogInformation("Service started and processing messages.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);

        _processor.ProcessMessageAsync -= MessageHandler;
        _processor.ProcessErrorAsync -= ErrorHandler;

        _logger.LogInformation("Service stopped.");
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string effectName = args.Message.Body.ToString();
        _logger.LogInformation($"Received: {effectName}");
        try
        {
            if (!string.IsNullOrWhiteSpace(effectName))
            {
                var url = $"signalrgb://effect/apply/{effectName}?-silentlaunch-";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to apply effect {EffectName}", effectName);
        }

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Message handler encountered an exception. ErrorSource: {ErrorSource}, Entity Path: {EntityPath}, FullyQualifiedNamespace: {FullyQualifiedNamespace}",
            args.ErrorSource, args.EntityPath, args.FullyQualifiedNamespace);

        return Task.CompletedTask;
    }
}

