using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using Serilog.Events;
using ExecutionContext = System.Threading.ExecutionContext;

namespace Rewrite.MSBuild;

public class ProjectContext : INuGetProjectContext
{
    private static Serilog.ILogger _logger = Serilog.Log.ForContext<ProjectContext>();
    
    public LogEventLevel GetLogLevel(MessageLevel level) => level switch
    {
        MessageLevel.Debug => LogEventLevel.Debug,
        MessageLevel.Info => LogEventLevel.Information,
        MessageLevel.Warning => LogEventLevel.Warning,
        MessageLevel.Error => LogEventLevel.Error,
        _ => LogEventLevel.Debug
    };
    
    public LogEventLevel GetLogLevel(LogLevel level) => level switch
    {
        LogLevel.Verbose => LogEventLevel.Verbose,
        LogLevel.Debug => LogEventLevel.Debug,
        LogLevel.Information => LogEventLevel.Information,
        LogLevel.Warning => LogEventLevel.Warning,
        LogLevel.Error => LogEventLevel.Error,
        LogLevel.Minimal => LogEventLevel.Error,
        _ => LogEventLevel.Debug
    };
    public void Log(MessageLevel level, string message, params object[] args) => _logger.Write(GetLogLevel(level), message, args);

    public void Log(ILogMessage message) => _logger.Write(GetLogLevel(message.Level), message.Message);

    public void ReportError(ILogMessage message) => _logger.Error(message.Message);

    public void ReportError(string message) => _logger.Error(message);

    public FileConflictAction ResolveFileConflict(string message) => FileConflictAction.Ignore;

    public PackageExtractionContext PackageExtractionContext { get; set; } = null!;

    NuGet.ProjectManagement.ExecutionContext INuGetProjectContext.ExecutionContext { get; } = null!;

    public XDocument OriginalPackagesConfig { get; set; } = null!;

    public ISourceControlManagerProvider SourceControlManagerProvider => null!;

    public ExecutionContext ExecutionContext => null!;

    public NuGetActionType ActionType { get; set; }
    public Guid OperationId { get; set; }
}