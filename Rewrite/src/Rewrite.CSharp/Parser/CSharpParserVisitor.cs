using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;
using Expression = Rewrite.RewriteJava.Tree.Expression;

namespace Rewrite.RewriteCSharp.Parser;

[SuppressMessage("ReSharper", "RedundantOverriddenMember")]
public class CSharpParserVisitor(CSharpParser parser, SemanticModel semanticModel) : CSharpSyntaxVisitor<J>
{
    private readonly CSharpTypeMapping _typeMapping = new();
    private readonly List<TextSpan> _seenTriviaSpans = [];

    public override Cs VisitCompilationUnit(CompilationUnitSyntax node)
    {
        // special case when the compilation unit is empty
        var empty = node.GetFirstToken().IsKind(SyntaxKind.None);
        var prefix = Format(empty ? node.GetLeadingTrivia() : Leading(node));
        var cu = new Cs.CompilationUnit(
            Core.Tree.RandomId(),
            prefix,
            Markers.EMPTY,
            semanticModel.SyntaxTree.FilePath,
            null,
            null,
            false,
            null,
            node.Externs.Select(u => new JRightPadded<Cs.ExternAlias>(
                Convert<Cs.ExternAlias>(u)!,
                Format(Leading(u.SemicolonToken)),
                Markers.EMPTY
            )).ToList(),
            node.Usings.Select(u => new JRightPadded<Cs.UsingDirective>(
                Convert<Cs.UsingDirective>(u)!,
                Format(Leading(u.SemicolonToken)),
                Markers.EMPTY
            )).ToList(),
            node.AttributeLists.Select(Convert<Cs.AttributeList>).ToList()!,
            node.Members.Select(MapMemberDeclaration).ToList(),
            Format(empty ? SyntaxTriviaList.Empty : Leading(node.EndOfFileToken))
        );
        return cu;
    }

    public override J.Unknown DefaultVisit(SyntaxNode node)
    {
        return new J.Unknown(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.Unknown.Source(
                Core.Tree.RandomId(),
                Space.EMPTY,
                new Markers(Core.Tree.RandomId(),
                    [
                        ParseExceptionResult.Build(parser, new InvalidOperationException("Unsupported AST type."))
                            .WithTreeType(node.Kind().ToString())
                    ]
                ),
                node.ToString()
            )
        );
    }

