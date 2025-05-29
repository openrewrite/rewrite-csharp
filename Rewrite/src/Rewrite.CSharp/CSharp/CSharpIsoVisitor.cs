using System.Runtime.CompilerServices;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public abstract class CSharpIsoVisitor<P> : CSharpVisitor<P>
{

public override J? VisitCompilationUnit(Cs.CompilationUnit node, P p)
{
    return base.VisitCompilationUnit(node, p) as Cs.CompilationUnit;
}

public override Cs.OperatorDeclaration? VisitOperatorDeclaration(Cs.OperatorDeclaration node, P p)
    {
        return base.VisitOperatorDeclaration(node, p) as Cs.OperatorDeclaration;
    }

public override Cs.RefExpression? VisitRefExpression(Cs.RefExpression node, P p)
    {
        return base.VisitRefExpression(node, p) as Cs.RefExpression;
    }

public override Cs.PointerType? VisitPointerType(Cs.PointerType node, P p)
    {
        return base.VisitPointerType(node, p) as Cs.PointerType;
    }

public override Cs.RefType? VisitRefType(Cs.RefType node, P p)
    {
        return base.VisitRefType(node, p) as Cs.RefType;
    }

public override Cs.ForEachVariableLoop? VisitForEachVariableLoop(Cs.ForEachVariableLoop node, P p)
    {
        return base.VisitForEachVariableLoop(node, p) as Cs.ForEachVariableLoop;
    }

public override Cs.ForEachVariableLoop.Control? VisitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control node, P p)
    {
        return (base.VisitForEachVariableLoopControl(node, p)) as Cs.ForEachVariableLoop.Control;
    }

public override Cs.NameColon? VisitNameColon(Cs.NameColon node, P p)
    {
        return base.VisitNameColon(node, p) as Cs.NameColon;
    }

public override Cs.Argument? VisitArgument(Cs.Argument node, P p)
    {
        return base.VisitArgument(node, p) as Cs.Argument;
    }

public override Cs.AnnotatedStatement? VisitAnnotatedStatement(Cs.AnnotatedStatement node, P p)
    {
        return base.VisitAnnotatedStatement(node, p) as Cs.AnnotatedStatement;
    }

public override Cs.ArrayRankSpecifier? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier node, P p)
    {
        return base.VisitArrayRankSpecifier(node, p) as Cs.ArrayRankSpecifier;
    }

public override Cs.AssignmentOperation? VisitAssignmentOperation(Cs.AssignmentOperation node, P p)
    {
        return base.VisitAssignmentOperation(node, p) as Cs.AssignmentOperation;
    }

public override Cs.AttributeList? VisitAttributeList(Cs.AttributeList node, P p)
    {
        return base.VisitAttributeList(node, p) as Cs.AttributeList;
    }

public override Cs.AwaitExpression? VisitAwaitExpression(Cs.AwaitExpression node, P p)
    {
        return base.VisitAwaitExpression(node, p) as Cs.AwaitExpression;
    }

public override Cs.StackAllocExpression? VisitStackAllocExpression(Cs.StackAllocExpression node, P p)
    {
        return base.VisitStackAllocExpression(node, p) as Cs.StackAllocExpression;
    }

public override Cs.GotoStatement? VisitGotoStatement(Cs.GotoStatement node, P p)
    {
        return base.VisitGotoStatement(node, p) as Cs.GotoStatement;
    }

public override Cs.EventDeclaration? VisitEventDeclaration(Cs.EventDeclaration node, P p)
    {
        return base.VisitEventDeclaration(node, p) as Cs.EventDeclaration;
    }

public override Cs.Binary? VisitBinary(Cs.Binary node, P p)
    {
        return base.VisitBinary(node, p) as Cs.Binary;
    }

public override Cs.BlockScopeNamespaceDeclaration? VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration node, P p)
    {
        return base.VisitBlockScopeNamespaceDeclaration(node, p) as Cs.BlockScopeNamespaceDeclaration;
    }

public override Cs.CollectionExpression? VisitCollectionExpression(Cs.CollectionExpression node, P p)
    {
        return base.VisitCollectionExpression(node, p) as Cs.CollectionExpression;
    }

