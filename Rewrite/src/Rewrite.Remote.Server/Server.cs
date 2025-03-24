using System.Formats.Cbor;
using System.Net;
using System.Net.Sockets;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.CodeAnalysis;
using NuGet.Configuration;
using PeterO.Cbor;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.MSBuild;
using Rewrite.Remote.Codec.CSharp;
using Rewrite.Remote.Codec.Java;
using Rewrite.Remote.Codec.Properties;
using Rewrite.Remote.Codec.Xml;
using Rewrite.Remote.Codec.Yaml;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJson.Tree;
using Rewrite.RewriteProperties.Tree;
using Rewrite.RewriteXml.Tree;
using Rewrite.RewriteYaml.Tree;

namespace Rewrite.Remote.Server;

public class Server
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Server));

    static Server()
    {
        Initialization.Initialize();

        SenderContext.Register(typeof(Cs), () => new CSharpSender());
        SenderContext.Register(typeof(Json), () => new Codec.Json.JsonSender());
        SenderContext.Register(typeof(Yaml), () => new YamlSender());
        SenderContext.Register(typeof(Xml), () => new XmlSender());
        SenderContext.Register(typeof(Properties), () => new PropertiesSender());
        SenderContext.Register(typeof(ParseError), () => new ParseErrorSender());

        ReceiverContext.Register(typeof(Cs), () => new CSharpReceiver());
        ReceiverContext.Register(typeof(Json), () => new Codec.Json.JsonReceiver());
        ReceiverContext.Register(typeof(Yaml), () => new YamlReceiver());
        ReceiverContext.Register(typeof(Xml), () => new XmlReceiver());
        ReceiverContext.Register(typeof(Properties), () => new PropertiesReceiver());
        ReceiverContext.Register(typeof(ParseError), () => new ParseErrorReceiver());

        IRemotingContext.RegisterRecipeFactory(Recipe.Noop().GetType().FullName!, _ => Recipe.Noop());

        RemotingContext.RegisterValueDeserializer<Properties.Value>((type, reader, context) =>
        {
            Guid id = default;
            string? prefix = null;
            Markers? markers = null;
            string? text = null;
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                switch (reader.ReadTextString())
                {
                    case "id":
                        id = (Guid)context.Deserialize(typeof(Guid), reader)!;
                        break;
                    case "prefix":
                        prefix = (string?)context.Deserialize(typeof(string), reader);
                        break;
                    case "markers":
                        markers = (Markers?)context.Deserialize(typeof(Markers), reader);
                        break;
                    case "text":
                        text = (string?)context.Deserialize(typeof(string), reader);
                        break;
                }
            }

            reader.ReadEndMap();
            return new Properties.Value(id, prefix!, markers!, text!);
        });
    }

    private const int Ok = 0;
    private const int Error = 1;

    private RemotingContext? _remotingContext;
    private RemotingMessenger? _messenger;
    private Solution? _solution;
    private Options _options;

    public Server(Options options)
    {
        this._options = options;

        var hierarchy = (Hierarchy)LogManager.GetRepository();

        var patternLayout = new PatternLayout
        {
            ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
        };
        patternLayout.ActivateOptions();

        ProjectParser.Init();
        IAppender appender;
        if (options.LogFilePath != null)
        {
            var fileAppender = new FileAppender
            {
                AppendToFile = false,
                File = options.LogFilePath,
                Layout = patternLayout,
            };
            fileAppender.ActivateOptions();
            appender = fileAppender;
        }
        else
        {
            var fileAppender = new ConsoleAppender
            {
                Layout = patternLayout,
            };
            fileAppender.ActivateOptions();
            appender = fileAppender;
        }

        hierarchy.Root.AddAppender(appender);

        var memory = new MemoryAppender();
        memory.ActivateOptions();
        hierarchy.Root.AddAppender(memory);

        hierarchy.Root.Level = Level.Debug;
        hierarchy.Configured = true;
    }

    public async Task Listen()
    {
        var port = _options.Port;
        var timeout = _options.Timeout;
        _remotingContext = new RemotingContext();

        _messenger = new RemotingMessenger(_remotingContext,
            additionalHandlers: new Dictionary<string, Func<NetworkStream, RemotingContext, CancellationToken, Task>>
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

            Console.WriteLine($"starting server on port ({port}) ...");
            if (log.IsDebugEnabled)
            {
                log.Debug($"starting server on port ({port}) ...");
            }

            server.Bind(endPoint);
            server.Listen(5);
            Console.WriteLine($"Server started ({port}) ...");
            if (log.IsDebugEnabled)
            {
                log.Debug($"Server started ({port}) ...");
            }

            while (server.Poll((int)double.Min(timeout.TotalMicroseconds, int.MaxValue), SelectMode.SelectRead))
            {
                try
                {
                    using var client = await server.AcceptAsync();
                    await HandleClient(client);
                }
                catch (Exception e)
                {
                    log.Error($"Failure while serving client: {e}");
                }
            }
        }
        catch (SocketException e)
        {
            log.Error($"Failed to start on port {port}\n{e}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            log.Error(e);
            throw;
        }
        finally
        {
            if (log.IsDebugEnabled)
            {
                log.Error("Closing server");
            }

            server.Close();
        }
    }

    private async Task InstallRecipe(NetworkStream stream, RemotingContext context,
        CancellationToken cancellationToken)
    {
        var packageId = CBORObject.Read(stream).AsString();
        var packageVersion = CBORObject.Read(stream).AsString();
        var includeDefaultSource = CBORObject.Read(stream).AsBoolean();
        var sources = CBORObject.Read(stream).Values.Select(rawSource =>
        {
            var source = rawSource["source"].AsString();
            var packageSource = new PackageSource(source);

            var rawCredential = rawSource["credential"];
            if (rawCredential is { IsNull: false })
            {
                packageSource.Credentials =
                    new PackageSourceCredential(source,
                        rawCredential["username"].AsString(),
                        rawCredential["password"].AsString(),
                        true,
                        null);
            }

            return packageSource;
        }).ToList();

        var commandEnd = CBORObject.Read(stream);

        log.Info($$"""
                   Handling InstallRecipe Request: {
                       packageId: {{packageId}},
                       packageVersion: {{packageVersion}},
                       packageSources: [{{sources}}],
                       includeDefaultSource: [{{includeDefaultSource}}],
                   }
                   """);

        if (sources.Count == 0 && !includeDefaultSource)
        {
            throw new ArgumentException("No sources provided");
        }

        var installableRecipes = await
            NugetManager.InstallRecipeAsync(packageId, packageVersion, includeDefaultSource ? sources.Concat([new PackageSource("https://api.nuget.org/v3/index.json")]).ToList() : sources, _options.NugetPackagesFolder, cancellationToken);

        if (log.IsDebugEnabled)
        {
            log.Debug($"Found {installableRecipes.Recipes.Count} recipes for package {packageId}");
        }


        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);

        CBORObject.Write(new Dictionary<string, object>
            {
                {
                    "recipes",
                    installableRecipes.Recipes.Select(d => new Dictionary<string, object>
                    {
                        { "name", d.Name },
                        { "source", d.Source.ToString() },
                        {
                            "options", d.Options.Select(od => new Dictionary<string, object>
                            {
                                { "name", od.Name },
                                { "type", od.Type },
                                { "required", od.Required },
                            })
                        }
                    }).ToList()
                },
                { "repository", installableRecipes.Repository },
                { "version", installableRecipes.Version },
            },
            stream
        );
    }

    private async Task LoadRecipeAssemblyAndRunVisitor(NetworkStream stream, RemotingContext context,
        CancellationToken cancellationToken)
    {
        var recipeName = CBORObject.Read(stream).AsString();
        var packageSource = new Uri(CBORObject.Read(stream).AsString());
        var packageId = packageSource.Host;
        var packageVersion = packageSource.AbsolutePath.Replace("/", "");

        var options = new Dictionary<string, Func<Type, object>>();

        var optionsObj = CBORObject.Read(stream);

        foreach (var optionsObjEntry in optionsObj.Entries)
        {
            options.Add(optionsObjEntry.Key.AsString(), optionsObjEntry.Value.ToObject);
        }

        var inputStream = RemoteUtils.ReadToCommandEnd(stream);

        var received = RemotingMessenger.ReceiveTree(context, inputStream, RemotingMessenger._state);
        var ctx = new InMemoryExecutionContext();
        RemotingExecutionContextView.View(ctx).RemotingContext = context;

        log.Info($$"""
                   Handling LoadRecipeAssemblyAndRunVisitor Request: {
                       recipeName: {{recipeName}},
                       packageId: {{packageId}},
                       packageVersion: {{packageVersion}},
                       options: {{options}},
                       sourceFilePath: {{((SourceFile)received).SourcePath}}
                   }
                   """);

        if (log.IsDebugEnabled)
        {
            log.Debug("Trying to load recipe assembly");
        }

        var loadedRecipe = await
            NugetManager.LoadRecipeAssemblyAsync(recipeName, packageId, packageVersion, _options.NugetPackagesFolder,
                options,
                cancellationToken);

        if (log.IsDebugEnabled)
        {
            log.Debug($"Recipe {loadedRecipe.Descriptor.Name} was successfully loaded into the assembly. \nTrying to run it on the SourceFile");
        }

        var treeVisitor = loadedRecipe.GetVisitor();

        if (received is SourceFile sf && treeVisitor.IsAcceptable(sf, ctx))
        {
            RemotingMessenger._state = treeVisitor.Visit(sf, ctx);
        }
        else
        {
            log.Warn($"SourceFile of type [{received.GetType()}] is not acceptable");
            RemotingMessenger._state = received;
        }

        if (RemotingMessenger._state == null)
        {
            throw new InvalidOperationException("_state cannot be null");
        }

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        RemotingMessenger.SendTree(context, stream, RemotingMessenger._state, received);
    }

    private Task ParseProjectSources(NetworkStream stream, RemotingContext context,
        CancellationToken cancellationToken)
    {
        var projectFile = CBORObject.Read(stream).AsString();
        var solutionFile = CBORObject.Read(stream).AsString();
        var rootDir = CBORObject.Read(stream).AsString();
        var commandEnd = CBORObject.Read(stream);

        log.Info($$"""
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
                log.Error($"Failed to handle request. Solution {solutionFile} was not loaded using `list-projects`");
                CBORObject.Write($"Solution {solutionFile} was not loaded using `list-projects`", stream);
            }
            else
            {
                log.Error(
                    $"Failed to handle request. Solution {solutionFile} does not match loaded solution {_solution.FilePath}");
                CBORObject.Write($"Solution {solutionFile} does not match loaded solution {_solution.FilePath}",
                    stream);
            }

            return Task.CompletedTask;
        }

        if (log.IsDebugEnabled)
        {
            log.Debug($"Requesting all sources for project {projectFile}");
        }

        var sourceFiles =
            new SolutionParser().ParseProjectSources(_solution, projectFile, rootDir, new InMemoryExecutionContext())
                .ToList();

        if (log.IsDebugEnabled)
        {
            log.Debug($"Sending back the following sources: [{sourceFiles}]");
        }

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        CBORObject.Write(sourceFiles.Count, stream);
        foreach (var sourceFile in sourceFiles)
        {
            RemotingMessenger.SendTree(context, stream, sourceFile, default);
        }

        return Task.CompletedTask;
    }

    private async Task ListProjects(NetworkStream stream, RemotingContext context,
        CancellationToken cancellationToken)
    {
        var solutionFile = CBORObject.Read(stream).AsString();
        var commandEnd = CBORObject.Read(stream);


        log.Info($$"""
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

        if (log.IsDebugEnabled)
        {
            log.Debug($"Found the following projects [{projects}]");
        }

        CBORObject.Write(projects, stream);
    }


    private async Task HandleClient(Socket socket)
    {
        log.Info($"Received a new client connection {socket.RemoteEndPoint}");
        _remotingContext?.Connect(socket);
        await using var stream = new NetworkStream(socket);
        do
        {
            var dataWritten = false;
            try
            {
                var messageType = (RemotingMessageType)CBORObject.Read(stream).AsInt32();
                if (messageType != RemotingMessageType.Request)
                {
                    throw new ArgumentException($"Unexpected message type {messageType}");
                }


                var cancellationTokenSource = new CancellationTokenSource(_options.Timeout);
                var requestHandlingTask = _messenger!.ProcessRequest(stream, cancellationTokenSource.Token);
                if (await Task.WhenAny(requestHandlingTask, Task.Delay(_options.Timeout, CancellationToken.None)) ==
                    requestHandlingTask)
                {
                    dataWritten = await requestHandlingTask;
                }
                else
                {
                    throw new TimeoutException($"Request was not fulfilled withing given {_options.Timeout} timeout");
                }

                if (!dataWritten && stream.CanWrite)
                {
                    try
                    {
                        log.Error("Response was not completed after processing");
                        CBORObject.Write((int)RemotingMessageType.Response, stream);
                        CBORObject.Write(Error, stream);
                        CBORObject.Write("Response was not completed", stream);
                    }
                    catch (IOException exception)
                    {
                        log.Error("Failed to write response", exception);
                        // the socket was closed
                        // Console.Error.WriteLine(ignore);
                        break;
                    }
                    catch (Exception exception)
                    {
                        log.Error("Unexpected response sending exception", exception);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Response handling exception", e);
                if (!dataWritten && stream.CanWrite)
                {
                    try
                    {
                        CBORObject.Write((int)RemotingMessageType.Response, stream);
                        CBORObject.Write(Error, stream);
                        CBORObject.Write(e.ToString(), stream);
                    }
                    catch (IOException exception)
                    {
                        log.Error("Failed to write response", exception);
                        break;
                    }
                    catch (Exception exception)
                    {
                        log.Error("Unexpected response sending exception", exception);
                    }
                }
            }
            finally
            {
                RemotingMessenger.SendEndMessage(stream);
                stream.Flush();
            }
        } while (socket.Connected && socket.Poll(TimeSpan.FromSeconds(5), SelectMode.SelectRead) &&
                 socket.Available > 0);


        log.Info($"Client socket disconnected {socket.RemoteEndPoint}");
    }




}
