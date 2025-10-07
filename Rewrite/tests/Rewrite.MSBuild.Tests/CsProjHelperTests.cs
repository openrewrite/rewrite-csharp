using FluentAssertions;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using TUnit.Core;

namespace Rewrite.MSBuild.Tests;

public class CsProjHelperTests : BaseTests
{
    private const string SampleCsProjContent = """
        <Project Sdk="Microsoft.NET.Sdk">
            <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <OutputType>Exe</OutputType>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <LangVersion>latest</LangVersion>
                <RootNamespace>TestApp</RootNamespace>
                <AssemblyName>TestApp</AssemblyName>
            </PropertyGroup>
            <ItemGroup>
                <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                <PackageReference Include="Serilog" Version="4.0.0" ExcludeAssets="runtime" />
            </ItemGroup>
        </Project>
        """;

    private string CreateTempCsProj(string content = "")
    {
        var tempDir = Directory.CreateTempSubdirectory().FullName;
        var csprojPath = Path.Combine(tempDir, "Test.csproj");
        File.WriteAllText(csprojPath, string.IsNullOrEmpty(content) ? SampleCsProjContent : content);
        return csprojPath;
    }

    #region Package Management Tests

    [Test]
    public void HasPackage_ShouldReturnTrue_WhenPackageExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var hasPackage = helper.HasPackage("Newtonsoft.Json");