public override Cs.ExpressionStatement? VisitExpressionStatement(Cs.ExpressionStatement node, P p)
    {
        return base.VisitExpressionStatement(node, p) as Cs.ExpressionStatement;
    }

public override Cs.ExternAlias? VisitExternAlias(Cs.ExternAlias node, P p)
    {
        return base.VisitExternAlias(node, p) as Cs.ExternAlias;
    }

public override Cs.FileScopeNamespaceDeclaration? VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration node, P p)
    {
        return base.VisitFileScopeNamespaceDeclaration(node, p) as Cs.FileScopeNamespaceDeclaration;
    }

public override Cs.InterpolatedString? VisitInterpolatedString(Cs.InterpolatedString node, P p)
    {
        return base.VisitInterpolatedString(node, p) as Cs.InterpolatedString;
    }

public override Cs.Interpolation? VisitInterpolation(Cs.Interpolation node, P p)
    {
        return base.VisitInterpolation(node, p) as Cs.Interpolation;
    }

public override Cs.NullSafeExpression? VisitNullSafeExpression(Cs.NullSafeExpression node, P p)
    {
        return base.VisitNullSafeExpression(node, p) as Cs.NullSafeExpression;
    }

public override Cs.StatementExpression? VisitStatementExpression(Cs.StatementExpression node, P p)
    {
        return base.VisitStatementExpression(node, p) as Cs.StatementExpression;
    }

public override Cs.UsingDirective? VisitUsingDirective(Cs.UsingDirective node, P p)
    {
        return base.VisitUsingDirective(node, p) as Cs.UsingDirective;
    }

public override Cs.PropertyDeclaration? VisitPropertyDeclaration(Cs.PropertyDeclaration node, P p)
    {
        return base.VisitPropertyDeclaration(node, p) as Cs.PropertyDeclaration;
    }

public override Cs.Keyword? VisitKeyword(Cs.Keyword node, P p)
    {
        return base.VisitKeyword(node, p) as Cs.Keyword;
    }

public override Cs.Lambda? VisitLambda(Cs.Lambda node, P p)
    {
        return base.VisitLambda(node, p) as Cs.Lambda;
    }

public override Cs.ClassDeclaration? VisitClassDeclaration(Cs.ClassDeclaration node, P p)
    {
        return base.VisitClassDeclaration(node, p) as Cs.ClassDeclaration;
    }

public override Cs.MethodDeclaration? VisitMethodDeclaration(Cs.MethodDeclaration node, P p)
    {
        return base.VisitMethodDeclaration(node, p) as Cs.MethodDeclaration;
    }

public override Cs.UsingStatement? VisitUsingStatement(Cs.UsingStatement node, P p)
    {
        return base.VisitUsingStatement(node, p) as Cs.UsingStatement;
    }

public override Cs.TypeParameterConstraintClause? VisitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause node, P p)
    {
        return base.VisitTypeParameterConstraintClause(node, p) as Cs.TypeParameterConstraintClause;
    }

public override Cs.TypeConstraint? VisitTypeConstraint(Cs.TypeConstraint node, P p)
    {
        return base.VisitTypeConstraint(node, p) as Cs.TypeConstraint;
    }

public override Cs.AllowsConstraintClause? VisitAllowsConstraintClause(Cs.AllowsConstraintClause node, P p)
    {
        return base.VisitAllowsConstraintClause(node, p) as Cs.AllowsConstraintClause;
    }

public override Cs.RefStructConstraint? VisitRefStructConstraint(Cs.RefStructConstraint node, P p)
    {
        return base.VisitRefStructConstraint(node, p) as Cs.RefStructConstraint;
    }

public override Cs.ClassOrStructConstraint? VisitClassOrStructConstraint(Cs.ClassOrStructConstraint node, P p)
    {
        return base.VisitClassOrStructConstraint(node, p) as Cs.ClassOrStructConstraint;
    }

public override Cs.ConstructorConstraint? VisitConstructorConstraint(Cs.ConstructorConstraint node, P p)
    {
        return base.VisitConstructorConstraint(node, p) as Cs.ConstructorConstraint;
    }

public override Cs.DefaultConstraint? VisitDefaultConstraint(Cs.DefaultConstraint node, P p)
    {
        return base.VisitDefaultConstraint(node, p) as Cs.DefaultConstraint;
    }

