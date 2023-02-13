namespace LightLog.Interfaces;

public interface ILogger : IDisposable
{
    internal TextWriter _textWriter { get; set; }
    static string DatePrefix => $"[{DateTime.Now}]";

    bool Log(string log);
}