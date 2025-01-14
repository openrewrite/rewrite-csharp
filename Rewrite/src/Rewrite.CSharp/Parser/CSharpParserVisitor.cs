using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
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

[SuppressMessage(category: "ReSharper", checkId: "RedundantOverriddenMember")]
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
            id: Core.Tree.RandomId(),
            prefix: prefix,
            markers: Markers.EMPTY,
            sourcePath: semanticModel.SyntaxTree.FilePath,
            fileAttributes: null,
            charsetName: null,
            charsetBomMarked: false,
            checksum: null,
            externs: node.Externs.Select(selector: u => new JRightPadded<Cs.ExternAlias>(
                element: Convert<Cs.ExternAlias>(u)!,
                after: Format(Leading(u.SemicolonToken)),
                markers: Markers.EMPTY
            )).ToList(),
            usings: node.Usings.Select(selector: u => new JRightPadded<Cs.UsingDirective>(
                element: Convert<Cs.UsingDirective>(u)!,
                after: Format(Leading(u.SemicolonToken)),
                markers: Markers.EMPTY
            )).ToList(),
            attributeLists: node.AttributeLists.Select(selector: Convert<Cs.AttributeList>).ToList()!,
            members: node.Members.Select(selector: MapMemberDeclaration).ToList(),
            eof: Format(empty ? SyntaxTriviaList.Empty : Leading(node.EndOfFileToken))
        );
        return cu;
    }

    public override J.Unknown DefaultVisit(SyntaxNode node)
    {
        return new J.Unknown(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            unknownSource: new J.Unknown.Source(
                id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,
                markers: new Markers(Id: Core.Tree.RandomId(),
                    MarkerList:
                    [
                        ParseExceptionResult.Build(parser: parser, t: new InvalidOperationException(message: "Unsupported AST type."))
                            // .WithTreeType(node.Kind().ToString())
                            .WithTreeType(newTreeType: node.GetType().Name)
                    ]
                ),
                text: node.ToString()
            )
        );
    }

    public override Cs VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        return new Cs.FileScopeNamespaceDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            name: new JRightPadded<Expression>(
                element: Convert<Expression>(node.Name)!,
                after: Format(Trailing(node.Name)),
                markers: Markers.EMPTY
            ),
            externs: node.Externs.Select(selector: u => new JRightPadded<Cs.ExternAlias>(
                element: Convert<Cs.ExternAlias>(u)!,
                after: Format(Leading(u.SemicolonToken)),
                markers: Markers.EMPTY
            )).ToList(),
            usings: node.Usings.Select(selector: u => new JRightPadded<Cs.UsingDirective>(
                element: Convert<Cs.UsingDirective>(u)!,
                after: Format(Leading(u.SemicolonToken)),
                markers: Markers.EMPTY
            )).ToList(),
            members: node.Members.Select(selector: MapMemberDeclaration).ToList()
        );
    }

    public override J? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        return new Cs.BlockScopeNamespaceDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            name: new JRightPadded<Expression>(
                element: Convert<Expression>(node.Name)!,
                after: Format(Trailing(node.Name)),
                markers: Markers.EMPTY
            ),
            externs: node.Externs.Select(selector: u => new JRightPadded<Cs.ExternAlias>(
                element: Convert<Cs.ExternAlias>(u)!,
                after: Format(Leading(u.SemicolonToken)),
                markers: Markers.EMPTY
            )).ToList(),
            usings: node.Usings.Select(selector: u => new JRightPadded<Cs.UsingDirective>(
                element: Convert<Cs.UsingDirective>(u)!,
                after: Format(Leading(u.SemicolonToken)),
                markers: Markers.EMPTY
            )).ToList(),
            members: node.Members.Select(selector: MapMemberDeclaration).ToList(),
            end: Format(Leading(node.CloseBraceToken))
        );
    }

    public override J? VisitStructDeclaration(StructDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Value);
    }


    public override J? VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        var enumDeclaration = new Cs.EnumDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(node.AttributeLists),
            modifiers: MapModifiers(stl: node.Modifiers),
            name: JLeftPadded.Create(MapIdentifier(node.Identifier), Format(Leading(node.EnumKeyword))),
            baseType: ToLeftPadded<TypeTree>(node.BaseList?.Types[0]),
            members: ToJContainer<EnumMemberDeclarationSyntax, Expression>(node.Members, node.OpenBraceToken)
        );
        return enumDeclaration;
    }



    J.EnumValueSet MapEnumMembers(SeparatedSyntaxList<EnumMemberDeclarationSyntax> members)
    {
        return new J.EnumValueSet(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            enums: members.Select(selector: x => JRightPadded.Create(element: new J.EnumValue(
                id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,
                markers: Markers.EMPTY,
                annotations: [], // todo: figure out how to make c# attributes map here
                name: MapIdentifier(identifier: x.Identifier, type: MapType( x.Parent!)),
                initializer: null
            ), after: Format(Trailing(x)))).ToList(),
            terminatedWithSemicolon: false
        );
    }

    public override J? VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Record);
    }

    public override J? VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Interface);
    }

    public override J? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Class);
    }

    private Statement VisitTypeDeclaration(TypeDeclarationSyntax node, J.ClassDeclaration.Kind.Types type)
    {
        var attributeLists = MapAttributes(m: node.AttributeLists);
        var javaType = MapType( node);
        var hasBaseClass = node.BaseList is { Types.Count: > 0 } &&
                           semanticModel.GetTypeInfo(expression: node.BaseList.Types[index: 0].Type).Type?.TypeKind == TypeKind.Class;
        var hasBaseInterfaces = node.BaseList != null && node.BaseList.Types.Count > (hasBaseClass ? 1 : 0);

        var isEmptyBody = node.OpenBraceToken.IsKind(SyntaxKind.None);

        var classDeclaration = new Cs.ClassDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeList: attributeLists,
            modifiers: MapModifiers(stl: node.Modifiers),
            kind: new J.ClassDeclaration.Kind(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.Keyword)),
                markers: Markers.EMPTY,
                annotations: [],
                kindType: type
            ),
            name: MapIdentifier(node.Identifier),
            typeParameters: node.TypeParameterList != null ? ToJContainer<TypeParameterSyntax, Cs.TypeParameter>(node.TypeParameterList.Parameters, node.TypeParameterList.LessThanToken) : null,
            primaryConstructor: MapParameters<Statement>(pls: node.ParameterList),
            extendings: hasBaseClass
                ? new JLeftPadded<TypeTree>(before: Format(Leading(node.BaseList!)), element: (Visit(node.BaseList!.Types[index: 0]) as TypeTree)!, markers: Markers.EMPTY)
                : null,
            implementings: hasBaseInterfaces
                ? new JContainer<TypeTree>(before: hasBaseClass ? Space.EMPTY : Format(Leading(node.BaseList!)),
                    elements: node.BaseList!.Types.Skip(count: hasBaseClass ? 1 : 0)
                        .Select(selector: bts => new JRightPadded<TypeTree>(element: (Visit(bts) as TypeTree)!, after: Format(Trailing(bts)), markers: Markers.EMPTY))
                        .ToList(), markers: Markers.EMPTY)
                : null,
            body: new J.Block(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(isEmptyBody ? node.SemicolonToken : node.OpenBraceToken)),
                markers: isEmptyBody ? Markers.Create(markers: new OmitBraces()) : Markers.EMPTY,
                @static: JRightPadded.Create(element: false),
                statements: node.Members.Select(selector: MapMemberDeclaration).ToList(),
                end: Format(Leading(node.CloseBraceToken))
            ),
            type: javaType as JavaType.FullyQualified,
            typeParameterConstraintClauses: MapTypeParameterConstraintClauses(node.ConstraintClauses)
        );



        return classDeclaration;
    }

    /// <summary>
    /// Collection of "where T : class, ISomething"
    /// </summary>
    private JContainer<Cs.TypeParameterConstraintClause> MapTypeParameterConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> list)
    {
        return JContainer.Create(
                elements: list.Select(selector: x => JRightPadded.Create(element: Convert<Cs.TypeParameterConstraintClause>(x)!, after: Format(Trailing(x)))).ToList(),
                before: Format(Leading(list: list)),
                markers: Markers.EMPTY
            );
    }

    /// <summary>
    /// Single "where T : class, ISomething"
    /// </summary>
    public override Cs.TypeParameterConstraintClause? VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
    {
        return new Cs.TypeParameterConstraintClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeParameter: JRightPadded.Create(element: Convert<J.Identifier>(node.Name)!, after: Format(Trailing(node.Name))),
            typeParameterConstraints: MapTypeParameterConstraints(list: node.Constraints)
        );
    }

    /// <summary>
    /// Collection of comma separated constraints that appear after semicolon in an expression such as this: "where T : class, ISomething"
    /// </summary>
    private JContainer<Cs.TypeParameterConstraint> MapTypeParameterConstraints(SeparatedSyntaxList<TypeParameterConstraintSyntax> list)
    {
        return JContainer.Create(
                elements: list.Select(selector: MapTypeParameterConstraint).ToList(),
                before: Space.EMPTY,
                markers: Markers.EMPTY
            );

    }

    private JRightPadded<Cs.TypeParameterConstraint> MapTypeParameterConstraint(TypeParameterConstraintSyntax argument)
    {
        return JRightPadded.Create(
            element: Convert<Cs.TypeParameterConstraint>(argument)!,
            after: Format(Trailing(argument)),
            markers: Markers.EMPTY
        );
    }


    public override Statement VisitParameter(ParameterSyntax p)
    {
        var attributeLists = MapAttributes(m: p.AttributeLists);
        var javaType = MapType( p) as JavaType.Variable;
        var variableDeclarations = new J.VariableDeclarations(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(p)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: MapModifiers(stl: p.Modifiers),
            typeExpression: Convert<TypeTree>(p.Type),
            varargs: null!,
            dimensionsBeforeName: [],
            variables:
            [
                new JRightPadded<J.VariableDeclarations.NamedVariable>(
                    element: new J.VariableDeclarations.NamedVariable(
                        id: Core.Tree.RandomId(),
                        prefix: Format(Leading(p.Identifier)),
                        markers: Markers.EMPTY,
                        name: new J.Identifier(
                            id: Core.Tree.RandomId(),
                            prefix: Space.EMPTY,
                            markers: Markers.EMPTY,
                            annotations: [],
                            simpleName: p.Identifier.Text,
                            type: javaType?.Type,
                            fieldType: javaType
                        ),
                        dimensionsAfterName: [],
                        initializer: p.Default != null
                            ? new JLeftPadded<Expression>(
                                before: Format(Leading(p.Default)),
                                element: Convert<Expression>(p.Default.Value)!,
                                markers: Markers.EMPTY
                            )
                            : null,
                        variableType: javaType
                    ),
                    after: Format(Trailing(p.Identifier)),
                    markers: Markers.EMPTY
                )
            ]
        );
        return attributeLists.Count > 0
            ? new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(p)),
                markers: Markers.EMPTY,
                attributeLists: attributeLists,
                statement: variableDeclarations
            )
            : variableDeclarations;
    }

    public override J? VisitSimpleBaseType(SimpleBaseTypeSyntax node)
    {
        return new J.Identifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.Type)),
            markers: Markers.EMPTY,
            annotations: [],
            simpleName: node.ToString(),
            type: MapType( node),
            fieldType: null
        );
    }

    public override J? VisitPrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax node)
    {
        return new J.Identifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.Type)),
            markers: Markers.EMPTY,
            annotations: [],
            simpleName: node.ToString(),
            type: MapType( node),
            fieldType: null
        );
    }

    public override Cs.TypeParameter VisitTypeParameter(TypeParameterSyntax node)
    {
        return new Cs.TypeParameter(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(node.AttributeLists)!,
            variance: ToLeftPadded<Cs.TypeParameter.VarianceKind>(node.VarianceKeyword),
            name: MapIdentifier(node.Identifier)
        );
    }


    public override J.TypeParameters? VisitTypeParameterList(TypeParameterListSyntax node)
    {
        if (node.Parameters.Count == 0)
        {
            return null;
        }

        return new J.TypeParameters(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            annotations: [],
            parameters: node.Parameters.Select(selector: MapTypeParameter).ToList()
        );
    }

    public override J VisitIdentifierName(IdentifierNameSyntax node)
    {
        return new J.Identifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            annotations: [],
            simpleName: node.Identifier.Text,
            type: null, // FIXME type attribution
            fieldType: null // FIXME type attribution
        );
    }

    public override J? VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var prefix = Format(Leading(node));
        var select = Convert<Expression>(node.Expression);
        switch (select)
        {
            case J.FieldAccess fa:
            {
                return new J.MethodInvocation(
                    id: Core.Tree.RandomId(),
                    prefix: prefix,
                    markers: fa.Markers,
                    select: new JRightPadded<Expression>(element: fa.Target, after: fa.Padding.Name.Before, markers: Markers.EMPTY),
                    typeParameters: null,
                    name: fa.Name,
                    arguments: MapArgumentList(argumentList: node.ArgumentList),
                    methodType: MapType( node) as JavaType.Method
                );
            }
            case J.Identifier id:
                return new J.MethodInvocation(
                    id: Core.Tree.RandomId(),
                    prefix: prefix,
                    markers: Markers.EMPTY,
                    select: null,
                    typeParameters: null,
                    name: id,
                    arguments: MapArgumentList(argumentList: node.ArgumentList),
                    methodType: MapType( node) as JavaType.Method
                );
            case J.ParameterizedType pt:
                // return mi
                //     .WithPrefix(prefix)
                //     .Padding.WithArguments(MapArgumentList(node.ArgumentList))
                //     .WithMethodType(MapType(node) as JavaType.Method);

                return new J.MethodInvocation(
                    id: Core.Tree.RandomId(),
                    prefix: prefix,
                    markers: pt.Markers,
                    select: pt.Clazz is J.FieldAccess lfa
                        ? new JRightPadded<Expression>(element: lfa.Target, after: lfa.Padding.Name.Before, markers: Markers.EMPTY)
                        : null,
                    typeParameters: pt.TypeParameters != null
                        ? new JContainer<Expression>(
                            before: Space.EMPTY,
                            elements: pt.TypeParameters.Select(selector: JRightPadded.Create).ToList(),
                            markers: Markers.EMPTY
                        )
                        : null, // TODO: type parameters
                    name: pt.Clazz is J.Identifier i
                        ? i
                        : (pt.Clazz as J.FieldAccess)?.Name ??
                          MapIdentifier(identifier: node.Expression.GetFirstToken(), type: MapType( node.Expression)),
                    arguments: MapArgumentList(argumentList: node.ArgumentList),
                    methodType: MapType( node) as JavaType.Method
                );
            // chained method invocation (method returns a delegate). ex. Something()()
            case J.MethodInvocation or J.ArrayAccess:
                return new J.MethodInvocation(
                    id: Core.Tree.RandomId(),
                    prefix: prefix,
                    markers: Markers.EMPTY,
                    select: JRightPadded.Create(element: select),
                    typeParameters: null,
                    name: new J.Identifier(
                        id: Core.Tree.RandomId(),
                        prefix: Space.EMPTY,
                        markers: Markers.EMPTY,
                        annotations: new List<J.Annotation>(),
                        simpleName: "",
                        type: null,
                        fieldType: null),
                    arguments: MapArgumentList(argumentList: node.ArgumentList),
                    methodType: MapType( node) as JavaType.Method
                );
            case Cs.AliasQualifiedName alias:
                return new Cs.AliasQualifiedName(
                    id: Core.Tree.RandomId(),
                    prefix: prefix,
                    markers: Markers.EMPTY,
                    alias: alias.Padding.Alias,
                    name: new J.MethodInvocation(
                        id: Core.Tree.RandomId(),
                        prefix: Space.EMPTY,
                        markers: Markers.EMPTY,
                        select: null,
                        typeParameters: null,
                        name: (J.Identifier)alias.Name,
                        arguments: MapArgumentList(argumentList: node.ArgumentList),
                        methodType: MapType(node) as JavaType.Method
                    ));
        }

        for (var index = 0; index < node.ArgumentList.Arguments.Count; index++)
        {
            var arg = node.ArgumentList.Arguments[index: index];
            var typeInfo = semanticModel.GetTypeInfo(expression: arg.Expression);
        }

        return base.VisitInvocationExpression(node);
    }

    private JContainer<Expression> MapArgumentList(ArgumentListSyntax argumentList)
    {
        return new JContainer<Expression>(
            before: Format(Leading(argumentList.OpenParenToken)),
            elements: argumentList.Arguments.Select(selector: MapArgument).ToList(),
            markers: Markers.EMPTY
        );
    }

    private JRightPadded<Expression> MapArgument(ArgumentSyntax argument)
    {
        return new JRightPadded<Expression>(
            element: Convert<Expression>(argument)!,
            after: Format(Trailing(argument)),
            markers: Markers.EMPTY
        );
    }


    public override J? VisitAttribute(AttributeSyntax node)
    {
        return new J.Annotation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            annotationType: Convert<NameTree>(node.Name)!,
            arguments: node.ArgumentList != null ? ToJContainer<AttributeArgumentSyntax, Expression>(node.ArgumentList!.Arguments, node.ArgumentList!.OpenParenToken) : null
        );
    }

    public override J? VisitAttributeList(AttributeListSyntax node)
    {
        return new Cs.AttributeList(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            target: node.Target != null
                ? new JRightPadded<J.Identifier>(
                    element: Convert<J.Identifier>(node.Target)!,
                    after: Format(Leading(node.Target.ColonToken)),
                    markers: Markers.EMPTY
                )
                : null,
            attributes: node.Attributes.Select(selector: a =>
            {
                var trailingComma = a == node.Attributes.Last() &&
                                    a.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
                return new JRightPadded<J.Annotation>(
                    element: Convert<J.Annotation>(a)!,
                    after: Format(Trailing(a)),
                    markers: trailingComma
                        ? Markers.EMPTY.Add(marker: new TrailingComma(Id: Core.Tree.RandomId(),
                            Suffix: Format(Trailing(a.GetLastToken().GetNextToken()))))
                        : Markers.EMPTY
                );
            }).ToList()
        );
    }


    /// <summary>
    /// Converts either a block (MyMethod {}), arrow expression clause (MyMethod () =&lt; x) or no-body construct (MyMethod();) into a J.Block
    /// </summary>
    private J.Block MapBody(BlockSyntax? block, ArrowExpressionClauseSyntax? expressionBody, SyntaxToken semicolonToken)
    {

        J.Block body;
        if (block != null)
        {
            body = Convert<J.Block>(block)!;
        }
        else if (expressionBody != null)
        {

            body = new J.Block(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(expressionBody)),
                markers: Markers.Create(new SingleExpressionBlock(Core.Tree.RandomId())),
                @static: JRightPadded.Create(false),
                // statements: [JRightPadded.Create((Statement)arrowExpressionClause.Expression, arrowExpressionClause.Padding.Expression.After)],
                statements: [MapExpressionStatement(expressionBody.Expression)],
                end: Space.EMPTY
            );
            // body = Convert<Cs.ArrowExpressionClause>(expressionBody)!;
        }
        else if(semicolonToken.IsPresent())
        {
            body = new J.Block(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(semicolonToken)),
                markers: Markers.Create(markers: new OmitBraces()),
                @static: JRightPadded.Create(element: false),
                statements: new List<JRightPadded<Statement>>(),
                end: Format(Trailing(semicolonToken)));
        }
        else
        {
            throw new InvalidOperationException("Block or expression body or semicolon token must be set");
        }

        return body;
    }
    public override J? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        Statement body = MapBody(node.Body, node.ExpressionBody, node.SemicolonToken);
        Statement returnValue = new Cs.MethodDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            modifiers: MapModifiers(stl: node.Modifiers),
            typeParameters: MapTypeParameters(node.TypeParameterList),
            returnTypeExpression: Convert<TypeTree>(node.ReturnType)!,
            explicitInterfaceSpecifier: ToRightPadded<TypeTree>(node.ExplicitInterfaceSpecifier),
            name: MapIdentifier(node.Identifier, null),
            parameters: MapParameters<Statement>(pls: node.ParameterList)!,
            body: body,
            methodType: MapType( node) as JavaType.Method,
            attributes: MapAttributes(node.AttributeLists),
            typeParameterConstraintClauses: MapTypeParameterConstraintClauses(node.ConstraintClauses)
        );

        return returnValue;
    }

    private JContainer<Cs.TypeParameter>? MapTypeParameters(TypeParameterListSyntax? parameters)
    {
        return parameters != null ? ToJContainer<TypeParameterSyntax, Cs.TypeParameter>(parameters.Parameters, parameters.GreaterThanToken) : null;
    }

    public override J? VisitUsingDirective(UsingDirectiveSyntax node)
    {
        var global = node.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword);
        var @static = node.StaticKeyword.IsKind(SyntaxKind.StaticKeyword);
        var @unsafe = node.UnsafeKeyword.IsKind(SyntaxKind.UnsafeKeyword);

        return new Cs.UsingDirective(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            global: new JRightPadded<bool>(element: global, after: global ? Format(Trailing(node.GlobalKeyword)) : Space.EMPTY, markers: Markers.EMPTY),
            @static: new JLeftPadded<bool>(before: @static ? Format(Leading(node.StaticKeyword)) : Space.EMPTY, element: @static, markers: Markers.EMPTY),
            @unsafe: new JLeftPadded<bool>(before: @unsafe ? Format(Leading(node.UnsafeKeyword)) : Space.EMPTY, element: @unsafe, markers: Markers.EMPTY),
            alias: node.Alias != null
                ? new JRightPadded<J.Identifier>(
                    element: Convert<J.Identifier>(node.Alias.Name)!,
                    after: Format(Leading(node.Alias.EqualsToken)),
                    markers: Markers.EMPTY
                )
                : null,
            namespaceOrType: Convert<TypeTree>(node.NamespaceOrType)!
        );
    }

    public override J? VisitNullableType(NullableTypeSyntax node)
    {
        return new J.NullableType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            annotations: [],
            typeTree: new JRightPadded<TypeTree>(
                element: Convert<TypeTree>(node.ElementType)!,
                after: Format(Leading(node.QuestionToken)),
                markers: Markers.EMPTY
            )
        );
    }

    public override J? VisitArgument(ArgumentSyntax node)
    {
        // if (node.NameColon == null && node.RefKindKeyword.IsKind(SyntaxKind.None))
        // {
        //     return Convert<Expression>(node.Expression);
        // }
        // else
        // {
            var nameColumn = node.NameColon != null
                ? new JRightPadded<J.Identifier>(
                    element: MapIdentifier(identifier: node.NameColon.Name.Identifier, type: null),
                    after: Format(Trailing(node.NameColon.Name)), markers: Markers.EMPTY)
                : null;
            return new Cs.Argument(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                nameColumn: nameColumn,
                refKindKeyword: VisitKeyword(node.RefKindKeyword),
                expression: Convert<Expression>(node.Expression)!
                );
        // }
    }

    public override J.Block VisitBlock(BlockSyntax node)
    {
        return new J.Block(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: node.OpenBraceToken.IsKind(SyntaxKind.None)
                ? Markers.EMPTY.Add(marker: new OmitBraces(Id: Core.Tree.RandomId()))
                : Markers.EMPTY,
            @static: JRightPadded.Create(element: false),
            statements: node.Statements.Select(selector: MapStatement).ToList(),
            end: Format(Leading(node.CloseBraceToken))
        );
    }



    public override J? VisitInterpolation(InterpolationSyntax node)
    {
        // This was added in C# 6.0
        return new Cs.Interpolation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: new JRightPadded<Expression>(
                element: Convert<Expression>(node.Expression)!,
                after: Format(Trailing(node.Expression)),
                markers: Markers.EMPTY
            ),
            alignment: node.AlignmentClause != null
                ? new JRightPadded<Expression>(
                    element: Convert<Expression>(node.AlignmentClause)!,
                    after: Format(Trailing(node.AlignmentClause.Value)),
                    markers: Markers.EMPTY
                )
                : null,
            format: node.FormatClause != null
                ? new JRightPadded<Expression>(
                    element: Convert<Expression>(node.FormatClause)!,
                    after: Format(Trailing(node.FormatClause.FormatStringToken)),
                    markers: Markers.EMPTY
                )
                : null
        );
    }

    public override J? VisitOrdering(OrderingSyntax node)
    {
        Cs.Ordering.DirectionKind? direction = null;
        if (Enum.TryParse(node.AscendingOrDescendingKeyword.ToString(), true, out Cs.Ordering.DirectionKind outVal))
        {
            direction = outVal;
        }
        return new Cs.Ordering(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToRightPadded<Expression>(node.Expression)!,
            direction: direction);
    }

    public override J? VisitSubpattern(SubpatternSyntax node)
    {
        // This was added in C# 8.0
        return new Cs.Subpattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            name: Convert<Expression>(node.ExpressionColon?.Expression),
            pattern: JLeftPadded.Create(Convert<Cs.Pattern>(node.Pattern)!, Format(Leading(node.Pattern)))
        );
    }

    public override J? VisitAccessorDeclaration(AccessorDeclarationSyntax node)
    {
        return new Cs.AccessorDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY.Add(marker: new CompactConstructor(Id: Core.Tree.RandomId())),
            attributes: MapAttributes(node.AttributeLists),
            modifiers: MapModifiers(stl: node.Modifiers),
            expressionBody: Convert<Cs.ArrowExpressionClause>(node.ExpressionBody),
            kind: ToLeftPadded<Cs.AccessorDeclaration.AccessorKinds>(node.Keyword)!,
            body: Convert<J.Block>(node.Body)
        );
        // var javaType = MapType( node);
        // return new Cs.MethodDeclaration(
        //     id: Core.Tree.RandomId(),
        //     prefix: Format(Leading(node)),
        //     markers: Markers.EMPTY.Add(marker: new CompactConstructor(Id: Core.Tree.RandomId())),
        //     attributes: MapAttributes(node.AttributeLists),
        //     modifiers: MapModifiers(stl: node.Modifiers),
        //     typeParameters: null,
        //     returnTypeExpression: null!,
        //     explicitInterfaceSpecifier: null,
        //     name: MapIdentifier(identifier: node.Keyword, type: javaType),
        //     parameters: new JContainer<Statement>(
        //         before: Space.EMPTY,
        //         elements: [],
        //         markers: Markers.EMPTY
        //     ),
        //     body: node.ExpressionBody != null ? Convert<J.Block>(node.ExpressionBody) : node.Body != null ? Convert<J.Block>(node.Body) : null,
        //     methodType: javaType as JavaType.Method,
        //     typeParameterConstraintClauses: JContainer<Cs.TypeParameterConstraintClause>.Empty()
        // );
    }

    public override J.Block VisitAccessorList(AccessorListSyntax node)
    {
        return new J.Block(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.OpenBraceToken)),
            markers: Markers.EMPTY,
            @static: JRightPadded.Create(element: false),
            statements: node.Accessors.Select(selector: MapAccessor).ToList(),
            end: Format(Leading(node.CloseBraceToken))
        );
    }

    private JRightPadded<Statement> MapAccessor(AccessorDeclarationSyntax accessorDeclarationSyntax)
    {
        var accessor = Convert<Cs.AccessorDeclaration>(accessorDeclarationSyntax)!;
        var trailingSemicolon = accessorDeclarationSyntax.GetLastToken().IsKind(SyntaxKind.SemicolonToken);
        return new JRightPadded<Statement>(
            element: accessor,
            after: trailingSemicolon ? Format(Leading(accessorDeclarationSyntax.SemicolonToken)) : Space.EMPTY,
            markers: Markers.EMPTY
        );
    }

    public override J? VisitArgumentList(ArgumentListSyntax node)
    {
        return base.VisitArgumentList(node);
    }

    public override J? VisitArrayType(ArrayTypeSyntax node)
    {
        return new Cs.ArrayType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(node.ElementType),
            dimensions: node.RankSpecifiers.Select(selector: r => Convert<J.ArrayDimension>(r)!).ToList(),
            type: MapType( node)
        );
        // return MapArrayType(node, rank: 0);
    }

    private J.ArrayType MapArrayType(ArrayTypeSyntax node, int rank)
    {
        return new J.ArrayType(
            id: Core.Tree.RandomId(),
            prefix: rank == 0 ? Format(Leading(node)) : Space.EMPTY,
            markers: Markers.EMPTY,
            elementType: rank == node.RankSpecifiers.Count - 1 ? Convert<TypeTree>(node.ElementType)! : MapArrayType(node, rank: rank + 1),
            annotations: [], // no attributes on type use
            dimension: new JLeftPadded<Space>(
                before: Format(Leading(node.RankSpecifiers[index: rank])),
                element: Format(Trailing(node.RankSpecifiers[index: rank].OpenBracketToken)),
                markers: Markers.EMPTY),
            type: MapType( node)
        );
    }

    public override J? VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.SimpleAssignmentExpression))
        {
            return new J.Assignment(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                variable: Convert<Expression>(node.Left)!,
                expression: new JLeftPadded<Expression>(
                    before: Format(Leading(node.OperatorToken)),
                    element: Convert<Expression>(node.Right)!,
                    markers: Markers.EMPTY),
                type: MapType( node)
            );
        }

        if (node.OperatorToken.IsKind(SyntaxKind.QuestionQuestionEqualsToken))
        {
            return new Cs.AssignmentOperation(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                variable: Convert<Expression>(node.Left)!,
                @operator: new JLeftPadded<Cs.AssignmentOperation.OperatorType>(
                    before: Format(Leading(node.OperatorToken)),
                    element: Cs.AssignmentOperation.OperatorType.NullCoalescing,
                    markers: Markers.EMPTY),
                assignment: Convert<Expression>(node.Right)!,
                type: MapType( node)
            );
        }

        return new J.AssignmentOperation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            variable: Convert<Expression>(node.Left)!,
            @operator: new JLeftPadded<J.AssignmentOperation.Types>(
                before: Format(Leading(node.OperatorToken)),
                element: MapAssignmentOperator(op: node.OperatorToken),
                markers: Markers.EMPTY),
            assignment: Convert<Expression>(node.Right)!,
            type: MapType( node)
        );
    }

    private J.AssignmentOperation.Types MapAssignmentOperator(SyntaxToken op)
    {
        return op.Kind() switch
        {
            SyntaxKind.PlusEqualsToken => J.AssignmentOperation.Types.Addition,
            SyntaxKind.MinusEqualsToken => J.AssignmentOperation.Types.Subtraction,
            SyntaxKind.AsteriskEqualsToken => J.AssignmentOperation.Types.Multiplication,
            SyntaxKind.SlashEqualsToken => J.AssignmentOperation.Types.Division,
            SyntaxKind.PercentEqualsToken => J.AssignmentOperation.Types.Modulo,
            SyntaxKind.AmpersandEqualsToken => J.AssignmentOperation.Types.BitAnd,
            SyntaxKind.BarEqualsToken => J.AssignmentOperation.Types.BitOr,
            SyntaxKind.CaretEqualsToken => J.AssignmentOperation.Types.BitXor,
            SyntaxKind.LessThanLessThanEqualsToken => J.AssignmentOperation.Types.LeftShift,
            SyntaxKind.GreaterThanGreaterThanEqualsToken => J.AssignmentOperation.Types.RightShift,
            SyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken => J.AssignmentOperation.Types.UnsignedRightShift,
            // TODO the `??=` operator will require a custom LST element
            // SyntaxKind.QuestionQuestionEqualsToken => ???,
            _ => throw new NotSupportedException(message: op.Kind().ToString())
        };
    }

    public override J? VisitAttributeArgument(AttributeArgumentSyntax node)
    {
        return base.VisitAttributeArgument(node);
    }

    public override J? VisitAwaitExpression(AwaitExpressionSyntax node)
    {
        return new Cs.AwaitExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!,
            type: MapType( node)
        );
    }

    public override J? VisitBaseExpression(BaseExpressionSyntax node)
    {
        return MapIdentifier(identifier: node.Token, type: null);
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
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                left: Convert<Expression>(node.Left)!,
                @operator: new JLeftPadded<Cs.Binary.OperatorType>(
                    before: Format(Leading(node.OperatorToken)),
                    element: node.OperatorToken.Kind() switch
                    {
                        SyntaxKind.QuestionQuestionToken => Cs.Binary.OperatorType.NullCoalescing,
                        SyntaxKind.AsKeyword => Cs.Binary.OperatorType.As,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    markers: Markers.EMPTY),
                right: Convert<Expression>(node.Right)!,
                type: MapType( node)
            );
        }

        if (node.OperatorToken.IsKind(SyntaxKind.IsKeyword))
        {
            return new J.InstanceOf(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                expression: new JRightPadded<Expression>(
                    element: Convert<Expression>(node.Left)!,
                    after: Format(Trailing(node.Left)),
                    markers: Markers.EMPTY),
                clazz: Convert<TypeTree>(node.Right)!,
                pattern: null,
                type: MapType( node)
            );
        }

        return new J.Binary(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            left: Convert<Expression>(node.Left)!,
            @operator: new JLeftPadded<J.Binary.Types>(
                before: Format(Leading(node.OperatorToken)),
                element: MapBinaryExpressionOperator(operatorToken: node.OperatorToken),
                markers: Markers.EMPTY),
            right: Convert<Expression>(node.Right)!,
            type: MapType( node)
        );
    }

    private static J.Binary.Types MapBinaryExpressionOperator(SyntaxToken operatorToken)
    {
        return operatorToken.Kind() switch
        {
            SyntaxKind.PlusToken => J.Binary.Types.Addition,
            SyntaxKind.MinusToken => J.Binary.Types.Subtraction,
            SyntaxKind.AsteriskToken => J.Binary.Types.Multiplication,
            SyntaxKind.SlashToken => J.Binary.Types.Division,
            SyntaxKind.PercentToken => J.Binary.Types.Modulo,
            SyntaxKind.AmpersandToken => J.Binary.Types.BitAnd,
            SyntaxKind.BarToken => J.Binary.Types.BitOr,
            SyntaxKind.CaretToken => J.Binary.Types.BitXor,
            SyntaxKind.AmpersandAmpersandToken => J.Binary.Types.And,
            SyntaxKind.BarBarToken => J.Binary.Types.Or,
            SyntaxKind.LessThanLessThanToken => J.Binary.Types.LeftShift,
            SyntaxKind.GreaterThanGreaterThanToken => J.Binary.Types.RightShift,
            SyntaxKind.GreaterThanGreaterThanGreaterThanToken => J.Binary.Types.UnsignedRightShift,
            SyntaxKind.LessThanToken => J.Binary.Types.LessThan,
            SyntaxKind.GreaterThanToken => J.Binary.Types.GreaterThan,
            SyntaxKind.LessThanEqualsToken => J.Binary.Types.LessThanOrEqual,
            SyntaxKind.GreaterThanEqualsToken => J.Binary.Types.GreaterThanOrEqual,
            SyntaxKind.EqualsEqualsToken => J.Binary.Types.Equal,
            SyntaxKind.ExclamationEqualsToken => J.Binary.Types.NotEqual,
            _ => throw new NotImplementedException(message: operatorToken.Kind().ToString())
        };
    }

    public override J? VisitBinaryPattern(BinaryPatternSyntax node)
    {
        var @operator = node.OperatorToken.Kind() switch
        {
            SyntaxKind.OrKeyword => Cs.BinaryPattern.OperatorType.Or,
            SyntaxKind.AndKeyword => Cs.BinaryPattern.OperatorType.And,
            _ => throw new ArgumentOutOfRangeException()
        };
        return new Cs.BinaryPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            left: Convert<Cs.Pattern>(node.Left)!,
            @operator: JLeftPadded.Create(@operator, Format(Leading(node.OperatorToken))),
            right: Convert<Cs.Pattern>(node.Right)!);
    }

    public override J? VisitBreakStatement(BreakStatementSyntax node)
    {
        return new J.Break(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            label: null
        );
    }

    public override J? VisitCastExpression(CastExpressionSyntax node)
    {
        return new J.TypeCast(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            clazz: new J.ControlParentheses<TypeTree>(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                tree: new JRightPadded<TypeTree>(
                    element: Convert<TypeTree>(node.Type)!,
                    after: Format(Leading(node.CloseParenToken)),
                    markers: Markers.EMPTY
                )
            ),
            expression: Convert<Expression>(node.Expression)!
        );
    }

    public override J? VisitCatchClause(CatchClauseSyntax node)
    {
        var result =  new Cs.Try.Catch(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            parameter: new J.ControlParentheses<J.VariableDeclarations>(
                id: Core.Tree.RandomId(),
                prefix: node.Declaration != null ? Format(Leading(node.Declaration)) : Space.EMPTY,
                markers: Markers.EMPTY,
                tree: new JRightPadded<J.VariableDeclarations>(
                    element: node.Declaration != null
                        ? Convert<J.VariableDeclarations>(node.Declaration)!
                        : new J.VariableDeclarations(
                            id: Core.Tree.RandomId(),
                            prefix: Space.EMPTY,
                            markers: Markers.EMPTY,
                            leadingAnnotations: [],
                            modifiers: [],
                            typeExpression: null,
                            varargs: null!,
                            dimensionsBeforeName: [],
                            variables: []
                        ),
                    after: node.Declaration != null ? Format(Leading(node.Declaration.CloseParenToken)) : Space.EMPTY,
                    markers: Markers.EMPTY
                )
            ),
            body: Convert<J.Block>(node.Block)!,
            filterExpression: node.Filter != null ? JLeftPadded.Create( Convert<J.ControlParentheses<Expression>>(node.Filter)!, Format(Leading(node.Filter!.WhenKeyword))) : null
        );
        return result;
    }

    public override J? VisitCatchDeclaration(CatchDeclarationSyntax node)
    {
        return new J.VariableDeclarations(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: [],
            typeExpression: Convert<TypeTree>(node.Type),
            varargs: null!,
            dimensionsBeforeName: [],
            variables: node.Identifier.IsKind(SyntaxKind.None)
                ? []
                :
                [
                    new JRightPadded<J.VariableDeclarations.NamedVariable>(
                        element: new J.VariableDeclarations.NamedVariable(
                            id: Core.Tree.RandomId(),
                            prefix: Format(Leading(node.Identifier)),
                            markers: Markers.EMPTY,
                            name: MapIdentifier(identifier: node.Identifier, type: MapType( node.Type))!,
                            dimensionsAfterName: [],
                            initializer: null,
                            variableType: MapType( node) as JavaType.Variable
                        ),
                        after: Format(Leading(node.CloseParenToken)),
                        markers: Markers.EMPTY
                    )
                ]
        );
    }

    public override J? VisitCheckedExpression(CheckedExpressionSyntax node)
    {
        return new Cs.CheckedExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            checkedOrUncheckedKeyword: MapKeyword(node.Keyword)!,
            expression: ToControlParentheses<Expression>(node.Expression, node.OpenParenToken));
    }

    public override J? VisitQualifiedName(QualifiedNameSyntax node)
    {
        if (node.Right is GenericNameSyntax genericNameSyntax)
        {
            var mapIdentifier = MapIdentifier(identifier: genericNameSyntax.Identifier, type: MapType( genericNameSyntax));
            return new J.ParameterizedType(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                clazz: new J.FieldAccess(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    target: (Visit(node.Left) as Expression)!,
                    name: new JLeftPadded<J.Identifier>(
                        before: Format(Leading(node.DotToken)),
                        element: mapIdentifier,
                        markers: Markers.EMPTY
                    ),
                    type: MapType( node.Left)
                ),
                typeParameters: MapTypeArguments(typeArgumentList: genericNameSyntax.TypeArgumentList),
                type: mapIdentifier.Type
            );
        }

        return new J.FieldAccess(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            target: (Visit(node.Left) as Expression)!,
            name: new JLeftPadded<J.Identifier>(
                before: Format(Leading(node.DotToken)),
                element: Convert<J.Identifier>(node.Right)!,
                markers: Markers.EMPTY
            ),
            type: MapType( node)
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
        var nameTree = MapIdentifier(identifier: node.Identifier, type: MapType( node));
        return new J.ParameterizedType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            clazz: nameTree,
            typeParameters: MapTypeArguments(typeArgumentList: node.TypeArgumentList),
            type: nameTree.Type
        );
    }

    private JContainer<Expression>? MapTypeArguments(TypeArgumentListSyntax typeArgumentList)
    {
        if (typeArgumentList.Arguments.Count == 0) return null;

        return new JContainer<Expression>(
            before: Format(Leading(typeArgumentList)),
            elements: typeArgumentList.Arguments.Select(selector: t => new JRightPadded<Expression>(
                element: Convert<Expression>(t)!,
                after: Format(Trailing(t)),
                markers: Markers.EMPTY)
            ).ToList(),
            markers: Markers.EMPTY
        );
    }

    private J.Identifier MapIdentifier(SyntaxToken identifier)
    {
        var type = identifier.Parent != null ? MapType(identifier.Parent) : null;
        return MapIdentifier(identifier, type);
    }
    private J.Identifier MapIdentifier(SyntaxToken identifier, JavaType? type)
    {
        var variable = type as JavaType.Variable;
        return new J.Identifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(identifier)),
            markers: Markers.EMPTY,
            annotations: [],
            simpleName: identifier.Text,
            type: variable?.Type ?? type,
            fieldType: variable
        );
    }

    public override J? VisitTypeArgumentList(TypeArgumentListSyntax node)
    {
        return base.VisitTypeArgumentList(node);
    }

    public override J? VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
    {
        return new Cs.AliasQualifiedName(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            alias: ToRightPadded<J.Identifier>(node.Alias)!,
            name: Convert<Expression>(node.Name)!
        );
    }

    public override J? VisitPredefinedType(PredefinedTypeSyntax node)
    {
        return MapTypeTree(node);
    }

    public TypeTree MapTypeTree(SyntaxNode node)
    {
        var type = MapType( node);
        if (type is JavaType.Primitive)
            return new J.Primitive(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                type: (JavaType.Primitive)type
            );
        // TODO also for types like `sbyte` we need to use J.Identifier or a custom `Cs.Primitive`
        return new J.Identifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            annotations: [],
            simpleName: node.ToString(),
            type: type is JavaType.Variable variable ? variable.Type : type,
            fieldType: MapType( node) as JavaType.Variable
        );
    }

    public override J? VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
    {
        return new J.ArrayDimension(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            index: new JRightPadded<Expression>(
                element: new Cs.ArrayRankSpecifier(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Trailing(node.OpenBracketToken)),
                    markers: Markers.EMPTY,
                    sizes: new JContainer<Expression>(
                        before: Format(Leading(node.OpenBracketToken)),
                        elements: node.Sizes.Select(selector: t => new JRightPadded<Expression>(
                            element: Convert<Expression>(t)!,
                            after: Format(Trailing(t.GetLastToken())), markers: Markers.EMPTY)).ToList(),
                        markers: Markers.EMPTY
                    )
                ),
                after: Space.EMPTY,
                markers: Markers.EMPTY
            )
        );
    }

    public override J? VisitPointerType(PointerTypeSyntax node)
    {
        return new Cs.PointerType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            elementType: JRightPadded.Create(
                element: MapTypeTree(node.ElementType),
                after: Format(Trailing(node.ElementType))
            ));

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
        return new Cs.TupleType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            elements: JContainer.Create(elements: node.Elements.Select(selector: x => JRightPadded.Create(element: Convert<Cs.TupleElement>(x)!, after: Format(Trailing(x)))).ToList()),
            type: MapType( node)
            );
    }

    public override Cs VisitTupleElement(TupleElementSyntax node)
    {
        var type = MapType( node.Type);
        return new Cs.TupleElement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            type: Convert<TypeTree>(node.Type)!,
            name: MapIdentifier(identifier: node.Identifier, type: type));

    }

    public override J? VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
    {
        return null; // not special representation for generic type definition without type parameters. Ex: List<>
    }

    public override J? VisitRefType(RefTypeSyntax node)
    {
        return new Cs.RefType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            readonlyKeyword: MapModifier(node.ReadOnlyKeyword),
            typeIdentifier: Convert<TypeTree>(node.Type)!,
            type: MapType(node.Type));
    }

    public override J? VisitScopedType(ScopedTypeSyntax node)
    {
        return base.VisitScopedType(node);
    }

    public override J? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
    {
        return new J.Parentheses<Expression>(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            tree: MapExpression(es: node.Expression)
        );
    }

    public override J? VisitTupleExpression(TupleExpressionSyntax node)
    {
        return new Cs.TupleExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.OpenParenToken)),
            markers: Markers.EMPTY,
            arguments: ToJContainer<ArgumentSyntax, Cs.Argument>(syntaxList: node.Arguments, openingToken: node.OpenParenToken));
    }

    public override J? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
    {

        try
        {
            Cs.Unary.Types? operatorType = node.OperatorToken.Kind() switch
            {
                SyntaxKind.CaretToken => Cs.Unary.Types.FromEnd, // [^3]
                SyntaxKind.AsteriskToken => Cs.Unary.Types.PointerIndirection, // *myvar
                SyntaxKind.AmpersandToken => Cs.Unary.Types.AddressOf, // &myvar
                _ => null,
            };
            if (operatorType != null)
            {
                return new Cs.Unary(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    @operator: new JLeftPadded<Cs.Unary.Types>(
                        before: Format(Leading(node.OperatorToken)),
                        element: operatorType.Value,
                        markers: Markers.EMPTY),
                    expression: Convert<Expression>(node.Operand)!,
                    type: MapType( node)
                );
            }
            return new J.Unary(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                @operator: MapPrefixUnaryOperator(operatorToken: node.OperatorToken),
                expression: Convert<Expression>(node.Operand)!,
                type: MapType( node)
            );
        }
        catch (NotImplementedException)
        {
            // todo: temporary stopgap until all cases can be covered. known gaps are pointers (*something), and "from-end" [^3]
            return base.VisitPrefixUnaryExpression(node);
        }

    }

    private JLeftPadded<J.Unary.Types> MapPrefixUnaryOperator(SyntaxToken operatorToken)
    {
        return operatorToken.Kind() switch
        {
            SyntaxKind.ExclamationToken => new JLeftPadded<J.Unary.Types>(
                before: Format(Leading(operatorToken)),
                element: J.Unary.Types.Not,
                markers: Markers.EMPTY
            ),
            SyntaxKind.PlusPlusToken => new JLeftPadded<J.Unary.Types>(
                before: Format(Leading(operatorToken)),
                element: J.Unary.Types.PreIncrement,
                markers: Markers.EMPTY
            ),
            SyntaxKind.MinusMinusToken => new JLeftPadded<J.Unary.Types>(
                before: Format(Leading(operatorToken)),
                element: J.Unary.Types.PreDecrement,
                markers: Markers.EMPTY
            ),
            SyntaxKind.MinusToken => new JLeftPadded<J.Unary.Types>(
                before: Format(Leading(operatorToken)),
                element: J.Unary.Types.Negative,
                markers: Markers.EMPTY
            ),
            SyntaxKind.PlusToken => new JLeftPadded<J.Unary.Types>(
                before: Format(Leading(operatorToken)),
                element: J.Unary.Types.Positive,
                markers: Markers.EMPTY
            ),
            SyntaxKind.TildeToken => new JLeftPadded<J.Unary.Types>(
                before: Format(Leading(operatorToken)),
                element: J.Unary.Types.Complement,
                markers: Markers.EMPTY
            ),
            _ => throw new NotImplementedException(message: operatorToken.ToString())
        };
    }

    public override J? VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
    {
        var operatorEnum = MapPostfixUnaryOperator(operatorToken: node.OperatorToken);
        if (operatorEnum is J.Unary.Types jOperator)
        {
            return new J.Unary(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                @operator: new JLeftPadded<J.Unary.Types>(
                    before: Format(Leading(node.OperatorToken)),
                    element: jOperator,
                    markers: Markers.EMPTY
                ),
                expression: Convert<Expression>(node.Operand)!,
                type: MapType( node));
        }
        else
        {
            return new Cs.Unary(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                @operator: JLeftPadded.Create(
                    element: (Cs.Unary.Types)operatorEnum,
                    before: Format(Leading(node.OperatorToken)),
                    markers: Markers.EMPTY
                ),
                expression: Convert<Expression>(node.Operand)!,
                type: MapType( node));
        }
    }

    private Enum MapPostfixUnaryOperator(SyntaxToken operatorToken)
    {
        Enum type = operatorToken.Kind() switch
        {
            SyntaxKind.PlusPlusToken => J.Unary.Types.PostIncrement,
            SyntaxKind.MinusMinusToken => J.Unary.Types.PostDecrement,
            SyntaxKind.ExclamationToken => Cs.Unary.Types.SuppressNullableWarning,
            _ => throw new InvalidOperationException(message: $"Unsupported token type {operatorToken} in PostfixUnaryExpression")
        };
        return type;
    }


    public override J? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        var name = Convert<Expression>(node.Name)!;
        J result;
        if (node.IsKind(SyntaxKind.PointerMemberAccessExpression))
        {
            result = name switch
            {
                J.Identifier id => new Cs.PointerFieldAccess(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    target: Convert<Expression>(node.Expression)!,
                    name: new JLeftPadded<J.Identifier>(
                        before: Format(Leading(node.OperatorToken)),
                        element: id,
                        markers: Markers.EMPTY
                    ),
                    type: MapType( node)),
                J.ParameterizedType pi => pi.WithClazz(newClazz: new Cs.PointerFieldAccess(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    target: Convert<Expression>(node.Expression)!,
                    name: new JLeftPadded<J.Identifier>(
                        before: Format(Leading(node.OperatorToken)),
                        element: (J.Identifier)pi.Clazz,
                        markers: Markers.EMPTY
                    ),
                    type: MapType( node)
                )),
                _ => throw new NotImplementedException()
            };
        }
        else
        {

            result = name switch
            {
                J.Identifier id => new J.FieldAccess(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    target: Convert<Expression>(node.Expression)!,
                    name: new JLeftPadded<J.Identifier>(
                        before: Format(Leading(node.OperatorToken)),
                        element: id,
                        markers: Markers.EMPTY
                    ),
                    type: MapType(node)),
                J.ParameterizedType pi => pi.WithClazz(newClazz: new J.FieldAccess(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    target: Convert<Expression>(node.Expression)!,
                    name: new JLeftPadded<J.Identifier>(
                        before: Format(Leading(node.OperatorToken)),
                        element: (J.Identifier)pi.Clazz,
                        markers: Markers.EMPTY
                    ),
                    type: MapType(node)
                )),
                _ => throw new NotImplementedException()
            };
        }


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
            conditionalExpressions.Add(item: (conditionalNode.Expression, Format(Leading(conditionalNode.OperatorToken))));
            currentNode = conditionalNode.WhenNotNull;
        }
        conditionalExpressions.Add(item: (currentNode, Format(Leading(currentNode))));
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
                .FirstOrDefault(predicate: x => x.Markers.Contains<MemberBinding>());
            if (bindingNode != null)
            {
                var newMarkers = bindingNode.Markers
                    .WithMarkers(markers: bindingNode.Markers.MarkerList.Where(predicate: x => x is not MemberBinding).ToList());
                if (bindingNode is J.MethodInvocation methodNode)
                {
                    // var innerPrefix = methodNode.Prefix;
                    var innerPrefix = !isLastSegment ? methodNode.Prefix : afterSpace;
                    var outerExpression = currentExpression;
                    var newMethod = methodNode
                        .WithPrefix(Space.EMPTY)
                        .Padding.WithSelect(JRightPadded.Create(outerExpression,innerPrefix))
                        .WithMarkers(newMarkers: newMarkers);
                    lstNode = methodNode.Equals(other: lstNode) ? newMethod : lstNode.ReplaceNode(oldNode: methodNode, newNode: newMethod);
                }
                else if (bindingNode is J.FieldAccess fieldAccess)
                {
                    var newFieldAccess = fieldAccess

                        .WithTarget(newTarget: currentExpression)
                        .WithMarkers(newMarkers: newMarkers);
                    if (isLastSegment)
                        newFieldAccess = newFieldAccess.Padding.WithName(JLeftPadded.Create(newFieldAccess.Name, afterSpace));
                        // newFieldAccess = newFieldAccess.WithPrefix(afterSpace);
                    lstNode = fieldAccess.Equals(other: lstNode) ? newFieldAccess : lstNode.ReplaceNode(oldNode: fieldAccess, newNode: newFieldAccess);
                } else if (bindingNode is J.ArrayAccess arrayAccess)
                {
                    var newArrayAccess = arrayAccess
                        .WithIndexed(newIndexed: currentExpression)
                        .WithMarkers(newMarkers: newMarkers);
                    lstNode = newArrayAccess.Equals(other: lstNode) ? newArrayAccess : lstNode.ReplaceNode(oldNode: lstNode, newNode: newArrayAccess);
                }
            }

            // right hand side is the root and doesn't get wrapped
            if (!isLastSegment)
            {
                lstNode = new Cs.NullSafeExpression(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(expressionPortion)),
                    markers: Markers.EMPTY,
                    expression: new JRightPadded<Expression>(
                        element: lstNode!,
                        after: afterSpace,
                        markers: Markers.EMPTY
                    )
                );
            }

            currentExpression = lstNode;

            i++;
        }


        return currentExpression;

    }


    /// <summary>
    /// Very similar to MemberAccessExpression, but doesn't have an expression portion - just identifier
    /// Used in ConditionalAccessExpression since they are constructed left to right, then right to left like normal field access
    /// </summary>
    public override J? VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
    {
        var name = Convert<Expression>(node.Name)!;

        if (name is J.ParameterizedType generic)
        {
            return generic.WithMarkers(Markers.Create(markers: new MemberBinding()));
        }
        else
        {
            return new J.FieldAccess(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.Create(markers: new MemberBinding()),
                target: new J.Empty(id: Core.Tree.RandomId(), prefix: Space.EMPTY, markers: Markers.EMPTY),
                name: ((J.Identifier)name).AsLeftPadded(before: Format(Leading(node.OperatorToken))),
                type: MapType( node)
            );
        }

    }

    public override J? VisitElementBindingExpression(ElementBindingExpressionSyntax node)
    {


        var placeholderExpression = new J.Empty(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY);


        return new J.ArrayAccess(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.Create(markers: new MemberBinding()),
            indexed: placeholderExpression,
            dimension: new J.ArrayDimension(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.ArgumentList.OpenBracketToken)),
                markers: Markers.EMPTY,
                index: new JRightPadded<Expression>(
                    element: Convert<Expression>(node.ArgumentList.Arguments[index: 0])!,
                    after: Format(Trailing(node.ArgumentList.Arguments[index: 0])),
                    markers: Markers.EMPTY
                )
            ),
            type: MapType( node)
        );
    }

    public override J? VisitRangeExpression(RangeExpressionSyntax node)
    {
        return new Cs.RangeExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            start: ToRightPadded<Expression>(node.LeftOperand),
            end: Convert<Expression>(node.RightOperand));
    }

    public override J? VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
    {

        return new Cs.ImplicitElementAccess(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            argumentList: ToJContainer<ArgumentSyntax, Cs.Argument>(syntaxList: node.ArgumentList.Arguments, openingToken: node.ArgumentList.OpenBracketToken));
    }

    public override J? VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        return new J.Ternary(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            condition: Convert<Expression>(node.Condition)!,
            truePart: new JLeftPadded<Expression>(
                before: Format(Leading(node.QuestionToken)),
                element: Convert<Expression>(node.WhenTrue)!,
                markers: Markers.EMPTY
            ),
            falsePart: new JLeftPadded<Expression>(
                before: Format(Leading(node.ColonToken)),
                element: Convert<Expression>(node.WhenFalse)!,
                markers: Markers.EMPTY
            ),
            type: MapType( node)
        );
    }

    public override J? VisitThisExpression(ThisExpressionSyntax node)
    {
        return new J.Identifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            annotations: [],
            simpleName: node.Token.Text,
            type: (MapType( node) as JavaType.Variable)?.Type,
            fieldType: MapType( node) as JavaType.Variable
        );
    }

    public override J? VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        return new J.Literal(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            value: node.Token.Value,
            valueSource: node.Token.Text,
            unicodeEscapes: null,
            type: (MapType( node) as JavaType.Primitive)!
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
        return new Cs.DefaultExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeOperator: node.OpenParenToken.IsPresent() ? ToJContainer<TypeSyntax, TypeTree>(syntaxList: [node.Type], openingToken: node.OpenParenToken) : null
        );
    }

    public override J? VisitTypeOfExpression(TypeOfExpressionSyntax node)
    {
        return new J.MethodInvocation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            select: null,
            typeParameters: null,
            name: new J.Identifier(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.Keyword)),
                markers: Markers.EMPTY,
                annotations: [],
                simpleName: node.Keyword.Text,
                type: null,
                fieldType: null
            ),
            arguments: new JContainer<Expression>(
                before: Format(Leading(node.OpenParenToken)),
                elements:
                [
                    new JRightPadded<Expression>(
                        element: Convert<Expression>(node.Type)!,
                        after: Format(Leading(node.CloseParenToken)),
                        markers: Markers.EMPTY
                    )
                ],
                markers: Markers.EMPTY
            ),
            methodType: MapType( node) as JavaType.Method
        );
    }

    public override J? VisitSizeOfExpression(SizeOfExpressionSyntax node)
    {
        return new J.MethodInvocation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            select: null,
            typeParameters: null,
            name: new J.Identifier(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.Keyword)),
                markers: Markers.EMPTY,
                annotations: [],
                simpleName: node.Keyword.Text,
                type: null,
                fieldType: null
            ),
            arguments: new JContainer<Expression>(
                before: Format(Leading(node.OpenParenToken)),
                elements:
                [
                    new JRightPadded<Expression>(
                        element: Convert<Expression>(node.Type)!,
                        after: Format(Leading(node.CloseParenToken)),
                        markers: Markers.EMPTY
                    )
                ],
                markers: Markers.EMPTY
            ),
            methodType: MapType( node) as JavaType.Method
        );
    }

    public override J? VisitElementAccessExpression(ElementAccessExpressionSyntax node)
    {
        // return MapArrayAccess(node, index: 0);
        var arguments = node.ArgumentList.Arguments.Count > 1 ? new Cs.ArrayRankSpecifier(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            sizes: ToJContainer<ArgumentSyntax, Expression>(node.ArgumentList.Arguments, node.ArgumentList.OpenBracketToken)) :
            Convert<Expression>(node.ArgumentList.Arguments[0])!;
        return new J.ArrayAccess(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            indexed: Convert<Expression>(node.Expression)!,
            dimension: new J.ArrayDimension(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.ArgumentList.OpenBracketToken)),
                markers: Markers.EMPTY,
                index: JRightPadded.Create(
                    element: arguments,
                    after: Format(Trailing(node.ArgumentList.Arguments.Last())))
            ),
            type: MapType( node) // TODO this probably needs to be specific to the current array dimension
        );
    }

    private J.ArrayAccess MapArrayAccess(ElementAccessExpressionSyntax node, int index)
    {
        return new J.ArrayAccess(
            id: Core.Tree.RandomId(),
            prefix: index == 0 ? Format(Leading(node)) : Space.EMPTY,
            markers: Markers.EMPTY,
            indexed: index == node.ArgumentList.Arguments.Count - 1
                ? Convert<Expression>(node.Expression)!
                : MapArrayAccess(node, index + 1),
            dimension: new J.ArrayDimension(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.ArgumentList.OpenBracketToken)),
                markers: Markers.EMPTY,
                index: new JRightPadded<Expression>(
                    element: Convert<Expression>(node.ArgumentList.Arguments[index])!,
                    after: Format(Trailing(node.ArgumentList.Arguments[index])),
                    markers: Markers.EMPTY
                )
            ),
            type: MapType( node) // TODO this probably needs to be specific to the current array dimension
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
        var result = new Cs.DeclarationExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(node.Type),
            variables: Convert<Cs.VariableDesignation>(node.Designation)!);
        return result;
    }

    public override J? VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
    {

        return new J.MethodDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: node.ParameterList == null ? Markers.Create(new CompactConstructor(Core.Tree.RandomId())) : Markers.EMPTY,
            leadingAnnotations: [], // attributes are not supported for anonymous methods
            modifiers: MapModifiers(stl: node.Modifiers),
            typeParameters: null,
            returnTypeExpression: null,
            name: new J.MethodDeclaration.IdentifierWithAnnotations(
                identifier: new J.Identifier(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node.DelegateKeyword)),
                    markers: Markers.EMPTY,
                    annotations: [],
                    simpleName: node.DelegateKeyword.Text,
                    type: null,
                    fieldType: null
                ),
                annotations: []
            ),
            parameters: MapParameters<Statement>(pls: node.ParameterList) ?? new JContainer<Statement>(
                before: Space.EMPTY,
                elements:
                [
                    JRightPadded.Create<Statement>(element: new J.Empty(id: Core.Tree.RandomId(),
                        prefix: Space.EMPTY,
                        markers: Markers.EMPTY))
                ],
                markers: Markers.EMPTY
            ),
            throws: null,
            body: node.ExpressionBody != null ? Convert<J.Block>(node.ExpressionBody) : Convert<J.Block>(node.Block),
            defaultValue: null,
            methodType: MapType( node) as JavaType.Method
        );
    }

    public override J? VisitRefExpression(RefExpressionSyntax node)
    {
        return new Cs.RefExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!);
    }

    public override J? VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        return VisitLambdaExpressionSyntax(node);
    }

    public override J? VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        return VisitLambdaExpressionSyntax(node);
    }

    private J? VisitLambdaExpressionSyntax(LambdaExpressionSyntax node)
    {
        var leadingNodeSpace = Format(Leading(node));
        var parametersSpace = node.AsyncKeyword.IsKind(SyntaxKind.AsyncKeyword)
            ? Format(Trailing(node.AsyncKeyword))
            : Space.EMPTY;
        J.Lambda.Parameters parameters = node switch
        {
            ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression => new J.Lambda.Parameters(
                id: Core.Tree.RandomId(),
                prefix: parametersSpace,
                markers: Markers.EMPTY,
                parenthesized: true,
                elements: MapParameters<J>(pls: parenthesizedLambdaExpression.ParameterList)!.Elements),
            SimpleLambdaExpressionSyntax simpleLambdaExpression => new J.Lambda.Parameters(
                id: Core.Tree.RandomId(),
                prefix: parametersSpace,
                markers: Markers.EMPTY,
                parenthesized: false,
                elements: [MapParameter<J>(tps: simpleLambdaExpression.Parameter)]),
            _ => throw new NotSupportedException(message: $"Unsupported type {node.GetType()}")
        };


        var jLambda = new J.Lambda(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            @params: parameters,
            arrow: Format(Leading(node.ArrowToken)),
            body: Convert<J>(node.Body)!,
            type: MapType( node)
        );
        var csLambda = new Cs.Lambda(
            id: Core.Tree.RandomId(),
            prefix: leadingNodeSpace,
            markers: Markers.EMPTY,
            lambdaExpression: jLambda,
            modifiers: MapModifiers(stl: node.Modifiers)
        );
        return csLambda;
    }

    public override J? VisitInitializerExpression(InitializerExpressionSyntax node)
    {
        var initializer = new Cs.InitializerExpression(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            expressions: ToJContainer<ExpressionSyntax, Expression>(syntaxList: node.Expressions, openingToken: node.OpenBraceToken));

        return initializer;
    }

    public override J? VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
    {
        var jNewClass =  new J.NewClass(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            enclosing: null,
            @new: Format(Leading(node.NewKeyword)),
            clazz: new J.Empty(id: Core.Tree.RandomId(), prefix: Space.EMPTY, markers: Markers.EMPTY),
            arguments: new JContainer<Expression>(
                before: Format(Leading(node.ArgumentList.OpenParenToken)),
                elements: node.ArgumentList.Arguments.Count == 0
                    ?
                    [
                        new JRightPadded<Expression>(
                            element: new J.Empty(
                                id: Core.Tree.RandomId(),
                                prefix: Space.EMPTY,
                                markers: Markers.EMPTY
                            ),
                            after: Format(Leading(node.ArgumentList.CloseParenToken)),
                            markers: Markers.EMPTY
                        )
                    ]
                    : node.ArgumentList.Arguments.Select(selector: MapArgument).ToList(),
                markers: Markers.EMPTY
            ),
            body: null,
            constructorType: MapType( node) as JavaType.Method // this should be FindConstructorType(node) which return explicitly javaType.Method
        );
        var csNewClass = new Cs.NewClass(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            newClassCore: jNewClass,
            initializer: Convert<Cs.InitializerExpression>(node.Initializer));
        return csNewClass;
    }

    public override J? VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        var jNewClass = new J.NewClass(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            enclosing: null,
            @new: Format(Leading(node.NewKeyword)),
            clazz: Convert<TypeTree>(node.Type),
            arguments: new JContainer<Expression>(
                before: node.ArgumentList == null ? Space.EMPTY : Format(Leading(node.ArgumentList.OpenParenToken)),
                elements: node.ArgumentList == null || node.ArgumentList.Arguments.Count == 0
                    ?
                    [
                        new JRightPadded<Expression>(
                            element: new J.Empty(
                                id: Core.Tree.RandomId(),
                                prefix: Space.EMPTY,
                                markers: Markers.EMPTY
                            ),
                            after: node.ArgumentList == null
                                ? Space.EMPTY
                                : Format(Leading(node.ArgumentList.CloseParenToken)),
                            markers: Markers.EMPTY
                        )
                    ]
                    : node.ArgumentList.Arguments.Select(selector: MapArgument).ToList(),
                markers: node.ArgumentList == null
                    ? Markers.EMPTY.Add(marker: new OmitParentheses(Id: Core.Tree.RandomId()))
                    : Markers.EMPTY
            ),
            body: null,
            constructorType: MapType( node) as JavaType.Method // this should be FindConstructorType(node) which return explicitly javaType.Method
        );

        var csNewClass = new Cs.NewClass(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            newClassCore: jNewClass,
            initializer: Convert<Cs.InitializerExpression>(node.Initializer));

        return csNewClass;
    }

    public override J? VisitWithExpression(WithExpressionSyntax node)
    {
        // This was added in C# 9.0
        return base.VisitWithExpression(node);
    }

    public override J? VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
    {
        if (node.NameEquals == null)
            return Convert<Expression>(node.Expression);
        return new J.Assignment(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            variable: MapIdentifier(identifier: node.NameEquals.Name.Identifier, type: MapType( node.Expression)),
            expression: JLeftPadded.Create(element: Convert<Expression>(node.Expression)!, before: Format(Leading(node.NameEquals.EqualsToken)), markers: Markers.EMPTY),
            type: MapType( node.Expression)
        );
    }

    private JRightPadded<J.VariableDeclarations.NamedVariable> MapNamedVariableFromNameEquals(
        NameEqualsSyntax nameEqualsSyntax, ExpressionSyntax expression)
    {
        return new JRightPadded<J.VariableDeclarations.NamedVariable>(
            element: new J.VariableDeclarations.NamedVariable(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(nameEqualsSyntax.Name)),
                markers: Markers.EMPTY,
                name: Convert<J.Identifier>(nameEqualsSyntax.Name)!,
                dimensionsAfterName: [],
                initializer: new JLeftPadded<Expression>(
                    before: Format(Leading(nameEqualsSyntax.EqualsToken)),
                    element: Convert<Expression>(expression)!,
                    markers: Markers.EMPTY
                ),
                variableType: MapType( expression) as JavaType.Variable
            ),
            after: Format(Trailing(expression)),
            markers: Markers.EMPTY
        );
    }

    private JRightPadded<J.VariableDeclarations.NamedVariable> MapNamedVariableFromExpression(
        ExpressionSyntax expression)
    {
        var identifierOrFieldAccess = Convert<Expression>(expression)!;
        var identifier = identifierOrFieldAccess is J.Identifier i
            ? i
            : (identifierOrFieldAccess as J.FieldAccess)?.Name ?? throw new InvalidOperationException(message: "Can't determine identifier");
        return new JRightPadded<J.VariableDeclarations.NamedVariable>(
            element: new J.VariableDeclarations.NamedVariable(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(expression)),
                markers: Markers.EMPTY,
                name: new J.Identifier(id: Core.Tree.RandomId(), prefix: identifierOrFieldAccess.Prefix, markers: identifierOrFieldAccess.Markers,
                    annotations: identifier.Annotations, simpleName: expression.ToString(), type: identifier.Type, fieldType: identifier.FieldType),
                dimensionsAfterName: [],
                initializer: null,
                variableType: identifier.Type as JavaType.Variable
            ),
            after: Format(Trailing(expression)),
            markers: Markers.EMPTY
        );
    }

    public override J? VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
    {
        var jNewClass = new J.NewClass(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            enclosing: null,
            @new: Format(Leading(node.NewKeyword)),
            clazz: new J.Empty(id: Core.Tree.RandomId(), prefix: Space.EMPTY, markers: Markers.EMPTY),
            arguments: new JContainer<Expression>(
                before: Space.EMPTY,
                elements:
                [
                    new JRightPadded<Expression>(
                        element: new J.Empty(
                            id: Core.Tree.RandomId(),
                            prefix: Space.EMPTY,
                            markers: Markers.EMPTY
                        ),
                        after: Space.EMPTY,
                        markers: Markers.EMPTY
                    )
                ],
                markers: Markers.EMPTY.Add(marker: new OmitParentheses(Id: Core.Tree.RandomId()))
            ),
            body: null,
            constructorType: MapType( node) as JavaType.Method // this should be FindConstructorType(node) which return explicitly javaType.Method
        );
        var csNewClass = new Cs.NewClass(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            newClassCore: jNewClass,
            initializer: new Cs.InitializerExpression(
                id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,
                markers: Markers.EMPTY,
                expressions: ToJContainer<AnonymousObjectMemberDeclaratorSyntax, Expression>(syntaxList: node.Initializers, openingToken: node.OpenBraceToken)));
        return csNewClass;
    }

    private JRightPadded<Statement> MapAnonymousObjectMember(AnonymousObjectMemberDeclaratorSyntax aomds)
    {
        // This was added in C# 3.0
        return MapAnonymousObjectMember(aomds: aomds, isLastElement: false);
    }

    private JRightPadded<Statement> MapAnonymousObjectMember(AnonymousObjectMemberDeclaratorSyntax aomds,
        bool isLastElement)
    {
        var statement = Convert<Statement>(aomds)!;
        var trailingComma = isLastElement && aomds.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
        return new JRightPadded<Statement>(
            element: statement,
            after: Space.EMPTY,
            markers: trailingComma
                ? Markers.EMPTY.Add(marker: new TrailingComma(Id: Core.Tree.RandomId(),
                    Suffix: Format(Trailing(aomds.GetLastToken().GetNextToken()))))
                : Markers.EMPTY
        );
    }

    public override J? VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
    {

        var initializer = Convert<Cs.InitializerExpression>(node.Initializer);
        return new J.NewArray(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(node.Type.ElementType),
            dimensions: node.Type.RankSpecifiers.Select(selector: r => Convert<J.ArrayDimension>(r)!).ToList(),
            initializer: initializer?.Padding.Expressions,
            type: MapType( node)
        );
    }

    public override J? VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
    {
        var initializer = Convert<Cs.InitializerExpression>(node.Initializer);
        return new J.NewArray(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: null,
            dimensions:
            [ new J.ArrayDimension(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenBracketToken)),
                markers: Markers.EMPTY,
                index: JRightPadded.Create<Expression>(element: new Cs.ArrayRankSpecifier(
                    id: Core.Tree.RandomId(),
                    prefix: Space.EMPTY,
                    markers: Markers.EMPTY,
                    sizes: JContainer.Create(elements: node.Commas.Select(selector: c => JRightPadded.Create<Expression>(element: new J.Empty(id: Core.Tree.RandomId(), prefix: Format(Leading(c)), markers: Markers.EMPTY), after: Format(Trailing(c)))).ToList())))) ],
                // node.Commas.Select(c => new J.ArrayDimension(
                // Core.Tree.RandomId(),
                // Format(Leading(c)),
                // Markers.EMPTY,
                // JRightPadded.Create<Expression>(new J.Empty(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY)))).ToList(),
            initializer: initializer?.Padding.Expressions,
            type: MapType( node)
        );
    }

    public override J? VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
    {
        var initializer = Convert<Cs.InitializerExpression>(node.Initializer);
        var arrayType = (ArrayTypeSyntax)node.Type;
        var arrayCreation =  new J.NewArray(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.Type)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(arrayType.ElementType),
            dimensions: arrayType.RankSpecifiers.Select(selector: r => Convert<J.ArrayDimension>(r)!).ToList(),
            initializer: initializer?.Padding.Expressions,
            type: MapType( node)
        );

        return new Cs.StackAllocExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: arrayCreation
        );
    }


    public override J? VisitImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax node)
    {
        var initializer = Convert<Cs.InitializerExpression>(node.Initializer);

        var arrayCreation =  new J.NewArray(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: new J.Empty(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY),
            dimensions: [new J.ArrayDimension(
                    id: Core.Tree.RandomId(),
                    prefix: Space.EMPTY,
                    markers: Markers.EMPTY,
                    index: JRightPadded.Create(
                        (Expression)new J.Empty(
                            id: Core.Tree.RandomId(),
                            prefix: Space.EMPTY,
                            markers: Markers.EMPTY),
                        Format(Trailing(node.OpenBracketToken))
                    ))],
            initializer: initializer?.Padding.Expressions,
            type: MapType( node)
        );

        return new Cs.StackAllocExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: arrayCreation
        );
    }

    public override J? VisitCollectionExpression(CollectionExpressionSyntax node)
    {
        return new Cs.CollectionExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            elements: node.Elements.Count == 0
                ?
                [
                    new JRightPadded<Expression>(
                        element: new J.Empty(
                            id: Core.Tree.RandomId(),
                            prefix: Format(Trailing(node.OpenBracketToken)),
                            markers: Markers.EMPTY
                        ),
                        after: Space.EMPTY,
                        markers: node.CloseBracketToken.GetPreviousToken().IsKind(SyntaxKind.CommaToken)
                            ? Markers.EMPTY.Add(marker: new TrailingComma(Id: Core.Tree.RandomId(),
                                Suffix: Format(Leading(node.CloseBracketToken))))
                            : Markers.EMPTY
                    )
                ]
                : node.Elements.Select(selector: e => new JRightPadded<Expression>(
                    element: Convert<Expression>(e)!,
                    after: Format(Trailing(e)),
                    markers: e == node.Elements.Last() && e.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken)
                        ? Markers.EMPTY.Add(marker: new TrailingComma(Id: Core.Tree.RandomId(),
                            Suffix: Format(Trailing(e.GetLastToken().GetNextToken()))))
                        : Markers.EMPTY
                )).ToList(),
            type: MapType( node)
        );
    }

    public override J? VisitExpressionElement(ExpressionElementSyntax node)
    {
        return Convert<Expression>(node.Expression);
    }

    public override J? VisitSpreadElement(SpreadElementSyntax node)
    {
        return new Cs.Unary(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            @operator: JLeftPadded.Create(Cs.Unary.Types.Spread, Format(Leading(node.OperatorToken))),
            expression: Convert<Expression>(node.Expression)!,
            type: MapType(node));
    }

    public override J? VisitQueryExpression(QueryExpressionSyntax node)
    {
        return new Cs.QueryExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            fromClause: Convert<Cs.FromClause>(node.FromClause)!,
            body: Convert<Cs.QueryBody>(node.Body)!);
    }

    public override J? VisitQueryBody(QueryBodySyntax node)
    {
        return new Cs.QueryBody(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            clauses: node.Clauses.Select(x => Convert<Cs.QueryClause>(x)!).ToList(),
            selectOrGroup: Convert<Cs.SelectOrGroupClause>(node.SelectOrGroup)!,
            continuation: Convert<Cs.QueryContinuation>(node.Continuation)!);
    }

    public override J? VisitFromClause(FromClauseSyntax node)
    {
        return new Cs.FromClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeIdentifier: node.Type != null ? MapTypeTree(node.Type) : null,
            identifier: JRightPadded.Create(MapIdentifier(node.Identifier, MapType(node.Expression)), Format(Trailing(node.Identifier))),
            expression: Convert<Expression>(node.Expression)!);
    }

    public override J? VisitLetClause(LetClauseSyntax node)
    {
        return new Cs.LetClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: JRightPadded.Create(MapIdentifier(node.Identifier, MapType(node.Expression)), Format(Trailing(node.Identifier))),
            expression: Convert<Expression>(node.Expression)!);
    }

    public override J? VisitJoinClause(JoinClauseSyntax node)
    {
        return new Cs.JoinClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: JRightPadded.Create(MapIdentifier(node.Identifier, MapType(node.InExpression)), Format(Trailing(node.Identifier))),
            inExpression: ToRightPadded<Expression>(node.InExpression)!,
            leftExpression: ToRightPadded<Expression>(node.LeftExpression)!,
            rightExpression: Convert<Expression>(node.RightExpression)!,
            into: ToLeftPadded<Cs.JoinIntoClause>(node.Into));
    }

    public override J? VisitJoinIntoClause(JoinIntoClauseSyntax node)
    {
        return new Cs.JoinIntoClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: MapIdentifier(node.Identifier, MapType(node)));
    }

    public override J? VisitWhereClause(WhereClauseSyntax node)
    {
        return new Cs.WhereClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            condition: Convert<Expression>(node.Condition)!);
    }

    public override J? VisitOrderByClause(OrderByClauseSyntax node)
    {
        return new Cs.OrderByClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            orderings: ToRightPadded<OrderingSyntax, Cs.Ordering>(node.Orderings)!);
    }

    public override J? VisitSelectClause(SelectClauseSyntax node)
    {
        return new Cs.SelectClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!);
    }

    public override J? VisitGroupClause(GroupClauseSyntax node)
    {
        return new Cs.GroupClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            groupExpression: ToRightPadded<Expression>(node.GroupExpression)!,
            key: Convert<Expression>(node.ByExpression)!);
    }

    public override J? VisitQueryContinuation(QueryContinuationSyntax node)
    {
        return new Cs.QueryContinuation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: MapIdentifier(node.Identifier, MapType(node.Body)),
            body: Convert<Cs.QueryBody>(node.Body)!);
    }

    public override J? VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
    {
        return new J.Empty(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY
        );
    }

    public override J? VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
    {
        // This was added in C# 6.0
        return new Cs.InterpolatedString(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            start: node.StringStartToken.ToString(),
            parts: node.Contents.Count == 0
                ?
                [
                    new JRightPadded<Expression>(
                        element: new J.Empty(
                            id: Core.Tree.RandomId(),
                            prefix: Format(Trailing(node.StringStartToken)),
                            markers: Markers.EMPTY
                        ),
                        after: Space.EMPTY,
                        markers: Markers.EMPTY
                    )
                ]
                : node.Contents.Select(selector: c => new JRightPadded<Expression>(
                    element: Convert<Expression>(c)!,
                    after: Format(Trailing(c)),
                    markers: Markers.EMPTY
                )).ToList(),
            end: node.StringEndToken.ToString()
        );
    }

    public override J? VisitIsPatternExpression(IsPatternExpressionSyntax node)
    {
        return new Cs.IsPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!,
            JLeftPadded.Create(Convert<Cs.Pattern>(node.Pattern)!, Format(Leading(node.IsKeyword))));
        // return node.Pattern switch
        // {
        //     DeclarationPatternSyntax dp => new J.InstanceOf(
        //         id: Core.Tree.RandomId(),
        //         prefix: Format(Leading(node)),
        //         markers: Markers.EMPTY,
        //         expression: new JRightPadded<Expression>(
        //             element: Convert<Expression>(node.Expression)!,
        //             after: Format(Trailing(node.Expression)),
        //             markers: Markers.EMPTY),
        //         clazz: Convert<TypeTree>(dp.Type)!,
        //         pattern: Convert<J>(dp.Designation),
        //         type: MapType(node)
        //     ),
        //     VarPatternSyntax varPattern => new J.InstanceOf(
        //         id: Core.Tree.RandomId(),
        //         prefix: Format(Leading(node)),
        //         markers: Markers.EMPTY,
        //         expression: new JRightPadded<Expression>(
        //             element: Convert<Expression>(node.Expression)!,
        //             after: Format(Trailing(node.Expression)),
        //             markers: Markers.EMPTY),
        //         clazz: MapIdentifier(varPattern.VarKeyword, MapType(varPattern)),
        //         pattern: Convert<J>(varPattern.Designation),
        //         type: MapType(node)
        //     ),
        //     ConstantPatternSyntax constantPattern => new J.InstanceOf(
        //         id: Core.Tree.RandomId(),
        //         prefix: Format(Leading(node)),
        //         markers: Markers.EMPTY,
        //         expression: new JRightPadded<Expression>(
        //             element: Convert<Expression>(node.Expression)!,
        //             after: Format(Trailing(node.Expression)),
        //             markers: Markers.EMPTY),
        //         clazz: new J.Empty(
        //             id: Core.Tree.RandomId(),
        //             prefix: Space.EMPTY,
        //             markers: Markers.EMPTY),
        //         pattern: Convert<J>(constantPattern.Expression),
        //         type: MapType(node)
        //     ),
        //     UnaryPatternSyntax unaryPattern => new J.InstanceOf(
        //         id: Core.Tree.RandomId(),
        //         prefix: Format(Leading(node)),
        //         markers: Markers.EMPTY,
        //         expression: new JRightPadded<Expression>(
        //             element: Convert<Expression>(node.Expression)!,
        //             after: Format(Trailing(node.Expression)),
        //             markers: Markers.EMPTY),
        //         clazz: Convert<TypeTree>(dp.Type),
        //         pattern: Convert<J>(unaryPattern),
        //         type: MapType(node)
        //     ),
        //     _ => base.VisitIsPatternExpression(node)
        // };

    }

    public override J? VisitThrowExpression(ThrowExpressionSyntax node)
    {
        return new Cs.StatementExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            statement: new J.Throw(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                exception: Convert<Expression>(node.Expression)!
            )
        );
    }

    public override J? VisitWhenClause(WhenClauseSyntax node)
    {
        return Convert<Expression>(node.Condition);
    }

    public override J? VisitDiscardPattern(DiscardPatternSyntax node)
    {
        return new Cs.DiscardPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            type: MapType(node));
    }

    public override J? VisitDeclarationPattern(DeclarationPatternSyntax node)
    {
        return new Cs.TypePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeIdentifier: MapTypeTree(node.Type),
            designation: Convert<Cs.VariableDesignation>(node.Designation)
        );
    }

    public override J? VisitVarPattern(VarPatternSyntax node)
    {
        return new Cs.VarPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            designation: Convert<Cs.VariableDesignation>(node.Designation)!
        );
    }

    public override J? VisitRecursivePattern(RecursivePatternSyntax node)
    {
        return new Cs.RecursivePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeQualifier: node.Type != null ? MapTypeTree(node.Type) : null,
            positionalPattern: Convert<Cs.PositionalPatternClause>(node.PositionalPatternClause),
            propertyPattern: Convert<Cs.PropertyPatternClause>(node.PropertyPatternClause),
            designation: Convert<Cs.VariableDesignation>(node.Designation)
        );
    }

    public override J? VisitPositionalPatternClause(PositionalPatternClauseSyntax node)
    {
        return new Cs.PositionalPatternClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            subpatterns: ToJContainer<SubpatternSyntax, Cs.Subpattern>(node.Subpatterns, node.OpenParenToken));
    }

    public override J? VisitPropertyPatternClause(PropertyPatternClauseSyntax node)
    {
        return new Cs.PropertyPatternClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            subpatterns: ToJContainer<SubpatternSyntax, Expression>(node.Subpatterns, node.OpenBraceToken));
    }

    public override J? VisitConstantPattern(ConstantPatternSyntax node)
    {
        return new Cs.ConstantPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            value: Convert<Expression>(node.Expression)!
        );
    }

    public override J? VisitParenthesizedPattern(ParenthesizedPatternSyntax node)
    {
        return new Cs.ParenthesizedPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: ToJContainer<PatternSyntax, Cs.Pattern>([node.Pattern], node.OpenParenToken));
    }

    public override J? VisitRelationalPattern(RelationalPatternSyntax node)
    {
        var operatorType = node.OperatorToken.Kind() switch
        {
            SyntaxKind.LessThanToken => Cs.RelationalPattern.OperatorType.LessThan,
            SyntaxKind.LessThanEqualsToken => Cs.RelationalPattern.OperatorType.LessThanOrEqual,
            SyntaxKind.GreaterThanToken => Cs.RelationalPattern.OperatorType.GreaterThan,
            SyntaxKind.GreaterThanEqualsToken => Cs.RelationalPattern.OperatorType.GreaterThanOrEqual,
            _ => throw new InvalidOperationException($"Unsupported operator '{node.OperatorToken}' in {nameof(RelationalPatternSyntax)}")
        };
        return new Cs.RelationalPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            @operator: JLeftPadded.Create(operatorType, Format(Leading(node.OperatorToken))),
            value: Convert<Expression>(node.Expression)!
        );
    }

    public override J? VisitTypePattern(TypePatternSyntax node)
    {
        return new Cs.TypePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeIdentifier: MapTypeTree(node.Type),
            designation: null
        );
    }

    public override J? VisitUnaryPattern(UnaryPatternSyntax node)
    {
        return new Cs.UnaryPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            @operator: MapKeyword(node.OperatorToken)!,
            pattern: Convert<Cs.Pattern>(node.Pattern)!
        );
    }

    private Cs.Keyword? MapKeyword(SyntaxToken keyword)
    {
        if (!keyword.IsPresent())
            return null;
        if (!Enum.TryParse(keyword.ValueText, true, out Cs.Keyword.KeywordKind keywordKind))
            throw new InvalidOperationException($"Unable to parse '{keyword}' token as {nameof(Cs.Keyword)}.{nameof(Cs.Keyword.KeywordKind)} enum");
        return new Cs.Keyword(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(keyword)),
            markers: Markers.EMPTY,
            kind: keywordKind
        );
    }
    public override J? VisitListPattern(ListPatternSyntax node)
    {
        return new Cs.ListPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            patterns: ToJContainer<PatternSyntax, Cs.Pattern>(node.Patterns, node.OpenBracketToken),
            designation: Convert<Cs.VariableDesignation>(node.Designation)
        );
    }

    public override J? VisitSlicePattern(SlicePatternSyntax node)
    {
        return new Cs.SlicePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY
        );
    }

    public override J? VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
    {
        return new J.Literal(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            value: node.TextToken.Text,
            valueSource: node.TextToken.Text,
            unicodeEscapes: null,
            type: (JavaType.Primitive)MapType( node)
        );
    }

    public override J? VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
    {
        return Convert<Expression>(node.Value);
    }

    public override J? VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
    {
        return new J.Literal(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            value: node.FormatStringToken.Text,
            valueSource: node.FormatStringToken.Text,
            unicodeEscapes: null,
            type: (JavaType.Primitive)MapType( node)
        );
    }

    public override J? VisitGlobalStatement(GlobalStatementSyntax node)
    {
        return node.Statement.Accept(visitor: this);
    }

    public override J? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        return new J.MethodDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeParameters: Visit(node.TypeParameterList) as J.TypeParameters,
            returnTypeExpression: Convert<TypeTree>(node.ReturnType),
            name: new J.MethodDeclaration.IdentifierWithAnnotations(
                identifier: new J.Identifier(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node.Identifier)),
                    markers: Markers.EMPTY,
                    annotations: (Enumerable.Empty<J.Annotation>() as IList<J.Annotation>)!,
                    simpleName: node.Identifier.Text,
                    type: null,
                    fieldType: null
                ),
                annotations: []
            ),
            parameters: MapParameters<Statement>(pls: node.ParameterList)!,
            throws: null,
            body: MapBody(node.Body, node.ExpressionBody, node.SemicolonToken),
            defaultValue: null,
            methodType: MapType( node) as JavaType.Method
        );
    }

    public override J? VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
    {
        Statement result;
        var type = MapType(node);
        if (node.UsingKeyword.IsPresent())
        {
            result =
                new Cs.UsingStatement(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    awaitKeyword: VisitKeyword(node.AwaitKeyword),
                    expression: ToLeftPadded<Expression>(node.Declaration)!,
                    statement: new J.Empty(
                        id: Core.Tree.RandomId(),
                        prefix: Space.EMPTY,
                        markers: Markers.EMPTY));
        }
        else
        {



            // var usingModifier = node.UsingKeyword.IsKind(SyntaxKind.UsingKeyword)
            //     ? new J.Modifier(
            //         id: Core.Tree.RandomId(),
            //         prefix: Format(Leading(node.UsingKeyword)),
            //         markers: Markers.EMPTY,
            //         keyword: "using",
            //         modifierType: J.Modifier.Types.LanguageExtension,
            //         annotations: []
            //     )
            //     : null;

            var typeExpression = Convert<TypeTree>(node.Declaration.Type)!;

            result = new J.VariableDeclarations(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                leadingAnnotations: [],
                modifiers: MapModifiers(stl: node.Modifiers), //usingModifier != null ? [usingModifier, .. MapModifiers(stl: node.Modifiers)] : MapModifiers(stl: node.Modifiers),
                typeExpression: typeExpression,
                varargs: null!,
                dimensionsBeforeName: [],
                variables: node.Declaration.Variables.Select(selector: MapVariable).ToList()
            );
            if (node.AwaitKeyword.IsPresent())
            {
                result = new Cs.AwaitExpression(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.EMPTY,
                    expression: result,
                    type: type);

            }
        }

        return result;
    }

    private JRightPadded<J.VariableDeclarations.NamedVariable> MapVariable(VariableDeclaratorSyntax variableDeclarator)
    {
        var namedVariable = (Visit(variableDeclarator) as J.VariableDeclarations.NamedVariable)!;
        return new JRightPadded<J.VariableDeclarations.NamedVariable>(
            element: namedVariable,
            after: Format(Trailing(variableDeclarator)),
            markers: Markers.EMPTY
        );
    }

    public override J? VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        return new J.VariableDeclarations(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: [],
            typeExpression: Visit(node.Type) as TypeTree,
            varargs: null!,
            dimensionsBeforeName: [],
            variables: node.Variables.Select(selector: MapVariable).ToList()
        );
    }

    public override J? VisitVariableDeclarator(VariableDeclaratorSyntax node)
    {
        var javaType = (MapType( node) as JavaType.Variable)!;
        return new J.VariableDeclarations.NamedVariable(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.Identifier)),
            markers: Markers.EMPTY,
            name: new J.Identifier(
                id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,
                markers: Markers.EMPTY,
                annotations: [],
                simpleName: node.Identifier.Text,
                type: javaType.Type,
                fieldType: javaType
            ),
            dimensionsAfterName: MapArrayDimensions(nodeArgumentList: node.ArgumentList),
            initializer: node.Initializer != null
                ? new JLeftPadded<Expression>(
                    before: Format(Leading(node.Initializer)),
                    element: Convert<Expression>(node.Initializer.Value)!,
                    markers: Markers.EMPTY
                )
                : null,
            variableType: javaType
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
        return new Cs.SingleVariableDesignation(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            name: MapIdentifier(identifier: node.Identifier, type: MapType( node)));
    }

    public override J? VisitDiscardDesignation(DiscardDesignationSyntax node)
    {
        return new Cs.DiscardVariableDesignation(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            discard: MapIdentifier(identifier: node.UnderscoreToken, type: MapType( node)));
    }

    public override J? VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
    {
        return new Cs.ParenthesizedVariableDesignation(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            variables: ToJContainer<VariableDesignationSyntax, Cs.VariableDesignation>(syntaxList: node.Variables, openingToken: node.OpenParenToken),
            type: MapType( node));
    }

    public override J? VisitExpressionStatement(ExpressionStatementSyntax node)
    {
        return new Cs.ExpressionStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToRightPadded<Expression>(node.Expression)!
        );
    }

    public override J? VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return new J.Empty(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY
        );
    }

    public override J? VisitLabeledStatement(LabeledStatementSyntax node)
    {
        return new J.Label(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            name: new JRightPadded<J.Identifier>(
                element: MapIdentifier(identifier: node.Identifier, type: null),
                after: Format(Trailing(node.Identifier)),
                markers: Markers.EMPTY
            ),
            statement: Convert<Statement>(node.Statement)!
        );
    }

    public override J? VisitGotoStatement(GotoStatementSyntax node)
    {

        return new Cs.GotoStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            caseOrDefaultKeyword: MapKeyword(node.CaseOrDefaultKeyword),
            target: Convert<Expression>(node.Expression)
        );
    }

    public override J? VisitContinueStatement(ContinueStatementSyntax node)
    {
        return new J.Continue(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            label: null
        );
    }

    public override J? VisitReturnStatement(ReturnStatementSyntax node)
    {
        return new J.Return(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: node.Expression != null ? Convert<Expression>(node.Expression!) : null
        );
    }

    public override J? VisitThrowStatement(ThrowStatementSyntax node)
    {
        // FIXME: NODE has AttributesList
        return new J.Throw(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            exception: node.Expression != null
                ? Convert<Expression>(node.Expression)!
                : new J.Empty(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node.SemicolonToken)),
                    markers: Markers.EMPTY
                )
        );
    }

    public override J? VisitYieldStatement(YieldStatementSyntax node)
    {
        return new Cs.Yield(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            returnOrBreakKeyword: new Cs.Keyword(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.ReturnOrBreakKeyword)),
                markers: Markers.EMPTY,
                kind: node.ReturnOrBreakKeyword.IsKind(SyntaxKind.ReturnKeyword) ? Cs.Keyword.KeywordKind.Return : Cs.Keyword.KeywordKind.Break),
            expression: Convert<Expression>(node.Expression)!);
    }

    public override J? VisitWhileStatement(WhileStatementSyntax node)
    {
        return new J.WhileLoop(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            condition: new J.ControlParentheses<Expression>(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                tree: new JRightPadded<Expression>(
                    element: Convert<Expression>(node.Condition)!,
                    after: Format(Leading(node.CloseParenToken)),
                    markers: Markers.EMPTY)
            ),
            body: MapStatement(statementSyntax: node.Statement)
        );
    }

    public override J? VisitDoStatement(DoStatementSyntax node)
    {
        var condition = JLeftPadded.Create(
            new J.ControlParentheses<Expression>(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                tree: ToRightPadded<Expression>(node.Condition)!),
            Format(Leading(node.WhileKeyword)));
        var body = ToRightPadded<Statement>(node.Statement)!;
        return new J.DoWhileLoop(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            body: body,
            whileCondition: condition);

    }

    public override J? VisitForStatement(ForStatementSyntax node)
    {
        return new J.ForLoop(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            loopControl: new J.ForLoop.Control(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                init: node.Declaration == null && node.Initializers.Count == 0
                    ?
                    [
                        JRightPadded.Create<Statement>(element: new J.Empty(id: Core.Tree.RandomId(),
                            prefix: Format(Leading(node.FirstSemicolonToken)), markers: Markers.EMPTY))
                    ]
                    : node.Declaration != null
                        ?
                        [
                            new JRightPadded<Statement>(element: Convert<Statement>(node.Declaration!)!,
                                after: Format(Leading(node.FirstSemicolonToken)), markers: Markers.EMPTY)
                        ]
                        : node.Initializers.Select(selector: MapExpressionStatement).ToList(),
                condition: node.Condition != null
                    ? MapExpression(es: node.Condition)
                    : JRightPadded.Create<Expression>(
                        element: new J.Empty(id: Core.Tree.RandomId(), prefix: Format(Leading(node.SecondSemicolonToken)), markers: Markers.EMPTY)
                    ),
                update: node.Incrementors.Count == 0
                    ?
                    [
                        JRightPadded.Create<Statement>(element: new J.Empty(id: Core.Tree.RandomId(),
                            prefix: Format(Leading(node.CloseParenToken)), markers: Markers.EMPTY))
                    ]
                    : node.Incrementors.Select(MapExpressionStatement).ToList()
            ),
            body: MapStatement(statementSyntax: node.Statement)
        );
    }

    public override J? VisitForEachStatement(ForEachStatementSyntax node)
    {
        var javaType = (MapType( node) as JavaType.Variable)!;
        var loop = new J.ForEachLoop(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.ForEachKeyword)),
            markers: Markers.EMPTY,
            loopControl: new J.ForEachLoop.Control(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                variable: new JRightPadded<J.VariableDeclarations>(
                    element: new J.VariableDeclarations(
                        id: Core.Tree.RandomId(),
                        prefix: Format(Leading(node.Type)),
                        markers: Markers.EMPTY,
                        leadingAnnotations: [],
                        modifiers: [],
                        typeExpression: Convert<TypeTree>(node.Type),
                        varargs: null!,
                        dimensionsBeforeName: [],
                        variables:
                        [
                            new JRightPadded<J.VariableDeclarations.NamedVariable>(
                                element: new J.VariableDeclarations.NamedVariable(
                                    id: Core.Tree.RandomId(),
                                    prefix: Format(Leading(node.Identifier)),
                                    markers: Markers.EMPTY,
                                    name: new J.Identifier(
                                        id: Core.Tree.RandomId(),
                                        prefix: Space.EMPTY,
                                        markers: Markers.EMPTY,
                                        annotations: [],
                                        simpleName: node.Identifier.Text,
                                        type: javaType.Type,
                                        fieldType: javaType
                                    ),
                                    dimensionsAfterName: [],
                                    initializer: null,
                                    variableType: javaType
                                ),
                                after: Space.EMPTY,
                                markers: Markers.EMPTY
                            )
                        ]
                    ),
                    after: Format(Trailing(node.Identifier)),
                    markers: Markers.EMPTY
                ),
                iterable: MapExpression(es: node.Expression)
            ),
            body: MapStatement(statementSyntax: node.Statement)
        );

        if (node.AwaitKeyword.IsKeyword())
        {
            return new Cs.AwaitExpression(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                loop,
                loop.LoopControl.Iterable.Type);
        }
        return loop;
    }

    public override J? VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
    {
        return new Cs.ForEachVariableLoop(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            controlElement: new Cs.ForEachVariableLoop.Control(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                variable: ToRightPadded<Expression>(node.Variable)!,
                iterable: MapExpression(es: node.Expression)
            ),
            body: MapStatement(statementSyntax: node.Statement)
        );

    }

    public override J? VisitUsingStatement(UsingStatementSyntax node)
    {
        var statement = Convert<Statement>(node.Statement) ?? throw new InvalidOperationException(message: "Statement is empty after conversion");
        var expressionNode = (SyntaxNode?)node.Expression ?? node.Declaration!;
        var expression = node.OpenParenToken.IsPresent() ? ToControlParentheses<Expression>(expressionNode, node.OpenParenToken) : Convert<Expression>(expressionNode)!;

        var usingStatement =
            new Cs.UsingStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                awaitKeyword: VisitKeyword(node.AwaitKeyword),
                expression: JLeftPadded.Create(expression, Format(Leading(node.UsingKeyword))),
                statement: statement);
        return usingStatement;
    }

    public Cs.Keyword? VisitKeyword(SyntaxToken token)
    {
        if (token.IsKind(SyntaxKind.None))
            return null;
        var kind = token.Kind() switch
        {
            SyntaxKind.AwaitKeyword => Cs.Keyword.KeywordKind.Await,
            SyntaxKind.RefKeyword => Cs.Keyword.KeywordKind.Ref,
            SyntaxKind.OutKeyword => Cs.Keyword.KeywordKind.Out,
            _ => throw new NotSupportedException(message: $"Keyword is {token} supported.")
        };
        return new Cs.Keyword(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(token)),
            markers: Markers.EMPTY,
            kind: kind);

    }

    public override J? VisitFixedStatement(FixedStatementSyntax node)
    {
        return new Cs.FixedStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            declarations: ToControlParentheses<J.VariableDeclarations>(node.Declaration, node.OpenParenToken)!,
            block: node.Statement is BlockSyntax
                ? Convert<J.Block>(node.Statement)!
                : new J.Block(
                    id: Core.Tree.RandomId(),
                    prefix: Format(Leading(node)),
                    markers: Markers.Create(new OmitBraces(Core.Tree.RandomId())),
                    @static: JRightPadded.Create(false),
                    // statements: [JRightPadded.Create(Convert<Statement>(node.Statement), Format(Leading((SyntaxToken)((dynamic)node.Statement).SemicolonToken)))!],
                    statements: [MapStatement(node.Statement)],
                    Space.EMPTY
                )
        );

    }

    public override J? VisitCheckedStatement(CheckedStatementSyntax node)
    {
        return new Cs.CheckedStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            keyword: MapKeyword(node.Keyword)!,
            block: Convert<J.Block>(node.Block)!
        );
    }

    public override J? VisitUnsafeStatement(UnsafeStatementSyntax node)
    {
        return new Cs.UnsafeStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            block: Convert<J.Block>(node.Block)!
        );
    }

    public override J? VisitLockStatement(LockStatementSyntax node)
    {
        return new Cs.LockStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToControlParentheses<Expression>(node.Expression, node.OpenParenToken)!,
            statement: ToRightPadded<Statement>(node.Statement)!
        );
    }

    public override J? VisitIfStatement(IfStatementSyntax node)
    {
        return new J.If(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            ifCondition: new J.ControlParentheses<Expression>(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.OpenParenToken)),
                markers: Markers.EMPTY,
                tree: MapExpression(es: node.Condition)
            ),
            thenPart: MapStatement(statementSyntax: node.Statement),
            elsePart: node.Else != null ? Convert<J.If.Else>(node.Else) : null
        );
    }

    private JRightPadded<Expression> MapExpression(ExpressionSyntax es)
    {
        var expression = Convert<Expression>(es)!;

        return new JRightPadded<Expression>(
            element: expression,
            after: Format(Trailing(es)),
            markers: Markers.EMPTY
        );
    }

    private JRightPadded<Statement> MapExpressionStatement(ExpressionSyntax es) => MapExpressionStatement(es, false);
    private JRightPadded<Statement> MapExpressionStatement(ExpressionSyntax es, bool forceWrap)
    {
        var lst = Convert<J>(es)!;
        if (lst is Cs.ExpressionStatement || (lst is Statement && !forceWrap))
        {
            return JRightPadded.Create((Statement)lst, Format(Trailing(es)));
        }

        return JRightPadded.Create<Statement>(new Cs.ExpressionStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(es)),
                markers: Markers.EMPTY,
                expression: JRightPadded.Create((Expression)lst, Format(Trailing(es)))
            ));
    }

    public override J? VisitElseClause(ElseClauseSyntax node)
    {
        return new J.If.Else(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            body: MapStatement(statementSyntax: node.Statement)
        );
    }

    public override J? VisitSwitchStatement(SwitchStatementSyntax node)
    {
        Statement result = new Cs.SwitchStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToJContainer<ExpressionSyntax, Expression>([node.Expression], node.OpenParenToken),
            sections: ToJContainer<SwitchSectionSyntax, Cs.SwitchSection>(node.Sections, node.OpenBraceToken)
        );

        if (node.AttributeLists.Count > 0)
        {
            result = new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                attributeLists: MapAttributes(node.AttributeLists)!,
                statement: result);

        }

        return result;
    }


    public override J? VisitSwitchSection(SwitchSectionSyntax node)
    {
        return new Cs.SwitchSection(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            labels: node.Labels.Select(x => Convert<Cs.SwitchLabel>(x)!).ToList(),
            statements: node.Statements.Select(selector: MapStatement).ToList()
        );


    }

    // private JRightPadded<Expression> MapSwitchCaseLabel(SwitchLabelSyntax sls)
    // {
    //     var expression = Convert<Expression>(sls)!;
    //     return new JRightPadded<Expression>(
    //         element: expression,
    //         after: Space.EMPTY,
    //         markers: Markers.EMPTY
    //     );
    // }

    public override J? VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
    {
        return new Cs.CasePatternSwitchLabel(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: Convert<Cs.Pattern>(node.Pattern)!,
            whenClause: ToLeftPadded<Expression>(node.WhenClause?.Condition),
            colonToken: Format(Leading(node.ColonToken)));
    }

    public override J? VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
    {
        Cs.Pattern pattern = node.Value switch
        {
            IdentifierNameSyntax identifier => new Cs.TypePattern(
                id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,  //Format(Leading(node)),
                markers: Markers.EMPTY,
                typeIdentifier: Convert<TypeTree>(identifier)!,
                designation: null),
            _ => new Cs.ConstantPattern(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY, //Format(Leading(node.Value)),
            markers: Markers.EMPTY,
            value: Convert<Expression>(node.Value)!),
        };
        return new Cs.CasePatternSwitchLabel(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: pattern,
            whenClause: null,
            colonToken: Format(Leading(node.ColonToken)));
    }

    public override J? VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
    {
        return new Cs.DefaultSwitchLabel(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            colonToken: Format(Leading(node.ColonToken)));
    }

    public override J? VisitSwitchExpression(SwitchExpressionSyntax node)
    {
        return new Cs.SwitchExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToRightPadded<Expression>(node.GoverningExpression)!,
            arms: ToJContainer<SwitchExpressionArmSyntax, Cs.SwitchExpressionArm>(node.Arms, node.OpenBraceToken));
    }

    public override J? VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
    {
        return new Cs.SwitchExpressionArm(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: Convert<Cs.Pattern>(node.Pattern)!,
            whenExpression: ToLeftPadded<Expression>(node.WhenClause?.Condition),
            expression: ToLeftPadded<Expression>(node.Expression)!);
    }

    public override J? VisitTryStatement(TryStatementSyntax node)
    {
        return new Cs.Try(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            body: Convert<J.Block>(node.Block)!,
            catches: node.Catches.Select(selector: Convert<Cs.Try.Catch>).ToList()!,
            @finally: node.Finally != null
                ? new JLeftPadded<J.Block>(
                    before: Format(Leading(node.Finally)),
                    element: Convert<J.Block>(node.Finally)!,
                    markers: Markers.EMPTY)
                : null
        );
    }

    public override J? VisitCatchFilterClause(CatchFilterClauseSyntax node)
    {
        return new J.ControlParentheses<Expression>(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.OpenParenToken)),
            markers: Markers.EMPTY,
            tree: ToRightPadded<Expression>(node.FilterExpression)!
        );
    }

    public override J? VisitFinallyClause(FinallyClauseSyntax node)
    {
        return Convert<J.Block>(node.Block);
    }

    public override J? VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
    {
        return new Cs.ExternAlias(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: new JLeftPadded<J.Identifier>(
                before: Format(Leading(node.AliasKeyword)),
                element: MapIdentifier(identifier: node.Identifier, type: null),
                markers: Markers.EMPTY
            )
        );
    }

    public override J? VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
    {
        return MapIdentifier(identifier: node.Identifier, type: null);
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
        return new Cs.DelegateDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributes: MapAttributes(node.AttributeLists),
            modifiers: MapModifiers(node.Modifiers),
            returnType: ToLeftPadded<TypeTree>(node.ReturnType)!,
            identifier: MapIdentifier(node.Identifier),
            typeParameters: MapTypeParameters(node.TypeParameterList),
            parameters: MapParameters<Statement>(node.ParameterList)!,
            typeParameterConstraintClauses: MapTypeParameterConstraintClauses(node.ConstraintClauses));
    }

    public override J? VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        return new Cs.EnumMemberDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(node.AttributeLists),
            name: MapIdentifier(node.Identifier),
            initializer: ToLeftPadded<Expression>(node.EqualsValue?.Value));

    }

    public override J? VisitConstructorConstraint(ConstructorConstraintSyntax node)
    {
        return new Cs.ConstructorConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY);
    }

    public override J? VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
    {
        return new Cs.ClassOrStructConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            kind: node.ClassOrStructKeyword.IsKind(SyntaxKind.ClassKeyword) ? Cs.ClassOrStructConstraint.TypeKind.Class : Cs.ClassOrStructConstraint.TypeKind.Struct);
    }

    public override J? VisitTypeConstraint(TypeConstraintSyntax node)
    {
        return new Cs.TypeConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(node.Type)!);
    }

    public override J? VisitDefaultConstraint(DefaultConstraintSyntax node)
    {
        return new Cs.DefaultConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY);
    }

    public override J? VisitAllowsConstraintClause(AllowsConstraintClauseSyntax node)
    {
        return new Cs.AllowsConstraintClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expressions: JContainer.Create(elements: ToRightPadded<AllowsConstraintSyntax, Cs.AllowsConstraint>(syntaxList: node.Constraints)));
    }

    public override J? VisitRefStructConstraint(RefStructConstraintSyntax node)
    {
        return base.VisitRefStructConstraint(node);
    }

    public override J? VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        var attributeLists = MapAttributes(m: node.AttributeLists);
        var variableDeclarations = new J.VariableDeclarations(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: Convert<TypeTree>(node.Declaration.Type),
            varargs: null,
            dimensionsBeforeName: [],
            variables: node.Declaration.Variables.Select(selector: MapVariable).ToList()
        );
        return attributeLists.Count > 0
            ? new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                attributeLists: attributeLists,
                statement: variableDeclarations
            )
            : variableDeclarations;
    }

    public override J? VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        var typeTree = Convert<TypeTree>(node.Declaration.Type)!;
        return new Cs.EventDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(m: node.AttributeLists) ?? [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: JLeftPadded.Create(typeTree, Format(Leading(node.EventKeyword))),
            interfaceSpecifier: null,
            name: MapIdentifier(identifier: node.Declaration.Variables[0].Identifier, type: typeTree.Type),
            accessors: null

        );
    }

    public override J? VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
    {
        return Convert<NameTree>(node.Name);
    }

    public override J? VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        return base.VisitOperatorDeclaration(node);
    }

    public override J? VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
    {
        var attributeLists = MapAttributes(m: node.AttributeLists);
        Statement returnValue = new Cs.ConversionOperatorDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            modifiers: MapModifiers(node.Modifiers),
            kind: ToLeftPadded<Cs.ConversionOperatorDeclaration.ExplicitImplicit>(node.ImplicitOrExplicitKeyword)!,
            returnType: ToLeftPadded<TypeTree>(node.Type)!,
            parameters: MapParameters<Statement>(node.ParameterList)!,
            expressionBody: ToLeftPadded<Expression>(node.ExpressionBody),
            body: Convert<J.Block>(node.Body));

        if(attributeLists.Count > 0)
        {
            returnValue = new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                attributeLists: attributeLists,
                statement: returnValue);
        }

        return returnValue;
    }

    public override J? VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {


        var attributeLists = MapAttributes(m: node.AttributeLists);
        var body = MapBody(node.Body, node.ExpressionBody, node.SemicolonToken);
        // if (statement is Cs.ArrowExpressionClause arrowExpressionClause)
        // {
        //     statement = new J.Block(
        //         id: Core.Tree.RandomId(),
        //         prefix: Format(Leading(node)),
        //         markers: Markers.EMPTY,
        //         @static: JRightPadded.Create(false),
        //         statements: [JRightPadded.Create((Statement)arrowExpressionClause.Expression, arrowExpressionClause.Padding.Expression.After)],
        //         end: Space.EMPTY
        //     );
        // }
        var methodDeclaration = new J.MethodDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeParameters: null, // constructors have no type parameters
            returnTypeExpression: null, // constructors have no return type
            name: new J.MethodDeclaration.IdentifierWithAnnotations(
                identifier: MapIdentifier(identifier: node.Identifier, type: null),
                annotations: [] // attributes always appear as leading
            ),
            parameters: MapParameters<Statement>(pls: node.ParameterList)!,
            throws: null, // C# has no checked exceptions
            body: body,
            defaultValue: null, // not applicable to constructors
            methodType: MapType( node) as JavaType.Method
        );


        var constructor = new Cs.Constructor(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            initializer: Convert<Cs.ConstructorInitializer>(node.Initializer),
            constructorCore: methodDeclaration);

        return attributeLists.Count > 0
            ? new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                attributeLists: attributeLists,
                statement: constructor
            )
            : constructor;
    }

    public override J? VisitConstructorInitializer(ConstructorInitializerSyntax node)
    {
        return new Cs.ConstructorInitializer(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            keyword: new Cs.Keyword(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.ThisOrBaseKeyword)),
                markers: Markers.EMPTY,
                kind: node.ThisOrBaseKeyword.IsKind(SyntaxKind.BaseKeyword) ? Cs.Keyword.KeywordKind.Base : Cs.Keyword.KeywordKind.This
            ),
            arguments: ToJContainer<ArgumentSyntax, Expression>(syntaxList: node.ArgumentList.Arguments, openingToken: node.ArgumentList.OpenParenToken)
        );
    }

    public override J? VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {



        var attributeLists = MapAttributes(m: node.AttributeLists);
        var methodDeclaration = new J.MethodDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.TildeToken)),
            markers: Markers.EMPTY,
            leadingAnnotations: [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeParameters: null, // constructors have no type parameters
            returnTypeExpression: null, // constructors have no return type
            name: new J.MethodDeclaration.IdentifierWithAnnotations(
                identifier: MapIdentifier(identifier: node.Identifier, type: null),
                annotations: [] // attributes always appear as leading
            ),
            parameters: MapParameters<Statement>(pls: node.ParameterList)!,
            throws: null, // C# has no checked exceptions
            body: MapBody(node.Body, node.ExpressionBody, node.SemicolonToken),
            defaultValue: null, // not applicable to constructors
            methodType: MapType( node) as JavaType.Method
        );

        var destructor = new Cs.DestructorDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            methodCore: methodDeclaration);

        return attributeLists.Count > 0
            ? new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                attributeLists: attributeLists,
                statement: destructor
            )
            : destructor;
    }

    public override J? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        var typeTree = Convert<TypeTree>(node.Type)!;
        return new Cs.PropertyDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(m: node.AttributeLists) ?? [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: typeTree,
            interfaceSpecifier: node.ExplicitInterfaceSpecifier != null
                ? new JRightPadded<NameTree>(
                    element: Convert<NameTree>(node.ExplicitInterfaceSpecifier.Name)!,
                    after: Format(Leading(node.ExplicitInterfaceSpecifier.DotToken)),
                    markers: Markers.EMPTY
                )
                : null,
            name: MapIdentifier(identifier: node.Identifier, type: typeTree.Type),
            accessors: Convert<J.Block>(node.AccessorList),
            expressionBody: Convert<Cs.ArrowExpressionClause>(node.ExpressionBody),
            initializer: node.Initializer != null
                ? new JLeftPadded<Expression>(
                    before: Format(Leading(node.Initializer)),
                    element: Convert<Expression>(node.Initializer.Value)!,
                    markers: Markers.EMPTY
                )
                : null
        );
    }

    public override J? VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
    {
        return new Cs.ArrowExpressionClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: MapExpression(node.Expression)!
        );
    }

    public override J? VisitEventDeclaration(EventDeclarationSyntax node)
    {

        var typeTree = Convert<TypeTree>(node.Type)!;
        return new Cs.EventDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(m: node.AttributeLists) ?? [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: JLeftPadded.Create(typeTree, Format(Leading(node.EventKeyword))),
            interfaceSpecifier: node.ExplicitInterfaceSpecifier != null
                ? JRightPadded.Create(
                    Convert<NameTree>(node.ExplicitInterfaceSpecifier.Name)!,
                    Format(Leading(node.ExplicitInterfaceSpecifier.DotToken)))
                : null,
            name: MapIdentifier(identifier: node.Identifier, type: typeTree.Type),
            accessors: node.AccessorList != null ? ToJContainer<AccessorDeclarationSyntax, Statement>(node.AccessorList.Accessors, node.AccessorList.OpenBraceToken) : null

        );

    }

    public override J? VisitIndexerDeclaration(IndexerDeclarationSyntax node)
    {
        // return base.VisitIndexerDeclaration(node);
        var attributeLists = MapAttributes(m: node.AttributeLists);
        // Expression indexer = node.ExplicitInterfaceSpecifier
        Statement returnValue = new Cs.IndexerDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: MapTypeTree(node.Type),
            indexer: new J.Identifier(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node.ThisKeyword)),
                markers: Markers.EMPTY,
                annotations: [],
                simpleName: "this",
                type: null,
                fieldType: null),
            parameters: ToJContainer<ParameterSyntax, Expression>(node.ParameterList.Parameters, node.ParameterList.OpenBracketToken),
            expressionBody: ToLeftPadded<Expression>(node.ExpressionBody),
            accessors: Convert<J.Block>(node.AccessorList)
        );

        if(attributeLists != null)
        {
            returnValue = new Cs.AnnotatedStatement(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                attributeLists: attributeLists,
                statement: returnValue);
        }

        return returnValue;

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
    internal T? Convert<T>(SyntaxNode? node) where T : class, J
    {
        if (node == null) return default;

        var visit = Visit(node);
        if (typeof(T) == typeof(Expression) && visit is not Expression && visit is Statement statement)
        {
            return new Cs.StatementExpression(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                statement: statement
            ) as T;
        }


        return (T?)visit;
    }

    private JRightPadded<Statement> MapMemberDeclaration(MemberDeclarationSyntax m)
    {
        var memberDeclaration = (Statement)Visit(m)!;
        var trailingSemicolon = m.GetLastToken().IsKind(SyntaxKind.SemicolonToken);
        if (trailingSemicolon && m is BaseTypeDeclarationSyntax { CloseBraceToken.Text: "}" } or
                NamespaceDeclarationSyntax { CloseBraceToken.Text: "}" })
        {
            return new JRightPadded<Statement>(
                element: memberDeclaration,
                after: Format(Leading(m.GetLastToken())),
                markers: Markers.EMPTY.Add(marker: new Semicolon(Id: Core.Tree.RandomId()))
            );
        }

        return new JRightPadded<Statement>(element: memberDeclaration,
            after: trailingSemicolon ? Format(Leading(m.GetLastToken())) : Space.EMPTY,
            markers: Markers.EMPTY
        );
    }

    private JContainer<T>? MapParameters<T>(ParameterListSyntax? pls) where T : J
    {
        return pls == null
            ? null
            : new JContainer<T>(
                before: Format(Leading(pls)),
                elements: pls.Parameters.Count == 0
                    ?
                    [
                        JRightPadded<T>.Build(t: new J.Empty(id: Core.Tree.RandomId(),
                            prefix: Format(Leading(pls.CloseParenToken)),
                            markers: Markers.EMPTY) as dynamic)
                    ]
                    : pls.Parameters.Select(selector: MapParameter<T>).ToList(),
                markers: Markers.EMPTY
            );
    }

    private JRightPadded<T> MapParameter<T>(ParameterSyntax tps) where T : J
    {
        var parameter = ((T?)Visit(tps))!;
        return new JRightPadded<T>(
            element: parameter,
            after: Format(Trailing(tps)),
            markers: Markers.EMPTY
        );
    }

    // private JContainer<J.TypeParameter>? MapTypeParameters(TypeParameterListSyntax? tpls)
    // {
    //     return tpls == null || tpls.Parameters.Count == 0
    //         ? null
    //         : JContainer<J.TypeParameter>.Build(before: Format(Leading(tpls)),
    //             elements: tpls.Parameters.Select(selector: MapTypeParameter).ToList(),
    //             markers: Markers.EMPTY
    //         );
    // }

    private JRightPadded<J.TypeParameter> MapTypeParameter(TypeParameterSyntax tps)
    {
        var typeParameter = (Visit(tps) as J.TypeParameter)!;
        return new JRightPadded<J.TypeParameter>(
            element: typeParameter,
            after: Format(Trailing(tps.Identifier)),
            markers: Markers.EMPTY
        );
    }

    private static readonly IDictionary<SyntaxKind, J.Modifier.Types> AccessModifierMap =
        new Dictionary<SyntaxKind, J.Modifier.Types>
        {
            { SyntaxKind.PublicKeyword, J.Modifier.Types.Public },
            { SyntaxKind.PrivateKeyword, J.Modifier.Types.Private },
            { SyntaxKind.AbstractKeyword, J.Modifier.Types.Abstract },
            { SyntaxKind.ProtectedKeyword, J.Modifier.Types.Protected },
            { SyntaxKind.StaticKeyword, J.Modifier.Types.Static },
            { SyntaxKind.VolatileKeyword, J.Modifier.Types.Volatile },
            { SyntaxKind.SealedKeyword, J.Modifier.Types.Sealed },
            { SyntaxKind.AsyncKeyword, J.Modifier.Types.Async },
        };

    private IList<J.Modifier> MapModifiers(SyntaxTokenList stl)
    {
        return stl.Select(MapModifier).ToList();
    }

    private J.Modifier MapModifier(SyntaxToken token)
    {
        return new J.Modifier(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(token)),
            markers: Markers.EMPTY,
            keyword: AccessModifierMap.ContainsKey(key: token.Kind()) ? null : token.ToString(),
            modifierType: AccessModifierMap.ContainsKey(key: token.Kind()) ? AccessModifierMap[key: token.Kind()] : J.Modifier.Types.LanguageExtension,
            annotations: []);
    }


