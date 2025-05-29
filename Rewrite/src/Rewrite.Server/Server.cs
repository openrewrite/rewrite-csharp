using System.Formats.Cbor;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NMica.Utils.IO;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.Versioning;
using PeterO.Cbor;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.MSBuild;


namespace Rewrite.Remote.Server;

public class Server : BackgroundService
{
    private const int Ok = 0;
    private const int Error = 1;
    private RemotingMessenger? _messenger;
    private readonly ILogger<Server> _log;
    private readonly LanguageServerCommand.Settings _options;

    private RemotingContext? _remotingContext;
    private Solution? _solution;


    private readonly RecipeManager _recipeManager;
    public Server(ILogger<Server> log, LanguageServerCommand.Settings options, RecipeManager recipeManager)
    {
        _log = log;
        _options = options;
        _recipeManager = recipeManager;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var port = _options.Port;
        var timeout = _options.Timeout;
        _remotingContext = new RemotingContext();

        _messenger = new RemotingMessenger(_remotingContext, new Dictionary<string, Func<NetworkStream, RemotingContext, CancellationToken, Task>>
            {
                { "parse-project-sources", ParseProjectSources },
                { "list-projects", ListProjects },
                { "recipe-install", InstallRecipe },
                { "run-recipe-load-and-visitor", LoadRecipeAssemblyAndRunVisitor }
            });

        var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            var ipAddress = IPAddress.Loopback;
            var endPoint = new IPEndPoint(ipAddress, port);
            _log.LogDebug("Starting server on port ({Port}) ...", port);
            server.Bind(endPoint);
            server.Listen(5);
            _log.LogInformation("Server started ({Port}) ...", port);

            // while (server.Poll((int)double.Min(timeout.TotalMicroseconds, int.MaxValue), SelectMode.SelectRead))
            // {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var client = await server.AcceptAsync(stoppingToken);
                    await HandleClient(client, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    _log.LogError(e, "Failure while serving client");
                }
            }
            // }
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (SocketException e)
        {
            _log.LogError(e, "Failed to start on port {Port}", port);
        }
        catch (Exception e)
        {
            _log.LogError(e, "An exception occurred while running the application");
        }
    }

    private async Task InstallRecipe(NetworkStream stream, RemotingContext context, CancellationToken cancellationToken)
    {
        var packageId = CBORObject.Read(stream).AsString();
        var packageVersion = CBORObject.Read(stream).AsString();
        var includeDefaultSource = CBORObject.Read(stream).AsBoolean();
        var sources = CBORObject.Read(stream).Values.Select(rawSource =>
        {
            
            var source = Regex.Replace(rawSource["source"].AsString(), @"^file:(/(?=[a-zA-Z]:)|(?=/))", "");
            var packageSource = new PackageSource(source);

            var rawCredential = rawSource["credential"];
            if (rawCredential is { IsNull: false })
                packageSource.Credentials =
                    new PackageSourceCredential(source,
                        rawCredential["username"].AsString(),
                        rawCredential["password"].AsString(),
                        true,
                        null);

            return packageSource;
        }).ToList();
        CBORObject.Read(stream); // command end

        var requestDetails = new
        {
            PackageId = packageId,
            PackageVersion = packageVersion,
            Sources = sources.Select(x => x.Source).ToList(),
            IncludeDefaultSource = includeDefaultSource
        };
        
        _log.LogInformation("Handling InstallRecipe Request: {@Request}", requestDetails);

        if (sources.Count == 0 && !includeDefaultSource) throw new ArgumentException("No sources provided");

        VersionRange versionRange = packageVersion.ToUpper() switch
        {
            "LATEST" => VersionRange.All,
            "RELEASE" => VersionRange.AllStable,
            _ => VersionRange.Parse($"[{packageVersion}]")
        };
        var includePreRelease = packageVersion.ToUpper() == "LATEST";
        var libraryRange = new LibraryRange(packageId, versionRange, LibraryDependencyTarget.Package);
        if (includeDefaultSource)
        {
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));
        }

        // foreach (var source in sources)
        // {
        //     source.Source = Regex.Replace(source.Source, @"file:\/+", ""); //remove file:/ prefix for local sources
        // }
        

        var installableRecipes = await _recipeManager.InstallRecipePackage(
            libraryRange, 
            includePreRelease,
            sources,
            cancellationToken: cancellationToken);
        var selectedRecipeSource = "https://api.nuget.org/v3/index.json"; // recipe source makes no sense, as it can exist in multiple configured source repos, or even restored from global cache

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        
        CBORObject.Write(new Dictionary<string, object>
            {
                {
                    "recipes",
                    installableRecipes.Recipes.Select(d => new Dictionary<string, object>
                    {
                        { "name", d.TypeName.FullName },
                        { "source", new InstallableRecipe(d.TypeName, installableRecipes.Package.Id, installableRecipes.Package.Version.ToNormalizedString()).ToUri() },
                        {
                            "options", d.Options.Select(od => new Dictionary<string, object>
                            {
                                { "name", od.Name },
                                { "type", od.Type },
                                { "required", od.Required }
                            })
                        }
                    }).ToList()
                },
                { "repository", selectedRecipeSource },
                { "version", installableRecipes.Package.Version.ToNormalizedString() }
            },
            stream
        );
    }

    private Task LoadRecipeAssemblyAndRunVisitor(NetworkStream stream, RemotingContext context, CancellationToken cancellationToken)
    {
        var recipeName = CBORObject.Read(stream).AsString();
        var source = CBORObject.Read(stream).AsString();
        var recipeId = InstallableRecipe.ParseUri(new Uri(source));

        var options = new Dictionary<string, Func<Type, object>>();

        var optionsObj = CBORObject.Read(stream);

        foreach (var optionsObjEntry in optionsObj.Entries) options.Add(optionsObjEntry.Key.AsString(), optionsObjEntry.Value.ToObject);

        var inputStream = RemoteUtils.ReadToCommandEnd(stream);

        var received = RemotingMessenger.ReceiveTree(context, inputStream, RemotingMessenger._state);
        var ctx = new InMemoryExecutionContext();
        RemotingExecutionContextView.View(ctx).RemotingContext = context;

        var requestLog = new
        {
            RecipeIdentity = recipeId.ToString(),
            Options = options,
            SourceFilePath = ((SourceFile)received).SourcePath
        };
        _log.LogInformation("Handling LoadRecipeAssemblyAndRunVisitor Request: {@Request}", requestLog);


        var descriptor = _recipeManager.FindRecipeDescriptor(recipeId);
        var startInfo = descriptor.CreateRecipeStartInfo();
        foreach (var item in options)
        {
            var prop = startInfo.Arguments[item.Key];
            var type = Type.GetType(Core.Config.TypeName.Parse(prop.Type)) ?? throw new InvalidOperationException($"Unable to determine type {prop.Type}");
            var propValue = item.Value(type);
            startInfo.WithOption(item.Key, propValue);
        }
        var loadedRecipe =_recipeManager.CreateRecipe(recipeId, startInfo);
        // var loadedRecipe = await
        //     RecipeManager.LoadRecipeAssemblyAsync(recipeName, packageId, packageVersion, _options.NugetPackagesFolder,
        //         options,
        //         cancellationToken);

        _log.LogDebug($"Recipe {loadedRecipe.GetType().FullName} was successfully loaded into the assembly. \nTrying to run it on the SourceFile");

        var treeVisitor = loadedRecipe.GetVisitor();

        if (received is SourceFile sf && treeVisitor.IsAcceptable(sf, ctx))
        {
            RemotingMessenger._state = treeVisitor.Visit(sf, ctx);
        }
        else
        {
            _log.LogWarning($"SourceFile of type [{received.GetType()}] is not acceptable");
            RemotingMessenger._state = received;
        }

        if (RemotingMessenger._state == null) throw new InvalidOperationException("_state cannot be null");

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        RemotingMessenger.SendTree(context, stream, RemotingMessenger._state, received);
        return Task.CompletedTask;
    }

    private Task ParseProjectSources(NetworkStream stream, RemotingContext context,
        CancellationToken cancellationToken)
    {
        var projectFile = CBORObject.Read(stream).AsString();
        var solutionFile = CBORObject.Read(stream).AsString();
        var rootDir = CBORObject.Read(stream).AsString();
        var commandEnd = CBORObject.Read(stream);

        _log.LogInformation($$"""
                              Handling ParseProjectSources Request: {
                                  projectFile: {{projectFile}},
                                  solutionFile: {{solutionFile}},
                                  rootDir: {{rootDir}},
                              }
                              """);

        if (_solution is null || _solution.FilePath != solutionFile)
        {
            CBORObject.Write((int)RemotingMessageType.Response, stream);
            CBORObject.Write(Error, stream);
            if (_solution is null)
            {
                _log.LogError($"Failed to handle request. Solution {solutionFile} was not loaded using `list-projects`");
                CBORObject.Write($"Solution {solutionFile} was not loaded using `list-projects`", stream);
            }
            else
            {
                _log.LogError(
                    $"Failed to handle request. Solution {solutionFile} does not match loaded solution {_solution.FilePath}");
                CBORObject.Write($"Solution {solutionFile} does not match loaded solution {_solution.FilePath}",
                    stream);
            }

            return Task.CompletedTask;
        }

        _log.LogDebug($"Requesting all sources for project {projectFile}");

        var sourceFiles =
            new SolutionParser().ParseProjectSources(_solution, projectFile, rootDir, new InMemoryExecutionContext())
                .ToList();

        _log.LogDebug($"Sending back the following sources: [{sourceFiles}]");

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        CBORObject.Write(sourceFiles.Count, stream);
        foreach (var sourceFile in sourceFiles) RemotingMessenger.SendTree(context, stream, sourceFile, default);

        return Task.CompletedTask;
    }

    private async Task ListProjects(NetworkStream stream, RemotingContext context,
        CancellationToken cancellationToken)
    {
        var solutionFile = CBORObject.Read(stream).AsString();
        var commandEnd = CBORObject.Read(stream);


        _log.LogInformation($$"""
                              Handling ListProjects Request: {
                                  solutionFile: {{solutionFile}},
                              }
                              """);
        _solution = await new SolutionParser().LoadSolutionAsync(solutionFile, cancellationToken);

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);

        var projects = _solution.Projects
            .Where(project => project.FilePath is not null)
            .Select(project => project.FilePath!)
            // apply `Distinct()` as there may be multiple target frameworks
            .Distinct().ToList();

        _log.LogDebug($"Found the following projects [{projects}]");

        CBORObject.Write(projects, stream);
    }


    private async Task HandleClient(Socket socket, CancellationToken stoppingToken)
    {
        _log.LogInformation($"Received a new client connection {socket.RemoteEndPoint}");
        _remotingContext?.Connect(socket);
        await using var stream = new NetworkStream(socket);
        do
        {
            var dataWritten = false;
            try
            {
                var messageType = (RemotingMessageType)CBORObject.Read(stream).AsInt32();
                if (messageType != RemotingMessageType.Request) throw new ArgumentException($"Unexpected message type {messageType}");


                var cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, new CancellationTokenSource(_options.Timeout).Token).Token;
                var requestHandlingTask = _messenger!.ProcessRequest(stream, cancellationToken);
                if (await Task.WhenAny(requestHandlingTask, Task.Delay(_options.Timeout, cancellationToken)) == requestHandlingTask)
                    dataWritten = await requestHandlingTask;
                else
                    throw new TimeoutException($"Request was not fulfilled withing given {_options.Timeout} timeout");

                if (!dataWritten && stream.CanWrite)
                    try
                    {
                        _log.LogError("Response was not completed after processing");
                        CBORObject.Write((int)RemotingMessageType.Response, stream);
                        CBORObject.Write(Error, stream);
                        CBORObject.Write("Response was not completed", stream);
                    }
                    catch (IOException exception)
                    {
                        _log.LogError(exception, "Failed to write response");
                        // the socket was closed
                        // Console.Error.WriteLine(ignore);
                        break;
                    }
                    catch (Exception exception)
                    {
                        _log.LogError(exception, "Unexpected response sending exception");
                    }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Response handling exception");
                if (!dataWritten && stream.CanWrite)
                    try
                    {
                        CBORObject.Write((int)RemotingMessageType.Response, stream);
                        CBORObject.Write(Error, stream);
                        CBORObject.Write(e.ToString(), stream);
                    }
                    catch (IOException exception)
                    {
                        _log.LogError(exception, "Failed to write response");
                        break;
                    }
                    catch (Exception exception)
                    {
                        _log.LogError(exception, "Unexpected response sending exception");
                    }
            }
            finally
            {
                RemotingMessenger.SendEndMessage(stream);
                await stream.FlushAsync(stoppingToken);
            }
        } while (socket.Connected && socket.Poll(TimeSpan.FromSeconds(5), SelectMode.SelectRead) && socket.Available > 0);


        _log.LogInformation($"Client socket disconnected {socket.RemoteEndPoint}");
    }

}
