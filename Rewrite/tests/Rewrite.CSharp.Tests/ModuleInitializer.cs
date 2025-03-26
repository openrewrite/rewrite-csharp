// using System.Runtime.CompilerServices;
// using Microsoft.Build.Locator;
// using Rewrite.MSBuild;
// using Rewrite.Remote;
// using Rewrite.Remote.Codec.CSharp;
// using Rewrite.Remote.Codec.Java;
// using Rewrite.Remote.Codec.Properties;
// using Rewrite.Remote.Codec.Xml;
// using Rewrite.Remote.Codec.Yaml;
// using Rewrite.RewriteJson.Tree;
// using Rewrite.RewriteProperties.Tree;
// using Rewrite.RewriteXml.Tree;
// using Rewrite.RewriteYaml.Tree;
// using JsonReceiver = Rewrite.Remote.Codec.Json.JsonReceiver;
// using JsonSender = Rewrite.Remote.Codec.Json.JsonSender;
//
// namespace Rewrite.CSharp.Tests;
//
// public class ModuleInitializer
// {
//     [ModuleInitializer]
//     internal static void OnAssemblyLoad()
//     {
//         ProjectParser.Init();
//         if (ITestExecutionContext.Current() == null)
//         {
//             ITestExecutionContext.SetCurrent(new LocalTestExecutionContext());
//             var localPrinter = new LocalPrinterFactory();
//             IPrinterFactory.Set(localPrinter);
//             IPrinterFactory.Default = localPrinter;
//             Initialization.Initialize();
//             SenderContext.Register(typeof(Cs), () => new CSharpSender());
//             ReceiverContext.Register(typeof(Cs), () => new CSharpReceiver());
//             SenderContext.Register(typeof(Json), () => new JsonSender());
//             ReceiverContext.Register(typeof(Json), () => new JsonReceiver());
//             SenderContext.Register(typeof(Xml), () => new XmlSender());
//             ReceiverContext.Register(typeof(Xml), () => new XmlReceiver());
//             SenderContext.Register(typeof(Yaml), () => new YamlSender());
//             ReceiverContext.Register(typeof(Yaml), () => new YamlReceiver());
//             SenderContext.Register(typeof(Properties), () => new PropertiesSender());
//             ReceiverContext.Register(typeof(Properties), () => new PropertiesReceiver());
//         }
//     }
// }
