using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpIsoVisitor<P> : CSharpVisitor<P>
{
public override Cs.CompilationUnit? VisitCompilationUnit(Cs.CompilationUnit compilationUnit, P p)
    {
        return base.VisitCompilationUnit(compilationUnit, p) as Cs.CompilationUnit;
    }

public override Cs.OperatorDeclaration? VisitOperatorDeclaration(Cs.OperatorDeclaration operatorDeclaration, P p)
    {
        return base.VisitOperatorDeclaration(operatorDeclaration, p) as Cs.OperatorDeclaration;
    }

public override Cs.RefExpression? VisitRefExpression(Cs.RefExpression refExpression, P p)
    {
        return base.VisitRefExpression(refExpression, p) as Cs.RefExpression;
    }

public override Cs.PointerType? VisitPointerType(Cs.PointerType pointerType, P p)
    {
        return base.VisitPointerType(pointerType, p) as Cs.PointerType;
    }

public override Cs.RefType? VisitRefType(Cs.RefType refType, P p)
    {
        return base.VisitRefType(refType, p) as Cs.RefType;
    }

public override Cs.ForEachVariableLoop? VisitForEachVariableLoop(Cs.ForEachVariableLoop forEachVariableLoop, P p)
    {
        return base.VisitForEachVariableLoop(forEachVariableLoop, p) as Cs.ForEachVariableLoop;
    }

public override Cs.ForEachVariableLoop.Control? VisitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control control, P p)
    {
        return (base.VisitForEachVariableLoopControl(control, p)) as Cs.ForEachVariableLoop.Control;
    }

public override Cs.NameColon? VisitNameColon(Cs.NameColon nameColon, P p)
    {
        return base.VisitNameColon(nameColon, p) as Cs.NameColon;
    }

public override Cs.Argument? VisitArgument(Cs.Argument argument, P p)
    {
        return base.VisitArgument(argument, p) as Cs.Argument;
    }

public override Cs.AnnotatedStatement? VisitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, P p)
    {
        return base.VisitAnnotatedStatement(annotatedStatement, p) as Cs.AnnotatedStatement;
    }

public override Cs.ArrayRankSpecifier? VisitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, P p)
    {
        return base.VisitArrayRankSpecifier(arrayRankSpecifier, p) as Cs.ArrayRankSpecifier;
    }

public override Cs.AssignmentOperation? VisitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, P p)
    {
        return base.VisitAssignmentOperation(assignmentOperation, p) as Cs.AssignmentOperation;
    }

public override Cs.AttributeList? VisitAttributeList(Cs.AttributeList attributeList, P p)
    {
        return base.VisitAttributeList(attributeList, p) as Cs.AttributeList;
    }

public override Cs.AwaitExpression? VisitAwaitExpression(Cs.AwaitExpression awaitExpression, P p)
    {
        return base.VisitAwaitExpression(awaitExpression, p) as Cs.AwaitExpression;
    }

public override Cs.StackAllocExpression? VisitStackAllocExpression(Cs.StackAllocExpression stackAllocExpression, P p)
    {
        return base.VisitStackAllocExpression(stackAllocExpression, p) as Cs.StackAllocExpression;
    }

public override Cs.GotoStatement? VisitGotoStatement(Cs.GotoStatement gotoStatement, P p)
    {
        return base.VisitGotoStatement(gotoStatement, p) as Cs.GotoStatement;
    }

public override Cs.EventDeclaration? VisitEventDeclaration(Cs.EventDeclaration eventDeclaration, P p)
    {
        return base.VisitEventDeclaration(eventDeclaration, p) as Cs.EventDeclaration;
    }

public override Cs.Binary? VisitBinary(Cs.Binary binary, P p)
    {
        return base.VisitBinary(binary, p) as Cs.Binary;
    }

public override Cs.BlockScopeNamespaceDeclaration? VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, P p)
    {
        return base.VisitBlockScopeNamespaceDeclaration(blockScopeNamespaceDeclaration, p) as Cs.BlockScopeNamespaceDeclaration;
    }

public override Cs.CollectionExpression? VisitCollectionExpression(Cs.CollectionExpression collectionExpression, P p)
    {
        return base.VisitCollectionExpression(collectionExpression, p) as Cs.CollectionExpression;
    }

public override Cs.ExpressionStatement? VisitExpressionStatement(Cs.ExpressionStatement expressionStatement, P p)
    {
        return base.VisitExpressionStatement(expressionStatement, p) as Cs.ExpressionStatement;
    }

