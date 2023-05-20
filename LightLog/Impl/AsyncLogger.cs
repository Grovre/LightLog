using System.Collections.Concurrent;
using LightLog.Impl.Helpers;
using LightLog.Interfaces;

namespace LightLog.Impl;

/// <summary>
/// A logger class that is like the base Logger but with
/// async capabilities
/// </summary>
public sealed class AsyncLogger : IAsyncLogger, IFileLogger, IRedirection<TextWriter>
{
    public TextWriter TextWriter { get; private set; }

    /// <summary>
    /// Instantiates an AsyncLogger
    /// </summary>
    /// <param name="textWriter">TextWriter to write to</param>
    public AsyncLogger(TextWriter textWriter)
    {
        TextWriter = textWriter;
    }

    /// <summary>
    /// Adds the string to the logs that are
    /// being printed. Everything that happens
    /// in this method happens asynchronously.
    /// </summary>
    /// <param name="log">The string to print, prefixed by the time of logging</param>
    /// <returns>Whether the log was added or not. Will always be true unless this logger has completed logging.</returns>
    public bool Log(string log)
    {
        return LoggerHelper.CommitLog(this, log, LogType.Log, true);
    }

    public bool Warn(string logWarning)
    {
        return LoggerHelper.CommitLog(this, logWarning, LogType.Log, true);
    }

    public bool Error(string logError)
    {
        return LoggerHelper.CommitLog(this, logError, LogType.Log, true);
    }

    /// <inheritdoc cref="TextWriter"/>
    public void Flush()
    {
        TextWriter.Flush();
    }

    /// <inheritdoc cref="TextWriter"/>
    public async Task FlushAsync()
    {
        await TextWriter.FlushAsync();
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

    public static ILogger OpenFileLogger(string path)
    {
        var fs = File.OpenWrite(path);
        var writer = new StreamWriter(fs);
        var logger = new AsyncLogger(writer);
        return logger;
    }

    public TextWriter Redirect(TextWriter o)
    {
        (TextWriter, o) = (o, TextWriter);
        return o;
    }

    public async Task<bool> LogAsync(string log)
    {
        return await LoggerHelper.CommitLogAsync(this, log, LogType.Log, true);
    }

    public async Task<bool> WarnAsync(string log)
    {
        return await LoggerHelper.CommitLogAsync(this, log, LogType.Warning, true);
    }

    public async Task<bool> ErrorAsync(string log)
    {
        return await LoggerHelper.CommitLogAsync(this, log, LogType.Error, true);
    }
}