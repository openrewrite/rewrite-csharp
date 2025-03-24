using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using ExecutionContext = System.Threading.ExecutionContext;

namespace Rewrite.MSBuild;

public class ProjectContext : INuGetProjectContext
{
    public void Log(MessageLevel level, string message, params object[] args)
    {
        // Do your logging here...
    }

    public void Log(ILogMessage message)
    {
    }

    public void ReportError(ILogMessage message)
    {
    }

    public FileConflictAction ResolveFileConflict(string message) => FileConflictAction.Ignore;

    public PackageExtractionContext PackageExtractionContext { get; set; } = null!;

    NuGet.ProjectManagement.ExecutionContext INuGetProjectContext.ExecutionContext { get; } = null!;

    public XDocument OriginalPackagesConfig { get; set; } = null!;

    public ISourceControlManagerProvider SourceControlManagerProvider => null!;

    public ExecutionContext ExecutionContext => null!;

    public void ReportError(string message)
    {
    }

    public NuGetActionType ActionType { get; set; }
    public Guid OperationId { get; set; }
}