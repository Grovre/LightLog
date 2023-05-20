using System.Diagnostics;
using LightLog.Interfaces;

namespace LightLog.Impl.Helpers;

public static class LoggerHelper
{
    public static ToggleLogger UseForDebugging(this ILogger logger, bool startEnabled = true)
    {
        if (logger is ToggleLogger debugLogger)
            return debugLogger;
        
        var dbgLogger = new ToggleLogger(logger);
        dbgLogger.Toggle(enabled: startEnabled);
        return dbgLogger;
    }

    internal static string InterpolateLogString(string log, LogType logType)
        => $"{ILogger.DatePrefix} {logType.GetPrefix()}{log}";

    internal static bool CommitLog(ILogger logger, string log, LogType logType, bool flushAfter)
    {
        logger.TextWriter.WriteLine(InterpolateLogString(log, logType));
        if (flushAfter)
            logger.TextWriter.Flush();
        return true;
    }

    internal static async Task<bool> CommitLogAsync(ILogger logger, string log, LogType logType, bool flushAfter)
    {
        await logger.TextWriter.WriteLineAsync(InterpolateLogString(log, logType));
        if (flushAfter)
            await logger.TextWriter.FlushAsync();

        return true;
    }
}