public override Cs.DeclarationExpression? VisitDeclarationExpression(Cs.DeclarationExpression node, P p)
    {
        return base.VisitDeclarationExpression(node, p) as Cs.DeclarationExpression;
    }

public override Cs.SingleVariableDesignation? VisitSingleVariableDesignation(Cs.SingleVariableDesignation node, P p)
    {
        return base.VisitSingleVariableDesignation(node, p) as Cs.SingleVariableDesignation;
    }

public override Cs.ParenthesizedVariableDesignation? VisitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation node, P p)
    {
        return base.VisitParenthesizedVariableDesignation(node, p) as Cs.ParenthesizedVariableDesignation;
    }

public override Cs.DiscardVariableDesignation? VisitDiscardVariableDesignation(Cs.DiscardVariableDesignation node, P p)
    {
        return base.VisitDiscardVariableDesignation(node, p) as Cs.DiscardVariableDesignation;
    }

public override Cs.TupleExpression? VisitTupleExpression(Cs.TupleExpression node, P p)
    {
        return base.VisitTupleExpression(node, p) as Cs.TupleExpression;
    }

public override Cs.Constructor? VisitConstructor(Cs.Constructor node, P p)
    {
        return base.VisitConstructor(node, p) as Cs.Constructor;
    }

public override Cs.DestructorDeclaration? VisitDestructorDeclaration(Cs.DestructorDeclaration node, P p)
    {
        return base.VisitDestructorDeclaration(node, p) as Cs.DestructorDeclaration;
    }

public override Cs.Unary? VisitUnary(Cs.Unary node, P p)
    {
        return base.VisitUnary(node, p) as Cs.Unary;
    }

public override Cs.ConstructorInitializer? VisitConstructorInitializer(Cs.ConstructorInitializer node, P p)
    {
        return base.VisitConstructorInitializer(node, p) as Cs.ConstructorInitializer;
    }

public override Cs.TupleType? VisitTupleType(Cs.TupleType node, P p)
    {
        return base.VisitTupleType(node, p) as Cs.TupleType;
    }

public override Cs.TupleElement? VisitTupleElement(Cs.TupleElement node, P p)
    {
        return base.VisitTupleElement(node, p) as Cs.TupleElement;
    }

public override Cs.NewClass? VisitNewClass(Cs.NewClass node, P p)
    {
        return base.VisitNewClass(node, p) as Cs.NewClass;
    }

public override Cs.InitializerExpression? VisitInitializerExpression(Cs.InitializerExpression node, P p)
    {
        return base.VisitInitializerExpression(node, p) as Cs.InitializerExpression;
    }

public override Cs.ImplicitElementAccess? VisitImplicitElementAccess(Cs.ImplicitElementAccess node, P p)
    {
        return base.VisitImplicitElementAccess(node, p) as Cs.ImplicitElementAccess;
    }

public override Cs.Yield? VisitYield(Cs.Yield node, P p)
    {
        return base.VisitYield(node, p) as Cs.Yield;
    }

public override Cs.DefaultExpression? VisitDefaultExpression(Cs.DefaultExpression node, P p)
    {
        return base.VisitDefaultExpression(node, p) as Cs.DefaultExpression;
    }

public override Cs.IsPattern? VisitIsPattern(Cs.IsPattern node, P p)
    {
        return base.VisitIsPattern(node, p) as Cs.IsPattern;
    }

public override Cs.UnaryPattern? VisitUnaryPattern(Cs.UnaryPattern node, P p)
    {
        return base.VisitUnaryPattern(node, p) as Cs.UnaryPattern;
    }

public override Cs.TypePattern? VisitTypePattern(Cs.TypePattern node, P p)
    {
        return base.VisitTypePattern(node, p) as Cs.TypePattern;
    }

public override Cs.BinaryPattern? VisitBinaryPattern(Cs.BinaryPattern node, P p)
    {
        return base.VisitBinaryPattern(node, p) as Cs.BinaryPattern;
    }

public override Cs.ConstantPattern? VisitConstantPattern(Cs.ConstantPattern node, P p)
    {
        return base.VisitConstantPattern(node, p) as Cs.ConstantPattern;
    }