        // Assert
        hasPackage.Should().BeTrue();
    }

    [Test]
    public void HasPackage_ShouldReturnFalse_WhenPackageDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var hasPackage = helper.HasPackage("NonExistent.Package");

        // Assert
        hasPackage.Should().BeFalse();
    }

    [Test]
    public void GetPackageVersion_ShouldReturnVersion_WhenPackageExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var version = helper.GetPackageVersion("Newtonsoft.Json");

        // Assert
        version.Should().Be("13.0.3");
    }

    [Test]
    public void GetPackageVersion_ShouldReturnNull_WhenPackageDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var version = helper.GetPackageVersion("NonExistent.Package");

        // Assert
        version.Should().BeNull();
    }

    [Test]
    public void UpgradePackage_ShouldUpdateVersion_WhenPackageExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.UpgradePackage("Newtonsoft.Json", "13.0.4");

        // Assert
        helper.GetPackageVersion("Newtonsoft.Json").Should().Be("13.0.4");
    }

    [Test]
    public void UpgradePackage_ShouldThrow_WhenPackageDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        var act = () => helper.UpgradePackage("NonExistent.Package", "1.0.0");
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Package NonExistent.Package not found in project");
    }

    [Test]
    public void AddPackage_ShouldAddNewPackage()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.AddPackage("FluentAssertions", "7.0.0");

        // Assert
        helper.HasPackage("FluentAssertions").Should().BeTrue();
        helper.GetPackageVersion("FluentAssertions").Should().Be("7.0.0");
    }

    [Test]
    public void AddPackage_ShouldThrow_WhenPackageAlreadyExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        var act = () => helper.AddPackage("Newtonsoft.Json", "13.0.3");
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Package Newtonsoft.Json already exists in project");
    }

    [Test]
    public void AddPackage_WithExcludeAssets_ShouldIncludeExcludeAssetsAttribute()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.AddPackage("Microsoft.CodeAnalysis", "4.9.0", "runtime");

        // Assert
        helper.HasPackage("Microsoft.CodeAnalysis").Should().BeTrue();

        // Verify ExcludeAssets is set by reading the XML directly
        var xml = File.ReadAllText(csprojPath);
        xml.Should().Contain("ExcludeAssets=\"runtime\"");
    }

    [Test]
    public void RemovePackage_ShouldRemovePackage_WhenExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.RemovePackage("Newtonsoft.Json");

        // Assert
        helper.HasPackage("Newtonsoft.Json").Should().BeFalse();
    }

    [Test]
    public void RemovePackage_ShouldThrow_WhenPackageDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        var act = () => helper.RemovePackage("NonExistent.Package");
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Package NonExistent.Package not found in project");
    }

    [Test]
    public void ExcludePackageAssets_ShouldSetExcludeAssets_WhenPackageExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.ExcludePackageAssets("Newtonsoft.Json", "build;analyzers");

        // Assert - Save and reload to verify
        var tempPath = Path.Combine(Path.GetDirectoryName(csprojPath)!, "temp.csproj");
        helper.SaveAs(tempPath);
        var xml = File.ReadAllText(tempPath);
        xml.Should().Contain("ExcludeAssets=\"build;analyzers\"");
    }

    [Test]
    public void ExcludePackageAssets_ShouldUpdateExistingExcludeAssets()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.ExcludePackageAssets("Serilog", "compile;native");

        // Assert - Save and reload to verify
        var tempPath = Path.Combine(Path.GetDirectoryName(csprojPath)!, "temp.csproj");
        helper.SaveAs(tempPath);
        var xml = File.ReadAllText(tempPath);
        xml.Should().Contain("ExcludeAssets=\"compile;native\"");
        xml.Should().NotContain("ExcludeAssets=\"runtime\"");
    }

    [Test]
    public void ExcludePackageAssets_ShouldThrow_WhenPackageDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        var act = () => helper.ExcludePackageAssets("NonExistent.Package", "runtime");
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Package NonExistent.Package not found in project");
    }

    #endregion

    #region Property Management Tests

    [Test]
    public void HasProperty_ShouldReturnTrue_WhenPropertyExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var hasProperty = helper.HasProperty("TargetFramework");

        // Assert
        hasProperty.Should().BeTrue();
    }

    [Test]
    public void HasProperty_ShouldReturnFalse_WhenPropertyDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var hasProperty = helper.HasProperty("NonExistentProperty");

        // Assert
        hasProperty.Should().BeFalse();
    }

    [Test]
    public void GetProperty_ShouldReturnValue_WhenPropertyExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var value = helper.GetProperty("OutputType");

        // Assert
        value.Should().Be("Exe");
    }

    [Test]
    public void GetProperty_ShouldReturnNull_WhenPropertyDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var value = helper.GetProperty("NonExistentProperty");

        // Assert
        value.Should().BeNull();
    }

    [Test]
    public void SetProperty_ShouldUpdateValue_WhenPropertyExists()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.SetProperty("OutputType", "Library");

        // Assert
        helper.GetProperty("OutputType").Should().Be("Library");
    }

    [Test]
    public void SetProperty_ShouldCreateProperty_WhenPropertyDoesNotExist()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        helper.SetProperty("NewProperty", "NewValue");

        // Assert
        helper.HasProperty("NewProperty").Should().BeTrue();
        helper.GetProperty("NewProperty").Should().Be("NewValue");
    }

    #endregion

    #region Common Properties Tests

    [Test]
    public void TargetFramework_ShouldGetAndSet()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.TargetFramework.Should().Be("net9.0");

        helper.TargetFramework = "net8.0";
        helper.TargetFramework.Should().Be("net8.0");
    }

    [Test]
    public void OutputType_ShouldGetAndSet()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.OutputType.Should().Be("Exe");

        helper.OutputType = "Library";
        helper.OutputType.Should().Be("Library");
    }

    [Test]
    public void RootNamespace_ShouldGetAndSet()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.RootNamespace.Should().Be("TestApp");

        helper.RootNamespace = "MyApp";
        helper.RootNamespace.Should().Be("MyApp");
    }

    [Test]
    public void AssemblyName_ShouldGetAndSet()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.AssemblyName.Should().Be("TestApp");

        helper.AssemblyName = "MyApp.dll";
        helper.AssemblyName.Should().Be("MyApp.dll");
    }

    [Test]
    public void LangVersion_ShouldGetAndSet()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.LangVersion.Should().Be("latest");

        helper.LangVersion = "12.0";
        helper.LangVersion.Should().Be("12.0");
    }

    [Test]
    public void Nullable_ShouldGetAndSet()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.Nullable.Should().Be("enable");

        helper.Nullable = "disable";
        helper.Nullable.Should().Be("disable");
    }

    [Test]
    public void TargetFrameworks_ShouldGetAndSet()
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFrameworks>net8.0;net9.0</TargetFrameworks>
                </PropertyGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = new CsProjHelper(csprojPath);

        // Act & Assert
        helper.TargetFrameworks.Should().Be("net8.0;net9.0");

        helper.TargetFrameworks = "net7.0;net8.0;net9.0";
        helper.TargetFrameworks.Should().Be("net7.0;net8.0;net9.0");
    }

    #endregion

    #region Package Analysis Tests

    [Test]
    public void GetDirectPackageReferences_ShouldReturnAllPackages()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();
        var helper = new CsProjHelper(csprojPath);

        // Act
        var packages = helper.GetDirectPackageReferences();

        // Assert
        packages.Should().HaveCount(2);
        packages.Should().Contain(p => p.Id == "Newtonsoft.Json" && p.Version.ToString() == "13.0.3");
        packages.Should().Contain(p => p.Id == "Serilog" && p.Version.ToString() == "4.0.0");
    }

    [Test]
    public void GetDirectPackageReferences_ShouldReturnEmpty_WhenNoPackages()
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = new CsProjHelper(csprojPath);

        // Act
        var packages = helper.GetDirectPackageReferences();

        // Assert
        packages.Should().BeEmpty();
    }

    [Test]
    public void GetDirectPackageReferences_ShouldIgnoreInvalidPackages()
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                    <PackageReference Include="InvalidPackage" />
                </ItemGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = new CsProjHelper(csprojPath);

        // Act
        var packages = helper.GetDirectPackageReferences();

        // Assert
        packages.Should().HaveCount(1);
        packages.Should().Contain(p => p.Id == "Newtonsoft.Json");
    }

