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
        => $"{ILogger.DatePrefix}{logType.GetPrefix()} {log}";

    internal static string CommitLog(ILogger logger, string log, LogType logType, bool flushAfter)
    {
        log = InterpolateLogString(log, logType);
        logger.TextWriter.WriteLine(log);
        if (flushAfter)
            logger.TextWriter.Flush();

        return log;
    }

    internal static async Task<string> CommitLogAsync(ILogger logger, string log, LogType logType, bool flushAfter)
    {
        log = InterpolateLogString(log, logType);
        await logger.TextWriter.WriteLineAsync(log);
        if (flushAfter)
            await logger.TextWriter.FlushAsync();
        
        return log;
    }
}