public override Cs.DiscardPattern? VisitDiscardPattern(Cs.DiscardPattern node, P p)
    {
        return base.VisitDiscardPattern(node, p) as Cs.DiscardPattern;
    }

public override Cs.ListPattern? VisitListPattern(Cs.ListPattern node, P p)
    {
        return base.VisitListPattern(node, p) as Cs.ListPattern;
    }

public override Cs.ParenthesizedPattern? VisitParenthesizedPattern(Cs.ParenthesizedPattern node, P p)
    {
        return base.VisitParenthesizedPattern(node, p) as Cs.ParenthesizedPattern;
    }

public override Cs.RecursivePattern? VisitRecursivePattern(Cs.RecursivePattern node, P p)
    {
        return base.VisitRecursivePattern(node, p) as Cs.RecursivePattern;
    }

public override Cs.VarPattern? VisitVarPattern(Cs.VarPattern node, P p)
    {
        return base.VisitVarPattern(node, p) as Cs.VarPattern;
    }

public override Cs.PositionalPatternClause? VisitPositionalPatternClause(Cs.PositionalPatternClause node, P p)
    {
        return base.VisitPositionalPatternClause(node, p) as Cs.PositionalPatternClause;
    }

public override Cs.RelationalPattern? VisitRelationalPattern(Cs.RelationalPattern node, P p)
    {
        return base.VisitRelationalPattern(node, p) as Cs.RelationalPattern;
    }

public override Cs.SlicePattern? VisitSlicePattern(Cs.SlicePattern node, P p)
    {
        return base.VisitSlicePattern(node, p) as Cs.SlicePattern;
    }

public override Cs.PropertyPatternClause? VisitPropertyPatternClause(Cs.PropertyPatternClause node, P p)
    {
        return base.VisitPropertyPatternClause(node, p) as Cs.PropertyPatternClause;
    }

public override Cs.Subpattern? VisitSubpattern(Cs.Subpattern node, P p)
    {
        return base.VisitSubpattern(node, p) as Cs.Subpattern;
    }

public override Cs.SwitchExpression? VisitSwitchExpression(Cs.SwitchExpression node, P p)
    {
        return base.VisitSwitchExpression(node, p) as Cs.SwitchExpression;
    }

public override Cs.SwitchExpressionArm? VisitSwitchExpressionArm(Cs.SwitchExpressionArm node, P p)
    {
        return base.VisitSwitchExpressionArm(node, p) as Cs.SwitchExpressionArm;
    }

public override Cs.SwitchSection? VisitSwitchSection(Cs.SwitchSection node, P p)
    {
        return base.VisitSwitchSection(node, p) as Cs.SwitchSection;
    }

public override Cs.DefaultSwitchLabel? VisitDefaultSwitchLabel(Cs.DefaultSwitchLabel node, P p)
    {
        return base.VisitDefaultSwitchLabel(node, p) as Cs.DefaultSwitchLabel;
    }

public override Cs.CasePatternSwitchLabel? VisitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel node, P p)
    {
        return base.VisitCasePatternSwitchLabel(node, p) as Cs.CasePatternSwitchLabel;
    }

public override Cs.SwitchStatement? VisitSwitchStatement(Cs.SwitchStatement node, P p)
    {
        return base.VisitSwitchStatement(node, p) as Cs.SwitchStatement;
    }

public override Cs.LockStatement? VisitLockStatement(Cs.LockStatement node, P p)
    {
        return base.VisitLockStatement(node, p) as Cs.LockStatement;
    }

public override Cs.FixedStatement? VisitFixedStatement(Cs.FixedStatement node, P p)
    {
        return base.VisitFixedStatement(node, p) as Cs.FixedStatement;
    }

public override Cs.CheckedExpression? VisitCheckedExpression(Cs.CheckedExpression node, P p)
    {
        return base.VisitCheckedExpression(node, p) as Cs.CheckedExpression;
    }

public override Cs.CheckedStatement? VisitCheckedStatement(Cs.CheckedStatement node, P p)
    {
        return base.VisitCheckedStatement(node, p) as Cs.CheckedStatement;
    }

public override Cs.UnsafeStatement? VisitUnsafeStatement(Cs.UnsafeStatement node, P p)
    {
        return base.VisitUnsafeStatement(node, p) as Cs.UnsafeStatement;
    }

