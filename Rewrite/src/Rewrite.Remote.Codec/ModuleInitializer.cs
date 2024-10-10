#pragma warning disable CA2255
using System.Runtime.CompilerServices;
using Rewrite.Remote.Codec.CSharp;
using Rewrite.Remote.Codec.Java;
using Rewrite.Remote.Codec.Properties;
using Rewrite.Remote.Codec.Xml;
using Rewrite.Remote.Codec.Yaml;
using Rewrite.RewriteCSharp.Tree;

namespace Rewrite.Remote.Codec;

public class ModuleInitializer
{
    [ModuleInitializer]
    internal static void OnAssemblyLoad()
    {
        Initialization.Initialize();
        SenderContext.Register(typeof(Cs), () => new CSharpSender());
        ReceiverContext.Register(typeof(Cs), () => new CSharpReceiver());
        SenderContext.Register(typeof(RewriteJson.Tree.Json), () => new Json.JsonSender());
        ReceiverContext.Register(typeof(RewriteJson.Tree.Json), () => new Json.JsonReceiver());
        SenderContext.Register(typeof(RewriteXml.Tree.Xml), () => new XmlSender());
        ReceiverContext.Register(typeof(RewriteXml.Tree.Xml), () => new XmlReceiver());
        SenderContext.Register(typeof(RewriteYaml.Tree.Yaml), () => new YamlSender());
        ReceiverContext.Register(typeof(RewriteYaml.Tree.Yaml), () => new YamlReceiver());
        SenderContext.Register(typeof(RewriteProperties.Tree.Properties), () => new PropertiesSender());
        ReceiverContext.Register(typeof(RewriteProperties.Tree.Properties), () => new PropertiesReceiver());
    }
}
