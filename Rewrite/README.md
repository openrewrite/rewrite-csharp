## Publishing to NuGet

### Create local NuGet feed

Create a directory like `~/localNuGetFeed` and add it to your `~/.nuget/NuGet/NuGet.Config` file and replace `$HOME` with the absolute path to your home directory:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="LocalNugetFeed" value="$HOME/localNuGetFeed" />
  </packageSources>
</configuration>
```

This is the feed we will publish to locally.

### Phase 1: Create NuGet packages for rest of `Rewrite`

Commands are executed from `openrewrite/rewrite-csharp:Rewrite`.

1. [Optionally] Restore all dependencies:
   ```shell
      dotnet restore
   ```
2. Run `dotnet pack`
   ```shell
   dotnet pack --output ~/localNuGetFeed
   ```

Commands are executed from `openrewrite/rewrite-csharp:rewrite-csharp`.  

3. Publish Java bits of Rewrite project
   ```shell
   ./gradlew publishToMavenLocal
   ```

### Phase 2: Create NuGet packages for rest of `Rewrite.Remote`

Commands are executed from `moderneinc/rewrite-remote:rewrite-remote`.

1. Build Rewrite-Remote Java Server
   ```shell
   ./gradlew distZip
   ```

Commands are executed from `moderneinc/rewrite-remote:Rewrite.Remote`.

2. [Optionally] Restore all dependencies:
   ```shell
      dotnet restore
   ```
3. Check reference on the latest Rewrite dependencies & Run `dotnet pack`
   ```shell
   dotnet pack --output ~/localNuGetFeed
   ```
   

Commands are executed from `openrewrite/rewrite-csharp:Rewrite`.

3. Run Rewrite Integration tests with the latest `Rewrite.CShapr.Test` bundle to see all tests passes 

### Testing Locally with mod

1. Run `publishNebulaPublicationToMavenLocal` task on `rewrite-csharp` to publish recipes into local maven cache
2. Register them with mod using `mod config recipes jar install org.openrewrite:rewrite-csharp:0.28.4-SNAPSHOT` (adjust version)
3. Ensure that environmental variable `ROSLYN_RECIPE_EXECUTABLE` points to folder where `Rewrite.Server.dll` lives. Ex `C:\Projects\openrewrite\rewrite-csharp\Rewrite\src\Rewrite.Server\bin\Debug\net10.0\`. 
4. If you don't want the version of `Rewrite.Server` that is bundled into the jar file to be extracted to that location but instead use what's already in the folder, set environmental variable `ROSLYN_RECIPE_EXECUTABLE_SKIP_EXTRACT` to `true`
5. Ensure you have a test project code in folder that is git initialized AND has a remote `origin` configured
6. Execute `mod run C:\temp\TwoSolutions --recipe=org.openrewrite.csharp.recipes.microsoft.AvoidConstArraysAnalyzerCA1861`