public override Cs.ExternAlias? VisitExternAlias(Cs.ExternAlias externAlias, P p)
    {
        return base.VisitExternAlias(externAlias, p) as Cs.ExternAlias;
    }

public override Cs.FileScopeNamespaceDeclaration? VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, P p)
    {
        return base.VisitFileScopeNamespaceDeclaration(fileScopeNamespaceDeclaration, p) as Cs.FileScopeNamespaceDeclaration;
    }

public override Cs.InterpolatedString? VisitInterpolatedString(Cs.InterpolatedString interpolatedString, P p)
    {
        return base.VisitInterpolatedString(interpolatedString, p) as Cs.InterpolatedString;
    }

public override Cs.Interpolation? VisitInterpolation(Cs.Interpolation interpolation, P p)
    {
        return base.VisitInterpolation(interpolation, p) as Cs.Interpolation;
    }

public override Cs.NullSafeExpression? VisitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, P p)
    {
        return base.VisitNullSafeExpression(nullSafeExpression, p) as Cs.NullSafeExpression;
    }

public override Cs.StatementExpression? VisitStatementExpression(Cs.StatementExpression statementExpression, P p)
    {
        return base.VisitStatementExpression(statementExpression, p) as Cs.StatementExpression;
    }

public override Cs.UsingDirective? VisitUsingDirective(Cs.UsingDirective usingDirective, P p)
    {
        return base.VisitUsingDirective(usingDirective, p) as Cs.UsingDirective;
    }

public override Cs.PropertyDeclaration? VisitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, P p)
    {
        return base.VisitPropertyDeclaration(propertyDeclaration, p) as Cs.PropertyDeclaration;
    }

public override Cs.Keyword? VisitKeyword(Cs.Keyword keyword, P p)
    {
        return base.VisitKeyword(keyword, p) as Cs.Keyword;
    }

public override Cs.Lambda? VisitLambda(Cs.Lambda lambda, P p)
    {
        return base.VisitLambda(lambda, p) as Cs.Lambda;
    }

public override Cs.ClassDeclaration? VisitClassDeclaration(Cs.ClassDeclaration classDeclaration, P p)
    {
        return base.VisitClassDeclaration(classDeclaration, p) as Cs.ClassDeclaration;
    }

public override Cs.MethodDeclaration? VisitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, P p)
    {
        return base.VisitMethodDeclaration(methodDeclaration, p) as Cs.MethodDeclaration;
    }

public override Cs.UsingStatement? VisitUsingStatement(Cs.UsingStatement usingStatement, P p)
    {
        return base.VisitUsingStatement(usingStatement, p) as Cs.UsingStatement;
    }

public override Cs.TypeParameterConstraintClause? VisitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, P p)
    {
        return base.VisitTypeParameterConstraintClause(typeParameterConstraintClause, p) as Cs.TypeParameterConstraintClause;
    }

public override Cs.TypeConstraint? VisitTypeConstraint(Cs.TypeConstraint typeConstraint, P p)
    {
        return base.VisitTypeConstraint(typeConstraint, p) as Cs.TypeConstraint;
    }

public override Cs.AllowsConstraintClause? VisitAllowsConstraintClause(Cs.AllowsConstraintClause allowsConstraintClause, P p)
    {
        return base.VisitAllowsConstraintClause(allowsConstraintClause, p) as Cs.AllowsConstraintClause;
    }

public override Cs.RefStructConstraint? VisitRefStructConstraint(Cs.RefStructConstraint refStructConstraint, P p)
    {
        return base.VisitRefStructConstraint(refStructConstraint, p) as Cs.RefStructConstraint;
    }

public override Cs.ClassOrStructConstraint? VisitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, P p)
    {
        return base.VisitClassOrStructConstraint(classOrStructConstraint, p) as Cs.ClassOrStructConstraint;
    }

public override Cs.ConstructorConstraint? VisitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, P p)
    {
        return base.VisitConstructorConstraint(constructorConstraint, p) as Cs.ConstructorConstraint;
    }

public override Cs.DefaultConstraint? VisitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, P p)
    {
        return base.VisitDefaultConstraint(defaultConstraint, p) as Cs.DefaultConstraint;
    }

public override Cs.DeclarationExpression? VisitDeclarationExpression(Cs.DeclarationExpression declarationExpression, P p)
    {
        return base.VisitDeclarationExpression(declarationExpression, p) as Cs.DeclarationExpression;
    }

public override Cs.SingleVariableDesignation? VisitSingleVariableDesignation(Cs.SingleVariableDesignation singleVariableDesignation, P p)
    {
        return base.VisitSingleVariableDesignation(singleVariableDesignation, p) as Cs.SingleVariableDesignation;
    }

