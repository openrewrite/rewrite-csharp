
using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;


/// <summary><p>Gradle build tool for Java</p><p>For more details, visit the <a href="https://gradle.org/">official website</a>.</p></summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public partial class GradleTasks : ToolTasks
{
    public static string GradlePath { get => new GradleTasks().GetToolPathInternal(); set => new GradleTasks().SetToolPath(value); }
    /// <summary><p>Gradle build tool for Java</p><p>For more details, visit the <a href="https://gradle.org/">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> Gradle(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new GradleTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Runs task(s)</p><p>For more details, visit the <a href="https://gradle.org/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://www.nuke.build/docs/common/cli-tools/#fluent-api">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;tasks&gt;</c> via <see cref="GradleSettings.Tasks"/></li><li><c>-Dorg.gradle.jvmargs</c> via <see cref="GradleSettings.JvmOptions"/></li><li><c>-Dorg.gradle.warning.mode</c> via <see cref="GradleSettings.WarningMode"/></li><li><c>-P</c> via <see cref="GradleSettings.ProjectProperty"/></li><li><c>-x</c> via <see cref="GradleSettings.ExcludeTasks"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> Gradle(GradleSettings options = null) => new GradleTasks().Run<GradleSettings>(options);
    /// <inheritdoc cref="GradleTasks.Gradle(.GradleSettings)"/>
    public static IReadOnlyCollection<Output> Gradle(Configure<GradleSettings> configurator) => new GradleTasks().Run<GradleSettings>(configurator.Invoke(new GradleSettings()));
    /// <inheritdoc cref="GradleTasks.Gradle(.GradleSettings)"/>
    public static IEnumerable<(GradleSettings Settings, IReadOnlyCollection<Output> Output)> Gradle(CombinatorialConfigure<GradleSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(Gradle, degreeOfParallelism, completeOnFailure);
}
#region GradleSettings
/// <inheritdoc cref="GradleTasks.Gradle(.GradleSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Command(Type = typeof(GradleTasks), Command = nameof(GradleTasks.Gradle))]
public partial class GradleSettings : ToolOptions
{
    /// <summary></summary>
    [Argument(Format = "{value}", Position = 1)] public IReadOnlyList<KnownGradleTasks> Tasks => Get<List<KnownGradleTasks>>(() => Tasks);
    /// <summary></summary>
    [Argument(Format = "-x {value}")] public IReadOnlyList<KnownGradleTasks> ExcludeTasks => Get<List<KnownGradleTasks>>(() => ExcludeTasks);
    /// <summary></summary>
    [Argument(Format = "-Dorg.gradle.warning.mode={value}")] public WarningMode WarningMode => Get<WarningMode>(() => WarningMode);
    /// <summary></summary>
    [Argument(Format = "-Dorg.gradle.jvmargs={value}")] public string JvmOptions => Get<string>(() => JvmOptions);
    /// <summary></summary>
    [Argument(Format = "-P{value}")] public IReadOnlyList<string> ProjectProperty => Get<List<string>>(() => ProjectProperty);
}
#endregion
#region GradleSettingsExtensions
/// <inheritdoc cref="GradleTasks.Gradle(.GradleSettings)"/>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class GradleSettingsExtensions
{
    #region Tasks
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T SetTasks<T>(this T o, params KnownGradleTasks[] v) where T : GradleSettings => o.Modify(b => b.Set(() => o.Tasks, v));
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T SetTasks<T>(this T o, IEnumerable<KnownGradleTasks> v) where T : GradleSettings => o.Modify(b => b.Set(() => o.Tasks, v));
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T AddTasks<T>(this T o, params KnownGradleTasks[] v) where T : GradleSettings => o.Modify(b => b.AddCollection(() => o.Tasks, v));
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T AddTasks<T>(this T o, IEnumerable<KnownGradleTasks> v) where T : GradleSettings => o.Modify(b => b.AddCollection(() => o.Tasks, v));
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T RemoveTasks<T>(this T o, params KnownGradleTasks[] v) where T : GradleSettings => o.Modify(b => b.RemoveCollection(() => o.Tasks, v));
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T RemoveTasks<T>(this T o, IEnumerable<KnownGradleTasks> v) where T : GradleSettings => o.Modify(b => b.RemoveCollection(() => o.Tasks, v));
    /// <inheritdoc cref="GradleSettings.Tasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.Tasks))]
    public static T ClearTasks<T>(this T o) where T : GradleSettings => o.Modify(b => b.ClearCollection(() => o.Tasks));
    #endregion
    #region ExcludeTasks
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T SetExcludeTasks<T>(this T o, params KnownGradleTasks[] v) where T : GradleSettings => o.Modify(b => b.Set(() => o.ExcludeTasks, v));
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T SetExcludeTasks<T>(this T o, IEnumerable<KnownGradleTasks> v) where T : GradleSettings => o.Modify(b => b.Set(() => o.ExcludeTasks, v));
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T AddExcludeTasks<T>(this T o, params KnownGradleTasks[] v) where T : GradleSettings => o.Modify(b => b.AddCollection(() => o.ExcludeTasks, v));
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T AddExcludeTasks<T>(this T o, IEnumerable<KnownGradleTasks> v) where T : GradleSettings => o.Modify(b => b.AddCollection(() => o.ExcludeTasks, v));
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T RemoveExcludeTasks<T>(this T o, params KnownGradleTasks[] v) where T : GradleSettings => o.Modify(b => b.RemoveCollection(() => o.ExcludeTasks, v));
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T RemoveExcludeTasks<T>(this T o, IEnumerable<KnownGradleTasks> v) where T : GradleSettings => o.Modify(b => b.RemoveCollection(() => o.ExcludeTasks, v));
    /// <inheritdoc cref="GradleSettings.ExcludeTasks"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ExcludeTasks))]
    public static T ClearExcludeTasks<T>(this T o) where T : GradleSettings => o.Modify(b => b.ClearCollection(() => o.ExcludeTasks));
    #endregion
    #region WarningMode
    /// <inheritdoc cref="GradleSettings.WarningMode"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.WarningMode))]
    public static T SetWarningMode<T>(this T o, WarningMode v) where T : GradleSettings => o.Modify(b => b.Set(() => o.WarningMode, v));
    /// <inheritdoc cref="GradleSettings.WarningMode"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.WarningMode))]
    public static T ResetWarningMode<T>(this T o) where T : GradleSettings => o.Modify(b => b.Remove(() => o.WarningMode));
    #endregion
    #region JvmOptions
    /// <inheritdoc cref="GradleSettings.JvmOptions"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.JvmOptions))]
    public static T SetJvmOptions<T>(this T o, string v) where T : GradleSettings => o.Modify(b => b.Set(() => o.JvmOptions, v));
    /// <inheritdoc cref="GradleSettings.JvmOptions"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.JvmOptions))]
    public static T ResetJvmOptions<T>(this T o) where T : GradleSettings => o.Modify(b => b.Remove(() => o.JvmOptions));
    #endregion
    #region ProjectProperty
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T SetProjectProperty<T>(this T o, params string[] v) where T : GradleSettings => o.Modify(b => b.Set(() => o.ProjectProperty, v));
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T SetProjectProperty<T>(this T o, IEnumerable<string> v) where T : GradleSettings => o.Modify(b => b.Set(() => o.ProjectProperty, v));
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T AddProjectProperty<T>(this T o, params string[] v) where T : GradleSettings => o.Modify(b => b.AddCollection(() => o.ProjectProperty, v));
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T AddProjectProperty<T>(this T o, IEnumerable<string> v) where T : GradleSettings => o.Modify(b => b.AddCollection(() => o.ProjectProperty, v));
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T RemoveProjectProperty<T>(this T o, params string[] v) where T : GradleSettings => o.Modify(b => b.RemoveCollection(() => o.ProjectProperty, v));
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T RemoveProjectProperty<T>(this T o, IEnumerable<string> v) where T : GradleSettings => o.Modify(b => b.RemoveCollection(() => o.ProjectProperty, v));
    /// <inheritdoc cref="GradleSettings.ProjectProperty"/>
    [Pure] [Builder(Type = typeof(GradleSettings), Property = nameof(GradleSettings.ProjectProperty))]
    public static T ClearProjectProperty<T>(this T o) where T : GradleSettings => o.Modify(b => b.ClearCollection(() => o.ProjectProperty));
    #endregion
}
#endregion
#region WarningMode
/// <summary>Used within <see cref="GradleTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<WarningMode>))]
public partial class WarningMode : Enumeration
{
    public static WarningMode All = (WarningMode) "All";
    public static WarningMode Summary = (WarningMode) "Summary";
    public static WarningMode None = (WarningMode) "None";
    public static WarningMode Fail = (WarningMode) "Fail";
    public static implicit operator WarningMode(string value)
    {
        return new WarningMode { Value = value };
    }
}
#endregion
