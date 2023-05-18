using System.Resources;

namespace LightLog.Interfaces;

public interface ILogger : IDisposable
{
    internal TextWriter TextWriter { get; }
    static string DatePrefix => $"[{DateTime.Now}]";
    static string InfoPrefix => "[INFO]";
    static string WarnPrefix => "[*WARNING*]";
    static string ErrorPrefix => "[**ERROR**]";

    bool Log(string log);

    bool Warn(string logWarning);

    bool Error(string logError);

    void Flush();
    Task FlushAsync();
    void Close();
}