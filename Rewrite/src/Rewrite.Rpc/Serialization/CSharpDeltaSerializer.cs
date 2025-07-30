using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Rpc.Serialization;

public class CSharpDeltaSerializer : CSharpVisitor<SerializationContext>
{
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
    public void Visit<T>(JLeftPadded<T> node, SerializationContext context)
    {
        var ctx = context.As(node);
        ctx.SerializeProperty(x => x.Before, (x, diffContext) => Visit(x, diffContext));
        ctx.SerializeProperty(x => x.Element, (x, diffContext) => Visit((Tree)x!, diffContext), callingArgumentExpression: $"RP[{node.Element!.GetType().Name}]");
        
        ctx.SerializeProperty(x => x.Markers, (x, diffContext) => Visit(x, diffContext));
    }
    
    public void Visit<T>(JContainer<T> node, SerializationContext context) where T : Tree
    {
        var ctx = context.As(node);
        ctx.SerializeProperty(x => x.Before, (x, diffContext) => Visit(x, diffContext));
        ctx.SerializeList(x => x.Elements, x => x.Element.Id, (x, diffContext) => Visit(x, diffContext));
        
        ctx.SerializeProperty(x => x.Markers, (x, diffContext) => Visit(x, diffContext));
    }

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
        ctx.SerializeProperty(x => x.CharsetBomMarked);
        ctx.SerializeProperty(x => x.CharsetName);
        ctx.SerializeProperty(x => x.Checksum);
        ctx.SerializeProperty(x => x.SourcePath);
        ctx.SerializeList(x => x.Padding.Externs, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Usings, x => x.Element.Id, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.AttributeLists, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Members, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Eof, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitOperatorDeclaration(Cs.OperatorDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.AttributeLists, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.ExplicitInterfaceSpecifier, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.OperatorKeyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.CheckedKeyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.OperatorToken, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ReturnType, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Parameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitRefExpression(Cs.RefExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitPointerType(Cs.PointerType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.ElementType, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitRefType(Cs.RefType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.ReadonlyKeyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.TypeIdentifier, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitForEachVariableLoop(Cs.ForEachVariableLoop node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.ControlElement, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Variable, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Iterable, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitNameColon(Cs.NameColon node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Name, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitArgument(Cs.Argument node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.NameColumn, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.RefKindKeyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAnnotatedStatement(Cs.AnnotatedStatement node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.AttributeLists, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Statement, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Sizes, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAssignmentOperation(Cs.AssignmentOperation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Variable, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Operator, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Assignment, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAttributeList(Cs.AttributeList node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Target, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Attributes, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAwaitExpression(Cs.AwaitExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitStackAllocExpression(Cs.StackAllocExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitGotoStatement(Cs.GotoStatement node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.CaseOrDefaultKeyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Target, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitEventDeclaration(Cs.EventDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.AttributeLists, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.InterfaceSpecifier, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Accessors, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitBinary(Cs.Binary node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Left, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Operator, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Right, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Name, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Externs, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Usings, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Members, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.End, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitCollectionExpression(Cs.CollectionExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Elements, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitExpressionStatement(Cs.ExpressionStatement node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitExternAlias(Cs.ExternAlias node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Identifier, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Name, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Externs, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Usings, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Members, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitInterpolatedString(Cs.InterpolatedString node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Parts, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitInterpolation(Cs.Interpolation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Expression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Alignment, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Format, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitNullSafeExpression(Cs.NullSafeExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitStatementExpression(Cs.StatementExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Statement, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitUsingDirective(Cs.UsingDirective node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Global, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Static, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Unsafe, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Alias, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.NamespaceOrType, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitPropertyDeclaration(Cs.PropertyDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.AttributeLists, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.TypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.InterfaceSpecifier, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Accessors, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ExpressionBody, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Initializer, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitKeyword(Cs.Keyword node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Kind);
        return node;
    }

    public override J? VisitLambda(Cs.Lambda node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.LambdaExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ReturnType, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitClassDeclaration(Cs.ClassDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.AttributeList, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Kind, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.PrimaryConstructor, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Extendings, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Implementings, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameterConstraintClauses, (x, context) => Visit(x, context));
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

    public override J? VisitMethodDeclaration(Cs.MethodDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Attributes, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ReturnTypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.ExplicitInterfaceSpecifier, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Parameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameterConstraintClauses, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitUsingStatement(Cs.UsingStatement node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.AwaitKeyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Expression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Statement, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.TypeParameter, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameterConstraints, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTypeConstraint(Cs.TypeConstraint node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.TypeExpression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAllowsConstraintClause(Cs.AllowsConstraintClause node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Expressions, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitRefStructConstraint(Cs.RefStructConstraint node, SerializationContext p)
    {
        var ctx = p.As(node);
        return node;
    }

    public override J? VisitClassOrStructConstraint(Cs.ClassOrStructConstraint node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Kind);
        return node;
    }

    public override J? VisitConstructorConstraint(Cs.ConstructorConstraint node, SerializationContext p)
    {
        var ctx = p.As(node);
        return node;
    }

    public override J? VisitDefaultConstraint(Cs.DefaultConstraint node, SerializationContext p)
    {
        var ctx = p.As(node);
        return node;
    }

    public override J? VisitDeclarationExpression(Cs.DeclarationExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.TypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Variables, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitSingleVariableDesignation(Cs.SingleVariableDesignation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Variables, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitDiscardVariableDesignation(Cs.DiscardVariableDesignation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Discard, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTupleExpression(Cs.TupleExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Arguments, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitConstructor(Cs.Constructor node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Initializer, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ConstructorCore, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitDestructorDeclaration(Cs.DestructorDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.MethodCore, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitUnary(Cs.Unary node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Operator, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitConstructorInitializer(Cs.ConstructorInitializer node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Keyword, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Arguments, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTupleType(Cs.TupleType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Elements, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTupleElement(Cs.TupleElement node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Type, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitNewClass(Cs.NewClass node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.NewClassCore, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Initializer, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitInitializerExpression(Cs.InitializerExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Expressions, (x, context) => Visit(x, context));
        return node;
    }

    public override Markers VisitMarkers(Markers? node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Id);
        ctx.SerializeList(x => x.MarkerList);
        return node!;
    }
}