public override Cs.RangeExpression? VisitRangeExpression(Cs.RangeExpression node, P p)
    {
        return base.VisitRangeExpression(node, p) as Cs.RangeExpression;
    }

public override Cs.QueryExpression? VisitQueryExpression(Cs.QueryExpression node, P p)
    {
        return base.VisitQueryExpression(node, p) as Cs.QueryExpression;
    }

public override Cs.QueryBody? VisitQueryBody(Cs.QueryBody node, P p)
    {
        return base.VisitQueryBody(node, p) as Cs.QueryBody;
    }

public override Cs.FromClause? VisitFromClause(Cs.FromClause node, P p)
    {
        return base.VisitFromClause(node, p) as Cs.FromClause;
    }

public override Cs.LetClause? VisitLetClause(Cs.LetClause node, P p)
    {
        return base.VisitLetClause(node, p) as Cs.LetClause;
    }

public override Cs.JoinClause? VisitJoinClause(Cs.JoinClause node, P p)
    {
        return base.VisitJoinClause(node, p) as Cs.JoinClause;
    }

public override Cs.JoinIntoClause? VisitJoinIntoClause(Cs.JoinIntoClause node, P p)
    {
        return base.VisitJoinIntoClause(node, p) as Cs.JoinIntoClause;
    }

public override Cs.WhereClause? VisitWhereClause(Cs.WhereClause node, P p)
    {
        return base.VisitWhereClause(node, p) as Cs.WhereClause;
    }

public override Cs.OrderByClause? VisitOrderByClause(Cs.OrderByClause node, P p)
    {
        return base.VisitOrderByClause(node, p) as Cs.OrderByClause;
    }

public override Cs.QueryContinuation? VisitQueryContinuation(Cs.QueryContinuation node, P p)
    {
        return base.VisitQueryContinuation(node, p) as Cs.QueryContinuation;
    }

public override Cs.Ordering? VisitOrdering(Cs.Ordering node, P p)
    {
        return base.VisitOrdering(node, p) as Cs.Ordering;
    }

public override Cs.SelectClause? VisitSelectClause(Cs.SelectClause node, P p)
    {
        return base.VisitSelectClause(node, p) as Cs.SelectClause;
    }

public override Cs.GroupClause? VisitGroupClause(Cs.GroupClause node, P p)
    {
        return base.VisitGroupClause(node, p) as Cs.GroupClause;
    }

public override Cs.IndexerDeclaration? VisitIndexerDeclaration(Cs.IndexerDeclaration node, P p)
    {
        return base.VisitIndexerDeclaration(node, p) as Cs.IndexerDeclaration;
    }

public override Cs.DelegateDeclaration? VisitDelegateDeclaration(Cs.DelegateDeclaration node, P p)
    {
        return base.VisitDelegateDeclaration(node, p) as Cs.DelegateDeclaration;
    }

public override Cs.ConversionOperatorDeclaration? VisitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration node, P p)
    {
        return base.VisitConversionOperatorDeclaration(node, p) as Cs.ConversionOperatorDeclaration;
    }

public override Cs.TypeParameter? VisitTypeParameter(Cs.TypeParameter node, P p)
    {
        return base.VisitTypeParameter(node, p) as Cs.TypeParameter;
    }

public override Cs.EnumDeclaration? VisitEnumDeclaration(Cs.EnumDeclaration node, P p)
    {
        return base.VisitEnumDeclaration(node, p) as Cs.EnumDeclaration;
    }

public override Cs.EnumMemberDeclaration? VisitEnumMemberDeclaration(Cs.EnumMemberDeclaration node, P p)
    {
        return base.VisitEnumMemberDeclaration(node, p) as Cs.EnumMemberDeclaration;
    }

public override Cs.AliasQualifiedName? VisitAliasQualifiedName(Cs.AliasQualifiedName node, P p)
    {
        return base.VisitAliasQualifiedName(node, p) as Cs.AliasQualifiedName;
    }

public override Cs.ArrayType? VisitArrayType(Cs.ArrayType node, P p)
    {
        return base.VisitArrayType(node, p) as Cs.ArrayType;
    }

public override Cs.Try? VisitTry(Cs.Try node, P p)
    {
        return base.VisitTry(node, p) as Cs.Try;
    }

