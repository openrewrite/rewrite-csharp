using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Rewrite.Rpc;
using Rewrite.Rpc.Serialization;
using Rewrite.Test;
using Spectre.Console;
using Spectre.Console.Rendering;
using TUnit.Core;

namespace Rewrite.MSBuild.Tests;

public class RpcTests
{
    [Test]
    [Explicit]
    public void NewValue()
    {
        AnsiConsole.Reset();
        var code = """
        public class MyClass 
        { 

        }   
        """;
        // SyntaxHighlighter.Render(code);
        var before = (Cs.CompilationUnit)CSharpParser.Instance.Parse(code);
        var serializer = new DeltaSerializer();

        var newObj = new Cs.CompilationUnit(default!,default!,default!,default!,default!,default!,default!,default!,default!,default!,default!,default!,default!);
        var result = serializer.Serialize(null, before);
        Render(before, result);

        var deserializer = new DeltaSerializer();

        var obj = deserializer.Deserialize(newObj, result);
        AnsiConsole.Write(GetSourcePanel("Deserialized", obj));
        // var clazz = before.Descendents().OfType<Cs.ClassDeclaration>().First();
        // clazz = clazz.WithName(clazz.Name.WithSimpleName("TheirClass"));
        // var after = before.WithMembers(new List<Statement>() { clazz });
        // result = serializer.Serialize(before, after);
        // Render(before, after, result);
    }
    
    [Test]
    [Explicit]
    public void UpdatedValue()
    {
        
        var code = """public class MyClass {  }""";
        SyntaxHighlighter.Render(code);
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(code);
        var clazz = cu.Members.OfType<Cs.ClassDeclaration>().First();
        var serializer = new DeltaSerializer();

        var result = serializer.Serialize(null, clazz);
        Render(clazz, result);
    }


    public static void Render(object after, List<RpcObjectData> list) => Render(null, after, list);
    public static void Render(object? before, object? after, List<RpcObjectData> list)
    {
        
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Expand()
            .Title("Datrum")
            .AddColumn("State")
            .AddColumn("ValueType")
            .AddColumn("Value")
            .AddColumn("Ref")
            .AddColumn("Trace");

        foreach (var item in list)
        {
            
            table.AddRow(
                item.State.ToString(),
                item.ValueType ?? "null",
                RenderValue(item.Value),
                item.Ref?.ToString() ?? "null",
                Markup.Escape(item.Trace ?? "null")
            );
        }
        
        var beforePanel = GetSourcePanel("Before", before);
        var afterPanel = GetSourcePanel("After", after);
        var panel = new Panel(new Rows(beforePanel, afterPanel, table)) { Header = new PanelHeader((after ?? before)!.GetType().Name)};
        AnsiConsole.Write(panel);
    }
    static Panel GetSourcePanel(string header, object? source)
    {
        var src = SyntaxHighlighter.GetMarkup(source?.ToString() ?? "null");
        IRenderable content = src;
        if (source is Core.Tree tree)
        {
            content = new Rows(src, tree.RenderLstTree());
        }

        return new Panel(content)
        {
            Header = new PanelHeader(header),
            Expand = true
        };
    }
    private static string RenderValue(object? value)
    {
        if (value == null)
            return "null";
        if (value is IEnumerable enumerable)
        {
            return Markup.Escape(JsonConvert.SerializeObject(enumerable, Formatting.Indented));
        }
        return Markup.Escape(value!.ToString() ?? "");


    }
}