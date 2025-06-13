using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
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

namespace Rewrite.RewriteCSharp;

[SuppressMessage(category: "ReSharper", checkId: "RedundantOverriddenMember")]
public class CSharpParserVisitor : CSharpSyntaxVisitor<object>
{
    private readonly SemanticModel? _semanticModel;
    private readonly CSharpTypeMapping _typeMapping = new();
    private readonly List<TextSpan> _seenTriviaSpans = [];

    /// <summary>
    /// Creates a parsers that does not perform type attestation
    /// </summary>
    public CSharpParserVisitor()
    {
    }

    /// <summary>
    /// Creates a parsers that  performs type attestation
    /// </summary>
    public CSharpParserVisitor(SemanticModel semanticModel)
    {
        _semanticModel = semanticModel;
    }

    protected virtual Guid GetId()
    {
        return Core.Tree.RandomId();
    }
    public override Cs.CompilationUnit VisitCompilationUnit(CompilationUnitSyntax node)
    {
        // special case when the compilation unit is empty
        var empty = node.GetFirstToken().IsKind(SyntaxKind.None);
        var prefix = Format(empty ? node.GetLeadingTrivia() : Leading(node));
        var cu = new Cs.CompilationUnit(
            id: Core.Tree.RandomId(),
            prefix: prefix,
            markers: Markers.EMPTY,
            sourcePath: node.SyntaxTree.FilePath, //_semanticModel.SyntaxTree.FilePath,
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
                        ParseExceptionResult.Build(ExceptionDispatchInfo.SetCurrentStackTrace(new InvalidOperationException($"Unsupported AST type {node.GetType()}")))
                            .WithTreeType(newTreeType: node.GetType().Name)
                    ]
                ),
                text: node.ToString()
            )
        );
    }

    public override Cs.FileScopeNamespaceDeclaration VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
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

    public override Cs.BlockScopeNamespaceDeclaration VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
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

    public override Cs.ClassDeclaration VisitStructDeclaration(StructDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Value);
    }


    public override Cs.EnumDeclaration VisitEnumDeclaration(EnumDeclarationSyntax node)
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

    public override Cs.ClassDeclaration VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Record);
    }

    public override Cs.ClassDeclaration VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Interface);
    }

    public override Cs.ClassDeclaration VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        return VisitTypeDeclaration(node, type: J.ClassDeclaration.Kind.Types.Class);
    }

    private Cs.ClassDeclaration VisitTypeDeclaration(TypeDeclarationSyntax node, J.ClassDeclaration.Kind.Types type)
    {
        var attributeLists = MapAttributes(m: node.AttributeLists);
        var javaType = MapType( node);
        var hasBaseClass = node.BaseList is { Types.Count: > 0 } &&
                           _semanticModel.GetTypeInfo(expression: node.BaseList.Types[index: 0].Type).Type?.TypeKind == TypeKind.Class;
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

    public override J.Identifier VisitSimpleBaseType(SimpleBaseTypeSyntax node)
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

    public override J.Identifier VisitPrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax node)
    {
        // todo: this is just wrong as we're not capturing parameters
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
        // todo: this should really be mapped to Cs.TypeParameter as J version can't capture attributes
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

    public override J.Identifier VisitIdentifierName(IdentifierNameSyntax node)
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

    /// <returns>Either <see cref="J.MethodInvocation"/> or <see cref="Cs.AliasQualifiedName"/></returns>
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
            var typeInfo = _semanticModel.GetTypeInfo(expression: arg.Expression);
        }

        return (J?)base.VisitInvocationExpression(node);
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

    public override Cs.AttributeList VisitAttributeList(AttributeListSyntax node)
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
    public override Cs.MethodDeclaration VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        Statement body = MapBody(node.Body, node.ExpressionBody, node.SemicolonToken);
        var returnValue = new Cs.MethodDeclaration(
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
            methodType: MapType(node) as JavaType.Method,
            attributes: MapAttributes(node.AttributeLists),
            typeParameterConstraintClauses: MapTypeParameterConstraintClauses(node.ConstraintClauses)
        );

        return returnValue;
    }

    private JContainer<Cs.TypeParameter>? MapTypeParameters(TypeParameterListSyntax? parameters)
    {
        return parameters != null ? ToJContainer<TypeParameterSyntax, Cs.TypeParameter>(parameters.Parameters, parameters.GreaterThanToken) : null;
    }

    public override Cs.UsingDirective VisitUsingDirective(UsingDirectiveSyntax node)
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

    public override J.NullableType VisitNullableType(NullableTypeSyntax node)
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

    public override Cs.Argument VisitArgument(ArgumentSyntax node)
    {
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



    public override Cs.Interpolation VisitInterpolation(InterpolationSyntax node)
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

    public override Cs.Ordering VisitOrdering(OrderingSyntax node)
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

    public override Cs.Subpattern VisitSubpattern(SubpatternSyntax node)
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

    public override Cs.AccessorDeclaration VisitAccessorDeclaration(AccessorDeclarationSyntax node)
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

    public override JContainer<Cs.Argument> VisitArgumentList(ArgumentListSyntax node)
    {
        return ToJContainer<ArgumentSyntax, Cs.Argument>(node.Arguments, node.OpenParenToken);
    }

    public override Cs.ArrayType VisitArrayType(ArrayTypeSyntax node)
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

    public override Statement VisitAssignmentExpression(AssignmentExpressionSyntax node)
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

    public override Cs.Argument VisitAttributeArgument(AttributeArgumentSyntax node)
    {
        var nameColumn = node.NameColon != null
            ? new JRightPadded<J.Identifier>(
                element: MapIdentifier(identifier: node.NameColon.Name.Identifier, type: null),
                after: Format(Trailing(node.NameColon.Name)), markers: Markers.EMPTY)
            : null;
        var expression = Convert<Expression>(node.Expression)!;
        if (node.NameEquals != null)
        {
            expression = new J.Assignment(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(node)),
                markers: Markers.EMPTY,
                expression: JLeftPadded.Create(expression, Format(Leading(node.NameEquals.EqualsToken)))!,
                variable: MapIdentifier(node.NameEquals.Name.Identifier),
                type: MapType(node)
            );
        }
        return new Cs.Argument(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            nameColumn: nameColumn,
            refKindKeyword: null,
            expression: expression
        );
    }

    public override Cs.AwaitExpression VisitAwaitExpression(AwaitExpressionSyntax node)
    {
        return new Cs.AwaitExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!,
            type: MapType( node)
        );
    }

    public override J.Identifier VisitBaseExpression(BaseExpressionSyntax node)
    {
        return MapIdentifier(identifier: node.Token, type: null);
    }

    //todo: type declaration is deeply flawed atm as it tries to differentiate between base class vs interface via semantic analysis, which would not work in recipes
    // never called: handled in class declaration
    public override J? VisitBaseList(BaseListSyntax node)
    {
        return (J?)base.VisitBaseList(node);
    }

    public override Expression VisitBinaryExpression(BinaryExpressionSyntax node)
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
                type: MapType( node),
                modifier: null
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

    public override Cs.BinaryPattern VisitBinaryPattern(BinaryPatternSyntax node)
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

    public override J.Break VisitBreakStatement(BreakStatementSyntax node)
    {
        return new J.Break(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            label: null
        );
    }

    public override J.TypeCast VisitCastExpression(CastExpressionSyntax node)
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

    public override Cs.Try.Catch VisitCatchClause(CatchClauseSyntax node)
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

    public override J.VariableDeclarations VisitCatchDeclaration(CatchDeclarationSyntax node)
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

    public override Cs.CheckedExpression VisitCheckedExpression(CheckedExpressionSyntax node)
    {
        return new Cs.CheckedExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            checkedOrUncheckedKeyword: MapKeyword(node.Keyword)!,
            expression: ToControlParentheses<Expression>(node.Expression, node.OpenParenToken));
    }

    public override Expression VisitQualifiedName(QualifiedNameSyntax node)
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

    public override J.ParameterizedType VisitGenericName(GenericNameSyntax node)
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

    public override JContainer<Expression> VisitTypeArgumentList(TypeArgumentListSyntax node)
    {
        return ToJContainer<TypeSyntax, Expression>(node.Arguments, node.LessThanToken);
    }

    public override Cs.AliasQualifiedName VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
    {
        return new Cs.AliasQualifiedName(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            alias: ToRightPadded<J.Identifier>(node.Alias)!,
            name: Convert<Expression>(node.Name)!
        );
    }

    public override TypeTree VisitPredefinedType(PredefinedTypeSyntax node)
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

    public override J.ArrayDimension VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
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

    public override Cs.PointerType VisitPointerType(PointerTypeSyntax node)
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
        return (J?)base.VisitFunctionPointerType(node);
    }

    public override J? VisitFunctionPointerParameterList(FunctionPointerParameterListSyntax node)
    {
        // This was added in C# 9.0
        return (J?)base.VisitFunctionPointerParameterList(node);
    }

    public override J? VisitFunctionPointerCallingConvention(FunctionPointerCallingConventionSyntax node)
    {
        // This was added in C# 9.0
        return (J?)base.VisitFunctionPointerCallingConvention(node);
    }

    public override J? VisitFunctionPointerUnmanagedCallingConventionList(
        FunctionPointerUnmanagedCallingConventionListSyntax node)
    {
        // This was added in C# 9.0
        return (J?)base.VisitFunctionPointerUnmanagedCallingConventionList(node);
    }

    public override J? VisitFunctionPointerUnmanagedCallingConvention(
        FunctionPointerUnmanagedCallingConventionSyntax node)
    {
        // This was added in C# 9.0
        return (J?)base.VisitFunctionPointerUnmanagedCallingConvention(node);
    }

    public override Cs.TupleType VisitTupleType(TupleTypeSyntax node)
    {
        return new Cs.TupleType(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            elements: JContainer.Create(elements: node.Elements.Select(selector: x => JRightPadded.Create(element: Convert<Cs.TupleElement>(x)!, after: Format(Trailing(x)))).ToList()),
            type: MapType( node)
            );
    }

    public override Cs.TupleElement VisitTupleElement(TupleElementSyntax node)
    {
        return new Cs.TupleElement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            type: Convert<TypeTree>(node.Type)!,
            name: MapIdentifier(identifier: node.Identifier, type: MapType( node.Type)));

    }

    // this should never be called at it's handled at higher levels
    public override J? VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
    {
        return null; // not special representation for generic type definition without type parameters. Ex: List<>
    }

    public override Cs.RefType VisitRefType(RefTypeSyntax node)
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
        return (J?)base.VisitScopedType(node);
    }

    public override J.Parentheses<Expression> VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
    {
        return new J.Parentheses<Expression>(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            tree: MapExpression(es: node.Expression)
        );
    }

    public override Cs.TupleExpression VisitTupleExpression(TupleExpressionSyntax node)
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
            return (J?)base.VisitPrefixUnaryExpression(node);
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

    public override Expression VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
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


    public override Expression VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        var name = Convert<Expression>(node.Name)!;
        Expression result;
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

    public override Expression VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
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
                .FirstOrDefault(predicate: x => x?.Markers?.Contains<MemberBinding>( ) ?? false);
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
    public override Expression VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
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

    public override Expression VisitElementBindingExpression(ElementBindingExpressionSyntax node)
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

    public override Cs.RangeExpression VisitRangeExpression(RangeExpressionSyntax node)
    {
        return new Cs.RangeExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            start: ToRightPadded<Expression>(node.LeftOperand),
            end: Convert<Expression>(node.RightOperand));
    }

    public override Cs.ImplicitElementAccess VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
    {

        return new Cs.ImplicitElementAccess(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            argumentList: ToJContainer<ArgumentSyntax, Cs.Argument>(syntaxList: node.ArgumentList.Arguments, openingToken: node.ArgumentList.OpenBracketToken));
    }

    public override J.Ternary VisitConditionalExpression(ConditionalExpressionSyntax node)
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

    public override J.Identifier VisitThisExpression(ThisExpressionSyntax node)
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

    public override J.Literal VisitLiteralExpression(LiteralExpressionSyntax node)
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
        return (J?)base.VisitMakeRefExpression(node);
    }

    public override J? VisitRefTypeExpression(RefTypeExpressionSyntax node)
    {
        return (J?)base.VisitRefTypeExpression(node);
    }

    public override J? VisitRefValueExpression(RefValueExpressionSyntax node)
    {

        return (J?)base.VisitRefValueExpression(node);
    }

    public override Cs.DefaultExpression VisitDefaultExpression(DefaultExpressionSyntax node)
    {
        return new Cs.DefaultExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeOperator: node.OpenParenToken.IsPresent() ? ToJContainer<TypeSyntax, TypeTree>(syntaxList: [node.Type], openingToken: node.OpenParenToken) : null
        );
    }

    public override J.MethodInvocation VisitTypeOfExpression(TypeOfExpressionSyntax node)
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

    public override J.MethodInvocation VisitSizeOfExpression(SizeOfExpressionSyntax node)
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

    public override J.ArrayAccess VisitElementAccessExpression(ElementAccessExpressionSyntax node)
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

    

    public override JContainer<Cs.Argument> VisitBracketedArgumentList(BracketedArgumentListSyntax node)
    {
        return ToJContainer<ArgumentSyntax, Cs.Argument>(node.Arguments, node.OpenBracketToken);
    }

    public override J? VisitExpressionColon(ExpressionColonSyntax node)
    {
        return (J?)base.VisitExpressionColon(node);
    }

    public override J? VisitNameColon(NameColonSyntax node)
    {
        // This was added in C# 4.0
        return (J?)base.VisitNameColon(node);
    }

    public override Cs.DeclarationExpression VisitDeclarationExpression(DeclarationExpressionSyntax node)
    {
        var result = new Cs.DeclarationExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(node.Type),
            variables: Convert<Cs.VariableDesignation>(node.Designation)!);
        return result;
    }

    public override J.MethodDeclaration VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
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

    public override Cs.RefExpression VisitRefExpression(RefExpressionSyntax node)
    {
        return new Cs.RefExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!);
    }

    public override Cs.Lambda VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        return VisitLambdaExpressionSyntax(node);
    }

    public override Cs.Lambda VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        return VisitLambdaExpressionSyntax(node);
    }

    private Cs.Lambda VisitLambdaExpressionSyntax(LambdaExpressionSyntax node)
    {
        var modifiers = MapModifiers(node.Modifiers);

        J.Lambda.Parameters parameters = node switch
        {
            ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression => new J.Lambda.Parameters(
                id: Core.Tree.RandomId(),
                prefix: Format(Leading(parenthesizedLambdaExpression.ParameterList.OpenParenToken)),
                markers: Markers.EMPTY,
                parenthesized: true,
                elements: MapParameters<J>(pls: parenthesizedLambdaExpression.ParameterList)!.Elements),
            SimpleLambdaExpressionSyntax simpleLambdaExpression => new J.Lambda.Parameters(
                id: Core.Tree.RandomId(),
                prefix: Space.EMPTY,
                markers: Markers.EMPTY,
                parenthesized: false,
                elements: [MapParameter<J>(tps: simpleLambdaExpression.Parameter)]),
            _ => throw new NotSupportedException(message: $"Unsupported type {node.GetType()}")
        };


        var jLambda = new J.Lambda(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            @params: parameters,
            arrow: Format(Leading(node.ArrowToken)),
            body: Convert<J>(node.Body)!,
            type: MapType( node)
        );
        var returnType = node is ParenthesizedLambdaExpressionSyntax{ ReturnType: not null } p ? MapTypeTree(p.ReturnType) : null;
        var csLambda = new Cs.Lambda(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            lambdaExpression: jLambda,
            returnType: returnType,
            modifiers: modifiers
        );
        return csLambda;
    }

    public override Cs.InitializerExpression VisitInitializerExpression(InitializerExpressionSyntax node)
    {
        var initializer = new Cs.InitializerExpression(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            expressions: ToJContainer<ExpressionSyntax, Expression>(syntaxList: node.Expressions, openingToken: node.OpenBraceToken));

        return initializer;
    }

    public override Cs.NewClass VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
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

    public override Cs.NewClass VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
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
        return (J?)base.VisitWithExpression(node);
    }

    public override Expression? VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
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

    public override Cs.NewClass VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
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

    public override J.NewArray VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
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

    public override J.NewArray VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
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

    public override Cs.StackAllocExpression VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
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


    public override Cs.StackAllocExpression VisitImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax node)
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

    public override Cs.CollectionExpression VisitCollectionExpression(CollectionExpressionSyntax node)
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

    public override Expression? VisitExpressionElement(ExpressionElementSyntax node)
    {
        return Convert<Expression>(node.Expression);
    }

    public override Cs.Unary VisitSpreadElement(SpreadElementSyntax node)
    {
        return new Cs.Unary(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            @operator: JLeftPadded.Create(Cs.Unary.Types.Spread, Format(Leading(node.OperatorToken))),
            expression: Convert<Expression>(node.Expression)!,
            type: MapType(node));
    }

    public override Cs.QueryExpression VisitQueryExpression(QueryExpressionSyntax node)
    {
        return new Cs.QueryExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            fromClause: Convert<Cs.FromClause>(node.FromClause)!,
            body: Convert<Cs.QueryBody>(node.Body)!);
    }

    public override Cs.QueryBody VisitQueryBody(QueryBodySyntax node)
    {
        return new Cs.QueryBody(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            clauses: node.Clauses.Select(x => Convert<Cs.QueryClause>(x)!).ToList(),
            selectOrGroup: Convert<Cs.SelectOrGroupClause>(node.SelectOrGroup)!,
            continuation: Convert<Cs.QueryContinuation>(node.Continuation)!);
    }

    public override Cs.FromClause VisitFromClause(FromClauseSyntax node)
    {
        return new Cs.FromClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeIdentifier: node.Type != null ? MapTypeTree(node.Type) : null,
            identifier: JRightPadded.Create(MapIdentifier(node.Identifier, MapType(node.Expression)), Format(Trailing(node.Identifier))),
            expression: Convert<Expression>(node.Expression)!);
    }

    public override Cs.LetClause VisitLetClause(LetClauseSyntax node)
    {
        return new Cs.LetClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: JRightPadded.Create(MapIdentifier(node.Identifier, MapType(node.Expression)), Format(Trailing(node.Identifier))),
            expression: Convert<Expression>(node.Expression)!);
    }

    public override Cs.JoinClause VisitJoinClause(JoinClauseSyntax node)
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

    public override Cs.JoinIntoClause VisitJoinIntoClause(JoinIntoClauseSyntax node)
    {
        return new Cs.JoinIntoClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: MapIdentifier(node.Identifier, MapType(node)));
    }

    public override Cs.WhereClause VisitWhereClause(WhereClauseSyntax node)
    {
        return new Cs.WhereClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            condition: Convert<Expression>(node.Condition)!);
    }

    public override Cs.OrderByClause VisitOrderByClause(OrderByClauseSyntax node)
    {
        return new Cs.OrderByClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            orderings: ToRightPadded<OrderingSyntax, Cs.Ordering>(node.Orderings)!);
    }

    public override Cs.SelectClause VisitSelectClause(SelectClauseSyntax node)
    {
        return new Cs.SelectClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: Convert<Expression>(node.Expression)!);
    }

    public override Cs.GroupClause VisitGroupClause(GroupClauseSyntax node)
    {
        return new Cs.GroupClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            groupExpression: ToRightPadded<Expression>(node.GroupExpression)!,
            key: Convert<Expression>(node.ByExpression)!);
    }

    public override Cs.QueryContinuation VisitQueryContinuation(QueryContinuationSyntax node)
    {
        return new Cs.QueryContinuation(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            identifier: MapIdentifier(node.Identifier, MapType(node.Body)),
            body: Convert<Cs.QueryBody>(node.Body)!);
    }

    public override J.Empty VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
    {
        return new J.Empty(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY
        );
    }

    public override Cs.InterpolatedString VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
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

    public override Cs.IsPattern VisitIsPatternExpression(IsPatternExpressionSyntax node)
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

    public override Cs.StatementExpression VisitThrowExpression(ThrowExpressionSyntax node)
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

    public override Expression? VisitWhenClause(WhenClauseSyntax node)
    {
        return Convert<Expression>(node.Condition);
    }

    public override Cs.DiscardPattern VisitDiscardPattern(DiscardPatternSyntax node)
    {
        return new Cs.DiscardPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            type: MapType(node));
    }

    public override Cs.TypePattern VisitDeclarationPattern(DeclarationPatternSyntax node)
    {
        return new Cs.TypePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeIdentifier: MapTypeTree(node.Type),
            designation: Convert<Cs.VariableDesignation>(node.Designation)
        );
    }

    public override Cs.VarPattern VisitVarPattern(VarPatternSyntax node)
    {
        return new Cs.VarPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            designation: Convert<Cs.VariableDesignation>(node.Designation)!
        );
    }

    public override Cs.RecursivePattern VisitRecursivePattern(RecursivePatternSyntax node)
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

    public override Cs.PositionalPatternClause VisitPositionalPatternClause(PositionalPatternClauseSyntax node)
    {
        return new Cs.PositionalPatternClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            subpatterns: ToJContainer<SubpatternSyntax, Cs.Subpattern>(node.Subpatterns, node.OpenParenToken));
    }

    public override Cs.PropertyPatternClause VisitPropertyPatternClause(PropertyPatternClauseSyntax node)
    {
        return new Cs.PropertyPatternClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            subpatterns: ToJContainer<SubpatternSyntax, Expression>(node.Subpatterns, node.OpenBraceToken));
    }

    public override Cs.ConstantPattern VisitConstantPattern(ConstantPatternSyntax node)
    {
        return new Cs.ConstantPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            value: Convert<Expression>(node.Expression)!
        );
    }

    public override Cs.ParenthesizedPattern VisitParenthesizedPattern(ParenthesizedPatternSyntax node)
    {
        return new Cs.ParenthesizedPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: ToJContainer<PatternSyntax, Cs.Pattern>([node.Pattern], node.OpenParenToken));
    }

    public override Cs.RelationalPattern VisitRelationalPattern(RelationalPatternSyntax node)
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

    public override Cs.TypePattern VisitTypePattern(TypePatternSyntax node)
    {
        return new Cs.TypePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeIdentifier: MapTypeTree(node.Type),
            designation: null
        );
    }

    public override Cs.UnaryPattern VisitUnaryPattern(UnaryPatternSyntax node)
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
    public override Cs.ListPattern VisitListPattern(ListPatternSyntax node)
    {
        return new Cs.ListPattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            patterns: ToJContainer<PatternSyntax, Cs.Pattern>(node.Patterns, node.OpenBracketToken),
            designation: Convert<Cs.VariableDesignation>(node.Designation)
        );
    }

    public override Cs.SlicePattern VisitSlicePattern(SlicePatternSyntax node)
    {
        return new Cs.SlicePattern(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY
        );
    }

    public override J.Literal VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
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

    public override Expression? VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
    {
        return Convert<Expression>(node.Value);
    }

    public override J.Literal VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
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

    public override Statement? VisitGlobalStatement(GlobalStatementSyntax node)
    {
        return (Statement?)node.Statement.Accept(visitor: this);
    }

    public override J.MethodDeclaration? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
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

    public override Statement? VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
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

    public override J.VariableDeclarations VisitVariableDeclaration(VariableDeclarationSyntax node)
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

    public override J.VariableDeclarations.NamedVariable VisitVariableDeclarator(VariableDeclaratorSyntax node)
    {
        var javaType = MapType( node) as JavaType.Variable;
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
                type: javaType?.Type,
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
        return (J?)base.VisitEqualsValueClause(node);
    }

    public override Cs.SingleVariableDesignation VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
    {
        return new Cs.SingleVariableDesignation(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            name: MapIdentifier(identifier: node.Identifier, type: MapType( node)));
    }

    public override Cs.DiscardVariableDesignation VisitDiscardDesignation(DiscardDesignationSyntax node)
    {
        return new Cs.DiscardVariableDesignation(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            discard: MapIdentifier(identifier: node.UnderscoreToken, type: MapType( node)));
    }

    public override Cs.ParenthesizedVariableDesignation VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
    {
        return new Cs.ParenthesizedVariableDesignation(
            id: Core.Tree.RandomId(),
            prefix: Space.EMPTY,
            markers: Markers.EMPTY,
            variables: ToJContainer<VariableDesignationSyntax, Cs.VariableDesignation>(syntaxList: node.Variables, openingToken: node.OpenParenToken),
            type: MapType( node));
    }

    public override Cs.ExpressionStatement VisitExpressionStatement(ExpressionStatementSyntax node)
    {
        return new Cs.ExpressionStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToRightPadded<Expression>(node.Expression)!
        );
    }

    public override J.Empty VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return new J.Empty(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY
        );
    }

    public override J.Label VisitLabeledStatement(LabeledStatementSyntax node)
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

    public override Cs.GotoStatement VisitGotoStatement(GotoStatementSyntax node)
    {

        return new Cs.GotoStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            caseOrDefaultKeyword: MapKeyword(node.CaseOrDefaultKeyword),
            target: Convert<Expression>(node.Expression)
        );
    }

    public override J.Continue VisitContinueStatement(ContinueStatementSyntax node)
    {
        return new J.Continue(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            label: null
        );
    }

    public override J.Return VisitReturnStatement(ReturnStatementSyntax node)
    {
        return new J.Return(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: node.Expression != null ? Convert<Expression>(node.Expression!) : null
        );
    }

    public override J.Throw VisitThrowStatement(ThrowStatementSyntax node)
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

    public override Cs.Yield VisitYieldStatement(YieldStatementSyntax node)
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

    public override J.WhileLoop VisitWhileStatement(WhileStatementSyntax node)
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

    public override J.DoWhileLoop VisitDoStatement(DoStatementSyntax node)
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

    public override J.ForLoop VisitForStatement(ForStatementSyntax node)
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

    public override Statement VisitForEachStatement(ForEachStatementSyntax node)
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

    public override Cs.ForEachVariableLoop VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
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

    public override  Cs.UsingStatement VisitUsingStatement(UsingStatementSyntax node)
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

    public override Cs.FixedStatement VisitFixedStatement(FixedStatementSyntax node)
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

    public override Cs.CheckedStatement VisitCheckedStatement(CheckedStatementSyntax node)
    {
        return new Cs.CheckedStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            keyword: MapKeyword(node.Keyword)!,
            block: Convert<J.Block>(node.Block)!
        );
    }

    public override Cs.UnsafeStatement VisitUnsafeStatement(UnsafeStatementSyntax node)
    {
        return new Cs.UnsafeStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            block: Convert<J.Block>(node.Block)!
        );
    }

    public override Cs.LockStatement VisitLockStatement(LockStatementSyntax node)
    {
        return new Cs.LockStatement(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToControlParentheses<Expression>(node.Expression, node.OpenParenToken)!,
            statement: ToRightPadded<Statement>(node.Statement)!
        );
    }

    public override J.If VisitIfStatement(IfStatementSyntax node)
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

    public override J.If.Else VisitElseClause(ElseClauseSyntax node)
    {
        return new J.If.Else(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            body: MapStatement(statementSyntax: node.Statement)
        );
    }

    public override Statement VisitSwitchStatement(SwitchStatementSyntax node)
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


    public override Cs.SwitchSection VisitSwitchSection(SwitchSectionSyntax node)
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

    public override Cs.CasePatternSwitchLabel VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
    {
        return new Cs.CasePatternSwitchLabel(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: Convert<Cs.Pattern>(node.Pattern)!,
            whenClause: ToLeftPadded<Expression>(node.WhenClause?.Condition),
            colonToken: Format(Leading(node.ColonToken)));
    }

    public override Cs.CasePatternSwitchLabel VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
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

    public override Cs.DefaultSwitchLabel VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
    {
        return new Cs.DefaultSwitchLabel(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            colonToken: Format(Leading(node.ColonToken)));
    }

    public override Cs.SwitchExpression VisitSwitchExpression(SwitchExpressionSyntax node)
    {
        return new Cs.SwitchExpression(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: ToRightPadded<Expression>(node.GoverningExpression)!,
            arms: ToJContainer<SwitchExpressionArmSyntax, Cs.SwitchExpressionArm>(node.Arms, node.OpenBraceToken));
    }

    public override Cs.SwitchExpressionArm VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
    {
        return new Cs.SwitchExpressionArm(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            pattern: Convert<Cs.Pattern>(node.Pattern)!,
            whenExpression: ToLeftPadded<Expression>(node.WhenClause?.Condition),
            expression: ToLeftPadded<Expression>(node.Expression)!);
    }

    public override Cs.Try VisitTryStatement(TryStatementSyntax node)
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

    public override J.ControlParentheses<Expression> VisitCatchFilterClause(CatchFilterClauseSyntax node)
    {
        return new J.ControlParentheses<Expression>(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node.OpenParenToken)),
            markers: Markers.EMPTY,
            tree: ToRightPadded<Expression>(node.FilterExpression)!
        );
    }

    public override J.Block? VisitFinallyClause(FinallyClauseSyntax node)
    {
        return Convert<J.Block>(node.Block);
    }

    public override Cs.ExternAlias VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
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

    public override J.Identifier VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
    {
        return MapIdentifier(identifier: node.Identifier, type: null);
    }

    public override J? VisitAttributeArgumentList(AttributeArgumentListSyntax node)
    {
        return (J?)base.VisitAttributeArgumentList(node);
    }

    public override J? VisitNameEquals(NameEqualsSyntax node)
    {
        return (J?)base.VisitNameEquals(node);
    }

    public override Cs.DelegateDeclaration VisitDelegateDeclaration(DelegateDeclarationSyntax node)
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

    public override Cs.EnumMemberDeclaration VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        return new Cs.EnumMemberDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(node.AttributeLists),
            name: MapIdentifier(node.Identifier),
            initializer: ToLeftPadded<Expression>(node.EqualsValue?.Value));

    }

    public override Cs.ConstructorConstraint VisitConstructorConstraint(ConstructorConstraintSyntax node)
    {
        return new Cs.ConstructorConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY);
    }

    public override Cs.ClassOrStructConstraint VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
    {
        return new Cs.ClassOrStructConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            kind: node.ClassOrStructKeyword.IsKind(SyntaxKind.ClassKeyword) ? Cs.ClassOrStructConstraint.TypeKind.Class : Cs.ClassOrStructConstraint.TypeKind.Struct);
    }

    public override Cs.TypeConstraint VisitTypeConstraint(TypeConstraintSyntax node)
    {
        return new Cs.TypeConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            typeExpression: Convert<TypeTree>(node.Type)!);
    }

    public override Cs.DefaultConstraint VisitDefaultConstraint(DefaultConstraintSyntax node)
    {
        return new Cs.DefaultConstraint(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY);
    }

    public override Cs.AllowsConstraintClause VisitAllowsConstraintClause(AllowsConstraintClauseSyntax node)
    {
        return new Cs.AllowsConstraintClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expressions: JContainer.Create(elements: ToRightPadded<AllowsConstraintSyntax, Cs.AllowsConstraint>(syntaxList: node.Constraints)));
    }

    public override J? VisitRefStructConstraint(RefStructConstraintSyntax node)
    {
        return (J?)base.VisitRefStructConstraint(node);
    }

    public override Statement VisitFieldDeclaration(FieldDeclarationSyntax node)
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

    public override Cs.EventDeclaration VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
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

    public override NameTree? VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
    {
        return Convert<NameTree>(node.Name);
    }

    public override Cs.OperatorDeclaration VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        var @operator = node.OperatorToken.Kind() switch
        {
            SyntaxKind.PlusToken => Cs.OperatorDeclaration.Operator.Plus,
            SyntaxKind.MinusToken => Cs.OperatorDeclaration.Operator.Minus,
            SyntaxKind.ExclamationToken => Cs.OperatorDeclaration.Operator.Bang,
            SyntaxKind.TildeToken => Cs.OperatorDeclaration.Operator.Tilde,
            SyntaxKind.PlusPlusToken => Cs.OperatorDeclaration.Operator.PlusPlus,
            SyntaxKind.MinusMinusToken => Cs.OperatorDeclaration.Operator.MinusMinus,
            SyntaxKind.AsteriskToken => Cs.OperatorDeclaration.Operator.Star,
            SyntaxKind.SlashToken => Cs.OperatorDeclaration.Operator.Division,
            SyntaxKind.PercentToken => Cs.OperatorDeclaration.Operator.Percent,
            SyntaxKind.LessThanLessThanToken => Cs.OperatorDeclaration.Operator.LeftShift,
            SyntaxKind.GreaterThanGreaterThanToken => Cs.OperatorDeclaration.Operator.RightShift,
            SyntaxKind.LessThanToken => Cs.OperatorDeclaration.Operator.LessThan,
            SyntaxKind.GreaterThanToken => Cs.OperatorDeclaration.Operator.GreaterThan,
            SyntaxKind.LessThanEqualsToken => Cs.OperatorDeclaration.Operator.LessThanEquals,
            SyntaxKind.GreaterThanEqualsToken => Cs.OperatorDeclaration.Operator.GreaterThanEquals,
            SyntaxKind.EqualsEqualsToken => Cs.OperatorDeclaration.Operator.Equals,
            SyntaxKind.ExclamationEqualsToken => Cs.OperatorDeclaration.Operator.NotEquals,
            SyntaxKind.AmpersandToken => Cs.OperatorDeclaration.Operator.Ampersand,
            SyntaxKind.BarToken => Cs.OperatorDeclaration.Operator.Bar,
            SyntaxKind.CaretToken => Cs.OperatorDeclaration.Operator.Caret,
            SyntaxKind.TrueKeyword => Cs.OperatorDeclaration.Operator.True,
            SyntaxKind.FalseKeyword => Cs.OperatorDeclaration.Operator.False,
            _ => throw new InvalidOperationException($"Unsupported operator {node.OperatorToken}")
        };
        return new Cs.OperatorDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(node.AttributeLists),
            modifiers: MapModifiers(node.Modifiers),
            explicitInterfaceSpecifier: MapExplicitInterfaceSpecifier(node.ExplicitInterfaceSpecifier),
            operatorKeyword: MapKeyword(node.OperatorKeyword)!,
            checkedKeyword: MapKeyword(node.CheckedKeyword),
            operatorToken: JLeftPadded.Create(@operator, Format(Leading(node.OperatorToken))),
            returnType: Convert<TypeTree>(node.ReturnType)!,
            parameters: ToJContainer<ParameterSyntax, Expression>(node.ParameterList.Parameters, node.ParameterList.OpenParenToken),
            body: MapBody(node.Body, node.ExpressionBody, node.SemicolonToken),
            methodType: MapType(node) as JavaType.Method);

    }

    public override Statement VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
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

    public override Statement VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
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

    public override Cs.ConstructorInitializer VisitConstructorInitializer(ConstructorInitializerSyntax node)
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

    public override Statement VisitDestructorDeclaration(DestructorDeclarationSyntax node)
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

    private JRightPadded<TypeTree>? MapExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax? node)
    {
        var retval = node != null
            ? new JRightPadded<TypeTree>(
                element: Convert<TypeTree>(node.Name)!,
                after: Format(Leading(node.DotToken)),
                markers: Markers.EMPTY
            )
            : null;
        return retval;
    }
    public override Cs.PropertyDeclaration VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        var typeTree = Convert<TypeTree>(node.Type)!;
        return new Cs.PropertyDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(m: node.AttributeLists) ?? [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: typeTree,
            interfaceSpecifier: MapExplicitInterfaceSpecifier(node.ExplicitInterfaceSpecifier),
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

    public override Cs.ArrowExpressionClause VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
    {
        return new Cs.ArrowExpressionClause(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            expression: MapExpression(node.Expression)!
        );
    }

    public override Cs.EventDeclaration VisitEventDeclaration(EventDeclarationSyntax node)
    {

        var typeTree = Convert<TypeTree>(node.Type)!;
        return new Cs.EventDeclaration(
            id: Core.Tree.RandomId(),
            prefix: Format(Leading(node)),
            markers: Markers.EMPTY,
            attributeLists: MapAttributes(m: node.AttributeLists) ?? [],
            modifiers: MapModifiers(stl: node.Modifiers),
            typeExpression: JLeftPadded.Create(typeTree, Format(Leading(node.EventKeyword))),
            interfaceSpecifier: MapExplicitInterfaceSpecifier(node.ExplicitInterfaceSpecifier),
            name: MapIdentifier(identifier: node.Identifier, type: typeTree.Type),
            accessors: node.AccessorList != null ? ToJContainer<AccessorDeclarationSyntax, Statement>(node.AccessorList.Accessors, node.AccessorList.OpenBraceToken) : null

        );

    }

    public override Statement VisitIndexerDeclaration(IndexerDeclarationSyntax node)
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
            explicitInterfaceSpecifier: MapExplicitInterfaceSpecifier(node.ExplicitInterfaceSpecifier),
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

    public override J? VisitParameterList(ParameterListSyntax node)
    {
        return (J?)base.VisitParameterList(node);
    }

    public override J? VisitBracketedParameterList(BracketedParameterListSyntax node)
    {
        return (J?)base.VisitBracketedParameterList(node);
    }

    public override J? VisitFunctionPointerParameter(FunctionPointerParameterSyntax node)
    {
        return (J?)base.VisitFunctionPointerParameter(node);
    }

    public override J? VisitIncompleteMember(IncompleteMemberSyntax node)
    {
        return (J?)base.VisitIncompleteMember(node);
    }

    public override J? VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
    {
        return (J?)base.VisitSkippedTokensTrivia(node);
    }

    public override J? VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
    {
        return (J?)base.VisitDocumentationCommentTrivia(node);
    }

    public override J? VisitTypeCref(TypeCrefSyntax node)
    {
        return (J?)base.VisitTypeCref(node);
    }

    public override J? VisitQualifiedCref(QualifiedCrefSyntax node)
    {
        return (J?)base.VisitQualifiedCref(node);
    }

    public override J? VisitNameMemberCref(NameMemberCrefSyntax node)
    {
        return (J?)base.VisitNameMemberCref(node);
    }

    public override J? VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
    {
        return (J?)base.VisitIndexerMemberCref(node);
    }

    public override J? VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
    {
        return (J?)base.VisitOperatorMemberCref(node);
    }

    public override J? VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
    {
        return (J?)base.VisitConversionOperatorMemberCref(node);
    }

    public override J? VisitCrefParameterList(CrefParameterListSyntax node)
    {
        return (J?)base.VisitCrefParameterList(node);
    }

    public override J? VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
    {
        return (J?)base.VisitCrefBracketedParameterList(node);
    }

    public override J? VisitCrefParameter(CrefParameterSyntax node)
    {
        return (J?)base.VisitCrefParameter(node);
    }

    public override J? VisitXmlElement(XmlElementSyntax node)
    {
        return (J?)base.VisitXmlElement(node);
    }

    public override J? VisitXmlElementStartTag(XmlElementStartTagSyntax node)
    {
        return (J?)base.VisitXmlElementStartTag(node);
    }

    public override J? VisitXmlElementEndTag(XmlElementEndTagSyntax node)
    {
        return (J?)base.VisitXmlElementEndTag(node);
    }

    public override J? VisitXmlEmptyElement(XmlEmptyElementSyntax node)
    {
        return (J?)base.VisitXmlEmptyElement(node);
    }

    public override J? VisitXmlName(XmlNameSyntax node)
    {
        return (J?)base.VisitXmlName(node);
    }

    public override J? VisitXmlPrefix(XmlPrefixSyntax node)
    {
        return (J?)base.VisitXmlPrefix(node);
    }

    public override J? VisitXmlTextAttribute(XmlTextAttributeSyntax node)
    {
        return (J?)base.VisitXmlTextAttribute(node);
    }

    public override J? VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
    {
        return (J?)base.VisitXmlCrefAttribute(node);
    }

    public override J? VisitXmlNameAttribute(XmlNameAttributeSyntax node)
    {
        return (J?)base.VisitXmlNameAttribute(node);
    }

    public override J? VisitXmlText(XmlTextSyntax node)
    {
        return (J?)base.VisitXmlText(node);
    }

    public override J? VisitXmlCDataSection(XmlCDataSectionSyntax node)
    {
        return (J?)base.VisitXmlCDataSection(node);
    }

    public override J? VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
    {
        return (J?)base.VisitXmlProcessingInstruction(node);
    }

    public override J? VisitXmlComment(XmlCommentSyntax node)
    {
        return (J?)base.VisitXmlComment(node);
    }

    public override J? VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitIfDirectiveTrivia(node);
    }

    public override J? VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitElifDirectiveTrivia(node);
    }

    public override J? VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitElseDirectiveTrivia(node);
    }

    public override J? VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitEndIfDirectiveTrivia(node);
    }

    public override J? VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitRegionDirectiveTrivia(node);
    }

    public override J? VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitEndRegionDirectiveTrivia(node);
    }

    public override J? VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitErrorDirectiveTrivia(node);
    }

    public override J? VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitWarningDirectiveTrivia(node);
    }

    public override J? VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitBadDirectiveTrivia(node);
    }

    public override J? VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitDefineDirectiveTrivia(node);
    }

    public override J? VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitUndefDirectiveTrivia(node);
    }

    public override J? VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitLineDirectiveTrivia(node);
    }

    public override J? VisitLineDirectivePosition(LineDirectivePositionSyntax node)
    {
        return (J?)base.VisitLineDirectivePosition(node);
    }

    public override J? VisitLineSpanDirectiveTrivia(LineSpanDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitLineSpanDirectiveTrivia(node);
    }

    public override J? VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitPragmaWarningDirectiveTrivia(node);
    }

    public override J? VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitPragmaChecksumDirectiveTrivia(node);
    }

    public override J? VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitReferenceDirectiveTrivia(node);
    }

    public override J? VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitLoadDirectiveTrivia(node);
    }

    public override J? VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitShebangDirectiveTrivia(node);
    }

    public override J? VisitNullableDirectiveTrivia(NullableDirectiveTriviaSyntax node)
    {
        return (J?)base.VisitNullableDirectiveTrivia(node);
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

    private IList<J.Modifier> MapModifiers(SyntaxTokenList stl) => MapModifiers(SyntaxFactory.Token(SyntaxKind.None), stl);
    private IList<J.Modifier> MapModifiers(SyntaxToken asyncKeyword, SyntaxTokenList stl)
    {
        var modifiers = new List<J.Modifier>();
        if (asyncKeyword.IsPresent())
        {
            modifiers.Add(MapModifier(asyncKeyword));
        }
        modifiers.AddRange(stl.Select(MapModifier));
        return modifiers;

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
        return _typeMapping.Type(roslynSymbol: _semanticModel.GetTypeInfo(expression: ins).Type);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    private JavaType MapType(SyntaxNode ins)
    {
        return _typeMapping.Type(roslynSymbol: _semanticModel?.GetDeclaredSymbol(declaration: ins) ?? _semanticModel?.GetTypeInfo(ins).Type);
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