    public override Cs VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        return new Cs.FileScopeNamespaceDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<Expression>(
                Convert<Expression>(node.Name)!,
                Format(Trailing(node.Name)),
                Markers.EMPTY
            ),
            node.Externs.Select(u => new JRightPadded<Cs.ExternAlias>(
                Convert<Cs.ExternAlias>(u)!,
                Format(Leading(u.SemicolonToken)),
                Markers.EMPTY
            )).ToList(),
            node.Usings.Select(u => new JRightPadded<Cs.UsingDirective>(
                Convert<Cs.UsingDirective>(u)!,
                Format(Leading(u.SemicolonToken)),
                Markers.EMPTY
            )).ToList(),
            node.Members.Select(MapMemberDeclaration).ToList()
        );
    }

    public override J? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        return new Cs.BlockScopeNamespaceDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<Expression>(
                Convert<Expression>(node.Name)!,
                Format(Trailing(node.Name)),
                Markers.EMPTY
            ),
            node.Externs.Select(u => new JRightPadded<Cs.ExternAlias>(
                Convert<Cs.ExternAlias>(u)!,
                Format(Leading(u.SemicolonToken)),
                Markers.EMPTY
            )).ToList(),
            node.Usings.Select(u => new JRightPadded<Cs.UsingDirective>(
                Convert<Cs.UsingDirective>(u)!,
                Format(Leading(u.SemicolonToken)),
                Markers.EMPTY
            )).ToList(),
            node.Members.Select(MapMemberDeclaration).ToList(),
            Format(Leading(node.CloseBraceToken))
        );
    }

    public override J? VisitStructDeclaration(StructDeclarationSyntax node)
    {
        // This was added in C# 1.0
        var attributeLists = MapAttributes(node.AttributeLists);
        var classDeclaration = new J.ClassDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            MapModifiers(node.Modifiers),
            new J.ClassDeclaration.Kind(
                Core.Tree.RandomId(),
                Format(Leading(node.Keyword)),
                Markers.EMPTY,
                [],
                J.ClassDeclaration.Kind.Type.Value
            ),
            MapIdentifier(node.Identifier, null),
            MapTypeParameters(node.TypeParameterList),
            null,
            null,
            null,
            null,
            new J.Block(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenBraceToken)),
                Markers.EMPTY,
                JRightPadded<bool>.Build(false),
                node.Members.Select(MapMemberDeclaration).ToList(),
                Format(Leading(node.CloseBraceToken))
            ),
            MapType(node) as JavaType.FullyQualified
        );

        return attributeLists != null
            ? new Cs.AnnotatedStatement(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                attributeLists,
                classDeclaration
            )
            : classDeclaration;
    }

    public override J? VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        return base.VisitEnumDeclaration(node);
    }

    public override J? VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, J.ClassDeclaration.Kind.Type.Record);
    }

    public override J? VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, J.ClassDeclaration.Kind.Type.Interface);
    }

    public override J? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, J.ClassDeclaration.Kind.Type.Class);
    }

    private Statement VisitTypeDeclaration(TypeDeclarationSyntax node, J.ClassDeclaration.Kind.Type type)
    {
        var attributeLists = MapAttributes(node.AttributeLists);
        var javaType = MapType(node);
        var hasBaseClass = node.BaseList is { Types.Count: > 0 } &&
                           semanticModel.GetTypeInfo(node.BaseList.Types[0].Type).Type?.TypeKind == TypeKind.Class;
        var hasBaseInterfaces = node.BaseList != null && node.BaseList.Types.Count > (hasBaseClass ? 1 : 0);
        var classDeclaration = new J.ClassDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            MapModifiers(node.Modifiers),
            new J.ClassDeclaration.Kind(
                Core.Tree.RandomId(),
                Format(Leading(node.Keyword)),
                Markers.EMPTY,
                [],
                type
            ),
            new J.Identifier(
                Core.Tree.RandomId(),
                Format(Leading(node.Identifier)),
                Markers.EMPTY,
                [],
                node.Identifier.Text,
                javaType,
                null
            ),
            MapTypeParameters(node.TypeParameterList),
            MapParameters<Statement>(node.ParameterList),
            hasBaseClass
                ? new JLeftPadded<TypeTree>(Format(Leading(node.BaseList!)),
                    (Visit(node.BaseList!.Types[0]) as TypeTree)!, Markers.EMPTY)
                : null,
            hasBaseInterfaces
                ? new JContainer<TypeTree>(hasBaseClass ? Space.EMPTY : Format(Leading(node.BaseList!)),
                    node.BaseList!.Types.Skip(hasBaseClass ? 1 : 0)
                        .Select(bts =>
                            new JRightPadded<TypeTree>((Visit(bts) as TypeTree)!, Format(Trailing(bts)), Markers.EMPTY))
                        .ToList(), Markers.EMPTY)
                : null,
            null,
            new J.Block(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenBraceToken)),
                node.OpenBraceToken.IsKind(SyntaxKind.None)
                    ? Markers.EMPTY.Add(new OmitBraces(Core.Tree.RandomId()))
                    : Markers.EMPTY,
                JRightPadded<bool>.Build(false),
                node.Members.Select(MapMemberDeclaration).ToList(),
                Format(Leading(node.CloseBraceToken))
            ),
            javaType as JavaType.FullyQualified
        );

        return attributeLists != null
            ? new Cs.AnnotatedStatement(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                attributeLists,
                classDeclaration
            )
            : classDeclaration;
    }

    public override Statement VisitParameter(ParameterSyntax p)
    {
        var attributeLists = MapAttributes(p.AttributeLists);
        var javaType = MapType(p) as JavaType.Variable;
        var variableDeclarations = new J.VariableDeclarations(
            Core.Tree.RandomId(),
            Format(Leading(p)),
            Markers.EMPTY,
            [],
            MapModifiers(p.Modifiers),
            /*p.Type != null ? new J.Identifier(
                Core.Tree.RandomId(),
                Format(Leading(p.Type)),
                Markers.EMPTY,
                [],
                p.Type!.ToString(),
                MapType(p),
                null
            ) : null*/
            Convert<TypeTree>(p.Type),
            null!,
            [],
            [
                new JRightPadded<J.VariableDeclarations.NamedVariable>(
                    new J.VariableDeclarations.NamedVariable(
                        Core.Tree.RandomId(),
                        Format(Leading(p.Identifier)),
                        Markers.EMPTY,
                        new J.Identifier(
                            Core.Tree.RandomId(),
                            Space.EMPTY,
                            Markers.EMPTY,
                            [],
                            p.Identifier.Text,
                            javaType?.Type,
                            javaType
                        ),
                        [],
                        p.Default != null
                            ? new JLeftPadded<Expression>(
                                Format(Leading(p.Default)),
                                Convert<Expression>(p.Default.Value)!,
                                Markers.EMPTY
                            )
                            : null,
                        javaType
                    ),
                    Format(Trailing(p.Identifier)),
                    Markers.EMPTY
                )
            ]
        );
        return attributeLists != null
            ? new Cs.AnnotatedStatement(
                Core.Tree.RandomId(),
                Format(Leading(p)),
                Markers.EMPTY,
                attributeLists,
                variableDeclarations
            )
            : variableDeclarations;
    }

    public override J? VisitSimpleBaseType(SimpleBaseTypeSyntax node)
    {
        return new J.Identifier(
            Core.Tree.RandomId(),
            Format(Leading(node.Type)),
            Markers.EMPTY,
            [],
            node.ToString(),
            MapType(node),
            null
        );
    }

    public override J? VisitPrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax node)
    {
        return new J.Identifier(
            Core.Tree.RandomId(),
            Format(Leading(node.Type)),
            Markers.EMPTY,
            [],
            node.ToString(),
            MapType(node),
            null
        );
    }

    public override J.TypeParameter VisitTypeParameter(TypeParameterSyntax p)
    {
        return new J.TypeParameter(
            Core.Tree.RandomId(),
            Format(Leading(p)),
            Markers.EMPTY,
            [],
            [],
            new J.Identifier(
                Core.Tree.RandomId(),
                Format(Leading(p.Identifier)),
                Markers.EMPTY,
                [],
                p.Identifier.Text,
                MapType(p),
                null
            ),
            null
        );
    }


    public override J.TypeParameters? VisitTypeParameterList(TypeParameterListSyntax node)
    {
        if (node.Parameters.Count == 0)
        {
            return null;
        }

        return new J.TypeParameters(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            node.Parameters.Select(MapTypeParameter).ToList()
        );
    }

    public override J VisitIdentifierName(IdentifierNameSyntax node)
    {
        return new J.Identifier(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            node.Identifier.Text,
            null, // FIXME type attribution
            null // FIXME type attribution
        );
    }

    public override J? VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var prefix = Format(Leading(node));
        var select = Convert<J>(node.Expression);
        if (select is J.FieldAccess fa)
        {
            var operatorToken = node.Expression switch
            {
                MemberAccessExpressionSyntax mae => mae.OperatorToken,
                MemberBindingExpressionSyntax mbe => mbe.OperatorToken,
                _ => throw new InvalidOperationException($"Unexpected node of type {node.Expression.GetType()} encountered.")
            };

            return new J.MethodInvocation(
                Core.Tree.RandomId(),
                prefix,
                fa.Markers,
                new JRightPadded<Expression>(fa.Target, Format(operatorToken.LeadingTrivia), Markers.EMPTY),
                null,
                fa.Name,
                MapArgumentList(node.ArgumentList),
                MapType(node) as JavaType.Method
            );
        }
        else if (select is J.Identifier id)
        {
            return new J.MethodInvocation(
                Core.Tree.RandomId(),
                prefix,
                Markers.EMPTY,
                null,
                null,
                id,
                MapArgumentList(node.ArgumentList),
                MapType(node) as JavaType.Method
            );
        }
        else if (select is J.ParameterizedType pt)
        {
            // return mi
            //     .WithPrefix(prefix)
            //     .Padding.WithArguments(MapArgumentList(node.ArgumentList))
            //     .WithMethodType(MapType(node) as JavaType.Method);

            return new J.MethodInvocation(
                Core.Tree.RandomId(),
                prefix,
                Markers.EMPTY,
                pt.Clazz is J.FieldAccess lfa
                    ? new JRightPadded<Expression>(lfa.Target, Format(Leading(node.ArgumentList)), Markers.EMPTY)
                    : null,
                pt.TypeParameters != null
                    ? new JContainer<Expression>(
                        Space.EMPTY,
                        pt.TypeParameters.Select(JRightPadded<Expression>.Build).ToList(),
                        Markers.EMPTY
                    )
                    : null, // TODO: type parameters
                pt.Clazz is J.Identifier i
                    ? i
                    : (pt.Clazz as J.FieldAccess)?.Name ??
                      MapIdentifier(node.Expression.GetFirstToken(), MapType(node.Expression)),
                MapArgumentList(node.ArgumentList),
                MapType(node) as JavaType.Method
            );
        }
        else if (select is J.MethodInvocation mi) // chained method invocation (method returns a delegate). ex. Something()()
        {

            return new J.MethodInvocation(
                Core.Tree.RandomId(),
                prefix,
                Markers.EMPTY,
                JRightPadded<Expression>.Build(mi),
                null,
                new J.Identifier(
                    Core.Tree.RandomId(),
                    Space.EMPTY,
                    Markers.EMPTY,
                    new List<J.Annotation>(),
                    "",
                    null,
                    null),
                MapArgumentList(node.ArgumentList),
                MapType(node) as JavaType.Method
            );
        }

        for (var index = 0; index < node.ArgumentList.Arguments.Count; index++)
        {
            var arg = node.ArgumentList.Arguments[index];
            var typeInfo = semanticModel.GetTypeInfo(arg.Expression);
        }

        return base.VisitInvocationExpression(node);
    }

    private JContainer<Expression> MapArgumentList(ArgumentListSyntax argumentList)
    {
        return new JContainer<Expression>(
            Format(Trailing(argumentList.OpenParenToken)),
            argumentList.Arguments.Select(MapArgument).ToList(),
            Markers.EMPTY
        );
    }

    private JRightPadded<Expression> MapArgument(ArgumentSyntax argument)
    {
        return new JRightPadded<Expression>(
            Convert<Expression>(argument)!,
            Format(Trailing(argument)),
            Markers.EMPTY
        );
    }


    public override J? VisitAttribute(AttributeSyntax node)
    {
        return new J.Annotation(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            Convert<NameTree>(node.Name)!,
            node.ArgumentList != null
                ? new JContainer<Expression>(
                    Format(Leading(node.ArgumentList.OpenParenToken)),
                    node.ArgumentList.Arguments.Count > 0
                        ? node.ArgumentList.Arguments.Select(a =>
                            new JRightPadded<Expression>(
                                Convert<Expression>(a.Expression)!,
                                Format(Trailing(a)),
                                Markers.EMPTY
                            )).ToList()
                        :
                        [
                            new JRightPadded<Expression>(
                                new J.Empty(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY),
                                Format(Leading(node.ArgumentList.CloseParenToken)),
                                Markers.EMPTY
                            )
                        ],
                    Markers.EMPTY
                )
                : null
        );
    }

    public override J? VisitAttributeList(AttributeListSyntax node)
    {
        return new Cs.AttributeList(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.Target != null
                ? new JRightPadded<J.Identifier>(
                    Convert<J.Identifier>(node.Target)!,
                    Format(Leading(node.Target.ColonToken)),
                    Markers.EMPTY
                )
                : null,
            node.Attributes.Select(a =>
            {
                var trailingComma = a == node.Attributes.Last() &&
                                    a.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
                return new JRightPadded<J.Annotation>(
                    Convert<J.Annotation>(a)!,
                    Format(Trailing(a)),
                    trailingComma
                        ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                            Format(Trailing(a.GetLastToken().GetNextToken()))))
                        : Markers.EMPTY
                );
            }).ToList()
        );
    }

    public override J? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var attributeLists = MapAttributes(node.AttributeLists);
        var methodDeclaration = new J.MethodDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            MapModifiers(node.Modifiers),
            Visit(node.TypeParameterList) as J.TypeParameters,
            Convert<TypeTree>(node.ReturnType),
            new J.MethodDeclaration.IdentifierWithAnnotations(
                new J.Identifier(
                    Core.Tree.RandomId(),
                    Format(Leading(node.Identifier)),
                    Markers.EMPTY,
                    (Enumerable.Empty<J.Annotation>() as IList<J.Annotation>)!,
                    node.Identifier.Text,
                    null,
                    null
                ),
                []
            ),
            MapParameters<Statement>(node.ParameterList)!,
            null,
            node.Body != null ? Convert<J.Block>(node.Body) :
            node.ExpressionBody != null ? Convert<J.Block>(node.ExpressionBody) : null,
            null,
            MapType(node) as JavaType.Method
        );
        return attributeLists != null
            ? new Cs.AnnotatedStatement(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                attributeLists,
                methodDeclaration
            )
            : methodDeclaration;
    }

    public override J? VisitUsingDirective(UsingDirectiveSyntax node)
    {
        var global = node.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword);
        var @static = node.StaticKeyword.IsKind(SyntaxKind.StaticKeyword);
        var @unsafe = node.UnsafeKeyword.IsKind(SyntaxKind.UnsafeKeyword);

        return new Cs.UsingDirective(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<bool>(global, global ? Format(Trailing(node.GlobalKeyword)) : Space.EMPTY, Markers.EMPTY),
            new JLeftPadded<bool>(@static ? Format(Leading(node.StaticKeyword)) : Space.EMPTY, @static, Markers.EMPTY),
            new JLeftPadded<bool>(@unsafe ? Format(Leading(node.UnsafeKeyword)) : Space.EMPTY, @unsafe, Markers.EMPTY),
            node.Alias != null
                ? new JRightPadded<J.Identifier>(
                    Convert<J.Identifier>(node.Alias.Name)!,
                    Format(Leading(node.Alias.EqualsToken)),
                    Markers.EMPTY
                )
                : null,
            Convert<TypeTree>(node.NamespaceOrType)!
        );
    }

    public override J? VisitNullableType(NullableTypeSyntax node)
    {
        return new J.NullableType(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            new JRightPadded<TypeTree>(
                Convert<TypeTree>(node.ElementType)!,
                Format(Leading(node.QuestionToken)),
                Markers.EMPTY
            )
        );
    }

    public override J? VisitArgument(ArgumentSyntax node)
    {
        if (node.NameColon == null)
        {
            return Convert<Expression>(node.Expression);
        }
        else
        {
            //
            return new Cs.NamedArgument(
                Core.Tree.RandomId(),
                Format(Leading(node.NameColon)),
                Markers.EMPTY,
                new JRightPadded<J.Identifier>(
                    MapIdentifier(node.NameColon.Name.Identifier, null),
                    Format(Trailing(node.NameColon.Name)), Markers.EMPTY),
                Convert<Expression>(node.Expression)!
                );
        }
    }

    public override J.Block VisitBlock(BlockSyntax node)
    {
        return new J.Block(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            node.OpenBraceToken.IsKind(SyntaxKind.None)
                ? Markers.EMPTY.Add(new OmitBraces(Core.Tree.RandomId()))
                : Markers.EMPTY,
            JRightPadded<bool>.Build(false),
            node.Statements.Select(MapStatement).ToList(),
            Format(Leading(node.CloseBraceToken))
        );
    }

    public override J? VisitInterpolation(InterpolationSyntax node)
    {
        // This was added in C# 6.0
        return new Cs.Interpolation(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<Expression>(
                Convert<Expression>(node.Expression)!,
                Format(Trailing(node.Expression)),
                Markers.EMPTY
            ),
            node.AlignmentClause != null
                ? new JRightPadded<Expression>(
                    Convert<Expression>(node.AlignmentClause)!,
                    Format(Trailing(node.AlignmentClause.Value)),
                    Markers.EMPTY
                )
                : null,
            node.FormatClause != null
                ? new JRightPadded<Expression>(
                    Convert<Expression>(node.FormatClause)!,
                    Format(Trailing(node.FormatClause.FormatStringToken)),
                    Markers.EMPTY
                )
                : null
        );
    }

    public override J? VisitOrdering(OrderingSyntax node)
    {
        return base.VisitOrdering(node);
    }

    public override J? VisitSubpattern(SubpatternSyntax node)
    {
        // This was added in C# 8.0
        return base.VisitSubpattern(node);
    }

    public override J? VisitAccessorDeclaration(AccessorDeclarationSyntax node)
    {
        var javaType = MapType(node);
        return new J.MethodDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY.Add(new CompactConstructor(Core.Tree.RandomId())),
            [],
            MapModifiers(node.Modifiers),
            null,
            null,
            new J.MethodDeclaration.IdentifierWithAnnotations(MapIdentifier(node.Keyword, javaType), []),
            new JContainer<Statement>(
                Space.EMPTY,
                [],
                Markers.EMPTY
            ),
            null,
            node.ExpressionBody != null ? Convert<J.Block>(node.ExpressionBody) :
            node.Body != null ? Convert<J.Block>(node.Body) : null,
            null,
            javaType as JavaType.Method
        );
    }

    public override J.Block VisitAccessorList(AccessorListSyntax node)
    {
        return new J.Block(
            Core.Tree.RandomId(),
            Format(Leading(node.OpenBraceToken)),
            Markers.EMPTY,
            JRightPadded<bool>.Build(false),
            node.Accessors.Select(MapAccessor).ToList(),
            Format(Leading(node.CloseBraceToken))
        );
    }

    private JRightPadded<Statement> MapAccessor(AccessorDeclarationSyntax accessorDeclarationSyntax)
    {
        var methodDeclaration = Convert<J.MethodDeclaration>(accessorDeclarationSyntax)!;
        var trailingSemicolon = accessorDeclarationSyntax.GetLastToken().IsKind(SyntaxKind.SemicolonToken);
        return new JRightPadded<Statement>(
            methodDeclaration,
            trailingSemicolon ? Format(Leading(accessorDeclarationSyntax.SemicolonToken)) : Space.EMPTY,
            Markers.EMPTY
        );
    }

    public override J? VisitArgumentList(ArgumentListSyntax node)
    {
        return base.VisitArgumentList(node);
    }

    public override J? VisitArrayType(ArrayTypeSyntax node)
    {
        return MapArrayType(node, 0);
    }

    private J.ArrayType MapArrayType(ArrayTypeSyntax node, int rank)
    {
        return new J.ArrayType(
            Core.Tree.RandomId(),
            rank == 0 ? Format(Leading(node)) : Space.EMPTY,
            Markers.EMPTY,
            rank == node.RankSpecifiers.Count - 1 ? Convert<TypeTree>(node.ElementType)! : MapArrayType(node, rank + 1),
            [], // no attributes on type use
            new JLeftPadded<Space>(
                Format(Leading(node.RankSpecifiers[rank])),
                Format(Trailing(node.RankSpecifiers[rank].OpenBracketToken)),
                Markers.EMPTY),
            MapType(node)
        );
    }

    public override J? VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.SimpleAssignmentExpression))
        {
            return new J.Assignment(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                Convert<Expression>(node.Left)!,
                new JLeftPadded<Expression>(
                    Format(Leading(node.OperatorToken)),
                    Convert<Expression>(node.Right)!,
                    Markers.EMPTY),
                MapType(node)
            );
        }

        if (node.OperatorToken.IsKind(SyntaxKind.QuestionQuestionEqualsToken))
        {
            return new Cs.AssignmentOperation(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                Convert<Expression>(node.Left)!,
                new JLeftPadded<Cs.AssignmentOperation.OperatorType>(
                    Format(Leading(node.OperatorToken)),
                    Cs.AssignmentOperation.OperatorType.NullCoalescing,
                    Markers.EMPTY),
                Convert<Expression>(node.Right)!,
                MapType(node)
            );
        }

        return new J.AssignmentOperation(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            Convert<Expression>(node.Left)!,
            new JLeftPadded<J.AssignmentOperation.Type>(
                Format(Leading(node.OperatorToken)),
                MapAssignmentOperator(node.OperatorToken),
                Markers.EMPTY),
            Convert<Expression>(node.Right)!,
            MapType(node)
        );
    }

    private J.AssignmentOperation.Type MapAssignmentOperator(SyntaxToken op)
    {
        return op.Kind() switch
        {
            SyntaxKind.PlusEqualsToken => J.AssignmentOperation.Type.Addition,
            SyntaxKind.MinusEqualsToken => J.AssignmentOperation.Type.Subtraction,
            SyntaxKind.AsteriskEqualsToken => J.AssignmentOperation.Type.Multiplication,
            SyntaxKind.SlashEqualsToken => J.AssignmentOperation.Type.Division,
            SyntaxKind.PercentEqualsToken => J.AssignmentOperation.Type.Modulo,
            SyntaxKind.AmpersandEqualsToken => J.AssignmentOperation.Type.BitAnd,
            SyntaxKind.BarEqualsToken => J.AssignmentOperation.Type.BitOr,
            SyntaxKind.CaretEqualsToken => J.AssignmentOperation.Type.BitXor,
            SyntaxKind.LessThanLessThanEqualsToken => J.AssignmentOperation.Type.LeftShift,
            SyntaxKind.GreaterThanGreaterThanEqualsToken => J.AssignmentOperation.Type.RightShift,
            SyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken => J.AssignmentOperation.Type.UnsignedRightShift,
            // TODO the `??=` operator will require a custom LST element
            // SyntaxKind.QuestionQuestionEqualsToken => ???,
            _ => throw new NotSupportedException(op.Kind().ToString())
        };
    }

    public override J? VisitAttributeArgument(AttributeArgumentSyntax node)
    {
        return base.VisitAttributeArgument(node);
    }

    public override J? VisitAwaitExpression(AwaitExpressionSyntax node)
    {
        return new Cs.AwaitExpression(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            Convert<Expression>(node.Expression)!,
            MapType(node)
        );
    }

    public override J? VisitBaseExpression(BaseExpressionSyntax node)
    {
        return base.VisitBaseExpression(node);
    }

    public override J? VisitBaseList(BaseListSyntax node)
    {
        return base.VisitBaseList(node);
    }

    public override J? VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        if (node.OperatorToken.IsKind(SyntaxKind.QuestionQuestionToken) ||
            node.OperatorToken.IsKind(SyntaxKind.AsKeyword))
        {
            return new Cs.Binary(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                Convert<Expression>(node.Left)!,
                new JLeftPadded<Cs.Binary.OperatorType>(
                    Format(Leading(node.OperatorToken)),
                    node.OperatorToken.Kind() switch
                    {
                        SyntaxKind.QuestionQuestionToken => Cs.Binary.OperatorType.NullCoalescing,
                        SyntaxKind.AsKeyword => Cs.Binary.OperatorType.As,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Markers.EMPTY),
                Convert<Expression>(node.Right)!,
                MapType(node)
            );
        }

        if (node.OperatorToken.IsKind(SyntaxKind.IsKeyword))
        {
            return new J.InstanceOf(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                new JRightPadded<Expression>(
                    Convert<Expression>(node.Left)!,
                    Format(Trailing(node.Left)),
                    Markers.EMPTY),
                Convert<TypeTree>(node.Right)!,
                null,
                MapType(node)
            );
        }

        return new J.Binary(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            Convert<Expression>(node.Left)!,
            new JLeftPadded<J.Binary.Type>(
                Format(Leading(node.OperatorToken)),
                MapBinaryExpressionOperator(node.OperatorToken),
                Markers.EMPTY),
            Convert<Expression>(node.Right)!,
            MapType(node)
        );
    }

    private static J.Binary.Type MapBinaryExpressionOperator(SyntaxToken operatorToken)
    {
        return operatorToken.Kind() switch
        {
            SyntaxKind.PlusToken => J.Binary.Type.Addition,
            SyntaxKind.MinusToken => J.Binary.Type.Subtraction,
            SyntaxKind.AsteriskToken => J.Binary.Type.Multiplication,
            SyntaxKind.SlashToken => J.Binary.Type.Division,
            SyntaxKind.PercentToken => J.Binary.Type.Modulo,
            SyntaxKind.AmpersandToken => J.Binary.Type.BitAnd,
            SyntaxKind.BarToken => J.Binary.Type.BitOr,
            SyntaxKind.CaretToken => J.Binary.Type.BitXor,
            SyntaxKind.AmpersandAmpersandToken => J.Binary.Type.And,
            SyntaxKind.BarBarToken => J.Binary.Type.Or,
            SyntaxKind.LessThanLessThanToken => J.Binary.Type.LeftShift,
            SyntaxKind.GreaterThanGreaterThanToken => J.Binary.Type.RightShift,
            SyntaxKind.GreaterThanGreaterThanGreaterThanToken => J.Binary.Type.UnsignedRightShift,
            SyntaxKind.LessThanToken => J.Binary.Type.LessThan,
            SyntaxKind.GreaterThanToken => J.Binary.Type.GreaterThan,
            SyntaxKind.LessThanEqualsToken => J.Binary.Type.LessThanOrEqual,
            SyntaxKind.GreaterThanEqualsToken => J.Binary.Type.GreaterThanOrEqual,
            SyntaxKind.EqualsEqualsToken => J.Binary.Type.Equal,
            SyntaxKind.ExclamationEqualsToken => J.Binary.Type.NotEqual,
            _ => throw new NotImplementedException(operatorToken.Kind().ToString())
        };
    }

    public override J? VisitBinaryPattern(BinaryPatternSyntax node)
    {
        return base.VisitBinaryPattern(node);
    }

    public override J? VisitBreakStatement(BreakStatementSyntax node)
    {
        return new J.Break(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            null
        );
    }

    public override J? VisitCastExpression(CastExpressionSyntax node)
    {
        return new J.TypeCast(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ControlParentheses<TypeTree>(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenParenToken)),
                Markers.EMPTY,
                new JRightPadded<TypeTree>(
                    Convert<TypeTree>(node.Type)!,
                    Format(Leading(node.CloseParenToken)),
                    Markers.EMPTY
                )
            ),
            Convert<Expression>(node.Expression)!
        );
    }

    public override J? VisitCatchClause(CatchClauseSyntax node)
    {
        return new J.Try.Catch(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ControlParentheses<J.VariableDeclarations>(
                Core.Tree.RandomId(),
                node.Declaration != null ? Format(Leading(node.Declaration)) : Space.EMPTY,
                Markers.EMPTY,
                new JRightPadded<J.VariableDeclarations>(
                    node.Declaration != null
                        ? Convert<J.VariableDeclarations>(node.Declaration)!
                        : new J.VariableDeclarations(
                            Core.Tree.RandomId(),
                            Space.EMPTY,
                            Markers.EMPTY,
                            [],
                            [],
                            null,
                            null!,
                            [],
                            []
                        ),
                    node.Declaration != null ? Format(Leading(node.Declaration.CloseParenToken)) : Space.EMPTY,
                    Markers.EMPTY
                )
            ),
            Convert<J.Block>(node.Block)!
        );
    }

    public override J? VisitCatchDeclaration(CatchDeclarationSyntax node)
    {
        return new J.VariableDeclarations(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            [],
            Convert<TypeTree>(node.Type),
            null!,
            [],
            node.Identifier.IsKind(SyntaxKind.None)
                ? []
                :
                [
                    new JRightPadded<J.VariableDeclarations.NamedVariable>(
                        new J.VariableDeclarations.NamedVariable(
                            Core.Tree.RandomId(),
                            Format(Leading(node.Identifier)),
                            Markers.EMPTY,
                            MapIdentifier(node.Identifier, MapType(node.Type))!,
                            [],
                            null,
                            MapType(node) as JavaType.Variable
                        ),
                        Format(Leading(node.CloseParenToken)),
                        Markers.EMPTY
                    )
                ]
        );
    }

    public override J? VisitCheckedExpression(CheckedExpressionSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitCheckedExpression(node);
    }

    public override J? VisitQualifiedName(QualifiedNameSyntax node)
    {
        if (node.Right is GenericNameSyntax genericNameSyntax)
        {
            var mapIdentifier = MapIdentifier(genericNameSyntax.Identifier, MapType(genericNameSyntax));
            return new J.ParameterizedType(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                new J.FieldAccess(
                    Core.Tree.RandomId(),
                    Format(Leading(node)),
                    Markers.EMPTY,
                    (Visit(node.Left) as Expression)!,
                    new JLeftPadded<J.Identifier>(
                        Format(Leading(node.DotToken)),
                        mapIdentifier,
                        Markers.EMPTY
                    ),
                    MapType(node.Left)
                ),
                MapTypeArguments(genericNameSyntax.TypeArgumentList),
                mapIdentifier.Type
            );
        }

        return new J.FieldAccess(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            (Visit(node.Left) as Expression)!,
            new JLeftPadded<J.Identifier>(
                Format(Leading(node.DotToken)),
                Convert<J.Identifier>(node.Right)!,
                Markers.EMPTY
            ),
            MapType(node)
        );
    }

    public override J? VisitGenericName(GenericNameSyntax node)
    {
        // // return `J.MethodInvocation` as that is the only way to capture generic type arguments
        // var type = MapType(node) as JavaType.Method;
        // return new J.MethodInvocation(
        //     Core.Tree.RandomId(),
        //     Format(Leading(node)),
        //     Markers.EMPTY,
        //     null,
        //     MapTypeArguments(node.TypeArgumentList),
        //     MapIdentifier(node.Identifier, type?.ReturnType),
        //     JContainer<Expression>.Empty(),
        //     type
        // );
        var nameTree = MapIdentifier(node.Identifier, MapType(node));
        return new J.ParameterizedType(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            nameTree,
            MapTypeArguments(node.TypeArgumentList),
            nameTree.Type
        );
    }

    private JContainer<Expression>? MapTypeArguments(TypeArgumentListSyntax typeArgumentList)
    {
        if (typeArgumentList.Arguments.Count == 0) return null;

        return new JContainer<Expression>(
            Format(Leading(typeArgumentList)),
            typeArgumentList.Arguments.Select(t => new JRightPadded<Expression>(
                Convert<Expression>(t)!,
                Format(Trailing(t)),
                Markers.EMPTY)
            ).ToList(),
            Markers.EMPTY
        );
    }

    private J.Identifier MapIdentifier(SyntaxToken identifier, JavaType? type)
    {
        var variable = type as JavaType.Variable;
        return new J.Identifier(
            Core.Tree.RandomId(),
            Format(Leading(identifier)),
            Markers.EMPTY,
            [],
            identifier.Text,
            variable?.Type ?? type,
            variable
        );
    }

    public override J? VisitTypeArgumentList(TypeArgumentListSyntax node)
    {
        return base.VisitTypeArgumentList(node);
    }

    public override J? VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
    {
        return base.VisitAliasQualifiedName(node);
    }

    public override J? VisitPredefinedType(PredefinedTypeSyntax node)
    {
        var type = MapType(node);
        if (type is JavaType.Primitive)
            return new J.Primitive(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                (JavaType.Primitive)type
            );
        // TODO also for types like `sbyte` we need to use J.Identifier or a custom `Cs.Primitive`
        return new J.Identifier(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            node.ToString(),
            type is JavaType.Variable variable ? variable.Type : type,
            MapType(node) as JavaType.Variable
        );
    }

    public override J? VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
    {
        return new J.ArrayDimension(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<Expression>(
                new Cs.ArrayRankSpecifier(
                    Core.Tree.RandomId(),
                    Format(Trailing(node.OpenBracketToken)),
                    Markers.EMPTY,
                    new JContainer<Expression>(
                        Format(Leading(node.OpenBracketToken)),
                        node.Sizes.Select(t => new JRightPadded<Expression>(
                            Convert<Expression>(t)!,
                            Format(Trailing(t.GetLastToken())), Markers.EMPTY)).ToList(),
                        Markers.EMPTY
                    )
                ),
                Space.EMPTY,
                Markers.EMPTY
            )
        );
    }

    public override J? VisitPointerType(PointerTypeSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitPointerType(node);
    }

    public override J? VisitFunctionPointerType(FunctionPointerTypeSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitFunctionPointerType(node);
    }

    public override J? VisitFunctionPointerParameterList(FunctionPointerParameterListSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitFunctionPointerParameterList(node);
    }

    public override J? VisitFunctionPointerCallingConvention(FunctionPointerCallingConventionSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitFunctionPointerCallingConvention(node);
    }

    public override J? VisitFunctionPointerUnmanagedCallingConventionList(
        FunctionPointerUnmanagedCallingConventionListSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitFunctionPointerUnmanagedCallingConventionList(node);
    }

    public override J? VisitFunctionPointerUnmanagedCallingConvention(
        FunctionPointerUnmanagedCallingConventionSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitFunctionPointerUnmanagedCallingConvention(node);
    }

    public override J? VisitTupleType(TupleTypeSyntax node)
    {
        // This was added in C# 7.0
        return base.VisitTupleType(node);
    }

    public override J? VisitTupleElement(TupleElementSyntax node)
    {
        // This was added in C# 7.0
        return base.VisitTupleElement(node);
    }

    public override J? VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
    {
        return base.VisitOmittedTypeArgument(node);
    }

    public override J? VisitRefType(RefTypeSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitRefType(node);
    }

    public override J? VisitScopedType(ScopedTypeSyntax node)
    {
        return base.VisitScopedType(node);
    }

    public override J? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
    {
        return new J.Parentheses<Expression>(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            MapExpression(node.Expression)
        );
    }

    public override J? VisitTupleExpression(TupleExpressionSyntax node)
    {
        // This was added in C# 7.0
        return base.VisitTupleExpression(node);
    }

    public override J? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
    {
        if (node.OperatorToken.IsKind(SyntaxKind.CaretToken))
            // TODO implement C# 8.0 "index from the end" operator `^`
            return base.VisitPrefixUnaryExpression(node);

        return new J.Unary(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            MapPrefixUnaryOperator(node.OperatorToken),
            Convert<Expression>(node.Operand)!,
            MapType(node)
        );
    }

    private JLeftPadded<J.Unary.Type> MapPrefixUnaryOperator(SyntaxToken operatorToken)
    {
        return operatorToken.Kind() switch
        {
            SyntaxKind.ExclamationToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.Not,
                Markers.EMPTY
            ),
            SyntaxKind.PlusPlusToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.PreIncrement,
                Markers.EMPTY
            ),
            SyntaxKind.MinusMinusToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.PreDecrement,
                Markers.EMPTY
            ),
            SyntaxKind.MinusToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.Negative,
                Markers.EMPTY
            ),
            SyntaxKind.PlusToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.Positive,
                Markers.EMPTY
            ),
            SyntaxKind.TildeToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.Complement,
                Markers.EMPTY
            ),
            _ => throw new NotImplementedException(operatorToken.ToString())
        };
    }

    public override J? VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
    {
        // TODO remove `if` once we support all
        if (!node.OperatorToken.IsKind(SyntaxKind.PlusPlusToken) &&
            !node.OperatorToken.IsKind(SyntaxKind.MinusMinusToken))
            return DefaultVisit(node);

        return new J.Unary(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            MapPostfixUnaryOperator(node.OperatorToken),
            Convert<Expression>(node.Operand)!,
            MapType(node)
        );
    }

    private JLeftPadded<J.Unary.Type> MapPostfixUnaryOperator(SyntaxToken operatorToken)
    {
        return operatorToken.Kind() switch
        {
            SyntaxKind.PlusPlusToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.PostIncrement,
                Markers.EMPTY
            ),
            SyntaxKind.MinusMinusToken => new JLeftPadded<J.Unary.Type>(
                Format(Leading(operatorToken)),
                J.Unary.Type.PostDecrement,
                Markers.EMPTY
            ),
            // TODO implement all
            _ => throw new NotImplementedException(operatorToken.ToString())
        };
    }

    public override J? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        var name = Convert<Expression>(node.Name)!;
        J result;
        result = name switch
        {
            J.Identifier id => new J.FieldAccess(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                Convert<Expression>(node.Expression)!,
                new JLeftPadded<J.Identifier>(
                    Format(Leading(node.OperatorToken)),
                    id,
                    Markers.EMPTY
                ),
                MapType(node)),
            J.ParameterizedType pi => pi.WithClazz(new J.FieldAccess(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                Convert<Expression>(node.Expression)!,
                new JLeftPadded<J.Identifier>(
                    Format(Leading(node.OperatorToken)),
                    (J.Identifier)pi.Clazz,
                    Markers.EMPTY
                ),
                MapType(node)
            )),
            _ => throw new NotImplementedException()
        };

        // if (name is J.Identifier id)
        // {
        //     return new J.FieldAccess(
        //         Core.Tree.RandomId(),
        //         Format(Leading(node)),
        //         Markers.EMPTY,
        //         Convert<Expression>(node.Expression)!,
        //         new JLeftPadded<J.Identifier>(
        //             Format(Leading(node.OperatorToken)),
        //             id,
        //             Markers.EMPTY
        //         ),
        //         MapType(node)
        //     );
        // }
        //
        // if (name is J.ParameterizedType pi)
        // {
        //     var identifier = (pi.Clazz as J.Identifier)!;
        //     // return pi.WithPrefix(Format(Leading(node))).WithSelect(Convert<Expression>(node.Expression)!);
        //     var parameterizedType = pi.WithClazz(new J.FieldAccess(
        //         Core.Tree.RandomId(),
        //         Format(Leading(node)),
        //         Markers.EMPTY,
        //         Convert<Expression>(node.Expression)!,
        //         new JLeftPadded<J.Identifier>(
        //             Format(Leading(node.OperatorToken)),
        //             identifier,
        //             Markers.EMPTY
        //         ),
        //         MapType(node)
        //     ));
        //     return parameterizedType;
        // }

        return result;
    }

    public override J? VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
    {
        // conditional expressions appear in their "natural order"
        // meaning for an expression like this "a?.b", node a will be at the top of hierarchy
        // vs in normal expression such as "a.b", b will be at the top of hierarchy

        // LST doesn't reverse this order, so we need to traverse down any chain of nullable expression tree, and then
        // process them in reverse
        var conditionalExpressions = new List<(ExpressionSyntax, Space)>();
        ExpressionSyntax currentNode = node;
        while(currentNode is ConditionalAccessExpressionSyntax conditionalNode)
        {
            conditionalExpressions.Add((conditionalNode.Expression, Format(Leading(conditionalNode.OperatorToken))));
            currentNode = conditionalNode.WhenNotNull;
        }
        conditionalExpressions.Add((currentNode, Format(Leading(currentNode))));
        // at this point conditionalExpressions for something like this: a?.b?.c
        // would look like this ['a','.b','.c']

        Expression currentExpression = null!;// = Convert<Expression>(conditionalExpressions[0].Item1)!;
        // each item in list will be individual expressions that form null access path, last one being the "normal"
        // expression that is at
        var i = 0;
        foreach (var (expressionPortion, afterSpace) in conditionalExpressions)
        {
            var isLastSegment = i == conditionalExpressions.Count - 1;
            var lstNode = Convert<Expression>(expressionPortion)!;
            // somewhere in this node, a MemberBindingExpression got converted to either FieldAccess or MethodInvocation
            // the expression is "fake" and needs to be adjusted. luckly we got a marker to locate this special node that needs to be
            // fixed up. The expression for it will become lhs from previous loop iteration (stored in currentExpression)
            // ps: god help you if you need to fix this logic :)
            var bindingNode = lstNode.Descendents()
                .FirstOrDefault(x => x.Markers.Contains<MemberBinding>());
            if (bindingNode != null)
            {
                if (bindingNode is J.MethodInvocation methodNode)
                {
                    var newMethod = methodNode.WithSelect(currentExpression);
                    lstNode = methodNode.Equals(lstNode) ? newMethod : lstNode.ReplaceNode(methodNode, newMethod);
                }
                else if (bindingNode is J.FieldAccess fieldAccess)
                {
                    var newFieldAccess = fieldAccess.WithTarget(currentExpression);
                    lstNode = fieldAccess.Equals(lstNode) ? newFieldAccess : lstNode.ReplaceNode(fieldAccess, newFieldAccess);
                }
            }

            // right hand side is the root and doesn't get wrapped
            if (!isLastSegment)
            {
                lstNode = new Cs.NullSafeExpression(
                    Core.Tree.RandomId(),
                    Format(Leading(expressionPortion)),
                    Markers.EMPTY,
                    new JRightPadded<Expression>(
                        lstNode!,
                        afterSpace,
                        Markers.EMPTY
                    )
                );
            }

            currentExpression = lstNode;

            i++;
        }

        // var result = Convert<Expression>(node.WhenNotNull)!;
        return currentExpression;

        // return base.VisitConditionalAccessExpression(node);
    }

    /// <summary>
    /// Very similar to MemberAccessExpression, but doesn't have an expression portion - just identifier
    /// Used in ConditionalAccessExpression since they are constructed left to right, then right to left like normal field access
    /// </summary>
    public override J? VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
    {


        // due to the fact that the `ConditionalAccessExpressionSyntax` is at the root of an expression like `foo?.Bar.Baz`
        // we need to find that root here, as the containment hierarchy using `J.FieldAccess` and `Cs.NullSafeExpression`
        // ends up being very different
        // ExpressionSyntax? parent = node;
        // while (parent is not ConditionalAccessExpressionSyntax)
        //     if ((parent = parent.Parent as ExpressionSyntax) == null)
        //         throw new InvalidOperationException(
        //             "Cannot find a `ConditionalAccessExpressionSyntax` in the containment hierarchy.");
        //
        // var conditionalAccess = (ConditionalAccessExpressionSyntax)parent;
        // var lhs = new Cs.NullSafeExpression(
        //     Core.Tree.RandomId(),
        //     Format(Leading(node)),
        //     Markers.EMPTY,
        //     new JRightPadded<Expression>(
        //         Convert<Expression>(conditionalAccess.Expression)!,
        //         Format(Leading(conditionalAccess.OperatorToken)),
        //         Markers.EMPTY
        //     )
        // );

        return new J.FieldAccess(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            new Markers(
                Core.Tree.RandomId(),
                new List<Core.Marker.Marker>
                {
                    new MemberBinding(Core.Tree.RandomId())
                }),
            Convert<Expression>(node.Name)!,
            new JLeftPadded<J.Identifier>(
                Format(Leading(node.OperatorToken)),
                Convert<J.Identifier>(node.Name)!,
                Markers.EMPTY
            ),
            MapType(node)
        );
    }

    public override J? VisitElementBindingExpression(ElementBindingExpressionSyntax node)
    {
        // due to the fact that the `ConditionalAccessExpressionSyntax` is at the root of an expression like `foo?.Bar.Baz`
        // we need to find that root here, as the containment hierarchy using `J.FieldAccess` and `Cs.NullSafeExpression`
        // ends up being very different
        ExpressionSyntax? parent = node;
        while (parent is not ConditionalAccessExpressionSyntax)
            if ((parent = parent.Parent as ExpressionSyntax) == null)
                throw new InvalidOperationException(
                    "Cannot find a `ConditionalAccessExpressionSyntax` in the containment hierarchy.");

        var conditionalAccess = (ConditionalAccessExpressionSyntax)parent;
        var lhs = new Cs.NullSafeExpression(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<Expression>(
                Convert<Expression>(conditionalAccess.Expression)!,
                Format(Leading(conditionalAccess.OperatorToken)),
                Markers.EMPTY
            )
        );

        return new J.ArrayAccess(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            lhs,
            new J.ArrayDimension(
                Core.Tree.RandomId(),
                Format(Leading(node.ArgumentList.OpenBracketToken)),
                Markers.EMPTY,
                new JRightPadded<Expression>(
                    Convert<Expression>(node.ArgumentList.Arguments[0])!,
                    Format(Trailing(node.ArgumentList.Arguments[0])),
                    Markers.EMPTY
                )
            ),
            MapType(node)
        );
    }

    public override J? VisitRangeExpression(RangeExpressionSyntax node)
    {
        // This was added in C# 8.0
        return base.VisitRangeExpression(node);
    }

    public override J? VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
    {
        return base.VisitImplicitElementAccess(node);
    }

    public override J? VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        return new J.Ternary(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            Convert<Expression>(node.Condition)!,
            new JLeftPadded<Expression>(
                Format(Leading(node.QuestionToken)),
                Convert<Expression>(node.WhenTrue)!,
                Markers.EMPTY
            ),
            new JLeftPadded<Expression>(
                Format(Leading(node.ColonToken)),
                Convert<Expression>(node.WhenFalse)!,
                Markers.EMPTY
            ),
            MapType(node)
        );
    }

    public override J? VisitThisExpression(ThisExpressionSyntax node)
    {
        return new J.Identifier(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            node.Token.Text,
            (MapType(node) as JavaType.Variable)?.Type,
            MapType(node) as JavaType.Variable
        );
    }

    public override J? VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        return new J.Literal(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.Token.Value,
            node.Token.Text,
            null,
            (MapType(node) as JavaType.Primitive)!
        );
    }

    public override J? VisitMakeRefExpression(MakeRefExpressionSyntax node)
    {
        return base.VisitMakeRefExpression(node);
    }

    public override J? VisitRefTypeExpression(RefTypeExpressionSyntax node)
    {
        return base.VisitRefTypeExpression(node);
    }

    public override J? VisitRefValueExpression(RefValueExpressionSyntax node)
    {
        return base.VisitRefValueExpression(node);
    }

    public override J? VisitDefaultExpression(DefaultExpressionSyntax node)
    {
        return base.VisitDefaultExpression(node);
    }

    public override J? VisitTypeOfExpression(TypeOfExpressionSyntax node)
    {
        return new J.MethodInvocation(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            null,
            null,
            new J.Identifier(
                Core.Tree.RandomId(),
                Format(Leading(node.Keyword)),
                Markers.EMPTY,
                [],
                node.Keyword.Text,
                null,
                null
            ),
            new JContainer<Expression>(
                Format(Leading(node.OpenParenToken)),
                [
                    new JRightPadded<Expression>(
                        Convert<Expression>(node.Type)!,
                        Format(Leading(node.CloseParenToken)),
                        Markers.EMPTY
                    )
                ],
                Markers.EMPTY
            ),
            MapType(node) as JavaType.Method
        );
    }

    public override J? VisitSizeOfExpression(SizeOfExpressionSyntax node)
    {
        return new J.MethodInvocation(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            null,
            null,
            new J.Identifier(
                Core.Tree.RandomId(),
                Format(Leading(node.Keyword)),
                Markers.EMPTY,
                [],
                node.Keyword.Text,
                null,
                null
            ),
            new JContainer<Expression>(
                Format(Leading(node.OpenParenToken)),
                [
                    new JRightPadded<Expression>(
                        Convert<Expression>(node.Type)!,
                        Format(Leading(node.CloseParenToken)),
                        Markers.EMPTY
                    )
                ],
                Markers.EMPTY
            ),
            MapType(node) as JavaType.Method
        );
    }

    public override J? VisitElementAccessExpression(ElementAccessExpressionSyntax node)
    {
        return MapArrayAccess(node, 0);
    }

    private J.ArrayAccess MapArrayAccess(ElementAccessExpressionSyntax node, int index)
    {
        return new J.ArrayAccess(
            Core.Tree.RandomId(),
            index == 0 ? Format(Leading(node)) : Space.EMPTY,
            Markers.EMPTY,
            index == node.ArgumentList.Arguments.Count - 1
                ? Convert<Expression>(node.Expression)!
                : MapArrayAccess(node, index + 1),
            new J.ArrayDimension(
                Core.Tree.RandomId(),
                Format(Leading(node.ArgumentList.OpenBracketToken)),
                Markers.EMPTY,
                new JRightPadded<Expression>(
                    Convert<Expression>(node.ArgumentList.Arguments[index])!,
                    Format(Trailing(node.ArgumentList.Arguments[index])),
                    Markers.EMPTY
                )
            ),
            MapType(node) // TODO this probably needs to be specific to the current array dimension
        );
    }

    public override J? VisitBracketedArgumentList(BracketedArgumentListSyntax node)
    {
        return base.VisitBracketedArgumentList(node);
    }

    public override J? VisitExpressionColon(ExpressionColonSyntax node)
    {
        return base.VisitExpressionColon(node);
    }

    public override J? VisitNameColon(NameColonSyntax node)
    {
        // This was added in C# 4.0
        return base.VisitNameColon(node);
    }

    public override J? VisitDeclarationExpression(DeclarationExpressionSyntax node)
    {
        return base.VisitDeclarationExpression(node);
    }

    public override J? VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
    {
        return new J.MethodDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [], // attributes are not supported for anonymous methods
            MapModifiers(node.Modifiers),
            null,
            null,
            new J.MethodDeclaration.IdentifierWithAnnotations(
                new J.Identifier(
                    Core.Tree.RandomId(),
                    Format(Leading(node.DelegateKeyword)),
                    Markers.EMPTY,
                    [],
                    node.DelegateKeyword.Text,
                    null,
                    null
                ),
                []
            ),
            MapParameters<Statement>(node.ParameterList) ?? new JContainer<Statement>(
                Space.EMPTY,
                [
                    JRightPadded<Statement>.Build(new J.Empty(Core.Tree.RandomId(),
                        Space.EMPTY,
                        Markers.EMPTY))
                ],
                Markers.EMPTY
            ),
            null,
            node.ExpressionBody != null ? Convert<J.Block>(node.ExpressionBody) : Convert<J.Block>(node.Block),
            null,
            MapType(node) as JavaType.Method
        );
    }

    public override J? VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        return new J.Lambda(
            Core.Tree.RandomId(),
            Format((Leading(node))),
            Markers.EMPTY,
            new J.Lambda.Parameters(
                Core.Tree.RandomId(),
                Space.EMPTY,
                Markers.EMPTY,
                false,
                [
                    MapParameter<J>(node.Parameter)
                ]
            ),
            Format(Leading(node.ArrowToken)),
            Convert<J>(node.Body)!,
            MapType(node)
        );
    }

    public override J? VisitRefExpression(RefExpressionSyntax node)
    {
        return base.VisitRefExpression(node);
    }

    public override J? VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        return new J.Lambda(
            Core.Tree.RandomId(),
            Format((Leading(node))),
            Markers.EMPTY,
            new J.Lambda.Parameters(
                Core.Tree.RandomId(),
                Space.EMPTY,
                Markers.EMPTY,
                true,
                MapParameters<J>(node.ParameterList)!.Elements
            ),
            Format(Leading(node.ArrowToken)),
            Convert<J>(node.Body)!,
            MapType(node)
        );
    }

    public override J? VisitInitializerExpression(InitializerExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.ArrayInitializerExpression))
        {
            return new J.NewArray(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                null, // must be `null` so that no `new` keyword is printed
                [], // must be empty so that no `[]` keyword is printed
                new JContainer<Expression>(
                    Format(Leading(node.OpenBraceToken)),
                    node.Expressions.Select((e, i) =>
                    {
                        var trailingComma = i == node.Expressions.Count - 1 &&
                                            e.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
                        return new JRightPadded<Expression>(
                            Convert<Expression>(e)!,
                            Format(Trailing(e)),
                            trailingComma
                                ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                                    Format(Trailing(e.GetLastToken().GetNextToken()))))
                                : Markers.EMPTY
                        );
                    }).ToList(), Markers.EMPTY),
                MapType(node)
            );
        }

        return new J.Block(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            JRightPadded<bool>.Build(false),
            node.Expressions.Count == 0
                ? []
                : node.Expressions.SkipLast(1).Select(MapExpressionStatement)
                    .Append(MapExpressionStatement(node.Expressions.Last(), true)).ToList(),
            Format(Leading(node.CloseBraceToken))
        );
    }

    public override J? VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
    {
        return new J.NewClass(
            Core.Tree.RandomId(),
            node.Initializer == null ? Format(Leading(node)) : Space.EMPTY,
            Markers.EMPTY,
            null,
            Format(Leading(node.NewKeyword)),
            new J.Empty(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY),
            new JContainer<Expression>(
                Format(Leading(node.ArgumentList.OpenParenToken)),
                node.ArgumentList.Arguments.Count == 0
                    ?
                    [
                        new JRightPadded<Expression>(
                            new J.Empty(
                                Core.Tree.RandomId(),
                                Space.EMPTY,
                                Markers.EMPTY
                            ),
                            Format(Leading(node.ArgumentList.CloseParenToken)),
                            Markers.EMPTY
                        )
                    ]
                    : node.ArgumentList.Arguments.Select(MapArgument).ToList(),
                Markers.EMPTY
            ),
            node.Initializer != null ? Convert<J.Block>(node.Initializer) : null,
            MapType(node) as JavaType.Method // this should be FindConstructorType(node) which return explicitly javaType.Method
        );
    }

    public override J? VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        return new J.NewClass(
            Core.Tree.RandomId(),
            node.Initializer == null ? Format(Leading(node)) : Space.EMPTY,
            Markers.EMPTY,
            null,
            Format(Leading(node.NewKeyword)),
            Convert<TypeTree>(node.Type),
            new JContainer<Expression>(
                node.ArgumentList == null ? Space.EMPTY : Format(Leading(node.ArgumentList.OpenParenToken)),
                node.ArgumentList == null || node.ArgumentList.Arguments.Count == 0
                    ?
                    [
                        new JRightPadded<Expression>(
                            new J.Empty(
                                Core.Tree.RandomId(),
                                Space.EMPTY,
                                Markers.EMPTY
                            ),
                            node.ArgumentList == null
                                ? Space.EMPTY
                                : Format(Leading(node.ArgumentList.CloseParenToken)),
                            Markers.EMPTY
                        )
                    ]
                    : node.ArgumentList.Arguments.Select(MapArgument).ToList(),
                node.ArgumentList == null
                    ? Markers.EMPTY.Add(new OmitParentheses(Core.Tree.RandomId()))
                    : Markers.EMPTY
            ),
            node.Initializer != null ? Convert<J.Block>(node.Initializer) : null,
            MapType(node) as JavaType.Method // this should be FindConstructorType(node) which return explicitly javaType.Method
        );
    }

    public override J? VisitWithExpression(WithExpressionSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitWithExpression(node);
    }

    public override J? VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
    {
        return new J.VariableDeclarations(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            [],
            null,
            null!,
            [],
            [
                node.NameEquals != null
                    ? MapNamedVariableFromNameEquals(node.NameEquals, node.Expression)
                    : MapNamedVariableFromExpression(node.Expression)
            ]
        );
    }

    private JRightPadded<J.VariableDeclarations.NamedVariable> MapNamedVariableFromNameEquals(
        NameEqualsSyntax nameEqualsSyntax, ExpressionSyntax expression)
    {
        return new JRightPadded<J.VariableDeclarations.NamedVariable>(
            new J.VariableDeclarations.NamedVariable(
                Core.Tree.RandomId(),
                Format(Leading(nameEqualsSyntax.Name)),
                Markers.EMPTY,
                Convert<J.Identifier>(nameEqualsSyntax.Name)!,
                [],
                new JLeftPadded<Expression>(
                    Format(Leading(nameEqualsSyntax.EqualsToken)),
                    Convert<Expression>(expression)!,
                    Markers.EMPTY
                ),
                MapType(expression) as JavaType.Variable
            ),
            Format(Trailing(expression)),
            Markers.EMPTY
        );
    }

    private JRightPadded<J.VariableDeclarations.NamedVariable> MapNamedVariableFromExpression(
        ExpressionSyntax expression)
    {
        var identifierOrFieldAccess = Convert<Expression>(expression)!;
        var identifier = identifierOrFieldAccess is J.Identifier i
            ? i
            : (identifierOrFieldAccess as J.FieldAccess)?.Name ?? throw new InvalidOperationException("Can't determine identifier");
        return new JRightPadded<J.VariableDeclarations.NamedVariable>(
            new J.VariableDeclarations.NamedVariable(
                Core.Tree.RandomId(),
                Format(Leading(expression)),
                Markers.EMPTY,
                new J.Identifier(Core.Tree.RandomId(), identifierOrFieldAccess.Prefix, identifierOrFieldAccess.Markers,
                    identifier.Annotations, expression.ToString(), identifier.Type, identifier.FieldType),
                [],
                null,
                identifier.Type as JavaType.Variable
            ),
            Format(Trailing(expression)),
            Markers.EMPTY
        );
    }

    public override J? VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
    {
        return new J.NewClass(
            Core.Tree.RandomId(),
            Space.EMPTY,
            Markers.EMPTY,
            null,
            Format(Leading(node.NewKeyword)),
            new J.Empty(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY),
            new JContainer<Expression>(
                Space.EMPTY,
                [
                    new JRightPadded<Expression>(
                        new J.Empty(
                            Core.Tree.RandomId(),
                            Space.EMPTY,
                            Markers.EMPTY
                        ),
                        Space.EMPTY,
                        Markers.EMPTY
                    )
                ],
                Markers.EMPTY.Add(new OmitParentheses(Core.Tree.RandomId()))
            ),
            new J.Block(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenBraceToken)),
                Markers.EMPTY,
                JRightPadded<bool>.Build(false),
                node.Initializers.Count == 0
                    ? []
                    : node.Initializers.SkipLast(1).Select(MapAnonymousObjectMember)
                        .Append(MapAnonymousObjectMember(node.Initializers.Last(), true)).ToList(),
                Format(Leading(node.CloseBraceToken))
            ),
            MapType(node) as JavaType.Method // this should be FindConstructorType(node) which return explicitly javaType.Method
        );
    }

    private JRightPadded<Statement> MapAnonymousObjectMember(AnonymousObjectMemberDeclaratorSyntax aomds)
    {
        // This was added in C# 3.0
        return MapAnonymousObjectMember(aomds, false);
    }

    private JRightPadded<Statement> MapAnonymousObjectMember(AnonymousObjectMemberDeclaratorSyntax aomds,
        bool isLastElement)
    {
        var statement = Convert<Statement>(aomds)!;
        var trailingComma = isLastElement && aomds.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
        return new JRightPadded<Statement>(
            statement,
            Space.EMPTY,
            trailingComma
                ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                    Format(Trailing(aomds.GetLastToken().GetNextToken()))))
                : Markers.EMPTY
        );
    }

    public override J? VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
    {
        return new J.NewArray(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            Convert<TypeTree>(node.Type.ElementType),
            node.Type.RankSpecifiers.Select(r => Convert<J.ArrayDimension>(r)!).ToList(),
            node.Initializer != null
                ? new JContainer<Expression>(
                    Format(Leading(node.Initializer.OpenBraceToken)),
                    node.Initializer.Expressions.Select(e =>
                            new JRightPadded<Expression>(Convert<Expression>(e)!, Format(Trailing(e)), Markers.EMPTY))
                        .ToList(),
                    Markers.EMPTY
                )
                : null,
            MapType(node)
        );
    }

    public override J? VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitImplicitArrayCreationExpression(node);
    }

    public override J? VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
    {
        return base.VisitStackAllocArrayCreationExpression(node);
    }

    public override J? VisitImplicitStackAllocArrayCreationExpression(
        ImplicitStackAllocArrayCreationExpressionSyntax node)
    {
        return base.VisitImplicitStackAllocArrayCreationExpression(node);
    }

    public override J? VisitCollectionExpression(CollectionExpressionSyntax node)
    {
        return new Cs.CollectionExpression(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.Elements.Count == 0
                ?
                [
                    new JRightPadded<Expression>(
                        new J.Empty(
                            Core.Tree.RandomId(),
                            Format(Trailing(node.OpenBracketToken)),
                            Markers.EMPTY
                        ),
                        Space.EMPTY,
                        node.CloseBracketToken.GetPreviousToken().IsKind(SyntaxKind.CommaToken)
                            ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                                Format(Leading(node.CloseBracketToken))))
                            : Markers.EMPTY
                    )
                ]
                : node.Elements.Select(e => new JRightPadded<Expression>(
                    Convert<Expression>(e)!,
                    Format(Trailing(e)),
                    e == node.Elements.Last() && e.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken)
                        ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                            Format(Trailing(e.GetLastToken().GetNextToken()))))
                        : Markers.EMPTY
                )).ToList(),
            MapType(node)
        );
    }

    public override J? VisitExpressionElement(ExpressionElementSyntax node)
    {
        return Convert<Expression>(node.Expression);
    }

    public override J? VisitSpreadElement(SpreadElementSyntax node)
    {
        // This was added in C# 12.0
        return base.VisitSpreadElement(node);
    }

    public override J? VisitQueryExpression(QueryExpressionSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitQueryExpression(node);
    }

    public override J? VisitQueryBody(QueryBodySyntax node)
    {
        // This was added in C# 3.0
        return base.VisitQueryBody(node);
    }

    public override J? VisitFromClause(FromClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitFromClause(node);
    }

    public override J? VisitLetClause(LetClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitLetClause(node);
    }

    public override J? VisitJoinClause(JoinClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitJoinClause(node);
    }

    public override J? VisitJoinIntoClause(JoinIntoClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitJoinIntoClause(node);
    }

    public override J? VisitWhereClause(WhereClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitWhereClause(node);
    }

    public override J? VisitOrderByClause(OrderByClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitOrderByClause(node);
    }

    public override J? VisitSelectClause(SelectClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitSelectClause(node);
    }

    public override J? VisitGroupClause(GroupClauseSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitGroupClause(node);
    }

    public override J? VisitQueryContinuation(QueryContinuationSyntax node)
    {
        // This was added in C# 3.0
        return base.VisitQueryContinuation(node);
    }

    public override J? VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
    {
        return new J.Empty(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY
        );
    }

    public override J? VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
    {
        // This was added in C# 6.0
        return new Cs.InterpolatedString(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.StringStartToken.ToString(),
            node.Contents.Count == 0
                ?
                [
                    new JRightPadded<Expression>(
                        new J.Empty(
                            Core.Tree.RandomId(),
                            Format(Trailing(node.StringStartToken)),
                            Markers.EMPTY
                        ),
                        Space.EMPTY,
                        Markers.EMPTY
                    )
                ]
                : node.Contents.Select(c => new JRightPadded<Expression>(
                    Convert<Expression>(c)!,
                    Format(Trailing(c)),
                    Markers.EMPTY
                )).ToList(),
            node.StringEndToken.ToString()
        );
    }

    public override J? VisitIsPatternExpression(IsPatternExpressionSyntax node)
    {
        if (node.Pattern is DeclarationPatternSyntax dp)
        {
            return new J.InstanceOf(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                new JRightPadded<Expression>(
                    Convert<Expression>(node.Expression)!,
                    Format(Trailing(node.Expression)),
                    Markers.EMPTY),
                Convert<TypeTree>(dp.Type)!,
                Convert<J>(dp.Designation),
                MapType(node)
            );
        }

        return base.VisitIsPatternExpression(node);
    }

    public override J? VisitThrowExpression(ThrowExpressionSyntax node)
    {
        return new Cs.StatementExpression(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.Throw(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                Convert<Expression>(node.Expression)!
            )
        );
    }

    public override J? VisitWhenClause(WhenClauseSyntax node)
    {
        // This was added in C# 7.0
        return base.VisitWhenClause(node);
    }

    public override J? VisitDiscardPattern(DiscardPatternSyntax node)
    {
        return base.VisitDiscardPattern(node);
    }

    public override J? VisitDeclarationPattern(DeclarationPatternSyntax node)
    {
        return base.VisitDeclarationPattern(node);
    }

    public override J? VisitVarPattern(VarPatternSyntax node)
    {
        return base.VisitVarPattern(node);
    }

    public override J? VisitRecursivePattern(RecursivePatternSyntax node)
    {
        return base.VisitRecursivePattern(node);
    }

    public override J? VisitPositionalPatternClause(PositionalPatternClauseSyntax node)
    {
        return base.VisitPositionalPatternClause(node);
    }

    public override J? VisitPropertyPatternClause(PropertyPatternClauseSyntax node)
    {
        return base.VisitPropertyPatternClause(node);
    }

    public override J? VisitConstantPattern(ConstantPatternSyntax node)
    {
        return base.VisitConstantPattern(node);
    }

    public override J? VisitParenthesizedPattern(ParenthesizedPatternSyntax node)
    {
        return base.VisitParenthesizedPattern(node);
    }

    public override J? VisitRelationalPattern(RelationalPatternSyntax node)
    {
        return new J.Binary(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.Empty(
                Core.Tree.RandomId(),
                Space.EMPTY,
                Markers.EMPTY
            ),
            new JLeftPadded<J.Binary.Type>(
                Format(Leading(node.OperatorToken)),
                MapBinaryExpressionOperator(node.OperatorToken),
                Markers.EMPTY),
            Convert<Expression>(node.Expression)!,
            MapType(node)
        );
    }

    public override J? VisitTypePattern(TypePatternSyntax node)
    {
        return base.VisitTypePattern(node);
    }

    public override J? VisitUnaryPattern(UnaryPatternSyntax node)
    {
        return base.VisitUnaryPattern(node);
    }

    public override J? VisitListPattern(ListPatternSyntax node)
    {
        return base.VisitListPattern(node);
    }

    public override J? VisitSlicePattern(SlicePatternSyntax node)
    {
        return base.VisitSlicePattern(node);
    }

    public override J? VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
    {
        return new J.Literal(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.TextToken.Text,
            node.TextToken.Text,
            null,
            (JavaType.Primitive)MapType(node)
        );
    }

    public override J? VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
    {
        return Convert<Expression>(node.Value);
    }

    public override J? VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
    {
        return new J.Literal(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.FormatStringToken.Text,
            node.FormatStringToken.Text,
            null,
            (JavaType.Primitive)MapType(node)
        );
    }

    public override J? VisitGlobalStatement(GlobalStatementSyntax node)
    {
        return base.VisitGlobalStatement(node);
    }

    public override J? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        return new J.MethodDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            MapModifiers(node.Modifiers),
            Visit(node.TypeParameterList) as J.TypeParameters,
            Convert<TypeTree>(node.ReturnType),
            new J.MethodDeclaration.IdentifierWithAnnotations(
                new J.Identifier(
                    Core.Tree.RandomId(),
                    Format(Leading(node.Identifier)),
                    Markers.EMPTY,
                    (Enumerable.Empty<J.Annotation>() as IList<J.Annotation>)!,
                    node.Identifier.Text,
                    null,
                    null
                ),
                []
            ),
            MapParameters<Statement>(node.ParameterList)!,
            null,
            node.Body != null ? Convert<J.Block>(node.Body) :
            node.ExpressionBody != null ? Convert<J.Block>(node.ExpressionBody) : null,
            null,
            MapType(node) as JavaType.Method
        );
    }

    public override J? VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
    {
        var usingModifier = node.UsingKeyword.IsKind(SyntaxKind.UsingKeyword)
            ? new J.Modifier(
                Core.Tree.RandomId(),
                Format(Leading(node.UsingKeyword)),
                Markers.EMPTY,
                "using",
                J.Modifier.Type.LanguageExtension,
                []
            )
            : null;

        return new J.VariableDeclarations(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            usingModifier != null ? [usingModifier, .. MapModifiers(node.Modifiers)] : MapModifiers(node.Modifiers),
            Visit(node.Declaration.Type) as TypeTree,
            null!,
            [],
            node.Declaration.Variables.Select(MapVariable).ToList()
        );
    }

    private JRightPadded<J.VariableDeclarations.NamedVariable> MapVariable(VariableDeclaratorSyntax variableDeclarator)
    {
        var namedVariable = (Visit(variableDeclarator) as J.VariableDeclarations.NamedVariable)!;
        return new JRightPadded<J.VariableDeclarations.NamedVariable>(
            namedVariable,
            Format(Trailing(variableDeclarator)),
            Markers.EMPTY
        );
    }

    public override J? VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        return new J.VariableDeclarations(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            [],
            Visit(node.Type) as TypeTree,
            null!,
            [],
            node.Variables.Select(MapVariable).ToList()
        );
    }

    public override J? VisitVariableDeclarator(VariableDeclaratorSyntax node)
    {
        var javaType = (MapType(node) as JavaType.Variable)!;
        return new J.VariableDeclarations.NamedVariable(
            Core.Tree.RandomId(),
            Format(Leading(node.Identifier)),
            Markers.EMPTY,
            new J.Identifier(
                Core.Tree.RandomId(),
                Space.EMPTY,
                Markers.EMPTY,
                [],
                node.Identifier.Text,
                javaType.Type,
                javaType
            ),
            MapArrayDimensions(node.ArgumentList),
            node.Initializer != null
                ? new JLeftPadded<Expression>(
                    Format(Leading(node.Initializer)),
                    Convert<Expression>(node.Initializer.Value)!,
                    Markers.EMPTY
                )
                : null,
            javaType
        );
    }

    private IList<JLeftPadded<Space>> MapArrayDimensions(BracketedArgumentListSyntax? nodeArgumentList)
    {
        return [];
    }

    public override J? VisitEqualsValueClause(EqualsValueClauseSyntax node)
    {
        return base.VisitEqualsValueClause(node);
    }

    public override J? VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
    {
        return MapIdentifier(node.Identifier, MapType(node));
    }

    public override J? VisitDiscardDesignation(DiscardDesignationSyntax node)
    {
        return MapIdentifier(node.UnderscoreToken, MapType(node));
    }

    public override J? VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
    {
        return base.VisitParenthesizedVariableDesignation(node);
    }

    public override J? VisitExpressionStatement(ExpressionStatementSyntax node)
    {
        var j = Convert<J>(node.Expression);
        if (j is Statement)
            return j;

        return new Cs.ExpressionStatement(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            (j as Expression)!
        );
    }

    public override J? VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return new J.Empty(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY
        );
    }

    public override J? VisitLabeledStatement(LabeledStatementSyntax node)
    {
        return new J.Label(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JRightPadded<J.Identifier>(
                MapIdentifier(node.Identifier, null),
                Format(Trailing(node.Identifier)),
                Markers.EMPTY
            ),
            Convert<Statement>(node.Statement)!
        );
    }

    public override J? VisitGotoStatement(GotoStatementSyntax node)
    {
        return base.VisitGotoStatement(node);
    }

    public override J? VisitContinueStatement(ContinueStatementSyntax node)
    {
        return new J.Continue(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            null
        );
    }

    public override J? VisitReturnStatement(ReturnStatementSyntax node)
    {
        return new J.Return(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.Expression != null ? Convert<Expression>(node.Expression!) : null
        );
    }

    public override J? VisitThrowStatement(ThrowStatementSyntax node)
    {
        // FIXME: NODE has AttributesList
        return new J.Throw(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            node.Expression != null
                ? Convert<Expression>(node.Expression)!
                : new J.Empty(
                    Core.Tree.RandomId(),
                    Format(Leading(node.SemicolonToken)),
                    Markers.EMPTY
                )
        );
    }

    public override J? VisitYieldStatement(YieldStatementSyntax node)
    {
        return base.VisitYieldStatement(node);
    }

    public override J? VisitWhileStatement(WhileStatementSyntax node)
    {
        return new J.WhileLoop(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ControlParentheses<Expression>(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenParenToken)),
                Markers.EMPTY,
                new JRightPadded<Expression>(
                    Convert<Expression>(node.Condition)!,
                    Format(Leading(node.CloseParenToken)),
                    Markers.EMPTY)
            ),
            MapStatement(node.Statement)
        );
    }

    public override J? VisitDoStatement(DoStatementSyntax node)
    {
        return base.VisitDoStatement(node);
    }

    public override J? VisitForStatement(ForStatementSyntax node)
    {
        return new J.ForLoop(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ForLoop.Control(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenParenToken)),
                Markers.EMPTY,
                node.Declaration == null && node.Initializers.Count == 0
                    ?
                    [
                        JRightPadded<Statement>.Build(new J.Empty(Core.Tree.RandomId(),
                            Format(Leading(node.FirstSemicolonToken)), Markers.EMPTY))
                    ]
                    : node.Declaration != null
                        ?
                        [
                            new JRightPadded<Statement>(Convert<Statement>(node.Declaration!)!,
                                Format(Leading(node.FirstSemicolonToken)), Markers.EMPTY)
                        ]
                        : node.Initializers.Select(MapExpressionStatement).ToList(),
                node.Condition != null
                    ? MapExpression(node.Condition)
                    : JRightPadded<Expression>.Build(
                        new J.Empty(Core.Tree.RandomId(), Format(Leading(node.SecondSemicolonToken)), Markers.EMPTY)
                    ),
                node.Incrementors.Count == 0
                    ?
                    [
                        JRightPadded<Statement>.Build(new J.Empty(Core.Tree.RandomId(),
                            Format(Leading(node.CloseParenToken)), Markers.EMPTY))
                    ]
                    : node.Incrementors.Select(MapExpressionStatement).ToList()
            ),
            MapStatement(node.Statement)
        );
    }

    public override J? VisitForEachStatement(ForEachStatementSyntax node)
    {
        var javaType = (MapType(node) as JavaType.Variable)!;
        return new J.ForEachLoop(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ForEachLoop.Control(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenParenToken)),
                Markers.EMPTY,
                new JRightPadded<J.VariableDeclarations>(
                    new J.VariableDeclarations(
                        Core.Tree.RandomId(),
                        Format(Leading(node.Type)),
                        Markers.EMPTY,
                        [],
                        [],
                        Convert<TypeTree>(node.Type),
                        null!,
                        [],
                        [
                            new JRightPadded<J.VariableDeclarations.NamedVariable>(
                                new J.VariableDeclarations.NamedVariable(
                                    Core.Tree.RandomId(),
                                    Format(Leading(node.Identifier)),
                                    Markers.EMPTY,
                                    new J.Identifier(
                                        Core.Tree.RandomId(),
                                        Space.EMPTY,
                                        Markers.EMPTY,
                                        [],
                                        node.Identifier.Text,
                                        javaType.Type,
                                        javaType
                                    ),
                                    [],
                                    null,
                                    javaType
                                ),
                                Space.EMPTY,
                                Markers.EMPTY
                            )
                        ]
                    ),
                    Format(Trailing(node.Identifier)),
                    Markers.EMPTY
                ),
                MapExpression(node.Expression)
            ),
            MapStatement(node.Statement)
        );
    }

    public override J? VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
    {
        return base.VisitForEachVariableStatement(node);
    }

    public override J? VisitUsingStatement(UsingStatementSyntax node)
    {
        var jContainer = new JContainer<J.Try.Resource>(
            Format(Leading(node.OpenParenToken)),
            node.Declaration != null
                ?
                [
                    new JRightPadded<J.Try.Resource>(
                        new J.Try.Resource(
                            Core.Tree.RandomId(),
                            Format(Leading(node.Declaration)),
                            Markers.EMPTY,
                            Convert<TypedTree>(node.Declaration)!,
                            false
                        ),
                        Format(Trailing(node.Declaration)),
                        Markers.EMPTY
                    )
                ]
                : [],
            Markers.EMPTY
        );
        var s = Convert<Statement>(node.Statement) ?? throw new InvalidOperationException("Statement is empty after conversion");

        return new J.Try(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            jContainer,
             s is not J.Block block
                ? new J.Block(
                    Core.Tree.RandomId(),
                    Space.EMPTY,
                    Markers.EMPTY.Add(new OmitBraces(Core.Tree.RandomId())),
                    JRightPadded<bool>.Build(false),
                    [
                        JRightPadded<Statement>.Build(s),
                    ],
                    Space.EMPTY
                )
                : block,
            [],
            null
        );
    }

    public override J? VisitFixedStatement(FixedStatementSyntax node)
    {
        return base.VisitFixedStatement(node);
    }

    public override J? VisitCheckedStatement(CheckedStatementSyntax node)
    {
        return base.VisitCheckedStatement(node);
    }

    public override J? VisitUnsafeStatement(UnsafeStatementSyntax node)
    {
        return base.VisitUnsafeStatement(node);
    }

    public override J? VisitLockStatement(LockStatementSyntax node)
    {
        return base.VisitLockStatement(node);
    }

    public override J? VisitIfStatement(IfStatementSyntax node)
    {
        return new J.If(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ControlParentheses<Expression>(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenParenToken)),
                Markers.EMPTY,
                MapExpression(node.Condition)
            ),
            MapStatement(node.Statement),
            node.Else != null ? Convert<J.If.Else>(node.Else) : null
        );
    }

    private JRightPadded<Expression> MapExpression(ExpressionSyntax es)
    {
        var expression = Convert<Expression>(es)!;

        return new JRightPadded<Expression>(
            expression,
            Format(Trailing(es)),
            Markers.EMPTY
        );
    }

    private JRightPadded<Statement> MapExpressionStatement(ExpressionSyntax es)
    {
        return MapExpressionStatement(es, false);
    }

    private JRightPadded<Statement> MapExpressionStatement(ExpressionSyntax es, bool isLastElement)
    {
        var j = Convert<J>(es);
        var trailingComma = isLastElement && es.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
        if (j is Statement s)
            return new JRightPadded<Statement>(
                s,
                Format(Trailing(es)),
                trailingComma
                    ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                        Format(Trailing(es.GetLastToken().GetNextToken()))))
                    : Markers.EMPTY
            );

        return new JRightPadded<Statement>(
            new Cs.ExpressionStatement(
                Core.Tree.RandomId(),
                Format(Leading(es)),
                Markers.EMPTY,
                (j as Expression)!
            ),
            Format(Trailing(es)),
            trailingComma
                ? Markers.EMPTY.Add(new TrailingComma(Core.Tree.RandomId(),
                    Format(Trailing(es.GetLastToken().GetNextToken()))))
                : Markers.EMPTY
        );
    }

    public override J? VisitElseClause(ElseClauseSyntax node)
    {
        return new J.If.Else(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            MapStatement(node.Statement)
        );
    }

    public override J? VisitSwitchStatement(SwitchStatementSyntax node)
    {
        return new J.Switch(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new J.ControlParentheses<Expression>(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenParenToken)),
                node.OpenParenToken.IsKind(SyntaxKind.None)
                    ? Markers.EMPTY.Add(new OmitParentheses(Core.Tree.RandomId()))
                    : Markers.EMPTY,
                MapExpression(node.Expression)
            ),
            new J.Block(
                Core.Tree.RandomId(),
                Format(Leading(node.OpenBraceToken)),
                Markers.EMPTY,
                new JRightPadded<bool>(false, Space.EMPTY, Markers.EMPTY),
                node.Sections.SelectMany(MapSwitchCase).ToList(),
                Format(Leading(node.CloseBraceToken))
            )
        );
    }

    private IList<JRightPadded<Statement>> MapSwitchCase(SwitchSectionSyntax sss)
    {
        return sss.Labels.SkipLast(1)
            .Select(sls =>
            {
                var statement = new J.Case(
                    Core.Tree.RandomId(),
                    Format(Leading(sls)),
                    Markers.EMPTY,
                    J.Case.Type.Statement,
                    new JContainer<Expression>(
                        Format(Leading(sls)),
                        [
                            MapSwitchCaseLabel(sls)
                        ],
                        Markers.EMPTY
                    ),
                    JContainer<Statement>.Build<Statement>(
                        Format(Leading(sls.ColonToken)),
                        [
                        ],
                        Markers.EMPTY
                    ),
                    null
                );
                return new JRightPadded<Statement>(
                    statement,
                    Space.EMPTY,
                    Markers.EMPTY
                );
            })
            .Append(
                new JRightPadded<Statement>(
                    new J.Case(
                        Core.Tree.RandomId(),
                        Format(Leading(sss.Labels.Last())),
                        Markers.EMPTY,
                        J.Case.Type.Statement,
                        new JContainer<Expression>(
                            Format(Leading(sss.Labels.Last())),
                            [
                                MapSwitchCaseLabel(sss.Labels.Last())
                            ],
                            Markers.EMPTY
                        ),
                        JContainer<Statement>.Build(
                            Format(Leading(sss.Labels.Last().ColonToken)),
                            sss.Statements.Select(MapStatement).ToList(),
                            Markers.EMPTY
                        ),
                        null
                    ),
                    Space.EMPTY,
                    Markers.EMPTY
                ))
            .ToList();
    }


    public override J? VisitSwitchSection(SwitchSectionSyntax node)
    {
        return base.VisitSwitchSection(node);
    }

    private JRightPadded<Expression> MapSwitchCaseLabel(SwitchLabelSyntax sls)
    {
        var expression = Convert<Expression>(sls)!;
        return new JRightPadded<Expression>(
            expression,
            Space.EMPTY,
            Markers.EMPTY
        );
    }

    public override J? VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
    {
        return Convert<J>(node.Pattern);
    }

    public override J? VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
    {
        return Convert<J>(node.Value);
    }

    public override J? VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
    {
        return new J.Identifier(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY, [], node.Keyword.Text, null, null);
    }

    public override J? VisitSwitchExpression(SwitchExpressionSyntax node)
    {
        return base.VisitSwitchExpression(node);
    }

    public override J? VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
    {
        return base.VisitSwitchExpressionArm(node);
    }

    public override J? VisitTryStatement(TryStatementSyntax node)
    {
        return new J.Try(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            null,
            Convert<J.Block>(node.Block)!,
            node.Catches.Select(Convert<J.Try.Catch>).ToList()!,
            node.Finally != null
                ? new JLeftPadded<J.Block>(
                    Format(Leading(node.Finally)),
                    Convert<J.Block>(node.Finally)!,
                    Markers.EMPTY)
                : null
        );
    }

    public override J? VisitCatchFilterClause(CatchFilterClauseSyntax node)
    {
        return base.VisitCatchFilterClause(node);
    }

    public override J? VisitFinallyClause(FinallyClauseSyntax node)
    {
        return Convert<J.Block>(node.Block);
    }

    public override J? VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
    {
        return new Cs.ExternAlias(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            new JLeftPadded<J.Identifier>(
                Format(Leading(node.AliasKeyword)),
                MapIdentifier(node.Identifier, null),
                Markers.EMPTY
            )
        );
    }

    public override J? VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
    {
        return MapIdentifier(node.Identifier, null);
    }

    public override J? VisitAttributeArgumentList(AttributeArgumentListSyntax node)
    {
        return base.VisitAttributeArgumentList(node);
    }

    public override J? VisitNameEquals(NameEqualsSyntax node)
    {
        return base.VisitNameEquals(node);
    }

    public override J? VisitDelegateDeclaration(DelegateDeclarationSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitDelegateDeclaration(node);
    }

    public override J? VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        return base.VisitEnumMemberDeclaration(node);
    }

    public override J? VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
    {
        // This was added in C# 2.0
        return base.VisitTypeParameterConstraintClause(node);
    }

    public override J? VisitConstructorConstraint(ConstructorConstraintSyntax node)
    {
        // This was added in C# 2.0
        return base.VisitConstructorConstraint(node);
    }

    public override J? VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
    {
        return base.VisitClassOrStructConstraint(node);
    }

    public override J? VisitTypeConstraint(TypeConstraintSyntax node)
    {
        return base.VisitTypeConstraint(node);
    }

    public override J? VisitDefaultConstraint(DefaultConstraintSyntax node)
    {
        return base.VisitDefaultConstraint(node);
    }

    public override J? VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        var attributeLists = MapAttributes(node.AttributeLists);
        var variableDeclarations = new J.VariableDeclarations(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            MapModifiers(node.Modifiers),
            Convert<TypeTree>(node.Declaration.Type),
            null,
            [],
            node.Declaration.Variables.Select(MapVariable).ToList()
        );
        return attributeLists != null
            ? new Cs.AnnotatedStatement(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                attributeLists,
                variableDeclarations
            )
            : variableDeclarations;
    }

    public override J? VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        return base.VisitEventFieldDeclaration(node);
    }

    public override J? VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
    {
        return base.VisitExplicitInterfaceSpecifier(node);
    }

    public override J? VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        return base.VisitOperatorDeclaration(node);
    }

    public override J? VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
    {
        return base.VisitConversionOperatorDeclaration(node);
    }

    public override J? VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        if (node.Initializer != null)
        {
            // TODO support constructor delegation
            return base.VisitConstructorDeclaration(node);
        }

        var attributeLists = MapAttributes(node.AttributeLists);

        var methodDeclaration = new J.MethodDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            [],
            MapModifiers(node.Modifiers),
            null, // constructors have no type parameters
            null, // constructors have no return type
            new J.MethodDeclaration.IdentifierWithAnnotations(
                MapIdentifier(node.Identifier, null),
                [] // attributes always appear as leading
            ),
            MapParameters<Statement>(node.ParameterList)!,
            null, // C# has no checked exceptions
            Convert<J.Block>(node.Body),
            null, // not applicable to constructors
            MapType(node) as JavaType.Method
        );

        return attributeLists != null
            ? new Cs.AnnotatedStatement(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                attributeLists,
                methodDeclaration
            )
            : methodDeclaration;
    }

    public override J? VisitConstructorInitializer(ConstructorInitializerSyntax node)
    {
        return base.VisitConstructorInitializer(node);
    }

    public override J? VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitDestructorDeclaration(node);
    }

    public override J? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        var typeTree = Convert<TypeTree>(node.Type)!;
        return new Cs.PropertyDeclaration(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY,
            MapAttributes(node.AttributeLists) ?? [],
            MapModifiers(node.Modifiers),
            typeTree,
            node.ExplicitInterfaceSpecifier != null
                ? new JRightPadded<NameTree>(
                    Convert<NameTree>(node.ExplicitInterfaceSpecifier.Name)!,
                    Format(Leading(node.ExplicitInterfaceSpecifier.DotToken)),
                    Markers.EMPTY
                )
                : null,
            MapIdentifier(node.Identifier, typeTree.Type),
            node.ExpressionBody != null
                ? Convert<J.Block>(node.ExpressionBody)!
                : Convert<J.Block>(node.AccessorList)!,
            node.Initializer != null
                ? new JLeftPadded<Expression>(
                    Format(Leading(node.Initializer)),
                    Convert<Expression>(node.Initializer.Value)!,
                    Markers.EMPTY
                )
                : null
        );
    }

    public override J? VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
    {
        return new J.Block(
            Core.Tree.RandomId(),
            Format(Leading(node)),
            Markers.EMPTY.Add(new SingleExpressionBlock(Core.Tree.RandomId()))
                .Add(new OmitBraces(Core.Tree.RandomId())),
            JRightPadded<bool>.Build(false),
            [
                MapExpressionStatement(node.Expression)
            ],
            Space.EMPTY
        );
    }

    public override J? VisitEventDeclaration(EventDeclarationSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitEventDeclaration(node);
    }

    public override J? VisitIndexerDeclaration(IndexerDeclarationSyntax node)
    {
        // This was added in C# 1.0
        return base.VisitIndexerDeclaration(node);
    }

    public override J? VisitParameterList(ParameterListSyntax node)
    {
        return base.VisitParameterList(node);
    }

    public override J? VisitBracketedParameterList(BracketedParameterListSyntax node)
    {
        return base.VisitBracketedParameterList(node);
    }

    public override J? VisitFunctionPointerParameter(FunctionPointerParameterSyntax node)
    {
        return base.VisitFunctionPointerParameter(node);
    }

    public override J? VisitIncompleteMember(IncompleteMemberSyntax node)
    {
        return base.VisitIncompleteMember(node);
    }

    public override J? VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
    {
        return base.VisitSkippedTokensTrivia(node);
    }

    public override J? VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
    {
        return base.VisitDocumentationCommentTrivia(node);
    }

    public override J? VisitTypeCref(TypeCrefSyntax node)
    {
        return base.VisitTypeCref(node);
    }

    public override J? VisitQualifiedCref(QualifiedCrefSyntax node)
    {
        return base.VisitQualifiedCref(node);
    }

    public override J? VisitNameMemberCref(NameMemberCrefSyntax node)
    {
        return base.VisitNameMemberCref(node);
    }

    public override J? VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
    {
        return base.VisitIndexerMemberCref(node);
    }

    public override J? VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
    {
        return base.VisitOperatorMemberCref(node);
    }

    public override J? VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
    {
        return base.VisitConversionOperatorMemberCref(node);
    }

    public override J? VisitCrefParameterList(CrefParameterListSyntax node)
    {
        return base.VisitCrefParameterList(node);
    }

    public override J? VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
    {
        return base.VisitCrefBracketedParameterList(node);
    }

    public override J? VisitCrefParameter(CrefParameterSyntax node)
    {
        return base.VisitCrefParameter(node);
    }

    public override J? VisitXmlElement(XmlElementSyntax node)
    {
        return base.VisitXmlElement(node);
    }

    public override J? VisitXmlElementStartTag(XmlElementStartTagSyntax node)
    {
        return base.VisitXmlElementStartTag(node);
    }

    public override J? VisitXmlElementEndTag(XmlElementEndTagSyntax node)
    {
        return base.VisitXmlElementEndTag(node);
    }

    public override J? VisitXmlEmptyElement(XmlEmptyElementSyntax node)
    {
        return base.VisitXmlEmptyElement(node);
    }

    public override J? VisitXmlName(XmlNameSyntax node)
    {
        return base.VisitXmlName(node);
    }

    public override J? VisitXmlPrefix(XmlPrefixSyntax node)
    {
        return base.VisitXmlPrefix(node);
    }

    public override J? VisitXmlTextAttribute(XmlTextAttributeSyntax node)
    {
        return base.VisitXmlTextAttribute(node);
    }

    public override J? VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
    {
        return base.VisitXmlCrefAttribute(node);
    }

    public override J? VisitXmlNameAttribute(XmlNameAttributeSyntax node)
    {
        return base.VisitXmlNameAttribute(node);
    }

    public override J? VisitXmlText(XmlTextSyntax node)
    {
        return base.VisitXmlText(node);
    }

    public override J? VisitXmlCDataSection(XmlCDataSectionSyntax node)
    {
        return base.VisitXmlCDataSection(node);
    }

    public override J? VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
    {
        return base.VisitXmlProcessingInstruction(node);
    }

    public override J? VisitXmlComment(XmlCommentSyntax node)
    {
        return base.VisitXmlComment(node);
    }

    public override J? VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
    {
        return base.VisitIfDirectiveTrivia(node);
    }

    public override J? VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
    {
        return base.VisitElifDirectiveTrivia(node);
    }

    public override J? VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
    {
        return base.VisitElseDirectiveTrivia(node);
    }

    public override J? VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
    {
        return base.VisitEndIfDirectiveTrivia(node);
    }

    public override J? VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
    {
        return base.VisitRegionDirectiveTrivia(node);
    }

    public override J? VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
    {
        return base.VisitEndRegionDirectiveTrivia(node);
    }

    public override J? VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
    {
        return base.VisitErrorDirectiveTrivia(node);
    }

    public override J? VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
    {
        return base.VisitWarningDirectiveTrivia(node);
    }

    public override J? VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
    {
        return base.VisitBadDirectiveTrivia(node);
    }

    public override J? VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
    {
        return base.VisitDefineDirectiveTrivia(node);
    }

    public override J? VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
    {
        return base.VisitUndefDirectiveTrivia(node);
    }

    public override J? VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
    {
        return base.VisitLineDirectiveTrivia(node);
    }

    public override J? VisitLineDirectivePosition(LineDirectivePositionSyntax node)
    {
        return base.VisitLineDirectivePosition(node);
    }

    public override J? VisitLineSpanDirectiveTrivia(LineSpanDirectiveTriviaSyntax node)
    {
        return base.VisitLineSpanDirectiveTrivia(node);
    }

    public override J? VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
    {
        return base.VisitPragmaWarningDirectiveTrivia(node);
    }

    public override J? VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
    {
        return base.VisitPragmaChecksumDirectiveTrivia(node);
    }

    public override J? VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
    {
        return base.VisitReferenceDirectiveTrivia(node);
    }

    public override J? VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
    {
        return base.VisitLoadDirectiveTrivia(node);
    }

    public override J? VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
    {
        return base.VisitShebangDirectiveTrivia(node);
    }

    public override J? VisitNullableDirectiveTrivia(NullableDirectiveTriviaSyntax node)
    {
        return base.VisitNullableDirectiveTrivia(node);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    T? Convert<T>(SyntaxNode? node) where T : class, J
    {
        if (node == null) return default;

        var visit = Visit(node);
        if (typeof(T) == typeof(Expression) && visit is not Expression && visit is Statement statement)
        {
            return new Cs.StatementExpression(
                Core.Tree.RandomId(),
                Format(Leading(node)),
                Markers.EMPTY,
                statement
            ) as T;
        }

        return (T?)visit;
    }

    private JRightPadded<Statement> MapMemberDeclaration(MemberDeclarationSyntax m)
    {
        var memberDeclaration = (Visit(m) as Statement)!;
        var trailingSemicolon = m.GetLastToken().IsKind(SyntaxKind.SemicolonToken);
        if (trailingSemicolon && m is BaseTypeDeclarationSyntax { CloseBraceToken.Text: "}" } or
                NamespaceDeclarationSyntax { CloseBraceToken.Text: "}" })
        {
            return new JRightPadded<Statement>(
                memberDeclaration,
                Format(Leading(m.GetLastToken())),
                Markers.EMPTY.Add(new Semicolon(Core.Tree.RandomId()))
            );
        }

        return new JRightPadded<Statement>(memberDeclaration,
            trailingSemicolon ? Format(Leading(m.GetLastToken())) : Space.EMPTY,
            Markers.EMPTY
        );
    }

    private JContainer<T>? MapParameters<T>(ParameterListSyntax? pls) where T : J
    {
        return pls == null
            ? null
            : new JContainer<T>(
                Format(Leading(pls)),
                pls.Parameters.Count == 0
                    ?
                    [
                        JRightPadded<T>.Build(new J.Empty(Core.Tree.RandomId(),
                            Format(Leading(pls.CloseParenToken)),
                            Markers.EMPTY) as dynamic)
                    ]
                    : pls.Parameters.Select(MapParameter<T>).ToList(),
                Markers.EMPTY
            );
    }

    private JRightPadded<T> MapParameter<T>(ParameterSyntax tps) where T : J
    {
        var parameter = ((T?)Visit(tps))!;
        return new JRightPadded<T>(
            parameter,
            Format(Trailing(tps.Identifier)),
            Markers.EMPTY
        );
    }

    private JContainer<J.TypeParameter>? MapTypeParameters(TypeParameterListSyntax? tpls)
    {
        return tpls == null || tpls.Parameters.Count == 0
            ? null
            : JContainer<J.TypeParameter>.Build(Format(Leading(tpls)),
                tpls.Parameters.Select(MapTypeParameter).ToList(),
                Markers.EMPTY
            );
    }

    private JRightPadded<J.TypeParameter> MapTypeParameter(TypeParameterSyntax tps)
    {
        var typeParameter = (Visit(tps) as J.TypeParameter)!;
        return new JRightPadded<J.TypeParameter>(
            typeParameter,
            Format(Trailing(tps.Identifier)),
            Markers.EMPTY
        );
    }

    private static readonly IDictionary<SyntaxKind, J.Modifier.Type> AccessModifierMap =
        new Dictionary<SyntaxKind, J.Modifier.Type>
        {
            { SyntaxKind.PublicKeyword, J.Modifier.Type.Public },
            { SyntaxKind.PrivateKeyword, J.Modifier.Type.Private },
            { SyntaxKind.AbstractKeyword, J.Modifier.Type.Abstract },
            { SyntaxKind.ProtectedKeyword, J.Modifier.Type.Protected },
            { SyntaxKind.StaticKeyword, J.Modifier.Type.Static },
            { SyntaxKind.VolatileKeyword, J.Modifier.Type.Volatile },
            { SyntaxKind.SealedKeyword, J.Modifier.Type.Sealed },
            { SyntaxKind.AsyncKeyword, J.Modifier.Type.Async },
        };

    private IList<J.Modifier> MapModifiers(SyntaxTokenList stl)
    {
        return stl.Select(m => new J.Modifier(
            Core.Tree.RandomId(),
            Format(Leading(m)),
            Markers.EMPTY,
            AccessModifierMap.ContainsKey(m.Kind()) ? null : m.ToString(),
            AccessModifierMap.ContainsKey(m.Kind()) ? AccessModifierMap[m.Kind()] : J.Modifier.Type.LanguageExtension,
            []
        )).ToList();
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private List<Cs.AttributeList>? MapAttributes(SyntaxList<AttributeListSyntax> m)
    {
        return m.Count == 0 ? null : m.Select(x => Convert<Cs.AttributeList>(x)!).ToList();
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private JavaType MapType(ExpressionSyntax ins)
    {
        return _typeMapping.Type(semanticModel.GetTypeInfo(ins).Type);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private JavaType MapType(SyntaxNode ins)
    {
        return _typeMapping.Type(semanticModel.GetDeclaredSymbol(ins) ?? semanticModel.GetTypeInfo(ins).Type);
    }
// #if DEBUG_VISITOR
//     [DebuggerStepThrough]
// #endif
    private JRightPadded<Statement> MapStatement(StatementSyntax statementSyntax)
    {
        var statement = (Visit(statementSyntax) as Statement)!;
        // `J.Unknown` is a special case because it will already contain the semicolon
        var trailingSemicolon = statement is not J.Unknown &&
                                statementSyntax.GetLastToken().IsKind(SyntaxKind.SemicolonToken);
        return new JRightPadded<Statement>(
            statement,
            trailingSemicolon ? Format(Leading(statementSyntax.GetLastToken())) : Space.EMPTY,
            statementSyntax is LocalFunctionStatementSyntax
            {
                ExpressionBody: not null
            } // is a special case which returns MethodDeclaration which has to explicitly print colons at the end of an expression body
                ? Markers.EMPTY.Add(new Semicolon(Core.Tree.RandomId()))
                : Markers.EMPTY
        );
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList Leading<T>(SyntaxList<T> list) where T : SyntaxNode
    {
        return list.Count == 0 ? SyntaxTriviaList.Empty : Leading(list.First());
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList Leading(SyntaxNode node)
    {
        var firstToken = node.GetFirstToken();
        return Leading(firstToken);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList Leading(SyntaxToken token)
    {
        var previousToken = token.GetPreviousToken();
        var leading = token.LeadingTrivia;
        if (leading.Count == 0)
            return OnlyUnseenTrivia(previousToken.TrailingTrivia);
        var trailing = previousToken.TrailingTrivia;
        if (trailing.Count == 0)
            return OnlyUnseenTrivia(leading);
        return OnlyUnseenTrivia(trailing, leading);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList OnlyUnseenTrivia(SyntaxTriviaList trivia)
    {
        var span = trivia.Span;
        var idx = _seenTriviaSpans.BinarySearch(span);
        if (idx >= 0)
            return SyntaxTriviaList.Empty;
        idx = ~idx;
        if (idx > 0 && _seenTriviaSpans[idx - 1].End > span.Start)
            return SyntaxTriviaList.Empty;
        _seenTriviaSpans.Insert(idx, span);
        _seenTriviaSpans.Sort();
        return trivia;
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList OnlyUnseenTrivia(SyntaxTriviaList trivia1, SyntaxTriviaList trivia2)
    {
        var span = new TextSpan(trivia1.Span.Start, trivia2.Span.End - trivia1.Span.Start);
        var idx = _seenTriviaSpans.BinarySearch(span);
        if (idx >= 0)
            return SyntaxTriviaList.Empty;
        idx = ~idx;
        if (idx > 0 && _seenTriviaSpans[idx - 1].End > span.Start)
            return SyntaxTriviaList.Empty;
        _seenTriviaSpans.Insert(idx, span);
        _seenTriviaSpans.Sort();
        return trivia1.AddRange(trivia2);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList Trailing(SyntaxNode node)
    {
        return Trailing(node.GetLastToken());
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList Trailing(SyntaxToken token)
    {
        return Leading(token.GetNextToken());
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private static Space Format(SyntaxTriviaList trivia)
    {
        // FIXME optimize
        return Space.Format(trivia.ToString());
    }
}