#if NET9_0_OR_GREATER
    [Test]
    public async Task GetRequiredAssemblies_ShouldReturnAssembliesFromPackages(CancellationToken cancellationToken)
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                </ItemGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = CreateObject<CsProjHelper>(csprojPath);

        // Act
        var assemblies = await helper.GetRequiredAssemblies(cancellationToken);

        // Assert
        assemblies.Should().NotBeEmpty();
        assemblies.Should().Contain(a => a.PackageId == "Newtonsoft.Json" && a.AssemblyName == "Newtonsoft.Json");
    }

    [Test]
    public async Task GetTransitiveDependencyHierarchy_ShouldReturnDependencyTree(CancellationToken cancellationToken)
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
                </ItemGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = CreateObject<CsProjHelper>(csprojPath);

        // Act
        var hierarchy = await helper.GetTransitiveDependencyHierarchy(cancellationToken);

        // Assert
        hierarchy.Should().NotBeEmpty();
        hierarchy.Should().ContainKey(new PackageIdentity("Serilog.Sinks.Console", new NuGetVersion("6.0.0")));

        // Serilog.Sinks.Console depends on Serilog
        hierarchy.Values.SelectMany(v => v).Should().Contain(p => p.Id == "Serilog");
    }

    [Test]
    public async Task GetPackagesWithTransitiveDependency_ShouldFindPackagesWithDependency(CancellationToken cancellationToken)
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                </ItemGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = CreateObject<CsProjHelper>(csprojPath);

        // Act
        var packagesWithSerilog = await helper.GetPackagesWithTransitiveDependency("Serilog", cancellationToken);

        // Assert
        packagesWithSerilog.Should().NotBeEmpty();
        packagesWithSerilog.Should().Contain(p => p.Id == "Serilog.Sinks.Console");
        packagesWithSerilog.Should().NotContain(p => p.Id == "Newtonsoft.Json");
    }

    [Test]
    public async Task GetPackagesWithTransitiveDependency_ShouldReturnEmpty_WhenNoDependency(CancellationToken cancellationToken)
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                </ItemGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = CreateObject<CsProjHelper>(csprojPath);

        // Act
        var packagesWithSerilog = await helper.GetPackagesWithTransitiveDependency("Serilog", cancellationToken);

        // Assert
        packagesWithSerilog.Should().BeEmpty();
    }

    [Test]
    public async Task GetRequiredAssemblies_ShouldHandleMultiplePackages(CancellationToken cancellationToken)
    {
        // Arrange
        var content = """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net9.0</TargetFramework>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
                    <PackageReference Include="Serilog" Version="4.0.0" />
                </ItemGroup>
            </Project>
            """;
        var csprojPath = CreateTempCsProj(content);
        var helper = CreateObject<CsProjHelper>(csprojPath);

        // Act
        var assemblies = await helper.GetRequiredAssemblies(cancellationToken);

        // Assert
        assemblies.Should().NotBeEmpty();
        assemblies.Select(a => a.PackageId).Distinct().Should().Contain("Newtonsoft.Json");
        assemblies.Select(a => a.PackageId).Distinct().Should().Contain("Serilog");

        // Each assembly should have valid path and version info
        foreach (var assembly in assemblies)
        {
            assembly.PackageId.Should().NotBeNullOrEmpty();
            assembly.PackageVersion.Should().NotBeNullOrEmpty();
            assembly.AssemblyPath.Should().NotBeNullOrEmpty();
            assembly.AssemblyName.Should().NotBeNullOrEmpty();
            File.Exists(assembly.AssemblyPath).Should().BeTrue($"Assembly file should exist at {assembly.AssemblyPath}");
        }
    }
#endif

    #endregion

    #region Constructor Tests

    [Test]
    public void Constructor_ShouldThrow_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "test.csproj");

        // Act & Assert
        var act = () => new CsProjHelper(nonExistentPath);
        act.Should().Throw<FileNotFoundException>()
            .WithMessage($"Project file not found: {nonExistentPath}");
    }

    [Test]
    public void Constructor_ShouldSucceed_WithValidFile()
    {
        // Arrange
        var csprojPath = CreateTempCsProj();

        // Act
        var helper = new CsProjHelper(csprojPath);

        // Assert
        helper.Should().NotBeNull();
    }

    #endregion
}
