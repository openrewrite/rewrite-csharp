﻿using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Rpc.Serialization;

public class CSharpDeltaSerializer : CSharpVisitor<SerializationContext>
{
    public override J? PreVisit(Tree? node, SerializationContext p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(node))] string callingArgumentExpression = "")
    {
        var ctx = p.As((J)node!);
        // ctx.TraceContext.Push(callingArgumentExpression);
        ctx.SerializeProperty(x => x.Id);
        ctx.SerializeProperty(x => x.Prefix, Visit);
        ctx.SerializeProperty(x => x.Markers, (markers, context) => Visit(markers, context));
        // ctx.TraceContext.Pop();
        return (J?)node;
    }

    public override J? VisitCompilationUnit(Cs.CompilationUnit node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Externs, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Usings, x => x.Element.Id, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.AttributeLists, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Members, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Eof, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitClassDeclaration(Cs.ClassDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        // ctx.SerializeProperty(x => x.Prefix, (after, context) => Visit(after, context));
        ctx.SerializeList(x => x.Modifiers, (after, context) => Visit(after, context));
        ctx.SerializeProperty(x => x.Kind, (after, context) => Visit(after, context));
        ctx.SerializeProperty(x => x.Name, (after, context) => Visit(after, context));
        return node;

    }

    public override J? VisitClassDeclarationKind(J.ClassDeclaration.Kind node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.KindType);
        return node;
    }

    public override J? VisitModifier(J.Modifier node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.ModifierType);
        ctx.SerializeProperty(x => x.Keyword);
        return node;
    }

    public override J? VisitIdentifier(J.Identifier node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.SimpleName);
        return node;
    }

    private void Visit(Space space, SerializationContext context)
    {
        var ctx = context.As(space);
        ctx.SerializeProperty(x => x.Whitespace);
        ctx.SerializeList(x => x.Comments);
        VisitSpace(space, Space.Location.ANY, context);
    }

    public void Visit(Markers node, SerializationContext context) => VisitMarkers(node, context);
    public void Visit(Marker node, SerializationContext context) => VisitMarker(node, context);
    public void Visit<T>(JRightPadded<T> node, SerializationContext context)
    {
        var ctx = context.As(node);
        ctx.SerializeProperty(x => x.Element, (x, diffContext) => Visit((Tree)x!, diffContext), callingArgumentExpression: $"RP[{node.Element!.GetType().Name}]");
        ctx.SerializeProperty(x => x.After, (x, diffContext) => Visit(x, diffContext));
        ctx.SerializeProperty(x => x.Markers, (x, diffContext) => Visit(x, diffContext));
    }

    public override Markers VisitMarkers(Markers? node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Id);
        ctx.SerializeList(x => x.MarkerList);
        return node!;
    }
}