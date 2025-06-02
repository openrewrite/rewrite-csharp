using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

namespace BuildAnalyzers;

[Generator]
public class GradleTasksGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {

    }

    public void Execute(GeneratorExecutionContext context)
    {
        var configFile = context.AdditionalFiles.FirstOrDefault(f => Path.GetFileName(f.Path).Equals(".gradle-tasks.json", StringComparison.OrdinalIgnoreCase));
        var json = configFile?.GetText(context.CancellationToken)?.ToString();
        if(json == null)
            return;
        var buildType = context.Compilation.GetTypeByMetadataName("Build");
        var userDeclaredMembers = buildType?.MemberNames.ToImmutableHashSet() ?? ImmutableHashSet<string>.Empty;
        var tasks = JsonConvert.DeserializeObject<List<GradleTask>>(json) ?? [];
        var src = $$"""

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

                    /// <summary>Used within <see cref="GradleTasks"/>.</summary>
                    [PublicAPI]
                    [Serializable]
                    [ExcludeFromCodeCoverage]
                    [TypeConverter(typeof(TypeConverter<KnownGradleTasks>))]
                    public partial class KnownGradleTasks : Enumeration
                    {
                        {{tasks.Where(x => x.Name != null).Render(task => $$"""
                                                 /// <summary> {{task.Description}} </summary>
                                                 public static KnownGradleTasks {{task.Name!.ToPascalCase()}} = (KnownGradleTasks) "{{task.Name}}";
                                                 """)}}
                        public static implicit operator KnownGradleTasks(string value)
                        {
                            return new KnownGradleTasks { Value = value };
                        }
                    }
                    """;
        src = SyntaxFactory.ParseCompilationUnit(src).NormalizeWhitespace().ToFullString();

        context.AddSource($"KnownGradleTasks.g.cs", SourceText.From(src, Encoding.UTF8));

        //.Where(x => !userDeclaredMembers.Contains(x.Name.ToPascalCase()))

        src = $$"""

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
                using GradleTasks = GradleTasks;

                partial class Build
                {
                [Parameter("Target Module")] string Module;

                {{tasks.Render(task =>
                $$"""
                    [Category("Gradle (external)")]
                    Target Gradle{{task.Name.ToPascalCase()}} => _ => _
                        .Description("{{task.Description}}")
                        .Executes(() =>
                        {
                            var taskName = "{{task.Name}}";
                            if(Module != null)
                            {
                                taskName = $":{Module}:{taskName}";
                            }
                            global::GradleTasks.Gradle(c => c
                                .SetTasks(taskName)
                            );
                        });

                """)}}
                }
                """;

        context.AddSource($"GradleTasks.g.cs", SourceText.From(src, Encoding.UTF8));
    }

    public class GradleTask
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Group { get; set; } = null!;
    }
}
