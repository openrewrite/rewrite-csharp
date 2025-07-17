using Microsoft.Extensions.DependencyInjection;
using NMica.Utils.IO;
using Serilog;

namespace Rewrite.MSBuild.Tests;

public class BaseTests
{
    private readonly ServiceProvider _serviceProvider;

    public BaseTests()
    {
        var services = new ServiceCollection();
        services.AddLogging(c => c
            .AddSerilog());
        services.AddSingleton<RecipeManager>();
        services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }
    

    /// <summary>
    /// Constructs / gets object from DI container.
    /// Any constructor arguments that can be satisfied from ServiceProvider will be used, with the rest coming from user supplied parameters 
    /// </summary>
    /// <param name="args">Additional arguments used to construct the object</param>
    /// <typeparam name="T">Type of object to create / retrieve</typeparam>
    protected T CreateObject<T>(params object[] args)
    {
        return ActivatorUtilities.CreateInstance<T>(_serviceProvider, args);
    }

    protected virtual ServiceCollection ConfigureServices(ServiceCollection services)
    {
        return services;
    }
    

    public class TestProject : IDisposable
    {
        public AbsolutePath Directory { get; }
        public AbsolutePath SolutionFile { get; }
        public AbsolutePath ProjectFile { get; }

        public static TestProject CreateTemporaryLibrary(string? programCs) => new TestProject((AbsolutePath)System.IO.Directory.CreateTempSubdirectory().FullName, false, programCs);
        public static TestProject CreateTemporaryExecutable(string programCs) => new TestProject((AbsolutePath)System.IO.Directory.CreateTempSubdirectory().FullName, true, programCs);
        private TestProject(AbsolutePath directory, bool executable, string? programCs = null)
        {
            Directory = directory;
            SolutionFile = Directory / "TestApp.sln";
            ProjectFile = Directory / "TestApp.csproj";
            WriteTestProject(executable);
            if (programCs != null)
            {
                File.WriteAllText(Directory / "Program.cs", programCs);
            }
        }
        

        /// <summary>
        /// Creates test project & solution file at specified folder
        /// </summary>
        private void WriteTestProject(bool executable)
        {
            Directory.EnsureCleanDirectory();
            var projectContent = executable ? ProjectContent : ProjectContent.Replace("Exe","Library");
            File.WriteAllText(ProjectFile , projectContent);
            File.WriteAllText(SolutionFile , SolutionContent);
        }
        private const string ProjectContent = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                    <OutputType>Exe</OutputType>
                    <ImplicitUsings>enable</ImplicitUsings>
                    <Nullable>enable</Nullable>
                </PropertyGroup>
            </Project>
            """;

        private const string SolutionContent = """

            Microsoft Visual Studio Solution File, Format Version 12.00
            Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "TestApp", "TestApp.csproj", "{C9850883-4A87-422E-891E-518B4EA5C8E2}"
            EndProject
            Global
            	GlobalSection(SolutionConfigurationPlatforms) = preSolution
            		Debug|Any CPU = Debug|Any CPU
            		Release|Any CPU = Release|Any CPU
            	EndGlobalSection
            	GlobalSection(ProjectConfigurationPlatforms) = postSolution
            		{C9850883-4A87-422E-891E-518B4EA5C8E2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
            		{C9850883-4A87-422E-891E-518B4EA5C8E2}.Debug|Any CPU.Build.0 = Debug|Any CPU
            		{C9850883-4A87-422E-891E-518B4EA5C8E2}.Release|Any CPU.ActiveCfg = Release|Any CPU
            		{C9850883-4A87-422E-891E-518B4EA5C8E2}.Release|Any CPU.Build.0 = Release|Any CPU
            	EndGlobalSection
            EndGlobal
            """;

        public void Dispose()
        {
            Directory.DeleteDirectory();
        }
    }
    
}