namespace LightLog.Interfaces;

public interface ILogger : IDisposable
{
    internal TextWriter TextWriter { get; set; }
    static string DatePrefix => $"[{DateTime.Now}]";
    static string WarnPrefix => "*[WARNING]";
    static string ErrorPrefix => "***[ERROR]";

    bool Log(string log);

    bool Warn(string logWarning);

    bool Error(string logError);
}