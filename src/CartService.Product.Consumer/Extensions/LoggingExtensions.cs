using Microsoft.Extensions.Logging;

namespace CartService.Product.Consumer.Extensions;

public static class LoggingExtensions
{
    private static readonly Action<ILogger, string, Exception?> _logMessageId =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1000, nameof(LogProcessingMessage)),
            "Processing Message with content: {Content}");

    public static void LogProcessingMessage(this ILogger logger, string content)
    {
        _logMessageId(logger, content, null);
    }
}