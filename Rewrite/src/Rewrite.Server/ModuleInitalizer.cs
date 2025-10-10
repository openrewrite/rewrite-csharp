using System.Formats.Cbor;
using System.Runtime.CompilerServices;
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


public static class ModuleInitalizer
{
    [ModuleInitializer]
    public static void OnApplicationStartup()
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

            reader.ReadEndMap();
            return new Properties.Value(id, prefix!, markers!, text!);
        });
        
        // ProjectParser.Init();
    }
}