public override Cs.ParenthesizedVariableDesignation? VisitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation parenthesizedVariableDesignation, P p)
    {
        return base.VisitParenthesizedVariableDesignation(parenthesizedVariableDesignation, p) as Cs.ParenthesizedVariableDesignation;
    }

public override Cs.DiscardVariableDesignation? VisitDiscardVariableDesignation(Cs.DiscardVariableDesignation discardVariableDesignation, P p)
    {
        return base.VisitDiscardVariableDesignation(discardVariableDesignation, p) as Cs.DiscardVariableDesignation;
    }

public override Cs.TupleExpression? VisitTupleExpression(Cs.TupleExpression tupleExpression, P p)
    {
        return base.VisitTupleExpression(tupleExpression, p) as Cs.TupleExpression;
    }

public override Cs.Constructor? VisitConstructor(Cs.Constructor constructor, P p)
    {
        return base.VisitConstructor(constructor, p) as Cs.Constructor;
    }

public override Cs.DestructorDeclaration? VisitDestructorDeclaration(Cs.DestructorDeclaration destructorDeclaration, P p)
    {
        return base.VisitDestructorDeclaration(destructorDeclaration, p) as Cs.DestructorDeclaration;
    }

public override Cs.Unary? VisitUnary(Cs.Unary unary, P p)
    {
        return base.VisitUnary(unary, p) as Cs.Unary;
    }

public override Cs.ConstructorInitializer? VisitConstructorInitializer(Cs.ConstructorInitializer constructorInitializer, P p)
    {
        return base.VisitConstructorInitializer(constructorInitializer, p) as Cs.ConstructorInitializer;
    }

public override Cs.TupleType? VisitTupleType(Cs.TupleType tupleType, P p)
    {
        return base.VisitTupleType(tupleType, p) as Cs.TupleType;
    }

public override Cs.TupleElement? VisitTupleElement(Cs.TupleElement tupleElement, P p)
    {
        return base.VisitTupleElement(tupleElement, p) as Cs.TupleElement;
    }

public override Cs.NewClass? VisitNewClass(Cs.NewClass newClass, P p)
    {
        return base.VisitNewClass(newClass, p) as Cs.NewClass;
    }

public override Cs.InitializerExpression? VisitInitializerExpression(Cs.InitializerExpression initializerExpression, P p)
    {
        return base.VisitInitializerExpression(initializerExpression, p) as Cs.InitializerExpression;
    }

public override Cs.ImplicitElementAccess? VisitImplicitElementAccess(Cs.ImplicitElementAccess implicitElementAccess, P p)
    {
        return base.VisitImplicitElementAccess(implicitElementAccess, p) as Cs.ImplicitElementAccess;
    }

public override Cs.Yield? VisitYield(Cs.Yield yield, P p)
    {
        return base.VisitYield(yield, p) as Cs.Yield;
    }

public override Cs.DefaultExpression? VisitDefaultExpression(Cs.DefaultExpression defaultExpression, P p)
    {
        return base.VisitDefaultExpression(defaultExpression, p) as Cs.DefaultExpression;
    }

public override Cs.IsPattern? VisitIsPattern(Cs.IsPattern isPattern, P p)
    {
        return base.VisitIsPattern(isPattern, p) as Cs.IsPattern;
    }

public override Cs.UnaryPattern? VisitUnaryPattern(Cs.UnaryPattern unaryPattern, P p)
    {
        return base.VisitUnaryPattern(unaryPattern, p) as Cs.UnaryPattern;
    }

public override Cs.TypePattern? VisitTypePattern(Cs.TypePattern typePattern, P p)
    {
        return base.VisitTypePattern(typePattern, p) as Cs.TypePattern;
    }

public override Cs.BinaryPattern? VisitBinaryPattern(Cs.BinaryPattern binaryPattern, P p)
    {
        return base.VisitBinaryPattern(binaryPattern, p) as Cs.BinaryPattern;
    }

public override Cs.ConstantPattern? VisitConstantPattern(Cs.ConstantPattern constantPattern, P p)
    {
        return base.VisitConstantPattern(constantPattern, p) as Cs.ConstantPattern;
    }

public override Cs.DiscardPattern? VisitDiscardPattern(Cs.DiscardPattern discardPattern, P p)
    {
        return base.VisitDiscardPattern(discardPattern, p) as Cs.DiscardPattern;
    }

public override Cs.ListPattern? VisitListPattern(Cs.ListPattern listPattern, P p)
    {
        return base.VisitListPattern(listPattern, p) as Cs.ListPattern;
    }

