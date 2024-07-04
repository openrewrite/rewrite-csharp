using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Parser;

namespace Rewrite.RewriteCSharp;


public static class Program 
{
    
    public static void Main()
    {
        // language=C#
        var sourceCode = @"
            using System;

            namespace Foo {};


            public class Test1 {
                public Test1(string x) {
                    
                }
            }

            public interface Test2 {}

            public class Test(string x) : Test1, Test2 {}
            
            [Obsolete]
            public    class         Program< T   > (Object x ): Test(x) // trailing comment
            
            {
                public static void Main()
                { // trailing
                    Console.ReadKey();
                    Console.WriteLine(""Hello, World!"");
                }
            }
        ";

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var root = syntaxTree.GetRoot();
        var compilation = CSharpCompilation.Create("Dummy")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            )
            .AddSyntaxTrees(syntaxTree);
        var semanticModel = compilation.GetSemanticModel(syntaxTree);

        var visitor = new CSharpParserVisitor(semanticModel);
        var cu = visitor.Visit(root) as SourceFile;
        var text = cu?.PrintAll();
        Console.WriteLine(text);

        // Console.WriteLine(root);

        // var allTokens = root.DescendantTokens().ToList();
        // foreach (var syntaxToken in allTokens)
        // {
        //     Console.Write(syntaxToken.LeadingTrivia);
        //     Console.Write(syntaxToken);
        //     Console.Write(syntaxToken.TrailingTrivia);
        // }
    }
}