using LightLog.Interfaces;

namespace LightLog.Impl;

/// <summary>
/// Sealed wrapper class for an ILogger that includes a
/// boolean toggle that enables and disables logging to
/// provide greater control of logs to the user
/// </summary>
public sealed class ToggleLogger : ILogger
{
    internal ILogger _backingLogger { get; set; }
    /// <summary>
    /// A boolean determining whether or not to actively log.
    /// Set with the toggle method.
    /// </summary>
    public bool IsActive { get; internal set; }

    public TextWriter TextWriter => _backingLogger.TextWriter;

    internal ToggleLogger(ILogger backingLogger)
    {
        _backingLogger = backingLogger;
    }

    /// <summary>
    /// Toggles whether logging should be enabled or disabled.
    /// </summary>
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