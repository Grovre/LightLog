namespace LightLog.Interfaces;

public interface IFileLogger
{
    static abstract ILogger OpenFileLogger(string path);
}