public override Cs.Try.Catch? VisitTryCatch(Cs.Try.Catch node, P p)
    {
        return (base.VisitTryCatch(node, p)) as Cs.Try.Catch;
    }

public override Cs.ArrowExpressionClause? VisitArrowExpressionClause(Cs.ArrowExpressionClause node, P p)
    {
        return base.VisitArrowExpressionClause(node, p) as Cs.ArrowExpressionClause;
    }

public override Cs.AccessorDeclaration? VisitAccessorDeclaration(Cs.AccessorDeclaration node, P p)
    {
        return base.VisitAccessorDeclaration(node, p) as Cs.AccessorDeclaration;
    }

public override Cs.PointerFieldAccess? VisitPointerFieldAccess(Cs.PointerFieldAccess node, P p)
    {
        return base.VisitPointerFieldAccess(node, p) as Cs.PointerFieldAccess;
    }

public override Expression VisitExpression(Expression node, P p)
    {
        return (Expression )(base.VisitExpression(node, p));
    }

public override Statement VisitStatement(Statement node, P p)
    {
        return (Statement )(base.VisitStatement(node, p));
    }

public override J.AnnotatedType? VisitAnnotatedType(J.AnnotatedType node, P p)
    {
        return base.VisitAnnotatedType(node, p) as J.AnnotatedType;
    }

public override J.Annotation? VisitAnnotation(J.Annotation node, P p)
    {
        return base.VisitAnnotation(node, p) as J.Annotation;
    }

public override J.ArrayAccess? VisitArrayAccess(J.ArrayAccess node, P p)
    {
        return base.VisitArrayAccess(node, p) as J.ArrayAccess;
    }

public override J.ArrayType? VisitArrayType(J.ArrayType node, P p)
    {
        return base.VisitArrayType(node, p) as J.ArrayType;
    }

public override J.Assert? VisitAssert(J.Assert node, P p)
    {
        return base.VisitAssert(node, p) as J.Assert;
    }

public override J.Assignment? VisitAssignment(J.Assignment node, P p)
    {
        return base.VisitAssignment(node, p) as J.Assignment;
    }

public override J.AssignmentOperation? VisitAssignmentOperation(J.AssignmentOperation node, P p)
    {
        return base.VisitAssignmentOperation(node, p) as J.AssignmentOperation;
    }

public override J.Binary? VisitBinary(J.Binary node, P p)
    {
        return base.VisitBinary(node, p) as J.Binary;
    }

public override J.Block? VisitBlock(J.Block node, P p)
    {
        return base.VisitBlock(node, p) as J.Block;
    }

public override J.Break? VisitBreak(J.Break node, P p)
    {
        return base.VisitBreak(node, p) as J.Break;
    }

public override J.Case? VisitCase(J.Case node, P p)
    {
        return base.VisitCase(node, p) as J.Case;
    }

public override J.ClassDeclaration? VisitClassDeclaration(J.ClassDeclaration node, P p)
    {
        return base.VisitClassDeclaration(node, p) as J.ClassDeclaration;
    }

public override J.ClassDeclaration.Kind? VisitClassDeclarationKind(J.ClassDeclaration.Kind node, P p)
    {
        return (base.VisitClassDeclarationKind(node, p)) as J.ClassDeclaration.Kind;
    }

public override J.CompilationUnit? VisitCompilationUnit(J.CompilationUnit node, P p)
    {
        return base.VisitCompilationUnit(node, p) as J.CompilationUnit;
    }

public override J.Continue? VisitContinue(J.Continue node, P p)
    {
        return base.VisitContinue(node, p) as J.Continue;
    }

public override J.DoWhileLoop? VisitDoWhileLoop(J.DoWhileLoop node, P p)
    {
        return base.VisitDoWhileLoop(node, p) as J.DoWhileLoop;
    }

public override J.Empty? VisitEmpty(J.Empty node, P p)
    {
        return base.VisitEmpty(node, p) as J.Empty;
    }

public override J.EnumValue? VisitEnumValue(J.EnumValue node, P p)
    {
        return base.VisitEnumValue(node, p) as J.EnumValue;
    }