public override Cs.ParenthesizedPattern? VisitParenthesizedPattern(Cs.ParenthesizedPattern parenthesizedPattern, P p)
    {
        return base.VisitParenthesizedPattern(parenthesizedPattern, p) as Cs.ParenthesizedPattern;
    }

public override Cs.RecursivePattern? VisitRecursivePattern(Cs.RecursivePattern recursivePattern, P p)
    {
        return base.VisitRecursivePattern(recursivePattern, p) as Cs.RecursivePattern;
    }

public override Cs.VarPattern? VisitVarPattern(Cs.VarPattern varPattern, P p)
    {
        return base.VisitVarPattern(varPattern, p) as Cs.VarPattern;
    }

public override Cs.PositionalPatternClause? VisitPositionalPatternClause(Cs.PositionalPatternClause positionalPatternClause, P p)
    {
        return base.VisitPositionalPatternClause(positionalPatternClause, p) as Cs.PositionalPatternClause;
    }

public override Cs.RelationalPattern? VisitRelationalPattern(Cs.RelationalPattern relationalPattern, P p)
    {
        return base.VisitRelationalPattern(relationalPattern, p) as Cs.RelationalPattern;
    }

public override Cs.SlicePattern? VisitSlicePattern(Cs.SlicePattern slicePattern, P p)
    {
        return base.VisitSlicePattern(slicePattern, p) as Cs.SlicePattern;
    }

public override Cs.PropertyPatternClause? VisitPropertyPatternClause(Cs.PropertyPatternClause propertyPatternClause, P p)
    {
        return base.VisitPropertyPatternClause(propertyPatternClause, p) as Cs.PropertyPatternClause;
    }

public override Cs.Subpattern? VisitSubpattern(Cs.Subpattern subpattern, P p)
    {
        return base.VisitSubpattern(subpattern, p) as Cs.Subpattern;
    }

public override Cs.SwitchExpression? VisitSwitchExpression(Cs.SwitchExpression switchExpression, P p)
    {
        return base.VisitSwitchExpression(switchExpression, p) as Cs.SwitchExpression;
    }

public override Cs.SwitchExpressionArm? VisitSwitchExpressionArm(Cs.SwitchExpressionArm switchExpressionArm, P p)
    {
        return base.VisitSwitchExpressionArm(switchExpressionArm, p) as Cs.SwitchExpressionArm;
    }

public override Cs.SwitchSection? VisitSwitchSection(Cs.SwitchSection switchSection, P p)
    {
        return base.VisitSwitchSection(switchSection, p) as Cs.SwitchSection;
    }

public override Cs.DefaultSwitchLabel? VisitDefaultSwitchLabel(Cs.DefaultSwitchLabel defaultSwitchLabel, P p)
    {
        return base.VisitDefaultSwitchLabel(defaultSwitchLabel, p) as Cs.DefaultSwitchLabel;
    }

public override Cs.CasePatternSwitchLabel? VisitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel casePatternSwitchLabel, P p)
    {
        return base.VisitCasePatternSwitchLabel(casePatternSwitchLabel, p) as Cs.CasePatternSwitchLabel;
    }

public override Cs.SwitchStatement? VisitSwitchStatement(Cs.SwitchStatement switchStatement, P p)
    {
        return base.VisitSwitchStatement(switchStatement, p) as Cs.SwitchStatement;
    }

public override Cs.LockStatement? VisitLockStatement(Cs.LockStatement lockStatement, P p)
    {
        return base.VisitLockStatement(lockStatement, p) as Cs.LockStatement;
    }

public override Cs.FixedStatement? VisitFixedStatement(Cs.FixedStatement fixedStatement, P p)
    {
        return base.VisitFixedStatement(fixedStatement, p) as Cs.FixedStatement;
    }

public override Cs.CheckedExpression? VisitCheckedExpression(Cs.CheckedExpression checkedExpression, P p)
    {
        return base.VisitCheckedExpression(checkedExpression, p) as Cs.CheckedExpression;
    }

public override Cs.CheckedStatement? VisitCheckedStatement(Cs.CheckedStatement checkedStatement, P p)
    {
        return base.VisitCheckedStatement(checkedStatement, p) as Cs.CheckedStatement;
    }

public override Cs.UnsafeStatement? VisitUnsafeStatement(Cs.UnsafeStatement unsafeStatement, P p)
    {
        return base.VisitUnsafeStatement(unsafeStatement, p) as Cs.UnsafeStatement;
    }

public override Cs.RangeExpression? VisitRangeExpression(Cs.RangeExpression rangeExpression, P p)
    {
        return base.VisitRangeExpression(rangeExpression, p) as Cs.RangeExpression;
    }

