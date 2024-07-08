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

### Publish to NuGet

