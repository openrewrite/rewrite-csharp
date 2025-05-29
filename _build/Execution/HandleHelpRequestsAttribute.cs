// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nuke.Common.Utilities;
using Nuke.Common.ValueInjection;
using ReflectionMagic;
using Spectre.Console;

namespace Nuke.Common.Execution;

internal class HandleHelpRequestsAttribute : BuildExtensionAttributeBase, IOnBuildInitialized
{
    public void OnBuildInitialized(
        IReadOnlyCollection<ExecutableTarget> executableTargets,
        IReadOnlyCollection<ExecutableTarget> executionPlan)
    {
        if (Build.Help || executionPlan.Count == 0)
        {
            // using var writer = new StringWriter();
            var settings = new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Yes,
                // ColorSystem = ColorSystemSupport.NoColors,
                // Out = new AnsiConsoleOutput(writer),
            };

            var console = AnsiConsole.Create(settings);
            WriteUsage(console);
            WriteTargets(console);
            WriteParameters(console);
            Environment.Exit(exitCode: 0);
        }
    }

    private IReadOnlyCollection<ExecutableTarget> ExecutableTargets => Build.AsDynamic().ExecutableTargets;
    private dynamic ValueInjectionUtility => typeof(INukeBuild).Assembly.GetDynamicType("Nuke.Common.ValueInjection.ValueInjectionUtility");
    private dynamic ParameterService => typeof(INukeBuild).Assembly.GetDynamicType("Nuke.Common.ParameterService");
    private dynamic Host => typeof(INukeBuild).Assembly.GetDynamicType("Nuke.Common.Host");

    public void WriteUsage(IAnsiConsole console)
    {
        var extension = EnvironmentInfo.IsWin ? ".ps1" : ".sh";
        console.Write(new Markup($"[bold]Usage:[/] "));
        console.Write($"./build{extension} [Target1]  [Target2] ... [--option2 value] [--option2 value] ... \n");
        console.WriteLine();
    }
    public void WriteTargets(IAnsiConsole console)
    {
        console.Write(new Markup("[bold]Targets:[/]\n"));
        foreach (var category in ExecutableTargets.GroupBy(x => x.Member.GetCustomAttribute<CategoryAttribute>()?.Category).OrderBy(x => x.Key))
        {
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn().Width(160);
            foreach (var target in category.Where(x => x.Listed).OrderBy(x => x.Name))
            {
                var targetName = target.Name + (target.IsDefault ? " (default)" : string.Empty);
                var description = Markup.Escape(target.Description ?? "");
                var targetEntry = new Markup(target.IsDefault ? $"[bold]{targetName}[/]" : targetName);
                var targetDescription = new Markup(target.IsDefault ? $"[bold]{description}[/]" : description);
                grid.AddRow(targetEntry, targetDescription);
            }

            var panel = new Panel(grid)
            {
                Header = new PanelHeader(category.Key ?? "Default"),
                Border = BoxBorder.Rounded,
                Padding = new Padding(1),
                Width = 160
            };

            console.Write(panel);
        }


    }

    public void WriteParameters(IAnsiConsole console)
    {
        console.Write(new Markup("[bold]Parameters:[/]\n"));
        var defaultTargets = ExecutableTargets.Where(x => x.IsDefault).Select(x => x.Name).ToList();
        // var builder = new StringBuilder();

        IReadOnlyCollection<MemberInfo> parameters = ValueInjectionUtility.GetParameterMembers(Build.GetType(), includeUnlisted: false);

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn().Width(160);

        void PrintParameter(MemberInfo parameter)
        {
            var description =
                // TODO: remove
                ((string)(ParameterService.GetParameterDescription(parameter)))
                    ?.Replace("{default_target}", defaultTargets.Count > 0 ? defaultTargets.JoinCommaSpace() : "<none>")
                    .TrimEnd(".")!.Append(".")
                ?? "<no description>";
            var parameterName = ParameterService.GetParameterDashedName(parameter);
            grid.AddRow($"  --{parameterName}",  description);
        }

        var customParameters = parameters.Where(x => x.DeclaringType != typeof(NukeBuild)).ToList();
        if (customParameters.Count > 0)
            grid.AddEmptyRow();
        customParameters.ForEach(PrintParameter);

        grid.AddEmptyRow();

        var inheritedParameters = parameters.Where(x => x.DeclaringType == typeof(NukeBuild) && x.Name != nameof(NukeBuild.NoLogo)).ToList();
        inheritedParameters.ForEach(PrintParameter);

        console.Write(grid);
        console.WriteLine();
        console.Write(new Markup("[italic]Parameters automatically obtain values from environmental variables with matching constant-case syntax names (ex: MY_ENV_VARIABLE)[/]"));

    }
}
