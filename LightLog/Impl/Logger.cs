using LightLog.Impl.Helpers;
using LightLog.Interfaces;

namespace LightLog.Impl;

/// <summary>
/// A synchronous logger that writes to the given
/// TextWriter, prefixed by the local date and time.
/// </summary>
public sealed partial class Logger : ILogger, IFileLogger, IRedirection<TextWriter>
{
    public TextWriter TextWriter { get; private set; }
    public event Action<Logger>? PreLogActions;
    public event Action<Logger>? PostLogActions;

    /// <summary>
    /// Creates a new Logger for logging
    /// </summary>
    /// <param name="textWriter">The TextWriter to log to</param>
    public Logger(TextWriter textWriter)
    {
        TextWriter = textWriter;
    }

    /// <summary>
    /// Changes the TextWriter used to the given one and returns
    /// the previous one in use. Remember to dispose the
    /// returned TextWriter if it is no longer
    /// going to be used.
    /// </summary>
    /// <param name="textWriter"></param>
    /// <returns>The previous TextWriter in use</returns>
    public TextWriter Redirect(TextWriter textWriter)
    {
        (TextWriter, textWriter) = (textWriter, TextWriter);
        return textWriter;
    }

    /// <summary>
    /// Writes a log to the TextWriter that is
    /// prefixed by DateTime.Now
    /// </summary>
    /// <param name="log">The log to write</param>
    /// <returns>Whether the log was written correctly. Always true.</returns>
    public bool Log(string log)
    {
        PreLogActions?.Invoke(this);
        LoggerHelper.CommitLog(this, log, LogType.Log, true);
        PostLogActions?.Invoke(this);
        return true;
    }
    
    public bool Warn(string logWarning)
    {
        PreLogActions?.Invoke(this);
        LoggerHelper.CommitLog(this, logWarning, LogType.Warning, true);
        PostLogActions?.Invoke(this);
        return true;
    }

    public bool Error(string logError)
    {
        PreLogActions?.Invoke(this);
        LoggerHelper.CommitLog(this, logError, LogType.Error, true);
        PostLogActions?.Invoke(this);
        return true;
    }

    /// <inheritdoc cref="TextWriter"/>
    public void Flush()
    {
        TextWriter.Flush();
    }

    /// <inheritdoc cref="TextWriter"/>
    public void Close()
    {
        TextWriter.Close();
    }

    /// <summary>
    /// Disposes of everything this object
    /// uses internally, along with the TextWriter
    /// used to write to.
    /// </summary>
    public void Dispose()
    {
        TextWriter.Dispose();
    }

    /// <summary>
    /// Opens a file to log to with the defined path
    /// </summary>
    /// <param name="path">The path of the file</param>
    /// <returns>A new logger with a StreamWriter to the given file</returns>
    public static ILogger OpenFileLogger(string path)
    {
        var fs = File.OpenWrite(path);
        var writer = new StreamWriter(fs);
        var logger = new Logger(writer);
        return logger;
    }
}