
using System;
using Nuke.Common;
using Nuke.Common.Tooling;
using Serilog;

partial class GradleTasks
{
    protected override Action<OutputType, string> GetLogger(ToolOptions options = null)
        => (type, message) =>
    {
        if (message.StartsWith("Note:"))
        {
            type = OutputType.Std;
        }
        base.GetLogger(options)(type,message);
    };


    public GradleTasks()
    {

        SetToolPath(NukeBuild.RootDirectory / (EnvironmentInfo.IsWin ? "gradlew.bat" : "gradlew"));
    }
}
partial class GradleSettings
{
    public GradleSettings()
    {
        Set(() => ProcessWorkingDirectory, NukeBuild.RootDirectory);
    }
}

partial class KnownGradleTasks
{
    /// <summary>
    /// Builds the artifacts (JAR, sources, Javadoc, etc.)
    /// Resolves publication metadata (e.g., version, dependencies)
    /// Pushes them to the target repository (usually defined in publishing.repositories)
    /// </summary>
    public static KnownGradleTasks Publish = (KnownGradleTasks)"publish";
    public static KnownGradleTasks PublishToMavenLocal = (KnownGradleTasks)"publishToMavenLocal";
    public override string ToString()
    {
        var str = base.ToString();
        var result = $"{str[0].ToString().ToLowerInvariant()}{str[1..]}";
        return result;
    }
}
