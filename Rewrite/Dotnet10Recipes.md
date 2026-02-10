# .NET 10 Breaking Changes Recipes

| Category | Description | Status |
|----------|-------------|--------|
| ASP.NET Core | [Cookie login redirects disabled for known API endpoints](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/cookie-authentication-api-endpoints) | N/A<br />No specific APIs, requires manual developer review |
| ASP.NET Core | [Deprecation of WithOpenApi extension method](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/withopenapi-deprecated) | WithOpenApiDeprecatedCodeFixProvider |
| ASP.NET Core | [Exception diagnostics suppressed when TryHandleAsync returns true](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/exception-handler-diagnostics-suppressed) | TODO |
| ASP.NET Core | [IActionContextAccessor and ActionContextAccessor are obsolete](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/iactioncontextaccessor-obsolete) | ActionContextAccessorObsoleteCodeFixProvider |
| ASP.NET Core | [IncludeOpenAPIAnalyzers property and MVC API analyzers are deprecated](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/openapi-analyzers-deprecated) | TODO |
| ASP.NET Core | [IPNetwork and ForwardedHeadersOptions.KnownNetworks are obsolete](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete) | IPNetworkObsoleteCodeFixProvider |
| ASP.NET Core | [Microsoft.Extensions.ApiDescription.Client package deprecated](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/apidescription-client-deprecated) | TODO |
| ASP.NET Core | [Razor runtime compilation is obsolete](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/razor-runtime-compilation-obsolete) | TODO |
| ASP.NET Core | [WebHostBuilder, IWebHost, and WebHost are obsolete](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/webhostbuilder-deprecated) | TODO |
| Containers | [Default .NET images use Ubuntu](https://learn.microsoft.com/en-us/dotnet/core/compatibility/containers/10.0/default-images-use-ubuntu) | N/A <br />Docker base image compatibility out of scope |
| Core .NET libraries | [ActivitySource.CreateActivity and ActivitySource.StartActivity behavior change](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/activity-sampling) | ActivitySamplingAnalyzer |
| Core .NET libraries | [Arm64 SVE nonfaulting loads require mask](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/sve-nonfaulting-loads-mask-parameter) | TODO |
| Core .NET libraries | [BufferedStream.WriteByte no longer performs implicit flush](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/bufferedstream-writebyte-flush) | BufferedStreamWriteByteAnalyzer |
| Core .NET libraries | [C# 14 overload resolution with span parameters](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/csharp-overload-resolution) | TODO |
| Core .NET libraries | [Consistent shift behavior in generic math](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/generic-math) | GenericMathShiftAnalyzer |
| Core .NET libraries | [Default trace context propagator updated to W3C standard](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/default-trace-context-propagator) | DistributedContextPropagatorAnalyzer |
| Core .NET libraries | [DriveInfo.DriveFormat returns Linux filesystem types](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/driveinfo-driveformat-linux) | DriveInfoDriveFormatAnalyzer |
| Core .NET libraries | [DynamicallyAccessedMembers annotation removed from DefaultValueAttribute ctor](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/defaultvalueattribute-dynamically-accessed-members) | N/A<br />Binary change, doesn't affect source |
| Core .NET libraries | [Explicit struct Size disallowed with InlineArray](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/inlinearray-explicit-size-disallowed) | InlineArrayExplicitSizeAnalyzer |
| Core .NET libraries | [FilePatternMatch.Stem changed to non-nullable](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/filepatternmatch-stem-nonnullable) | FilePatternMatchStemAnalyzer |
| Core .NET libraries | [GnuTarEntry and PaxTarEntry no longer includes atime and ctime by default](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/tar-atime-ctime-default) | N/A<br />Extremely low risk of affecting anything |
| Core .NET libraries | [LDAP DirectoryControl parsing is now more stringent](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/ldap-directorycontrol-parsing) | N/A<br />Impossible to check statically, low risk |
| Core .NET libraries | [MacCatalyst version normalization](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/maccatalyst-version-normalization) | MacCatalystVersionNormalizationAnalyzer |
| Core .NET libraries | [.NET runtime no longer provides default termination signal handlers](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/sigterm-signal-handler) | SignalTerminationHandlerAnalyzer |
| Core .NET libraries | [System.Linq.AsyncEnumerable included in core libraries](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/asyncenumerable) | TODO |
| Core .NET libraries | [Type.MakeGenericSignatureType argument validation](https://learn.microsoft.com/en-us/dotnet/core/compatibility/reflection/10/makegeneric-signaturetype-validation) | N/A<br />Existing behavior would have been already broken |
| Cryptography | [CompositeMLDsa updated to draft-08](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/composite-mldsa-draft-08) | TODO |
| Cryptography | [CoseSigner.Key can be null](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/cosesigner-key-null) | TODO |
| Cryptography | [MLDsa and SlhDsa 'SecretKey' members renamed](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/mldsa-slhdsa-secretkey-to-privatekey) | TODO |
| Cryptography | [OpenSSL cryptographic primitives aren't supported on macOS](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/openssl-macos-unsupported) | TODO |
| Cryptography | [OpenSSL 1.1.1 or later required on Unix](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/openssl-version-requirement) | TODO |
| Cryptography | [X500DistinguishedName validation is stricter](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/x500distinguishedname-validation) | TODO |
| Cryptography | [X509Certificate and PublicKey key parameters can be null](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/x509-publickey-null) | X509PublicKeyNullParametersAnalyzer |
| Cryptography | [Environment variable renamed to DOTNET_OPENSSL_VERSION_OVERRIDE](https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/version-override) | OpenSslVersionOverrideEnvVarCodeFixProvider |
| Extensions | [BackgroundService runs all of ExecuteAsync as a Task](https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/backgroundservice-executeasync-task) | BackgroundServiceExecuteAsyncAnalyzer |
| Extensions | [Fix issues in GetKeyedService() and GetKeyedServices() with AnyKey](https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/getkeyedservice-anykey) | GetKeyedServiceAnyKeyAnalyzer |
| Extensions | [Null values preserved in configuration](https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/configuration-null-values-preserved) | TODO |
| Extensions | [Message no longer duplicated in Console log output](https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/console-json-logging-duplicate-messages) | TODO |
| Extensions | [ProviderAliasAttribute moved to Microsoft.Extensions.Logging.Abstractions assembly](https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/provideraliasattribute-moved-assembly) | TODO |
| Extensions | [Removed DynamicallyAccessedMembers annotation from trim-unsafe Microsoft.Extensions.Configuration code](https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/dynamically-accessed-members-configuration) | TODO |
| Globalization | [Environment variable renamed to DOTNET_ICU_VERSION_OVERRIDE](https://learn.microsoft.com/en-us/dotnet/core/compatibility/globalization/10.0/version-override) | TODO |
| Install tool | [dotnet.acquire API for VS Code no longer always downloads latest](https://learn.microsoft.com/en-us/dotnet/core/compatibility/install-tool/3.0.0/vscode-dotnet-acquire-no-latest) | TODO |
| Interop | [Casting IDispatchEx COM object to IReflect fails](https://learn.microsoft.com/en-us/dotnet/core/compatibility/interop/10.0/idispatchex-ireflect-cast) | TODO |
| Interop | [Single-file apps no longer look for native libraries in executable directory](https://learn.microsoft.com/en-us/dotnet/core/compatibility/interop/10.0/native-library-search) | TODO |
| Interop | [Specifying DllImportSearchPath.AssemblyDirectory only searches the assembly directory](https://learn.microsoft.com/en-us/dotnet/core/compatibility/interop/10.0/search-assembly-directory) | DllImportSearchPathCodeFixProvider |
| Networking | [HTTP/3 support disabled by default with PublishTrimmed](https://learn.microsoft.com/en-us/dotnet/core/compatibility/networking/10.0/http3-disabled-with-publishtrimmed) | TODO |
| Networking | [Streaming HTTP responses enabled by default in browser HTTP clients](https://learn.microsoft.com/en-us/dotnet/core/compatibility/networking/10.0/default-http-streaming) | TODO (complex) |
| Networking | [Uri length limits removed](https://learn.microsoft.com/en-us/dotnet/core/compatibility/networking/10.0/uri-length-limits-removed) | N/A<br />Shouldn't affect anyone |
| Core .NET libraries | [More restricted annotations on InvokeMember/FindMembers/DeclaredMembers](https://learn.microsoft.com/en-us/dotnet/core/compatibility/reflection/10/ireflect-damt-annotations) | ReflectDamtAnnotationsCodeFixProvider |
| SDK and MSBuild | [.NET CLI `--interactive` defaults to `true` in user scenarios](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-cli-interactive) | N/A |
| SDK and MSBuild | [`dotnet` CLI commands log non-command-relevant data to stderr](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-cli-stderr-output) | N/A |
| SDK and MSBuild | [.NET tool packaging creates RuntimeIdentifier-specific tool packages](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-tool-pack-publish) | N/A |
| SDK and MSBuild | [Default workload configuration from 'loose manifests' to 'workload sets' mode](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/default-workload-config) | N/A |
| SDK and MSBuild | [Code coverage EnableDynamicNativeInstrumentation defaults to false](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/code-coverage-dynamic-native-instrumentation) | N/A |
| SDK and MSBuild | [dnx.ps1 file is no longer included in .NET SDK](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dnx-ps1-removed) | N/A |
| SDK and MSBuild | [`dotnet new sln` defaults to SLNX file format](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-new-sln-slnx-default) | N/A |
| SDK and MSBuild | [`dotnet package list` performs restore](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-package-list-restore) | N/A |
| SDK and MSBuild | [`dotnet restore` audits transitive packages](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/nugetaudit-transitive-packages) | N/A |
| SDK and MSBuild | [`dotnet tool install --local` creates manifest by default](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-tool-install-local-manifest) | N/A |
| SDK and MSBuild | [`dotnet watch` logs to stderr instead of stdout](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-watch-stderr) | N/A |
| SDK and MSBuild | [project.json not supported in `dotnet restore`](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-restore-project-json-unsupported) | N/A |
| SDK and MSBuild | [SHA-1 fingerprint support deprecated in `dotnet nuget sign`](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-nuget-sign-sha1-deprecated) | N/A |
| SDK and MSBuild | [MSBUILDCUSTOMBUILDEVENTWARNING escape hatch removed](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/custom-build-event-warning) | N/A |
| SDK and MSBuild | [MSBuild custom culture resource handling](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/msbuild-custom-culture) | N/A |
| SDK and MSBuild | [NU1510 is raised for direct references pruned by NuGet](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/nu1510-pruned-references) | N/A |
| SDK and MSBuild | [NuGet packages with no runtime assets aren't included in deps.json](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/deps-json-trimmed-packages) | N/A |
| SDK and MSBuild | [PackageReference without a version raises an error](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/nu1015-packagereference-version) | N/A |
| SDK and MSBuild | [PrunePackageReference privatizes direct prunable references](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/prune-packagereference-privateassets) | N/A |
| SDK and MSBuild | [HTTP warnings promoted to errors in `dotnet package list` and `dotnet package search`](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/http-warnings-to-errors) | N/A |
| SDK and MSBuild | [NUGET_ENABLE_ENHANCED_HTTP_RETRY environment variable removed](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/nuget-enhanced-http-retry-removed) | N/A |
| SDK and MSBuild | [NuGet logs an error for invalid package IDs](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/nuget-packageid-validation) | N/A |
| SDK and MSBuild | [`ToolCommandName` not set for non-tool packages](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/toolcommandname-not-set) | N/A |
| Serialization | [System.Text.Json checks for property name conflicts](https://learn.microsoft.com/en-us/dotnet/core/compatibility/serialization/10/property-name-validation) | TODO |
| Serialization | [XmlSerializer no longer ignores properties marked with ObsoleteAttribute](https://learn.microsoft.com/en-us/dotnet/core/compatibility/serialization/10/xmlserializer-obsolete-properties) | TODO |
| Windows Forms | [API obsoletions](https://learn.microsoft.com/en-us/dotnet/core/compatibility/windows-forms/10.0/obsolete-apis) | TODO |
| Windows Forms | [Applications referencing both WPF and WinForms must disambiguate MenuItem and ContextMenu types](https://learn.microsoft.com/en-us/dotnet/core/compatibility/windows-forms/10.0/menuitem-contextmenu) | TODO |
| Windows Forms | [Renamed parameter in HtmlElement.InsertAdjacentElement](https://learn.microsoft.com/en-us/dotnet/core/compatibility/windows-forms/10.0/insertadjacentelement-orientation) | TODO |
| Windows Forms | [TreeView checkbox image truncation](https://learn.microsoft.com/en-us/dotnet/core/compatibility/windows-forms/10.0/treeview-text-location) | TODO |
| Windows Forms | [StatusStrip uses System RenderMode by default](https://learn.microsoft.com/en-us/dotnet/core/compatibility/windows-forms/10.0/statusstrip-renderer) | TODO |
| Windows Forms | [System.Drawing OutOfMemoryException changed to ExternalException](https://learn.microsoft.com/en-us/dotnet/core/compatibility/windows-forms/10.0/system-drawing-outofmemory-externalexception) | TODO |
| WPF | [Empty ColumnDefinitions and RowDefinitions are disallowed](https://learn.microsoft.com/en-us/dotnet/core/compatibility/wpf/10.0/empty-grid-definitions) | TODO |
| WPF | [Incorrect usage of DynamicResource causes application crash](https://learn.microsoft.com/en-us/dotnet/core/compatibility/wpf/10.0/dynamicresource-crash) | TODO |