public override J.EnumValueSet? VisitEnumValueSet(J.EnumValueSet node, P p)
    {
        return base.VisitEnumValueSet(node, p) as J.EnumValueSet;
    }

public override J.FieldAccess? VisitFieldAccess(J.FieldAccess node, P p)
    {
        return base.VisitFieldAccess(node, p) as J.FieldAccess;
    }

public override J.ForEachLoop? VisitForEachLoop(J.ForEachLoop node, P p)
    {
        return base.VisitForEachLoop(node, p) as J.ForEachLoop;
    }

public override J.ForEachLoop.Control? VisitForEachControl(J.ForEachLoop.Control node, P p)
    {
        return (base.VisitForEachControl(node, p)) as J.ForEachLoop.Control;
    }

public override J.ForLoop? VisitForLoop(J.ForLoop node, P p)
    {
        return base.VisitForLoop(node, p) as J.ForLoop;
    }

public override J.ForLoop.Control? VisitForControl(J.ForLoop.Control node, P p)
    {
        return (base.VisitForControl(node, p)) as J.ForLoop.Control;
    }

public override J.ParenthesizedTypeTree? VisitParenthesizedTypeTree(J.ParenthesizedTypeTree node, P p)
    {
        return base.VisitParenthesizedTypeTree(node, p) as J.ParenthesizedTypeTree;
    }

public override J.Identifier? VisitIdentifier(J.Identifier node, P p)
    {
        return base.VisitIdentifier(node, p) as J.Identifier;
    }

public override J.If? VisitIf(J.If node, P p)
    {
        return base.VisitIf(node, p) as J.If;
    }

public override J.If.Else? VisitElse(J.If.Else node, P p)
    {
        return (base.VisitElse(node, p)) as J.If.Else;
    }

public override J.Import? VisitImport(J.Import node, P p)
    {
        return base.VisitImport(node, p) as J.Import;
    }

public override J.InstanceOf? VisitInstanceOf(J.InstanceOf node, P p)
    {
        return base.VisitInstanceOf(node, p) as J.InstanceOf;
    }

public override J.IntersectionType? VisitIntersectionType(J.IntersectionType node, P p)
    {
        return base.VisitIntersectionType(node, p) as J.IntersectionType;
    }

public override J.Label? VisitLabel(J.Label node, P p)
    {
        return base.VisitLabel(node, p) as J.Label;
    }

public override J.Lambda? VisitLambda(J.Lambda node, P p)
    {
        return base.VisitLambda(node, p) as J.Lambda;
    }

public override J.Lambda.Parameters? VisitLambdaParameters(J.Lambda.Parameters node, P p)
    {
        return (base.VisitLambdaParameters(node, p)) as J.Lambda.Parameters;
    }

public override J.Literal? VisitLiteral(J.Literal node, P p)
    {
        return base.VisitLiteral(node, p) as J.Literal;
    }

public override J.MemberReference? VisitMemberReference(J.MemberReference node, P p)
    {
        return base.VisitMemberReference(node, p) as J.MemberReference;
    }

public override J.MethodDeclaration? VisitMethodDeclaration(J.MethodDeclaration node, P p)
    {
        return base.VisitMethodDeclaration(node, p) as J.MethodDeclaration;
    }

public override J.MethodInvocation? VisitMethodInvocation(J.MethodInvocation node, P p)
    {
        return base.VisitMethodInvocation(node, p) as J.MethodInvocation;
    }

public override J.Modifier? VisitModifier(J.Modifier node, P p)
    {
        return base.VisitModifier(node, p) as J.Modifier;
    }

public override J.MultiCatch? VisitMultiCatch(J.MultiCatch node, P p)
    {
        return base.VisitMultiCatch(node, p) as J.MultiCatch;
    }

public override J.NewArray? VisitNewArray(J.NewArray node, P p)
    {
        return base.VisitNewArray(node, p) as J.NewArray;
    }

public override J.ArrayDimension? VisitArrayDimension(J.ArrayDimension node, P p)
    {
        return base.VisitArrayDimension(node, p) as J.ArrayDimension;
    }

public override J.NewClass? VisitNewClass(J.NewClass node, P p)
    {
        return base.VisitNewClass(node, p) as J.NewClass;
    }

public override J.NullableType? VisitNullableType(J.NullableType node, P p)
    {
        return base.VisitNullableType(node, p) as J.NullableType;
    }