public override Cs.QueryExpression? VisitQueryExpression(Cs.QueryExpression queryExpression, P p)
    {
        return base.VisitQueryExpression(queryExpression, p) as Cs.QueryExpression;
    }

public override Cs.QueryBody? VisitQueryBody(Cs.QueryBody queryBody, P p)
    {
        return base.VisitQueryBody(queryBody, p) as Cs.QueryBody;
    }

public override Cs.FromClause? VisitFromClause(Cs.FromClause fromClause, P p)
    {
        return base.VisitFromClause(fromClause, p) as Cs.FromClause;
    }

public override Cs.LetClause? VisitLetClause(Cs.LetClause letClause, P p)
    {
        return base.VisitLetClause(letClause, p) as Cs.LetClause;
    }

public override Cs.JoinClause? VisitJoinClause(Cs.JoinClause joinClause, P p)
    {
        return base.VisitJoinClause(joinClause, p) as Cs.JoinClause;
    }

public override Cs.JoinIntoClause? VisitJoinIntoClause(Cs.JoinIntoClause joinIntoClause, P p)
    {
        return base.VisitJoinIntoClause(joinIntoClause, p) as Cs.JoinIntoClause;
    }

public override Cs.WhereClause? VisitWhereClause(Cs.WhereClause whereClause, P p)
    {
        return base.VisitWhereClause(whereClause, p) as Cs.WhereClause;
    }

public override Cs.OrderByClause? VisitOrderByClause(Cs.OrderByClause orderByClause, P p)
    {
        return base.VisitOrderByClause(orderByClause, p) as Cs.OrderByClause;
    }

public override Cs.QueryContinuation? VisitQueryContinuation(Cs.QueryContinuation queryContinuation, P p)
    {
        return base.VisitQueryContinuation(queryContinuation, p) as Cs.QueryContinuation;
    }

public override Cs.Ordering? VisitOrdering(Cs.Ordering ordering, P p)
    {
        return base.VisitOrdering(ordering, p) as Cs.Ordering;
    }

public override Cs.SelectClause? VisitSelectClause(Cs.SelectClause selectClause, P p)
    {
        return base.VisitSelectClause(selectClause, p) as Cs.SelectClause;
    }

public override Cs.GroupClause? VisitGroupClause(Cs.GroupClause groupClause, P p)
    {
        return base.VisitGroupClause(groupClause, p) as Cs.GroupClause;
    }

public override Cs.IndexerDeclaration? VisitIndexerDeclaration(Cs.IndexerDeclaration indexerDeclaration, P p)
    {
        return base.VisitIndexerDeclaration(indexerDeclaration, p) as Cs.IndexerDeclaration;
    }

public override Cs.DelegateDeclaration? VisitDelegateDeclaration(Cs.DelegateDeclaration delegateDeclaration, P p)
    {
        return base.VisitDelegateDeclaration(delegateDeclaration, p) as Cs.DelegateDeclaration;
    }

public override Cs.ConversionOperatorDeclaration? VisitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration conversionOperatorDeclaration, P p)
    {
        return base.VisitConversionOperatorDeclaration(conversionOperatorDeclaration, p) as Cs.ConversionOperatorDeclaration;
    }

public override Cs.TypeParameter? VisitTypeParameter(Cs.TypeParameter typeParameter, P p)
    {
        return base.VisitTypeParameter(typeParameter, p) as Cs.TypeParameter;
    }

public override Cs.EnumDeclaration? VisitEnumDeclaration(Cs.EnumDeclaration enumDeclaration, P p)
    {
        return base.VisitEnumDeclaration(enumDeclaration, p) as Cs.EnumDeclaration;
    }

public override Cs.EnumMemberDeclaration? VisitEnumMemberDeclaration(Cs.EnumMemberDeclaration enumMemberDeclaration, P p)
    {
        return base.VisitEnumMemberDeclaration(enumMemberDeclaration, p) as Cs.EnumMemberDeclaration;
    }

public override Cs.AliasQualifiedName? VisitAliasQualifiedName(Cs.AliasQualifiedName aliasQualifiedName, P p)
    {
        return base.VisitAliasQualifiedName(aliasQualifiedName, p) as Cs.AliasQualifiedName;
    }

public override Cs.ArrayType? VisitArrayType(Cs.ArrayType arrayType, P p)
    {
        return base.VisitArrayType(arrayType, p) as Cs.ArrayType;
    }

