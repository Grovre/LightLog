namespace LightLog.Interfaces;

public interface ILogger
{
    TextWriter _textWriter { get; internal set; }

    void Log(string log);
}