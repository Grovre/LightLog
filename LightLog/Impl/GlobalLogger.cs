using LightLog.Interfaces;

namespace LightLog.Impl;

public partial class Logger
{
    private static ILogger? _globalLogger;

    /// <summary>
    /// A static and globally-shared logger for simple use cases
    /// </summary>
    public static ILogger GlobalLogger
    {
        get
        {
            if (_globalLogger == null)
            {
                _globalLogger = new Logger(Console.Out);
            }

            return _globalLogger;
        }

        internal set => _globalLogger = value;
    }
}