public override Cs.Try? VisitTry(Cs.Try @try, P p)
    {
        return base.VisitTry(@try, p) as Cs.Try;
    }

public override Cs.Try.Catch? VisitTryCatch(Cs.Try.Catch @catch, P p)
    {
        return (base.VisitTryCatch(@catch, p)) as Cs.Try.Catch;
    }

public override Cs.ArrowExpressionClause? VisitArrowExpressionClause(Cs.ArrowExpressionClause arrowExpressionClause, P p)
    {
        return base.VisitArrowExpressionClause(arrowExpressionClause, p) as Cs.ArrowExpressionClause;
    }

public override Cs.AccessorDeclaration? VisitAccessorDeclaration(Cs.AccessorDeclaration accessorDeclaration, P p)
    {
        return base.VisitAccessorDeclaration(accessorDeclaration, p) as Cs.AccessorDeclaration;
    }

public override Cs.PointerFieldAccess? VisitPointerFieldAccess(Cs.PointerFieldAccess pointerFieldAccess, P p)
    {
        return base.VisitPointerFieldAccess(pointerFieldAccess, p) as Cs.PointerFieldAccess;
    }

public override Expression VisitExpression(Expression expression, P p)
    {
        return (Expression )(base.VisitExpression(expression, p));
    }

public override Statement VisitStatement(Statement statement, P p)
    {
        return (Statement )(base.VisitStatement(statement, p));
    }

public override J.AnnotatedType? VisitAnnotatedType(J.AnnotatedType annotatedType, P p)
    {
        return base.VisitAnnotatedType(annotatedType, p) as J.AnnotatedType;
    }

public override J.Annotation? VisitAnnotation(J.Annotation annotation, P p)
    {
        return base.VisitAnnotation(annotation, p) as J.Annotation;
    }

public override J.ArrayAccess? VisitArrayAccess(J.ArrayAccess arrayAccess, P p)
    {
        return base.VisitArrayAccess(arrayAccess, p) as J.ArrayAccess;
    }

public override J.ArrayType? VisitArrayType(J.ArrayType arrayType, P p)
    {
        return base.VisitArrayType(arrayType, p) as J.ArrayType;
    }

public override J.Assert? VisitAssert(J.Assert assert, P p)
    {
        return base.VisitAssert(assert, p) as J.Assert;
    }

public override J.Assignment? VisitAssignment(J.Assignment assignment, P p)
    {
        return base.VisitAssignment(assignment, p) as J.Assignment;
    }

public override J.AssignmentOperation? VisitAssignmentOperation(J.AssignmentOperation assignmentOperation, P p)
    {
        return base.VisitAssignmentOperation(assignmentOperation, p) as J.AssignmentOperation;
    }

public override J.Binary? VisitBinary(J.Binary binary, P p)
    {
        return base.VisitBinary(binary, p) as J.Binary;
    }

public override J.Block? VisitBlock(J.Block block, P p)
    {
        return base.VisitBlock(block, p) as J.Block;
    }

public override J.Break? VisitBreak(J.Break @break, P p)
    {
        return base.VisitBreak(@break, p) as J.Break;
    }

public override J.Case? VisitCase(J.Case @case, P p)
    {
        return base.VisitCase(@case, p) as J.Case;
    }

public override J.ClassDeclaration? VisitClassDeclaration(J.ClassDeclaration classDeclaration, P p)
    {
        return base.VisitClassDeclaration(classDeclaration, p) as J.ClassDeclaration;
    }

public override J.ClassDeclaration.Kind? VisitClassDeclarationKind(J.ClassDeclaration.Kind kind, P p)
    {
        return (base.VisitClassDeclarationKind(kind, p)) as J.ClassDeclaration.Kind;
    }

public override J.CompilationUnit? VisitCompilationUnit(J.CompilationUnit compilationUnit, P p)
    {
        return base.VisitCompilationUnit(compilationUnit, p) as J.CompilationUnit;
    }

public override J.Continue? VisitContinue(J.Continue @continue, P p)
    {
        return base.VisitContinue(@continue, p) as J.Continue;
    }

public override J.DoWhileLoop? VisitDoWhileLoop(J.DoWhileLoop doWhileLoop, P p)
    {
        return base.VisitDoWhileLoop(doWhileLoop, p) as J.DoWhileLoop;
    }

public override J.Empty? VisitEmpty(J.Empty empty, P p)
    {
        return base.VisitEmpty(empty, p) as J.Empty;
    }

public override J.EnumValue? VisitEnumValue(J.EnumValue enumValue, P p)
    {
        return base.VisitEnumValue(enumValue, p) as J.EnumValue;
    }

