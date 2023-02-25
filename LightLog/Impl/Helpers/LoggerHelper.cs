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
}