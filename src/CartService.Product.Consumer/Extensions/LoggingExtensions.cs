using Microsoft.Extensions.Logging;

namespace CartService.Product.Consumer.Extensions;

public static class LoggingExtensions
{
    private static readonly Action<ILogger, string, Exception?> _logMessageId =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1000, nameof(LogProcessingMessage)),
            "Processing Message with ID: {Id}");

    public static void LogProcessingMessage(this ILogger logger, string id)
    {
        _logMessageId(logger, id, null);
    }
}