#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private List<Cs.AttributeList> MapAttributes(SyntaxList<AttributeListSyntax> m)
    {
        return m.Select(selector: x => Convert<Cs.AttributeList>(x)!).ToList();
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private JavaType MapType(ExpressionSyntax ins)
    {
        if (ins.IsKind(SyntaxKind.NullLiteralExpression))
        {
            return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Null);
        }
        return _typeMapping.Type(roslynSymbol: semanticModel.GetTypeInfo(expression: ins).Type);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private JavaType MapType(SyntaxNode ins)
    {
        return _typeMapping.Type(roslynSymbol: semanticModel.GetDeclaredSymbol(declaration: ins) ?? semanticModel.GetTypeInfo(ins).Type);
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
            element: statement,
            after: trailingSemicolon ? Format(Leading(statementSyntax.GetLastToken())) : Space.EMPTY, markers: Markers.EMPTY
        );
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal  SyntaxTriviaList Leading<T>(SyntaxList<T> list) where T : SyntaxNode
    {
        return list.Count == 0 ? SyntaxTriviaList.Empty : Leading(list.First());
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal SyntaxTriviaList Leading(SyntaxNode node)
    {
        var firstToken = node.GetFirstToken();
        return Leading(firstToken);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal SyntaxTriviaList Leading(SyntaxToken token)
    {
        var previousToken = token.GetPreviousToken();
        var leading = token.LeadingTrivia;
        if (leading.Count == 0)
            return OnlyUnseenTrivia(trivia: previousToken.TrailingTrivia);
        var trailing = previousToken.TrailingTrivia;
        if (trailing.Count == 0)
            return OnlyUnseenTrivia(trivia: leading);
        return OnlyUnseenTrivia(trivia1: trailing, trivia2: leading);
    }

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList OnlyUnseenTrivia(SyntaxTriviaList trivia)
    {
        var span = trivia.Span;
        var idx = _seenTriviaSpans.BinarySearch(item: span);
        if (idx >= 0)
            return SyntaxTriviaList.Empty;
        idx = ~idx;
        if (idx > 0 && _seenTriviaSpans[index: idx - 1].End > span.Start)
            return SyntaxTriviaList.Empty;
        _seenTriviaSpans.Insert(index: idx, item: span);
        _seenTriviaSpans.Sort();
        return trivia;
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private SyntaxTriviaList OnlyUnseenTrivia(SyntaxTriviaList trivia1, SyntaxTriviaList trivia2)
    {
        var span = new TextSpan(start: trivia1.Span.Start, length: trivia2.Span.End - trivia1.Span.Start);
        var idx = _seenTriviaSpans.BinarySearch(item: span);
        if (idx >= 0)
            return SyntaxTriviaList.Empty;
        idx = ~idx;
        if (idx > 0 && _seenTriviaSpans[index: idx - 1].End > span.Start)
            return SyntaxTriviaList.Empty;
        _seenTriviaSpans.Insert(index: idx, item: span);
        _seenTriviaSpans.Sort();
        return trivia1.AddRange(trivia2);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal  SyntaxTriviaList Trailing(SyntaxNode node)
    {
        return Trailing(node.GetLastToken());
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal  SyntaxTriviaList Trailing(SyntaxToken token)
    {
        return Leading(token.GetNextToken());
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal static Space Format(SyntaxTriviaList triviaList)
    {
        return Space.Format(triviaList.ToFullString());
    }

    private JLeftPadded<TTo>? ToLeftPadded<TTo>(SyntaxToken node) where TTo : struct, Enum
    {
        if (node.IsKind(SyntaxKind.None))
            return null;
        var value = Enum.Parse<TTo>(node.ToString(), ignoreCase: true);
        return JLeftPadded.Create(value, Format(Leading(node)));
    }
    private JLeftPadded<TTo>? ToLeftPadded<TTo>(SyntaxNode? node)
        where TTo : class, J
    {
        if (node == null)
            return null;
        // find first token to left side of the node that doesn't belong to the node
        // ex: "a is b", "is" is the token to the left side of "b" node
        var leftSideToken = node.GetFirstToken().GetPreviousToken();
        return JLeftPadded.Create(Convert<TTo>(node)!, Format(Leading(leftSideToken)));
    }

    private JRightPadded<TTo>? ToRightPadded<TTo>(SyntaxNode? node)
        where TTo : class, J
    {
        if (node == null)
            return null;

        return JRightPadded.Create(Convert<TTo>(node)!, Format(Trailing(node)));
    }

    private List<JRightPadded<TTo>> ToRightPadded<TFrom, TTo>(IReadOnlyList<TFrom> syntaxList)
        where TFrom : SyntaxNode
        where TTo : class, J
    {
        // return syntaxList.Count == 0 ? [] : syntaxList.SkipLast(1).Select(x => JRightPadded.Create(Convert<TTo>(x)!)).Append(x => JRightPadded.Create(syntaxList.Last(), true)).ToList();
        int totalElements = syntaxList.Count;
        return syntaxList.Select(selector: (element,i) =>
        {
            var isLastElement = totalElements - i - 1 == 0;
            var isTrailingComma = isLastElement && element.GetLastToken().GetNextToken().IsKind(SyntaxKind.CommaToken);
            var markers = isTrailingComma ? Markers.Create(markers: new TrailingComma(Id: Core.Tree.RandomId(), Suffix: Format(Trailing(element.GetLastToken().GetNextToken())))) : Markers.EMPTY;
            return JRightPadded.Create(element: Convert<TTo>(element)!, after: Format(Trailing(element)), markers: markers);
        }).ToList();
    }

    private J.ControlParentheses<TTo> ToControlParentheses<TTo>(SyntaxNode node, SyntaxToken openingToken) where TTo : class, J
    {
        return new J.ControlParentheses<TTo>(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(openingToken)),
            markers: Markers.EMPTY,
            JRightPadded.Create(Convert<TTo>(node)!, Format(Trailing(node))));
    }
    private JContainer<TTo> ToJContainer<TFrom, TTo>(IReadOnlyList<TFrom> syntaxList, SyntaxToken openingToken)
        where TFrom : SyntaxNode
        where TTo : class, J
    {
        if (syntaxList.Count > 0)
        {
            return JContainer.Create(elements: ToRightPadded<TFrom, TTo>(syntaxList: syntaxList), space: Format(Leading(openingToken)));
        }
        else
        {
            J empty = new J.Empty(id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,
                markers: Markers.EMPTY);
            // hack - we use J.Empty to represent the space inside the brackets since we got no real elements in there
            return JContainer.Create(
                elements: [JRightPadded.Create(element: (TTo)empty, after: Format(Trailing(openingToken))) ],
                space: Format(Leading(openingToken)));
        }
    }
}
