using LightLog.Interfaces;

namespace LightLog.Impl;

/// <summary>
/// A synchronous logger that writes to the given
/// TextWriter, prefixed by the local date and time.
/// </summary>
public sealed class Logger : ILogger, IRedirection<TextWriter>
{
    public TextWriter TextWriter { get; private set; }

    /// <summary>
    /// Creates a new Logger for logging
    /// </summary>
    /// <param name="textWriter">The TextWriter to log to</param>
    public Logger(TextWriter textWriter)
    {
        TextWriter = textWriter;
    }

    /// <summary>
    /// Changes the TextWriter used and returns
    /// the previous one in use. Dispose the
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
        TextWriter.WriteLine($"{ILogger.DatePrefix} {log}");
        return true;
    }
    
    public bool Warn(string logWarning)
    {
        return Log($"{ILogger.WarnPrefix}{logWarning}");
    }

    public bool Error(string logError)
    {
        return Log($"{ILogger.ErrorPrefix}{logError}");
    }

    /// <summary>
    /// Disposes of everything this object
    /// uses internally, along with the TextWriter
    /// used to write to.
    /// </summary>
    /// </summary>
    public void Dispose()
    {
        TextWriter.Dispose();
    }
}