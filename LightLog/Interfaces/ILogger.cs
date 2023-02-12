namespace LightLog.Interfaces;

public interface ILogger : IDisposable
{
    TextWriter _textWriter { get; internal set; }
    static string DatePrefix => $"[{DateTime.Now}]";

    void Log(string log);
}