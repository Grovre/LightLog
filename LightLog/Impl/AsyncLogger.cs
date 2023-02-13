using System.Collections.Concurrent;
using LightLog.Interfaces;

namespace LightLog.Impl;

public sealed class AsyncLogger : ILogger
{
    internal readonly BlockingCollection<string> BlockingQueue;
    internal readonly ManualResetEvent DoneLoggingEvent;
    private readonly CancellationTokenSource _cancellationToken;
    public TextWriter _textWriter { get; set; }

    public AsyncLogger(TextWriter textWriter)
    {
        BlockingQueue = new();
        DoneLoggingEvent = new(false);
        _cancellationToken = new();
        _textWriter = textWriter;

        new Thread(() =>
        {
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var log = BlockingQueue.Take(_cancellationToken.Token);
                    _textWriter.WriteLine(log);
                    if (BlockingQueue.IsCompleted)
                        _cancellationToken.Cancel();
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (InvalidOperationException)
            {
                
            }

            DoneLoggingEvent.Set();
        }).Start();
    }

    public void ForceStopLogging()
    {
        _cancellationToken.Cancel();
        BlockingQueue.CompleteAdding();
    }

    public bool Log(string log)
    {
        if (BlockingQueue.IsAddingCompleted)
            return false;
        Task.Run(() => BlockingQueue.Add($"{ILogger.DatePrefix} {log}"));
        return true;
    }

    public void CompleteLogging()
    {
        BlockingQueue.CompleteAdding();
    }

    public void WaitForFinishedLoggingEvent()
    {
        DoneLoggingEvent.WaitOne();
    }
    
    public void Dispose()
    {
        
        _cancellationToken.Cancel();
        DoneLoggingEvent.Set();
        BlockingQueue.CompleteAdding();
        _cancellationToken.Dispose();
        _textWriter.Dispose();
        BlockingQueue.Dispose();
        DoneLoggingEvent.Dispose();
    }
}