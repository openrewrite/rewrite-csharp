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

    public override J? VisitAnnotatedType(J.AnnotatedType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.TypeExpression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAnnotation(J.Annotation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.AnnotationType, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Arguments, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitArrayAccess(J.ArrayAccess node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Indexed, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Dimension, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitArrayType(J.ArrayType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.ElementType, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Dimension, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAssert(J.Assert node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Condition, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Detail, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAssignment(J.Assignment node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Variable, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitAssignmentOperation(J.AssignmentOperation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Variable, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Operator, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Assignment, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitBinary(J.Binary node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Left, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Operator, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Right, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitBlock(J.Block node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Static, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Statements, x => x.Element.Id, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.End, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitBreak(J.Break node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Label, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitCase(J.Case node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.CaseLabels, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Statements, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Guard, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitClassDeclaration(J.ClassDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.LeadingAnnotations, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.DeclarationKind, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.PrimaryConstructor, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Extends, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Implements, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Permits, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitCompilationUnit(J.CompilationUnit node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.PackageDeclaration, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Imports, x => x.Element.Id, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Classes, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Eof, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitContinue(J.Continue node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Label, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitDoWhileLoop(J.DoWhileLoop node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.WhileCondition, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitEmpty(J.Empty node, SerializationContext p)
    {
        var ctx = p.As(node);
        return node;
    }

    public override J? VisitEnumValue(J.EnumValue node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Initializer, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitEnumValueSet(J.EnumValueSet node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Enums, x => x.Element.Id, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.TerminatedWithSemicolon);
        return node;
    }

    public override J? VisitFieldAccess(J.FieldAccess node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Target, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Name, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitForEachLoop(J.ForEachLoop node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.LoopControl, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitForEachControl(J.ForEachLoop.Control node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Variable, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Iterable, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitForLoop(J.ForLoop node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.LoopControl, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitForControl(J.ForLoop.Control node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Init, x => x.Element.Id, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Condition, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Update, x => x.Element.Id, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitParenthesizedTypeTree(J.ParenthesizedTypeTree node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ParenthesizedType, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitIf(J.If node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.IfCondition, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.ThenPart, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ElsePart, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitElse(J.If.Else node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitImport(J.Import node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Static, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Qualid, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Alias, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitInstanceOf(J.InstanceOf node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Expression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Clazz, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Pattern, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitIntersectionType(J.IntersectionType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Bounds, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitLabel(J.Label node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Statement, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitLambda(J.Lambda node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Params, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Arrow, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitLambdaParameters(J.Lambda.Parameters node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Parenthesized);
        ctx.SerializeList(x => x.Padding.Elements, x => x.Element.Id, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitLiteral(J.Literal node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Value);
        ctx.SerializeProperty(x => x.ValueSource);
        ctx.SerializeList(x => x.UnicodeEscapes, x => x);
        return node;
    }

    public override J? VisitMemberReference(J.MemberReference node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Containing, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Reference, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitMethodDeclaration(J.MethodDeclaration node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.LeadingAnnotations, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.ReturnTypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Parameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Throws, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.DefaultValue, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitMethodInvocation(J.MethodInvocation node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Select, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Arguments, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitMultiCatch(J.MultiCatch node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Alternatives, x => x.Element.Id, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitNewArray(J.NewArray node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.TypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Dimensions, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Initializer, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitArrayDimension(J.ArrayDimension node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Index, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitNewClass(J.NewClass node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Enclosing, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.New, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Clazz, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Arguments, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitNullableType(J.NullableType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeTree, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitPackage(J.Package node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitParameterizedType(J.ParameterizedType node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Clazz, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitParentheses<J2>(J.Parentheses<J2> node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Tree, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitControlParentheses<J2>(J.ControlParentheses<J2> node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Tree, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitPrimitive(J.Primitive node, SerializationContext p)
    {
        var ctx = p.As(node);
        return node;
    }

    public override J? VisitReturn(J.Return node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitSwitch(J.Switch node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Selector, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Cases, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitSwitchExpression(J.SwitchExpression node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Selector, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Cases, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitSynchronized(J.Synchronized node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Lock, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTernary(J.Ternary node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Condition, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.TruePart, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.FalsePart, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitThrow(J.Throw node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Exception, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTry(J.Try node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Resources, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Catches, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Finally, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTryResource(J.Try.Resource node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.VariableDeclarations, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitCatch(J.Try.Catch node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Parameter, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTypeCast(J.TypeCast node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Clazz, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTypeParameter(J.TypeParameter node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Annotations, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Bounds, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitTypeParameters(J.TypeParameters node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.Padding.Parameters, x => x.Element.Id, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitUnary(J.Unary node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.Operator, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Expression, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitVariableDeclarations(J.VariableDeclarations node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeList(x => x.LeadingAnnotations, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Modifiers, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.TypeExpression, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Varargs, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.Padding.Variables, x => x.Element.Id, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitVariable(J.VariableDeclarations.NamedVariable node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Name, (x, context) => Visit(x, context));
        ctx.SerializeList(x => x.DimensionsAfterName, x => x.Element.Whitespace, (x, context) => Visit(x, context)); //todo: dangerous and probably wrong
        ctx.SerializeProperty(x => x.Padding.Initializer, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitWhileLoop(J.WhileLoop node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Condition, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Body, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitWildcard(J.Wildcard node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Padding.WildcardBound, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.BoundedType, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitYield(J.Yield node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Implicit);
        ctx.SerializeProperty(x => x.Value, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitUnknown(J.Unknown node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.UnknownSource, (x, context) => Visit(x, context));
        return node;
    }

    public override J? VisitUnknownSource(J.Unknown.Source node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Text);
        return node;
    }

    public override J? VisitErroneous(J.Erroneous node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Text);
        return node;
    }

    public override J? VisitDeconstructionPattern(J.DeconstructionPattern node, SerializationContext p)
    {
        var ctx = p.As(node);
        ctx.SerializeProperty(x => x.Deconstructor, (x, context) => Visit(x, context));
        ctx.SerializeProperty(x => x.Padding.Nested, (x, context) => Visit(x, context));
        return node;
    }
}