public override J.EnumValueSet? VisitEnumValueSet(J.EnumValueSet enumValueSet, P p)
    {
        return base.VisitEnumValueSet(enumValueSet, p) as J.EnumValueSet;
    }

public override J.FieldAccess? VisitFieldAccess(J.FieldAccess fieldAccess, P p)
    {
        return base.VisitFieldAccess(fieldAccess, p) as J.FieldAccess;
    }

public override J.ForEachLoop? VisitForEachLoop(J.ForEachLoop forEachLoop, P p)
    {
        return base.VisitForEachLoop(forEachLoop, p) as J.ForEachLoop;
    }

public override J.ForEachLoop.Control? VisitForEachControl(J.ForEachLoop.Control control, P p)
    {
        return (base.VisitForEachControl(control, p)) as J.ForEachLoop.Control;
    }

public override J.ForLoop? VisitForLoop(J.ForLoop forLoop, P p)
    {
        return base.VisitForLoop(forLoop, p) as J.ForLoop;
    }

public override J.ForLoop.Control? VisitForControl(J.ForLoop.Control control, P p)
    {
        return (base.VisitForControl(control, p)) as J.ForLoop.Control;
    }

public override J.ParenthesizedTypeTree? VisitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, P p)
    {
        return base.VisitParenthesizedTypeTree(parenthesizedTypeTree, p) as J.ParenthesizedTypeTree;
    }

public override J.Identifier? VisitIdentifier(J.Identifier identifier, P p)
    {
        return base.VisitIdentifier(identifier, p) as J.Identifier;
    }

public override J.If? VisitIf(J.If @if, P p)
    {
        return base.VisitIf(@if, p) as J.If;
    }

public override J.If.Else? VisitElse(J.If.Else @else, P p)
    {
        return (base.VisitElse(@else, p)) as J.If.Else;
    }

public override J.Import? VisitImport(J.Import import, P p)
    {
        return base.VisitImport(import, p) as J.Import;
    }

public override J.InstanceOf? VisitInstanceOf(J.InstanceOf instanceOf, P p)
    {
        return base.VisitInstanceOf(instanceOf, p) as J.InstanceOf;
    }

public override J.IntersectionType? VisitIntersectionType(J.IntersectionType intersectionType, P p)
    {
        return base.VisitIntersectionType(intersectionType, p) as J.IntersectionType;
    }

public override J.Label? VisitLabel(J.Label label, P p)
    {
        return base.VisitLabel(label, p) as J.Label;
    }

public override J.Lambda? VisitLambda(J.Lambda lambda, P p)
    {
        return base.VisitLambda(lambda, p) as J.Lambda;
    }

public override J.Lambda.Parameters? VisitLambdaParameters(J.Lambda.Parameters parameters, P p)
    {
        return (base.VisitLambdaParameters(parameters, p)) as J.Lambda.Parameters;
    }

public override J.Literal? VisitLiteral(J.Literal literal, P p)
    {
        return base.VisitLiteral(literal, p) as J.Literal;
    }

public override J.MemberReference? VisitMemberReference(J.MemberReference memberReference, P p)
    {
        return base.VisitMemberReference(memberReference, p) as J.MemberReference;
    }

public override J.MethodDeclaration? VisitMethodDeclaration(J.MethodDeclaration methodDeclaration, P p)
    {
        return base.VisitMethodDeclaration(methodDeclaration, p) as J.MethodDeclaration;
    }

public override J.MethodInvocation? VisitMethodInvocation(J.MethodInvocation methodInvocation, P p)
    {
        return base.VisitMethodInvocation(methodInvocation, p) as J.MethodInvocation;
    }

public override J.Modifier? VisitModifier(J.Modifier modifier, P p)
    {
        return base.VisitModifier(modifier, p) as J.Modifier;
    }

public override J.MultiCatch? VisitMultiCatch(J.MultiCatch multiCatch, P p)
    {
        return base.VisitMultiCatch(multiCatch, p) as J.MultiCatch;
    }

public override J.NewArray? VisitNewArray(J.NewArray newArray, P p)
    {
        return base.VisitNewArray(newArray, p) as J.NewArray;
    }

public override J.ArrayDimension? VisitArrayDimension(J.ArrayDimension arrayDimension, P p)
    {
        return base.VisitArrayDimension(arrayDimension, p) as J.ArrayDimension;
    }

public override J.NewClass? VisitNewClass(J.NewClass newClass, P p)
    {
        return base.VisitNewClass(newClass, p) as J.NewClass;
    }

public override J.NullableType? VisitNullableType(J.NullableType nullableType, P p)
    {
        return base.VisitNullableType(nullableType, p) as J.NullableType;
    }

