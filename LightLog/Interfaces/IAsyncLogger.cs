namespace LightLog.Interfaces;

public interface IAsyncLogger : ILogger
{
    Task<bool> LogAsync(string log);

    Task<bool> WarnAsync(string log);

    Task<bool> ErrorAsync(string log);

    Task FlushAsync();
}