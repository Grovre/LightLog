using LightLog.Interfaces;

namespace LightLog.Impl;

public enum LogType
{
    Log,
    Info,
    Warning,
    Error,
}

public static class LogTypeHelper
{
    public static string GetPrefix(this LogType logType)
    {
        var prefix = logType switch
        {
            LogType.Log => string.Empty,
            LogType.Warning => ILogger.WarnPrefix,
            LogType.Error => ILogger.ErrorPrefix,
            LogType.Info => ILogger.InfoPrefix,
            _ => string.Empty
        };

        return prefix;
    }
}