public override J.Package? VisitPackage(J.Package package, P p)
    {
        return base.VisitPackage(package, p) as J.Package;
    }

public override J.ParameterizedType? VisitParameterizedType(J.ParameterizedType parameterizedType, P p)
    {
        return base.VisitParameterizedType(parameterizedType, p) as J.ParameterizedType;
    }

public override J.Parentheses<J2>? VisitParentheses<J2>(J.Parentheses<J2> parentheses, P p)
    {
        return (base.VisitParentheses(parentheses, p)) as J.Parentheses<J2>;
    }

public override J.ControlParentheses<J2>? VisitControlParentheses<J2>(J.ControlParentheses<J2> controlParentheses, P p)
    {
        return (base.VisitControlParentheses(controlParentheses, p)) as J.ControlParentheses<J2>;
    }

public override J.Primitive? VisitPrimitive(J.Primitive primitive, P p)
    {
        return base.VisitPrimitive(primitive, p) as J.Primitive;
    }

public override J.Return? VisitReturn(J.Return @return, P p)
    {
        return base.VisitReturn(@return, p) as J.Return;
    }

public override J.Switch? VisitSwitch(J.Switch @switch, P p)
    {
        return base.VisitSwitch(@switch, p) as J.Switch;
    }

public override J.SwitchExpression? VisitSwitchExpression(J.SwitchExpression switchExpression, P p)
    {
        return base.VisitSwitchExpression(switchExpression, p) as J.SwitchExpression;
    }

public override J.Synchronized? VisitSynchronized(J.Synchronized synchronized, P p)
    {
        return base.VisitSynchronized(synchronized, p) as J.Synchronized;
    }

public override J.Ternary? VisitTernary(J.Ternary ternary, P p)
    {
        return base.VisitTernary(ternary, p) as J.Ternary;
    }

public override J.Throw? VisitThrow(J.Throw @throw, P p)
    {
        return base.VisitThrow(@throw, p) as J.Throw;
    }

public override J.Try? VisitTry(J.Try @try, P p)
    {
        return base.VisitTry(@try, p) as J.Try;
    }

public override J.Try.Resource? VisitTryResource(J.Try.Resource resource, P p)
    {
        return (base.VisitTryResource(resource, p)) as J.Try.Resource;
    }

public override J.Try.Catch? VisitCatch(J.Try.Catch @catch, P p)
    {
        return (base.VisitCatch(@catch, p)) as J.Try.Catch;
    }

public override J.TypeCast? VisitTypeCast(J.TypeCast typeCast, P p)
    {
        return base.VisitTypeCast(typeCast, p) as J.TypeCast;
    }

public override J.TypeParameter? VisitTypeParameter(J.TypeParameter typeParameter, P p)
    {
        return base.VisitTypeParameter(typeParameter, p) as J.TypeParameter;
    }

public override J.TypeParameters? VisitTypeParameters(J.TypeParameters typeParameters, P p)
    {
        return base.VisitTypeParameters(typeParameters, p) as J.TypeParameters;
    }

public override J.Unary? VisitUnary(J.Unary unary, P p)
    {
        return base.VisitUnary(unary, p) as J.Unary;
    }

public override J.VariableDeclarations? VisitVariableDeclarations(J.VariableDeclarations variableDeclarations, P p)
    {
        return base.VisitVariableDeclarations(variableDeclarations, p) as J.VariableDeclarations;
    }

public override J.VariableDeclarations.NamedVariable? VisitVariable(J.VariableDeclarations.NamedVariable namedVariable, P p)
    {
        return base.VisitVariable(namedVariable, p) as J.VariableDeclarations.NamedVariable;
    }

public override J.WhileLoop? VisitWhileLoop(J.WhileLoop whileLoop, P p)
    {
        return base.VisitWhileLoop(whileLoop, p) as J.WhileLoop;
    }

public override J.Wildcard? VisitWildcard(J.Wildcard wildcard, P p)
    {
        return base.VisitWildcard(wildcard, p) as J.Wildcard;
    }

public override J.Yield? VisitYield(J.Yield yield, P p)
    {
        return base.VisitYield(yield, p) as J.Yield;
    }

public override J.Unknown? VisitUnknown(J.Unknown unknown, P p)
    {
        return base.VisitUnknown(unknown, p) as J.Unknown;
    }

public override J.Unknown.Source? VisitUnknownSource(J.Unknown.Source source, P p)
    {
        return (base.VisitUnknownSource(source, p)) as J.Unknown.Source;
    }
}
