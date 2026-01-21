#nullable disable

namespace Rewrite.MSBuild.Restore;

internal enum DependencyType
{
    Unknown,
    Target,
    Diagnostic,
    Package,
    Assembly,
    FrameworkAssembly,
    AnalyzerAssembly,
    Content,
    Project,
    ExternalProject,
    Reference,
    Winmd,
    Unresolved
}