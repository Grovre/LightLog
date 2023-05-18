using System.Collections.Concurrent;
using LightLog.Interfaces;

namespace LightLog.Impl;

/// <summary>
/// A logger class that runs asynchronously on its own thread.
/// Logging only passes the string and everything else will be
/// done on the logger thread to avoid the loss of time in
/// string interpolation and printing. Order of concurrent logs is not guaranteed.
/// </summary>
public sealed class AsyncLogger : ILogger
{
    internal readonly BlockingCollection<string> BlockingQueue;
    internal readonly ManualResetEvent DoneLoggingEvent;
    private readonly CancellationTokenSource _cancellationToken;
    public TextWriter TextWriter { get; private set; }

    /// <summary>
    /// Instantiates an AsyncLogger
    /// </summary>
    /// <param name="textWriter">TextWriter to write to</param>
    public AsyncLogger(TextWriter textWriter)
    {
        BlockingQueue = new();
        DoneLoggingEvent = new(false);
        _cancellationToken = new();
        TextWriter = textWriter;

        new Thread(() =>
        {
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var log = BlockingQueue.Take(_cancellationToken.Token);
                    TextWriter.WriteLine(log);
                    if (BlockingQueue.IsCompleted)
                        _cancellationToken.Cancel();
                }
            }
            catch (OperationCanceledException)
            {
                // Stopping a blocking queue Take is intended if the cancellation token ended the method
            }
            catch (InvalidOperationException)
            {
                // See catch above
            }

            DoneLoggingEvent.Set();
        }).Start();
    }

    /// <summary>
    /// Forces the logging thread to stop allowing new
    /// logs to be added, stop writing, and exits the thread
    /// peacefully. Any threads that have called
    /// <code>WaitForFinishedLoggingEvent</code>
    /// will stop waiting and continue.
    /// </summary>
    public void ForceStopLogging()
    {
        _cancellationToken.Cancel();
        BlockingQueue.CompleteAdding();
        CompleteLogging();
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
        if (BlockingQueue.IsAddingCompleted)
            return false;
        Task.Run(() => BlockingQueue.Add($"{ILogger.DatePrefix} {log}"));
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
    /// Denies any more logs to be logged by this
    /// logger. The logger will finish printing the
    /// remaining logs until disposed of, or with
    /// the guarantee of finishing if
    /// <code>WaitForFinishedLoggingEvent</code>
    /// was called.
    /// </summary>
    public void CompleteLogging()
    {
        BlockingQueue.CompleteAdding();
    }

    /// <summary>
    /// Blocks the calling thread until the
    /// logger has completed and is empty,
    /// or forced stop.
    /// </summary>
    public void WaitForFinishedLoggingEvent()
    {
        DoneLoggingEvent.WaitOne();
    }
    
    /// <summary>
    /// Disposes of everything this object
    /// uses internally, along with the TextWriter
    /// used to write to.
    /// </summary>
    public void Dispose()
    {
        
        _cancellationToken.Cancel();
        DoneLoggingEvent.Set();
        BlockingQueue.CompleteAdding();
        _cancellationToken.Dispose();
        TextWriter.Dispose();
        BlockingQueue.Dispose();
        DoneLoggingEvent.Dispose();
    }
}