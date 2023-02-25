using LightLog.Interfaces;

namespace LightLog.Impl;

public sealed class ToggleLogger : ILogger
{
    internal ILogger _backingLogger { get; set; }
    public bool IsActive { get; internal set; }

    public TextWriter TextWriter => _backingLogger.TextWriter;

    internal ToggleLogger(ILogger backingLogger)
    {
        _backingLogger = backingLogger;
    }

    public void Toggle(bool enabled)
    {
        IsActive = enabled;
    }

    public bool Log(string log)
    {
        return IsActive ? _backingLogger.Log(log) : false;
    }

    public bool Warn(string logWarning)
    {
        return IsActive ? _backingLogger.Warn(logWarning) : false;
    }

    public bool Error(string logError)
    {
        return IsActive ? _backingLogger.Error(logError) : false;
    }
    
    public void Dispose()
    {
        _backingLogger.Dispose();
    }
}