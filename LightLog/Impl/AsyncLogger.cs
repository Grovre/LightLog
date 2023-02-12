using System.Collections.Concurrent;
using LightLog.Interfaces;

namespace LightLog.Impl;

public sealed class AsyncLogger : ILogger
{
    internal BlockingCollection<string> _blockingQueue;
    private CancellationTokenSource _cancellationToken;
    public TextWriter _textWriter { get; set; }

    public AsyncLogger(TextWriter textWriter)
    {
        _blockingQueue = new();
        _cancellationToken = new();
        _textWriter = textWriter;

        new Thread(() =>
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var log = _blockingQueue.Take(_cancellationToken.Token);
                _textWriter.WriteLine($"{ILogger.DatePrefix} {log}");
            }
        }).Start();
    }

    public void StopLogging()
    {
        _cancellationToken.Cancel();
    }

    public void Log(string log)
    {
        
    }
    
    public void Dispose()
    {
        _textWriter.Dispose();
    }
}