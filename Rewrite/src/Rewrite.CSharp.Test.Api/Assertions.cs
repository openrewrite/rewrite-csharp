using FluentAssertions;
using JetBrains.Annotations;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.RewriteCSharp.Test.Api;

public static class Assertions
{
    private static readonly Core.Parser.Builder Parser = new CSharpParser.Builder();

    public static ValidateSource Noop()
    {
        return (sourceFile, _) => sourceFile;
    }

    public static SourceSpecs<Cs.CompilationUnit> CSharp([LanguageInjection("C#")] string before)
    {
        return CSharp(before, s => { });
    }

    public static SourceSpecs<Cs.CompilationUnit> CSharp([LanguageInjection("C#")] string before, Action<SourceSpec<Cs.CompilationUnit>> spec)
    {
        var cs = new SourceSpec<Cs.CompilationUnit>(
            Core.Tree.RandomId(), Parser, before, t => t,
            ValidateTypes,
            null!,
            CustomizeExecutionContext
        );
        AcceptSpec(spec, cs);
        return cs;
    }

    public static SourceSpecs<Cs.CompilationUnit> CSharp([LanguageInjection("C#")] string before, [LanguageInjection("C#")] string after)
    {
        return CSharp(before, after, s => { });
    }

    public static SourceSpecs<Cs.CompilationUnit> CSharp([LanguageInjection("C#")] string before, [LanguageInjection("C#")] string after, Action<SourceSpec<Cs.CompilationUnit>> spec)
    {
        var cs = new SourceSpec<Cs.CompilationUnit>(
            Core.Tree.RandomId(), Parser, before, t => t,
            ValidateTypes,
            null!,
            CustomizeExecutionContext
        );
        cs.After = after;
        AcceptSpec(spec, cs);
        return cs;
    }

    private static void AcceptSpec(Action<SourceSpec<Cs.CompilationUnit>> spec, SourceSpec<Cs.CompilationUnit> cs)
    {
        var userSuppliedAfterRecipe = cs.AfterRecipe;
        cs.AfterRecipe = userSuppliedAfterRecipe;
        spec(cs);
    }

    public static SourceFile ValidateTypes(SourceFile source, TypeValidation typeValidation)
    {
        // if (source is JavaSourceFile) {
        // assertValidTypes(typeValidation, (JavaSourceFile) source);
        // }
        if (source is J && typeValidation.Unknowns)
            new UnknownDetector().Visit(source, 0);

        return source;
    }

    static void CustomizeExecutionContext(ExecutionContext ctx)
    {
        // if (ctx.GetMessage(JavaParser.SKIP_SOURCE_SET_TYPE_GENERATION) == null) {
        // ctx.PutMessage(JavaParser.SKIP_SOURCE_SET_TYPE_GENERATION, true);
        // }
    }
}

internal class UnknownDetector : JavaVisitor<int>
{
    public override J VisitUnknown(J.Unknown unknown, int p)
    {
        var parentCursor = Cursor.Parent!;
        object? container = unknown;
        while (parentCursor.Value is not Core.Tree)
        {
            container = parentCursor.Value;
            parentCursor = parentCursor.Parent!;
        }

        var parent = parentCursor.DropParentUntil(p => p is J).Value as J;
        if (parent is J.MethodDeclaration md && ReferenceEquals(md.ReturnTypeExpression, container))
            return unknown;
        if (parent is J.MethodInvocation mi && ReferenceEquals(mi.Padding.TypeParameters, container))
            return unknown;
        if (parent is J.VariableDeclarations vd && ReferenceEquals(vd.TypeExpression, container))
            return unknown;
        if (parent is J.NullableType nt && nt.Padding.TypeTree == container)
            return unknown;
        if (parent is J.ControlParentheses<TypeTree> cp && cp.Padding.Tree == container)
            return unknown;
        if (parent is J.NewArray na && na.TypeExpression == container)
            return unknown;
        if (parent is J.ArrayType at && at.ElementType == container)
            return unknown;
        if (parent is J.InstanceOf io && io.Clazz == container)
            return unknown;
        if (parent is Cs.Binary { Operator: Cs.Binary.OperatorType.As } bi && bi.Right == container)
            return unknown;
        if (parent is J.FieldAccess fa && fa.Target == container)
            return unknown; // TODO remove once we have type attribution
        if (parent is J.ParameterizedType)
            return unknown;
        if (parent is J.Annotation)
            return unknown;

        unknown.Should().BeNull(because: "parser must not produce `J.Unknown` nodes");
        return unknown;
    }
}
