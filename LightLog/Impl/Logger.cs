using LightLog.Interfaces;

namespace LightLog.Impl;

public sealed class Logger : ILogger, IDisposable
{
    public TextWriter _textWriter { get; set; }

    public string DatePrefix => $"[{DateTime.Now}]";

    public Logger(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    public TextWriter Redirect(TextWriter textWriter)
    {
        (_textWriter, textWriter) = (textWriter, _textWriter);
        return textWriter;
    }

    public void Log(string log)
    {
        _textWriter.WriteLine($"{DatePrefix} {log}");
    }

    public void Dispose()
    {
        _textWriter.Dispose();
    }
}