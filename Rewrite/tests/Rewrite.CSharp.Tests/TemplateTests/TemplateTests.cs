using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Rewrite.RewriteCSharp.Format;
using Rewrite.RewriteCSharp.Template2;
using Template = Rewrite.RewriteCSharp.Template2.Template;
using static Rewrite.RewriteCSharp.Template2.Placeholder;
namespace Rewrite.CSharp.Tests.TemplateTests;

public class TemplateTests
{
    [Test]
    public void ReplaceArgumentsOnMethodInvocation()
    {
        var src = $$"""
                  void MyMethod()
                  {
                    Foo(1, "2");
                  }
                  """;
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(src);
        var methodInvocationCursor = cu.Find<J.MethodInvocation>()!;
        var methodInvocation = methodInvocationCursor.Value!;
        var arg0 = methodInvocation.Arguments[0];
        var arg1 = methodInvocation.Arguments[1];
        var result = methodInvocationCursor.ReplaceArguments($"{arg0}, Bar(), {arg1}");
        
        Console.WriteLine(result?.RenderLstTree());
    }
    
    [Test]
    public void ReplaceArgumentsInInvocation()
    {
        var src = $$"""
                    void MyMethod()
                    {
                      Foo(1, "2");
                    }
                    """;
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(src);
        var methodInvocationCursor = cu.Find<J.MethodInvocation>()!;
        var methodInvocation = methodInvocationCursor.Value!;
        var arg0 = methodInvocation.Arguments[0];
        var arg1 = methodInvocation.Arguments[1];
        var result = methodInvocationCursor.ReplaceArguments($"{arg0}, Bar(), {arg1}");
        
        Console.WriteLine(result?.RenderLstTree());
    }
    
    [Test]
    public void InsertArgumentAfterExistingInMethodInvocation()
    {
        var src = $$"""
                    
                    Foo(1, "2");

                    """;
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(src);
        var methodInvocationCursor = cu.Find<J.MethodInvocation>()!;
        var methodInvocation = methodInvocationCursor.Value!;
        var arg0 = (Cs.Argument)methodInvocation.Arguments[0];
        var result = methodInvocationCursor.InsertAfter(arg0, $"Bar()");
        
        Console.WriteLine(result?.RenderLstTree());
    }
    
    [Test]
    public void InsertArgumentBeforeExistingInMethodInvocation()
    {
        var src = $$"""
                    Foo(1, "2");
                    """;
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(src);
        var methodInvocationCursor = cu.Find<J.MethodInvocation>()!;
        var methodInvocation = methodInvocationCursor.Value!;
        var arg0 = (Cs.Argument)methodInvocation.Arguments[0];
        var result = methodInvocationCursor.InsertBefore(arg0, $"Bar()");
        
        Console.WriteLine(result?.RenderLstTree());
    }
    
    [Test]
    public void BooleanTest()
    {
        var before = $$"""
                       bool m() {
                           return 42 == 0x2A;
                       }
                       """;
        var after = $$"""
                      bool m() {
                          return 43 == 0x2A;
                      }
                      """;
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(before);
        var cursor = cu.Find<J.Binary>()!;
        var result = cursor.Apply($"43", x => x.Left);
        cu = cu.ReplaceNode(cursor.Value!, result).Format(cursor);
        Console.WriteLine(cu.RenderLstTree());

    }
    
    
    [Test]
    public void GenericTypeVariable()
    {
        
        var before = $$"""
                  T x = DoIt();
                  """;
        var after = $$"""
                      var x = DoIt();
                      """;
        var cu = (Cs.CompilationUnit)CSharpParser.Instance.Parse(before);
        var walker = new ExplitTypeToVarDeclarationVisitor();
        var result = walker.Visit(cu, 0);
        Console.WriteLine(result?.RenderLstTree());

    }
    class ExplitTypeToVarDeclarationVisitor : CSharpVisitor<int>
    {
        public override J? VisitVariableDeclarations(J.VariableDeclarations multiVariable, int p)
        {
            var cursor = Cursor.As<J.VariableDeclarations>();
            if (multiVariable is
                {
                    TypeExpression: J.Identifier
                    {
                        SimpleName: "var"
                    }

                })
            {
                return multiVariable; // it's already declared as var
            }

            var var0 = multiVariable.Variables[0];
            return cursor.Apply($"var {var0.Name} = {var0.Initializer}");
        }

        public override J? VisitMethodInvocation(J.MethodInvocation methodInvocation, int p)
        {
            Statement m = methodInvocation;
            var args = methodInvocation.Arguments;
            
            var template = new Template(NullLogger<Template>.Instance, $"{args[0]}, {InsertionPoint}, {args[1]}", new HashSet<string>());
            var result = template.Apply(Cursor, m.Coordinates.Replace());
            return result;
        }
    }
}