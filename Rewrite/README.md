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

### Phase 0: Start by creating the NuGet package for `Rewrite.Remote.Api`

Commands are executed from `moderneinc/rewrite-remote:Rewrite.Remote`.

NOTE: So that all packages can be published in one go, we collect them in one place.

1. Update `<Version>` and probably also `<RewriteVersion>` tag in `Directory.Build.props` file
2. Run `dotnet pack`
   ```shell
   PUBLISH_PHASE=0 dotnet pack --output ~/localNuGetFeed
   ```

### Phase 1: Create NuGet packages for `Rewrite`

Commands are executed from `openrewrite/rewrite-csharp:Rewrite`.

1. Update `<Version>` and probably also `<RewriteRemoteVersion>` tag in `Directory.Build.props` file
2. Run `dotnet pack`
   ```shell
   PUBLISH_PHASE=1 dotnet pack --output ~/localNuGetFeed
   ```

### Phase 2: Create NuGet packages for rest of `Rewrite.Remote`

Commands are executed from `moderneinc/rewrite-remote:Rewrite.Remote`.

Run `dotnet pack`
```shell
PUBLISH_PHASE=2 dotnet pack --output ~/localNuGetFeed
```

### Phase 3: Create NuGet packages for rest of `Rewrite`

Commands are executed from `openrewrite/rewrite-csharp:Rewrite:` unless otherwise indicated.

1. Run the following build in `moderneinc/rewrite-remote:rewrite-remote` to create the binaries which will be included in `Rewrite.CSharp.Test`
   ```shell
   ./gradlew installDist
   ```
2. Run `dotnet pack`
   ```shell
   PUBLISH_PHASE=3 dotnet pack --output ~/localNuGetFeed
   ```

### Publish to NuGet