public override J.Package? VisitPackage(J.Package node, P p)
    {
        return base.VisitPackage(node, p) as J.Package;
    }

public override J.ParameterizedType? VisitParameterizedType(J.ParameterizedType node, P p)
    {
        return base.VisitParameterizedType(node, p) as J.ParameterizedType;
    }

public override J.Parentheses<J2>? VisitParentheses<J2>(J.Parentheses<J2> node, P p)
    {
        return (base.VisitParentheses(node, p)) as J.Parentheses<J2>;
    }

public override J.ControlParentheses<J2>? VisitControlParentheses<J2>(J.ControlParentheses<J2> node, P p)
    {
        return (base.VisitControlParentheses(node, p)) as J.ControlParentheses<J2>;
    }

public override J.Primitive? VisitPrimitive(J.Primitive node, P p)
    {
        return base.VisitPrimitive(node, p) as J.Primitive;
    }

public override J.Return? VisitReturn(J.Return node, P p)
    {
        return base.VisitReturn(node, p) as J.Return;
    }

public override J.Switch? VisitSwitch(J.Switch node, P p)
    {
        return base.VisitSwitch(node, p) as J.Switch;
    }

public override J.SwitchExpression? VisitSwitchExpression(J.SwitchExpression node, P p)
    {
        return base.VisitSwitchExpression(node, p) as J.SwitchExpression;
    }

public override J.Synchronized? VisitSynchronized(J.Synchronized node, P p)
    {
        return base.VisitSynchronized(node, p) as J.Synchronized;
    }

public override J.Ternary? VisitTernary(J.Ternary node, P p)
    {
        return base.VisitTernary(node, p) as J.Ternary;
    }

public override J.Throw? VisitThrow(J.Throw node, P p)
    {
        return base.VisitThrow(node, p) as J.Throw;
    }

public override J.Try? VisitTry(J.Try node, P p)
    {
        return base.VisitTry(node, p) as J.Try;
    }

public override J.Try.Resource? VisitTryResource(J.Try.Resource node, P p)
    {
        return (base.VisitTryResource(node, p)) as J.Try.Resource;
    }

public override J.Try.Catch? VisitCatch(J.Try.Catch node, P p)
    {
        return (base.VisitCatch(node, p)) as J.Try.Catch;
    }

public override J.TypeCast? VisitTypeCast(J.TypeCast node, P p)
    {
        return base.VisitTypeCast(node, p) as J.TypeCast;
    }

public override J.TypeParameter? VisitTypeParameter(J.TypeParameter node, P p)
    {
        return base.VisitTypeParameter(node, p) as J.TypeParameter;
    }

public override J.TypeParameters? VisitTypeParameters(J.TypeParameters node, P p)
    {
        return base.VisitTypeParameters(node, p) as J.TypeParameters;
    }

public override J.Unary? VisitUnary(J.Unary node, P p)
    {
        return base.VisitUnary(node, p) as J.Unary;
    }

public override J.VariableDeclarations? VisitVariableDeclarations(J.VariableDeclarations node, P p)
    {
        return base.VisitVariableDeclarations(node, p) as J.VariableDeclarations;
    }

public override J.VariableDeclarations.NamedVariable? VisitVariable(J.VariableDeclarations.NamedVariable node, P p)
    {
        return base.VisitVariable(node, p) as J.VariableDeclarations.NamedVariable;
    }

public override J.WhileLoop? VisitWhileLoop(J.WhileLoop node, P p)
    {
        return base.VisitWhileLoop(node, p) as J.WhileLoop;
    }

public override J.Wildcard? VisitWildcard(J.Wildcard node, P p)
    {
        return base.VisitWildcard(node, p) as J.Wildcard;
    }

public override J.Yield? VisitYield(J.Yield node, P p)
    {
        return base.VisitYield(node, p) as J.Yield;
    }

public override J.Unknown? VisitUnknown(J.Unknown node, P p)
    {
        return base.VisitUnknown(node, p) as J.Unknown;
    }

public override J.Unknown.Source? VisitUnknownSource(J.Unknown.Source node, P p)
    {
        return (base.VisitUnknownSource(node, p)) as J.Unknown.Source;
    }
}
