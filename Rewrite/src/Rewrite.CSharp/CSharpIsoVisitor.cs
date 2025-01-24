#nullable disable
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpIsoVisitor<P> : CSharpVisitor<P>
{
    public override Cs.CompilationUnit VisitCompilationUnit(Cs.CompilationUnit compilationUnit, P p)
    {
        return (Cs.CompilationUnit )(base.VisitCompilationUnit(compilationUnit, p));
    }

    public override Cs.OperatorDeclaration VisitOperatorDeclaration(Cs.OperatorDeclaration operatorDeclaration, P p)
    {
        return (Cs.OperatorDeclaration )(base.VisitOperatorDeclaration(operatorDeclaration, p));
    }

    public override Cs.RefExpression VisitRefExpression(Cs.RefExpression refExpression, P p)
    {
        return (Cs.RefExpression )(base.VisitRefExpression(refExpression, p));
    }

    public override Cs.PointerType VisitPointerType(Cs.PointerType pointerType, P p)
    {
        return (Cs.PointerType )(base.VisitPointerType(pointerType, p));
    }

    public override Cs.RefType VisitRefType(Cs.RefType refType, P p)
    {
        return (Cs.RefType )(base.VisitRefType(refType, p));
    }

    public override Cs.ForEachVariableLoop VisitForEachVariableLoop(Cs.ForEachVariableLoop forEachVariableLoop, P p)
    {
        return (Cs.ForEachVariableLoop )(base.VisitForEachVariableLoop(forEachVariableLoop, p));
    }

    public override Cs.ForEachVariableLoop.Control VisitForEachVariableLoopControl(Cs.ForEachVariableLoop.Control control, P p)
    {
        return (Cs.ForEachVariableLoop.Control )(base.VisitForEachVariableLoopControl(control, p));
    }

    public override Cs.NameColon VisitNameColon(Cs.NameColon nameColon, P p)
    {
        return (Cs.NameColon )(base.VisitNameColon(nameColon, p));
    }

    public override Cs.Argument VisitArgument(Cs.Argument argument, P p)
    {
        return (Cs.Argument )(base.VisitArgument(argument, p));
    }

    public override Cs.AnnotatedStatement VisitAnnotatedStatement(Cs.AnnotatedStatement annotatedStatement, P p)
    {
        return (Cs.AnnotatedStatement )(base.VisitAnnotatedStatement(annotatedStatement, p));
    }

    public override Cs.ArrayRankSpecifier VisitArrayRankSpecifier(Cs.ArrayRankSpecifier arrayRankSpecifier, P p)
    {
        return (Cs.ArrayRankSpecifier )(base.VisitArrayRankSpecifier(arrayRankSpecifier, p));
    }

    public override Cs.AssignmentOperation VisitAssignmentOperation(Cs.AssignmentOperation assignmentOperation, P p)
    {
        return (Cs.AssignmentOperation )(base.VisitAssignmentOperation(assignmentOperation, p));
    }

    public override Cs.AttributeList VisitAttributeList(Cs.AttributeList attributeList, P p)
    {
        return (Cs.AttributeList )(base.VisitAttributeList(attributeList, p));
    }

    public override Cs.AwaitExpression VisitAwaitExpression(Cs.AwaitExpression awaitExpression, P p)
    {
        return (Cs.AwaitExpression )(base.VisitAwaitExpression(awaitExpression, p));
    }

    public override Cs.StackAllocExpression VisitStackAllocExpression(Cs.StackAllocExpression stackAllocExpression, P p)
    {
        return (Cs.StackAllocExpression )(base.VisitStackAllocExpression(stackAllocExpression, p));
    }

    public override Cs.GotoStatement VisitGotoStatement(Cs.GotoStatement gotoStatement, P p)
    {
        return (Cs.GotoStatement )(base.VisitGotoStatement(gotoStatement, p));
    }

    public override Cs.EventDeclaration VisitEventDeclaration(Cs.EventDeclaration eventDeclaration, P p)
    {
        return (Cs.EventDeclaration )(base.VisitEventDeclaration(eventDeclaration, p));
    }

    public override Cs.Binary VisitBinary(Cs.Binary binary, P p)
    {
        return (Cs.Binary )(base.VisitBinary(binary, p));
    }

    public override Cs.BlockScopeNamespaceDeclaration VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration blockScopeNamespaceDeclaration, P p)
    {
        return (Cs.BlockScopeNamespaceDeclaration )(base.VisitBlockScopeNamespaceDeclaration(blockScopeNamespaceDeclaration, p));
    }

    public override Cs.CollectionExpression VisitCollectionExpression(Cs.CollectionExpression collectionExpression, P p)
    {
        return (Cs.CollectionExpression )(base.VisitCollectionExpression(collectionExpression, p));
    }

    public override Cs.ExpressionStatement VisitExpressionStatement(Cs.ExpressionStatement expressionStatement, P p)
    {
        return (Cs.ExpressionStatement )(base.VisitExpressionStatement(expressionStatement, p));
    }

    public override Cs.ExternAlias VisitExternAlias(Cs.ExternAlias externAlias, P p)
    {
        return (Cs.ExternAlias )(base.VisitExternAlias(externAlias, p));
    }

    public override Cs.FileScopeNamespaceDeclaration VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration fileScopeNamespaceDeclaration, P p)
    {
        return (Cs.FileScopeNamespaceDeclaration )(base.VisitFileScopeNamespaceDeclaration(fileScopeNamespaceDeclaration, p));
    }

    public override Cs.InterpolatedString VisitInterpolatedString(Cs.InterpolatedString interpolatedString, P p)
    {
        return (Cs.InterpolatedString )(base.VisitInterpolatedString(interpolatedString, p));
    }

    public override Cs.Interpolation VisitInterpolation(Cs.Interpolation interpolation, P p)
    {
        return (Cs.Interpolation )(base.VisitInterpolation(interpolation, p));
    }

    public override Cs.NullSafeExpression VisitNullSafeExpression(Cs.NullSafeExpression nullSafeExpression, P p)
    {
        return (Cs.NullSafeExpression )(base.VisitNullSafeExpression(nullSafeExpression, p));
    }

    public override Cs.StatementExpression VisitStatementExpression(Cs.StatementExpression statementExpression, P p)
    {
        return (Cs.StatementExpression )(base.VisitStatementExpression(statementExpression, p));
    }

    public override Cs.UsingDirective VisitUsingDirective(Cs.UsingDirective usingDirective, P p)
    {
        return (Cs.UsingDirective )(base.VisitUsingDirective(usingDirective, p));
    }

    public override Cs.PropertyDeclaration VisitPropertyDeclaration(Cs.PropertyDeclaration propertyDeclaration, P p)
    {
        return (Cs.PropertyDeclaration )(base.VisitPropertyDeclaration(propertyDeclaration, p));
    }

    public override Cs.Keyword VisitKeyword(Cs.Keyword keyword, P p)
    {
        return (Cs.Keyword )(base.VisitKeyword(keyword, p));
    }

    public override Cs.Lambda VisitLambda(Cs.Lambda lambda, P p)
    {
        return (Cs.Lambda )(base.VisitLambda(lambda, p));
    }

    public override Cs.ClassDeclaration VisitClassDeclaration(Cs.ClassDeclaration classDeclaration, P p)
    {
        return (Cs.ClassDeclaration )(base.VisitClassDeclaration(classDeclaration, p));
    }

    public override Cs.MethodDeclaration VisitMethodDeclaration(Cs.MethodDeclaration methodDeclaration, P p)
    {
        return (Cs.MethodDeclaration )(base.VisitMethodDeclaration(methodDeclaration, p));
    }

    public override Cs.UsingStatement VisitUsingStatement(Cs.UsingStatement usingStatement, P p)
    {
        return (Cs.UsingStatement )(base.VisitUsingStatement(usingStatement, p));
    }

    public override Cs.TypeParameterConstraintClause VisitTypeParameterConstraintClause(Cs.TypeParameterConstraintClause typeParameterConstraintClause, P p)
    {
        return (Cs.TypeParameterConstraintClause )(base.VisitTypeParameterConstraintClause(typeParameterConstraintClause, p));
    }

    public override Cs.TypeConstraint VisitTypeConstraint(Cs.TypeConstraint typeConstraint, P p)
    {
        return (Cs.TypeConstraint )(base.VisitTypeConstraint(typeConstraint, p));
    }

    public override Cs.AllowsConstraintClause VisitAllowsConstraintClause(Cs.AllowsConstraintClause allowsConstraintClause, P p)
    {
        return (Cs.AllowsConstraintClause )(base.VisitAllowsConstraintClause(allowsConstraintClause, p));
    }

    public override Cs.RefStructConstraint VisitRefStructConstraint(Cs.RefStructConstraint refStructConstraint, P p)
    {
        return (Cs.RefStructConstraint )(base.VisitRefStructConstraint(refStructConstraint, p));
    }

    public override Cs.ClassOrStructConstraint VisitClassOrStructConstraint(Cs.ClassOrStructConstraint classOrStructConstraint, P p)
    {
        return (Cs.ClassOrStructConstraint )(base.VisitClassOrStructConstraint(classOrStructConstraint, p));
    }

    public override Cs.ConstructorConstraint VisitConstructorConstraint(Cs.ConstructorConstraint constructorConstraint, P p)
    {
        return (Cs.ConstructorConstraint )(base.VisitConstructorConstraint(constructorConstraint, p));
    }

    public override Cs.DefaultConstraint VisitDefaultConstraint(Cs.DefaultConstraint defaultConstraint, P p)
    {
        return (Cs.DefaultConstraint )(base.VisitDefaultConstraint(defaultConstraint, p));
    }

    public override Cs.DeclarationExpression VisitDeclarationExpression(Cs.DeclarationExpression declarationExpression, P p)
    {
        return (Cs.DeclarationExpression )(base.VisitDeclarationExpression(declarationExpression, p));
    }

    public override Cs.SingleVariableDesignation VisitSingleVariableDesignation(Cs.SingleVariableDesignation singleVariableDesignation, P p)
    {
        return (Cs.SingleVariableDesignation )(base.VisitSingleVariableDesignation(singleVariableDesignation, p));
    }

    public override Cs.ParenthesizedVariableDesignation VisitParenthesizedVariableDesignation(Cs.ParenthesizedVariableDesignation parenthesizedVariableDesignation, P p)
    {
        return (Cs.ParenthesizedVariableDesignation )(base.VisitParenthesizedVariableDesignation(parenthesizedVariableDesignation, p));
    }

    public override Cs.DiscardVariableDesignation VisitDiscardVariableDesignation(Cs.DiscardVariableDesignation discardVariableDesignation, P p)
    {
        return (Cs.DiscardVariableDesignation )(base.VisitDiscardVariableDesignation(discardVariableDesignation, p));
    }

    public override Cs.TupleExpression VisitTupleExpression(Cs.TupleExpression tupleExpression, P p)
    {
        return (Cs.TupleExpression )(base.VisitTupleExpression(tupleExpression, p));
    }

    public override Cs.Constructor VisitConstructor(Cs.Constructor constructor, P p)
    {
        return (Cs.Constructor )(base.VisitConstructor(constructor, p));
    }

    public override Cs.DestructorDeclaration VisitDestructorDeclaration(Cs.DestructorDeclaration destructorDeclaration, P p)
    {
        return (Cs.DestructorDeclaration )(base.VisitDestructorDeclaration(destructorDeclaration, p));
    }

    public override Cs.Unary VisitUnary(Cs.Unary unary, P p)
    {
        return (Cs.Unary )(base.VisitUnary(unary, p));
    }

    public override Cs.ConstructorInitializer VisitConstructorInitializer(Cs.ConstructorInitializer constructorInitializer, P p)
    {
        return (Cs.ConstructorInitializer )(base.VisitConstructorInitializer(constructorInitializer, p));
    }

    public override Cs.TupleType VisitTupleType(Cs.TupleType tupleType, P p)
    {
        return (Cs.TupleType )(base.VisitTupleType(tupleType, p));
    }

    public override Cs.TupleElement VisitTupleElement(Cs.TupleElement tupleElement, P p)
    {
        return (Cs.TupleElement )(base.VisitTupleElement(tupleElement, p));
    }

    public override Cs.NewClass VisitNewClass(Cs.NewClass newClass, P p)
    {
        return (Cs.NewClass )(base.VisitNewClass(newClass, p));
    }

    public override Cs.InitializerExpression VisitInitializerExpression(Cs.InitializerExpression initializerExpression, P p)
    {
        return (Cs.InitializerExpression )(base.VisitInitializerExpression(initializerExpression, p));
    }

    public override Cs.ImplicitElementAccess VisitImplicitElementAccess(Cs.ImplicitElementAccess implicitElementAccess, P p)
    {
        return (Cs.ImplicitElementAccess )(base.VisitImplicitElementAccess(implicitElementAccess, p));
    }

    public override Cs.Yield VisitYield(Cs.Yield yield, P p)
    {
        return (Cs.Yield )(base.VisitYield(yield, p));
    }

    public override Cs.DefaultExpression VisitDefaultExpression(Cs.DefaultExpression defaultExpression, P p)
    {
        return (Cs.DefaultExpression )(base.VisitDefaultExpression(defaultExpression, p));
    }

    public override Cs.IsPattern VisitIsPattern(Cs.IsPattern isPattern, P p)
    {
        return (Cs.IsPattern )(base.VisitIsPattern(isPattern, p));
    }

    public override Cs.UnaryPattern VisitUnaryPattern(Cs.UnaryPattern unaryPattern, P p)
    {
        return (Cs.UnaryPattern )(base.VisitUnaryPattern(unaryPattern, p));
    }

    public override Cs.TypePattern VisitTypePattern(Cs.TypePattern typePattern, P p)
    {
        return (Cs.TypePattern )(base.VisitTypePattern(typePattern, p));
    }

    public override Cs.BinaryPattern VisitBinaryPattern(Cs.BinaryPattern binaryPattern, P p)
    {
        return (Cs.BinaryPattern )(base.VisitBinaryPattern(binaryPattern, p));
    }

    public override Cs.ConstantPattern VisitConstantPattern(Cs.ConstantPattern constantPattern, P p)
    {
        return (Cs.ConstantPattern )(base.VisitConstantPattern(constantPattern, p));
    }

    public override Cs.DiscardPattern VisitDiscardPattern(Cs.DiscardPattern discardPattern, P p)
    {
        return (Cs.DiscardPattern )(base.VisitDiscardPattern(discardPattern, p));
    }

    public override Cs.ListPattern VisitListPattern(Cs.ListPattern listPattern, P p)
    {
        return (Cs.ListPattern )(base.VisitListPattern(listPattern, p));
    }

    public override Cs.ParenthesizedPattern VisitParenthesizedPattern(Cs.ParenthesizedPattern parenthesizedPattern, P p)
    {
        return (Cs.ParenthesizedPattern )(base.VisitParenthesizedPattern(parenthesizedPattern, p));
    }

    public override Cs.RecursivePattern VisitRecursivePattern(Cs.RecursivePattern recursivePattern, P p)
    {
        return (Cs.RecursivePattern )(base.VisitRecursivePattern(recursivePattern, p));
    }

    public override Cs.VarPattern VisitVarPattern(Cs.VarPattern varPattern, P p)
    {
        return (Cs.VarPattern )(base.VisitVarPattern(varPattern, p));
    }

    public override Cs.PositionalPatternClause VisitPositionalPatternClause(Cs.PositionalPatternClause positionalPatternClause, P p)
    {
        return (Cs.PositionalPatternClause )(base.VisitPositionalPatternClause(positionalPatternClause, p));
    }

    public override Cs.RelationalPattern VisitRelationalPattern(Cs.RelationalPattern relationalPattern, P p)
    {
        return (Cs.RelationalPattern )(base.VisitRelationalPattern(relationalPattern, p));
    }

    public override Cs.SlicePattern VisitSlicePattern(Cs.SlicePattern slicePattern, P p)
    {
        return (Cs.SlicePattern )(base.VisitSlicePattern(slicePattern, p));
    }

    public override Cs.PropertyPatternClause VisitPropertyPatternClause(Cs.PropertyPatternClause propertyPatternClause, P p)
    {
        return (Cs.PropertyPatternClause )(base.VisitPropertyPatternClause(propertyPatternClause, p));
    }

    public override Cs.Subpattern VisitSubpattern(Cs.Subpattern subpattern, P p)
    {
        return (Cs.Subpattern )(base.VisitSubpattern(subpattern, p));
    }

    public override Cs.SwitchExpression VisitSwitchExpression(Cs.SwitchExpression switchExpression, P p)
    {
        return (Cs.SwitchExpression )(base.VisitSwitchExpression(switchExpression, p));
    }

    public override Cs.SwitchExpressionArm VisitSwitchExpressionArm(Cs.SwitchExpressionArm switchExpressionArm, P p)
    {
        return (Cs.SwitchExpressionArm )(base.VisitSwitchExpressionArm(switchExpressionArm, p));
    }

    public override Cs.SwitchSection VisitSwitchSection(Cs.SwitchSection switchSection, P p)
    {
        return (Cs.SwitchSection )(base.VisitSwitchSection(switchSection, p));
    }

    public override Cs.DefaultSwitchLabel VisitDefaultSwitchLabel(Cs.DefaultSwitchLabel defaultSwitchLabel, P p)
    {
        return (Cs.DefaultSwitchLabel )(base.VisitDefaultSwitchLabel(defaultSwitchLabel, p));
    }

    public override Cs.CasePatternSwitchLabel VisitCasePatternSwitchLabel(Cs.CasePatternSwitchLabel casePatternSwitchLabel, P p)
    {
        return (Cs.CasePatternSwitchLabel )(base.VisitCasePatternSwitchLabel(casePatternSwitchLabel, p));
    }

    public override Cs.SwitchStatement VisitSwitchStatement(Cs.SwitchStatement switchStatement, P p)
    {
        return (Cs.SwitchStatement )(base.VisitSwitchStatement(switchStatement, p));
    }

    public override Cs.LockStatement VisitLockStatement(Cs.LockStatement lockStatement, P p)
    {
        return (Cs.LockStatement )(base.VisitLockStatement(lockStatement, p));
    }

    public override Cs.FixedStatement VisitFixedStatement(Cs.FixedStatement fixedStatement, P p)
    {
        return (Cs.FixedStatement )(base.VisitFixedStatement(fixedStatement, p));
    }

    public override Cs.CheckedExpression VisitCheckedExpression(Cs.CheckedExpression checkedExpression, P p)
    {
        return (Cs.CheckedExpression )(base.VisitCheckedExpression(checkedExpression, p));
    }

    public override Cs.CheckedStatement VisitCheckedStatement(Cs.CheckedStatement checkedStatement, P p)
    {
        return (Cs.CheckedStatement )(base.VisitCheckedStatement(checkedStatement, p));
    }

    public override Cs.UnsafeStatement VisitUnsafeStatement(Cs.UnsafeStatement unsafeStatement, P p)
    {
        return (Cs.UnsafeStatement )(base.VisitUnsafeStatement(unsafeStatement, p));
    }

    public override Cs.RangeExpression VisitRangeExpression(Cs.RangeExpression rangeExpression, P p)
    {
        return (Cs.RangeExpression )(base.VisitRangeExpression(rangeExpression, p));
    }

    public override Cs.QueryExpression VisitQueryExpression(Cs.QueryExpression queryExpression, P p)
    {
        return (Cs.QueryExpression )(base.VisitQueryExpression(queryExpression, p));
    }

    public override Cs.QueryBody VisitQueryBody(Cs.QueryBody queryBody, P p)
    {
        return (Cs.QueryBody )(base.VisitQueryBody(queryBody, p));
    }

    public override Cs.FromClause VisitFromClause(Cs.FromClause fromClause, P p)
    {
        return (Cs.FromClause )(base.VisitFromClause(fromClause, p));
    }

    public override Cs.LetClause VisitLetClause(Cs.LetClause letClause, P p)
    {
        return (Cs.LetClause )(base.VisitLetClause(letClause, p));
    }

    public override Cs.JoinClause VisitJoinClause(Cs.JoinClause joinClause, P p)
    {
        return (Cs.JoinClause )(base.VisitJoinClause(joinClause, p));
    }

    public override Cs.JoinIntoClause VisitJoinIntoClause(Cs.JoinIntoClause joinIntoClause, P p)
    {
        return (Cs.JoinIntoClause )(base.VisitJoinIntoClause(joinIntoClause, p));
    }

    public override Cs.WhereClause VisitWhereClause(Cs.WhereClause whereClause, P p)
    {
        return (Cs.WhereClause )(base.VisitWhereClause(whereClause, p));
    }

    public override Cs.OrderByClause VisitOrderByClause(Cs.OrderByClause orderByClause, P p)
    {
        return (Cs.OrderByClause )(base.VisitOrderByClause(orderByClause, p));
    }

    public override Cs.QueryContinuation VisitQueryContinuation(Cs.QueryContinuation queryContinuation, P p)
    {
        return (Cs.QueryContinuation )(base.VisitQueryContinuation(queryContinuation, p));
    }

    public override Cs.Ordering VisitOrdering(Cs.Ordering ordering, P p)
    {
        return (Cs.Ordering )(base.VisitOrdering(ordering, p));
    }

    public override Cs.SelectClause VisitSelectClause(Cs.SelectClause selectClause, P p)
    {
        return (Cs.SelectClause )(base.VisitSelectClause(selectClause, p));
    }

    public override Cs.GroupClause VisitGroupClause(Cs.GroupClause groupClause, P p)
    {
        return (Cs.GroupClause )(base.VisitGroupClause(groupClause, p));
    }

    public override Cs.IndexerDeclaration VisitIndexerDeclaration(Cs.IndexerDeclaration indexerDeclaration, P p)
    {
        return (Cs.IndexerDeclaration )(base.VisitIndexerDeclaration(indexerDeclaration, p));
    }

    public override Cs.DelegateDeclaration VisitDelegateDeclaration(Cs.DelegateDeclaration delegateDeclaration, P p)
    {
        return (Cs.DelegateDeclaration )(base.VisitDelegateDeclaration(delegateDeclaration, p));
    }

    public override Cs.ConversionOperatorDeclaration VisitConversionOperatorDeclaration(Cs.ConversionOperatorDeclaration conversionOperatorDeclaration, P p)
    {
        return (Cs.ConversionOperatorDeclaration )(base.VisitConversionOperatorDeclaration(conversionOperatorDeclaration, p));
    }

    public override Cs.TypeParameter VisitTypeParameter(Cs.TypeParameter typeParameter, P p)
    {
        return (Cs.TypeParameter )(base.VisitTypeParameter(typeParameter, p));
    }

    public override Cs.EnumDeclaration VisitEnumDeclaration(Cs.EnumDeclaration enumDeclaration, P p)
    {
        return (Cs.EnumDeclaration )(base.VisitEnumDeclaration(enumDeclaration, p));
    }

    public override Cs.EnumMemberDeclaration VisitEnumMemberDeclaration(Cs.EnumMemberDeclaration enumMemberDeclaration, P p)
    {
        return (Cs.EnumMemberDeclaration )(base.VisitEnumMemberDeclaration(enumMemberDeclaration, p));
    }

    public override Cs.AliasQualifiedName VisitAliasQualifiedName(Cs.AliasQualifiedName aliasQualifiedName, P p)
    {
        return (Cs.AliasQualifiedName )(base.VisitAliasQualifiedName(aliasQualifiedName, p));
    }

    public override Cs.ArrayType VisitArrayType(Cs.ArrayType arrayType, P p)
    {
        return (Cs.ArrayType )(base.VisitArrayType(arrayType, p));
    }

    public override Cs.Try VisitTry(Cs.Try @try, P p)
    {
        return (Cs.Try )(base.VisitTry(@try, p));
    }

    public override Cs.Try.Catch VisitTryCatch(Cs.Try.Catch @catch, P p)
    {
        return (Cs.Try.Catch )(base.VisitTryCatch(@catch, p));
    }

    public override Cs.ArrowExpressionClause VisitArrowExpressionClause(Cs.ArrowExpressionClause arrowExpressionClause, P p)
    {
        return (Cs.ArrowExpressionClause )(base.VisitArrowExpressionClause(arrowExpressionClause, p));
    }

    public override Cs.AccessorDeclaration VisitAccessorDeclaration(Cs.AccessorDeclaration accessorDeclaration, P p)
    {
        return (Cs.AccessorDeclaration )(base.VisitAccessorDeclaration(accessorDeclaration, p));
    }

    public override Cs.PointerFieldAccess VisitPointerFieldAccess(Cs.PointerFieldAccess pointerFieldAccess, P p)
    {
        return (Cs.PointerFieldAccess )(base.VisitPointerFieldAccess(pointerFieldAccess, p));
    }

    protected override Space VisitSpace(Space space, CsSpace.Location loc, P p)
    {
        return (Space )(base.VisitSpace(space, loc, p));
    }

    public override Expression VisitExpression(Expression expression, P p)
    {
        return (Expression )(base.VisitExpression(expression, p));
    }

    public override Statement VisitStatement(Statement statement, P p)
    {
        return (Statement )(base.VisitStatement(statement, p));
    }

    public override J.AnnotatedType VisitAnnotatedType(J.AnnotatedType annotatedType, P p)
    {
        return (J.AnnotatedType )(base.VisitAnnotatedType(annotatedType, p));
    }

    public override J.Annotation VisitAnnotation(J.Annotation annotation, P p)
    {
        return (J.Annotation )(base.VisitAnnotation(annotation, p));
    }

    public override J.ArrayAccess VisitArrayAccess(J.ArrayAccess arrayAccess, P p)
    {
        return (J.ArrayAccess )(base.VisitArrayAccess(arrayAccess, p));
    }

    public override J.ArrayType VisitArrayType(J.ArrayType arrayType, P p)
    {
        return (J.ArrayType )(base.VisitArrayType(arrayType, p));
    }

    public override J.Assert VisitAssert(J.Assert assert, P p)
    {
        return (J.Assert )(base.VisitAssert(assert, p));
    }

    public override J.Assignment VisitAssignment(J.Assignment assignment, P p)
    {
        return (J.Assignment )(base.VisitAssignment(assignment, p));
    }

    public override J.AssignmentOperation VisitAssignmentOperation(J.AssignmentOperation assignmentOperation, P p)
    {
        return (J.AssignmentOperation )(base.VisitAssignmentOperation(assignmentOperation, p));
    }

    public override J.Binary VisitBinary(J.Binary binary, P p)
    {
        return (J.Binary )(base.VisitBinary(binary, p));
    }

    public override J.Block VisitBlock(J.Block block, P p)
    {
        return (J.Block )(base.VisitBlock(block, p));
    }

    public override J.Break VisitBreak(J.Break @break, P p)
    {
        return (J.Break )(base.VisitBreak(@break, p));
    }

    public override J.Case VisitCase(J.Case @case, P p)
    {
        return (J.Case )(base.VisitCase(@case, p));
    }

    public override J.ClassDeclaration VisitClassDeclaration(J.ClassDeclaration classDeclaration, P p)
    {
        return (J.ClassDeclaration )(base.VisitClassDeclaration(classDeclaration, p));
    }

    public override J.ClassDeclaration.Kind VisitClassDeclarationKind(J.ClassDeclaration.Kind kind, P p)
    {
        return (J.ClassDeclaration.Kind )(base.VisitClassDeclarationKind(kind, p));
    }

    public override J.CompilationUnit VisitCompilationUnit(J.CompilationUnit compilationUnit, P p)
    {
        return (J.CompilationUnit )(base.VisitCompilationUnit(compilationUnit, p));
    }

    public override J.Continue VisitContinue(J.Continue @continue, P p)
    {
        return (J.Continue )(base.VisitContinue(@continue, p));
    }

    public override J.DoWhileLoop VisitDoWhileLoop(J.DoWhileLoop doWhileLoop, P p)
    {
        return (J.DoWhileLoop )(base.VisitDoWhileLoop(doWhileLoop, p));
    }

    public override J.Empty VisitEmpty(J.Empty empty, P p)
    {
        return (J.Empty )(base.VisitEmpty(empty, p));
    }

    public override J.EnumValue VisitEnumValue(J.EnumValue enumValue, P p)
    {
        return (J.EnumValue )(base.VisitEnumValue(enumValue, p));
    }

    public override J.EnumValueSet VisitEnumValueSet(J.EnumValueSet enumValueSet, P p)
    {
        return (J.EnumValueSet )(base.VisitEnumValueSet(enumValueSet, p));
    }

    public override J.FieldAccess VisitFieldAccess(J.FieldAccess fieldAccess, P p)
    {
        return (J.FieldAccess )(base.VisitFieldAccess(fieldAccess, p));
    }

    public override J.ForEachLoop VisitForEachLoop(J.ForEachLoop forEachLoop, P p)
    {
        return (J.ForEachLoop )(base.VisitForEachLoop(forEachLoop, p));
    }

    public override J.ForEachLoop.Control VisitForEachControl(J.ForEachLoop.Control control, P p)
    {
        return (J.ForEachLoop.Control )(base.VisitForEachControl(control, p));
    }

    public override J.ForLoop VisitForLoop(J.ForLoop forLoop, P p)
    {
        return (J.ForLoop )(base.VisitForLoop(forLoop, p));
    }

    public override J.ForLoop.Control VisitForControl(J.ForLoop.Control control, P p)
    {
        return (J.ForLoop.Control )(base.VisitForControl(control, p));
    }

    public override J.ParenthesizedTypeTree VisitParenthesizedTypeTree(J.ParenthesizedTypeTree parenthesizedTypeTree, P p)
    {
        return (J.ParenthesizedTypeTree )(base.VisitParenthesizedTypeTree(parenthesizedTypeTree, p));
    }

    public override J.Identifier VisitIdentifier(J.Identifier identifier, P p)
    {
        return (J.Identifier )(base.VisitIdentifier(identifier, p));
    }

    public override J.If VisitIf(J.If @if, P p)
    {
        return (J.If )(base.VisitIf(@if, p));
    }

    public override J.If.Else VisitElse(J.If.Else @else, P p)
    {
        return (J.If.Else )(base.VisitElse(@else, p));
    }

    public override J.Import VisitImport(J.Import import, P p)
    {
        return (J.Import )(base.VisitImport(import, p));
    }

    public override J.InstanceOf VisitInstanceOf(J.InstanceOf instanceOf, P p)
    {
        return (J.InstanceOf )(base.VisitInstanceOf(instanceOf, p));
    }

    public override J.IntersectionType VisitIntersectionType(J.IntersectionType intersectionType, P p)
    {
        return (J.IntersectionType )(base.VisitIntersectionType(intersectionType, p));
    }

    public override J.Label VisitLabel(J.Label label, P p)
    {
        return (J.Label )(base.VisitLabel(label, p));
    }

    public override J.Lambda VisitLambda(J.Lambda lambda, P p)
    {
        return (J.Lambda )(base.VisitLambda(lambda, p));
    }

    public override J.Lambda.Parameters VisitLambdaParameters(J.Lambda.Parameters parameters, P p)
    {
        return (J.Lambda.Parameters )(base.VisitLambdaParameters(parameters, p));
    }

    public override J.Literal VisitLiteral(J.Literal literal, P p)
    {
        return (J.Literal )(base.VisitLiteral(literal, p));
    }

    public override J.MemberReference VisitMemberReference(J.MemberReference memberReference, P p)
    {
        return (J.MemberReference )(base.VisitMemberReference(memberReference, p));
    }

    public override J.MethodDeclaration VisitMethodDeclaration(J.MethodDeclaration methodDeclaration, P p)
    {
        return (J.MethodDeclaration )(base.VisitMethodDeclaration(methodDeclaration, p));
    }

    public override J.MethodInvocation VisitMethodInvocation(J.MethodInvocation methodInvocation, P p)
    {
        return (J.MethodInvocation )(base.VisitMethodInvocation(methodInvocation, p));
    }

    public override J.Modifier VisitModifier(J.Modifier modifier, P p)
    {
        return (J.Modifier )(base.VisitModifier(modifier, p));
    }

    public override J.MultiCatch VisitMultiCatch(J.MultiCatch multiCatch, P p)
    {
        return (J.MultiCatch )(base.VisitMultiCatch(multiCatch, p));
    }

    public override J.NewArray VisitNewArray(J.NewArray newArray, P p)
    {
        return (J.NewArray )(base.VisitNewArray(newArray, p));
    }

    public override J.ArrayDimension VisitArrayDimension(J.ArrayDimension arrayDimension, P p)
    {
        return (J.ArrayDimension )(base.VisitArrayDimension(arrayDimension, p));
    }

    public override J.NewClass VisitNewClass(J.NewClass newClass, P p)
    {
        return (J.NewClass )(base.VisitNewClass(newClass, p));
    }

    public override J.NullableType VisitNullableType(J.NullableType nullableType, P p)
    {
        return (J.NullableType )(base.VisitNullableType(nullableType, p));
    }

    public override J.Package VisitPackage(J.Package package, P p)
    {
        return (J.Package )(base.VisitPackage(package, p));
    }

    public override J.ParameterizedType VisitParameterizedType(J.ParameterizedType parameterizedType, P p)
    {
        return (J.ParameterizedType )(base.VisitParameterizedType(parameterizedType, p));
    }

    public override J.Parentheses<J2> VisitParentheses<J2>(J.Parentheses<J2> parentheses, P p)
    {
        return (J.Parentheses<J2> )(base.VisitParentheses(parentheses, p));
    }

    public override J.ControlParentheses<J2> VisitControlParentheses<J2>(J.ControlParentheses<J2> controlParentheses, P p)
    {
        return (J.ControlParentheses<J2> )(base.VisitControlParentheses(controlParentheses, p));
    }

    public override J.Primitive VisitPrimitive(J.Primitive primitive, P p)
    {
        return (J.Primitive )(base.VisitPrimitive(primitive, p));
    }

    public override J.Return VisitReturn(J.Return @return, P p)
    {
        return (J.Return )(base.VisitReturn(@return, p));
    }

    public override J.Switch VisitSwitch(J.Switch @switch, P p)
    {
        return (J.Switch )(base.VisitSwitch(@switch, p));
    }

    public override J.SwitchExpression VisitSwitchExpression(J.SwitchExpression switchExpression, P p)
    {
        return (J.SwitchExpression )(base.VisitSwitchExpression(switchExpression, p));
    }

    public override J.Synchronized VisitSynchronized(J.Synchronized synchronized, P p)
    {
        return (J.Synchronized )(base.VisitSynchronized(synchronized, p));
    }

    public override J.Ternary VisitTernary(J.Ternary ternary, P p)
    {
        return (J.Ternary )(base.VisitTernary(ternary, p));
    }

    public override J.Throw VisitThrow(J.Throw @throw, P p)
    {
        return (J.Throw )(base.VisitThrow(@throw, p));
    }

    public override J.Try VisitTry(J.Try @try, P p)
    {
        return (J.Try )(base.VisitTry(@try, p));
    }

    public override J.Try.Resource VisitTryResource(J.Try.Resource resource, P p)
    {
        return (J.Try.Resource )(base.VisitTryResource(resource, p));
    }

    public override J.Try.Catch VisitCatch(J.Try.Catch @catch, P p)
    {
        return (J.Try.Catch )(base.VisitCatch(@catch, p));
    }

    public override J.TypeCast VisitTypeCast(J.TypeCast typeCast, P p)
    {
        return (J.TypeCast )(base.VisitTypeCast(typeCast, p));
    }

    public override J.TypeParameter VisitTypeParameter(J.TypeParameter typeParameter, P p)
    {
        return (J.TypeParameter )(base.VisitTypeParameter(typeParameter, p));
    }

    public override J.TypeParameters VisitTypeParameters(J.TypeParameters typeParameters, P p)
    {
        return (J.TypeParameters )(base.VisitTypeParameters(typeParameters, p));
    }

    public override J.Unary VisitUnary(J.Unary unary, P p)
    {
        return (J.Unary )(base.VisitUnary(unary, p));
    }

    public override J.VariableDeclarations VisitVariableDeclarations(J.VariableDeclarations variableDeclarations, P p)
    {
        return (J.VariableDeclarations )(base.VisitVariableDeclarations(variableDeclarations, p));
    }

    public override J.VariableDeclarations.NamedVariable VisitVariable(J.VariableDeclarations.NamedVariable namedVariable, P p)
    {
        return (J.VariableDeclarations.NamedVariable )(base.VisitVariable(namedVariable, p));
    }

    public override J.WhileLoop VisitWhileLoop(J.WhileLoop whileLoop, P p)
    {
        return (J.WhileLoop )(base.VisitWhileLoop(whileLoop, p));
    }

    public override J.Wildcard VisitWildcard(J.Wildcard wildcard, P p)
    {
        return (J.Wildcard )(base.VisitWildcard(wildcard, p));
    }

    public override J.Yield VisitYield(J.Yield yield, P p)
    {
        return (J.Yield )(base.VisitYield(yield, p));
    }

    public override J.Unknown VisitUnknown(J.Unknown unknown, P p)
    {
        return (J.Unknown )(base.VisitUnknown(unknown, p));
    }

    public override J.Unknown.Source VisitUnknownSource(J.Unknown.Source source, P p)
    {
        return (J.Unknown.Source )(base.VisitUnknownSource(source, p));
    }
}
