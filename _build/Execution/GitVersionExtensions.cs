using Nuke.Common.Tools.NerdbankGitVersioning;

namespace Nuke.Common.Execution;

public static class GitVersionExtensions
{
    public static string MavenSnapshotVersion(this NerdbankGitVersioning version) => $"{version.MajorMinorVersion}.0-SNAPSHOT";
}
