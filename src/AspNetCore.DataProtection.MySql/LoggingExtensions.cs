using Microsoft.Extensions.Logging;

namespace AspNetCore.DataProtection.MySql;

internal static partial class LoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Reading data with key {FriendlyName}", EventName = "ReadKeyFromElement")]
    public static partial void ReadingXmlFromKey(this ILogger<MySqlXmlRepository> logger, string? friendlyName);

    [LoggerMessage(2, LogLevel.Information, "Saving key {FriendlyName}", EventName = "SavingKey")]
    public static partial void LogSavingKey(this ILogger<MySqlXmlRepository> logger, string friendlyName);
}
