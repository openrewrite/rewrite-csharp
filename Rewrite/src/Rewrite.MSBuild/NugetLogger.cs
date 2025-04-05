using Microsoft.Extensions.Logging;
using NuGet.Common;
using ILogger = NuGet.Common.ILogger;
using LogLevel = NuGet.Common.LogLevel;


namespace Rewrite.MSBuild;

public class NugetLogger : NuGet.Common.ILogger
{
    private static Microsoft.Extensions.Logging.ILogger _log = null!;

    public NugetLogger(ILogger<NugetLogger> log)
    {
        _log = log;
    }
    

    public void LogDebug(string data) => _log.LogDebug(data);

    public void LogVerbose(string data) => _log.LogDebug(data);

    public void LogInformation(string data) => _log.LogInformation(data);

    public void LogMinimal(string data) => _log.LogInformation(data);

    public void LogWarning(string data) => _log.LogWarning(data);

    public void LogError(string data) => _log.LogError(data);

    public void LogInformationSummary(string data) => _log.LogInformation(data);

    public void Log(LogLevel level, string data)
    {
        switch (level)
        {
            case LogLevel.Debug:
            case LogLevel.Verbose:
                _log.LogDebug(data);
                break;
            case LogLevel.Information:
            case LogLevel.Minimal:
                _log.LogInformation(data);
                break;
            case LogLevel.Warning:
                _log.LogWarning(data);
                break;
            case LogLevel.Error:
                _log.LogError(data);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }

    public Task LogAsync(LogLevel level, string data)
    {
        Log(level, data);
        return Task.CompletedTask;
    }

    public void Log(ILogMessage message) => Log(message.Level, message.Message);

    public Task LogAsync(ILogMessage message) => LogAsync(message.Level, message.Message);
}