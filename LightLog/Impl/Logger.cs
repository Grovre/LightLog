using LightLog.Interfaces;

namespace LightLog.Impl;

public sealed class Logger : ILogger, IRedirection<TextWriter>
{
    public TextWriter _textWriter { get; set; }

    public Logger(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    public TextWriter Redirect(TextWriter textWriter)
    {
        (_textWriter, textWriter) = (textWriter, _textWriter);
        return textWriter;
    }

    public bool Log(string log)
    {
        _textWriter.WriteLine($"{ILogger.DatePrefix} {log}");
        return true;
    }

    public void Dispose()
    {
        _textWriter.Dispose();
    }
}