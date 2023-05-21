using LightLog.Interfaces;

namespace LightLog.Impl;

public partial class Logger
{
    private static Logger? _logger;
    
    public static Logger Shared
    {
        get => _logger ??= new Logger(Console.Out);
        set => _logger = value;
    }
}