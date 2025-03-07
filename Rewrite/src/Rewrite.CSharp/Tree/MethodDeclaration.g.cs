//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108 // 'member1' hides inherited member 'member2'. Use the new keyword if hiding was intended.
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Cs : J
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class MethodDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    IList<AttributeList> attributes,
    IList<J.Modifier> modifiers,
    JContainer<TypeParameter>? typeParameters,
    TypeTree returnTypeExpression,
    JRightPadded<TypeTree>? explicitInterfaceSpecifier,
    J.Identifier name,
    JContainer<Statement> parameters,
    Statement? body,
    JavaType.Method? methodType,
    JContainer<TypeParameterConstraintClause> typeParameterConstraintClauses
    ) : Cs, Statement, TypedTree, TypedTree<MethodDeclaration>, J<MethodDeclaration>, MutableTree<MethodDeclaration>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitMethodDeclaration(this, p);
        }

        public Guid Id => id;

        public MethodDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new MethodDeclaration(newId, prefix, markers, attributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        public Space Prefix => prefix;

        public MethodDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MethodDeclaration(id, newPrefix, markers, attributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        public Markers Markers => markers;

        public MethodDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MethodDeclaration(id, prefix, newMarkers, attributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        public IList<Cs.AttributeList> Attributes => attributes;

        public MethodDeclaration WithAttributes(IList<Cs.AttributeList> newAttributes)
        {
            return newAttributes == attributes ? this : new MethodDeclaration(id, prefix, markers, newAttributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        public IList<J.Modifier> Modifiers => modifiers;

        public MethodDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new MethodDeclaration(id, prefix, markers, attributes, newModifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        private readonly JContainer<Cs.TypeParameter>? _typeParameters = typeParameters;
        public IList<Cs.TypeParameter>? TypeParameters => _typeParameters?.GetElements();

        public MethodDeclaration WithTypeParameters(IList<Cs.TypeParameter>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Cs.TypeParameter>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        public TypeTree ReturnTypeExpression => returnTypeExpression;

        public MethodDeclaration WithReturnTypeExpression(TypeTree newReturnTypeExpression)
        {
            return ReferenceEquals(newReturnTypeExpression, returnTypeExpression) ? this : new MethodDeclaration(id, prefix, markers, attributes, modifiers, _typeParameters, newReturnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        private readonly JRightPadded<TypeTree>? _explicitInterfaceSpecifier = explicitInterfaceSpecifier;
        public TypeTree? ExplicitInterfaceSpecifier => _explicitInterfaceSpecifier?.Element;

        public MethodDeclaration WithExplicitInterfaceSpecifier(TypeTree? newExplicitInterfaceSpecifier)
        {
            return Padding.WithExplicitInterfaceSpecifier(JRightPadded<TypeTree>.WithElement(_explicitInterfaceSpecifier, newExplicitInterfaceSpecifier));
        }
        public J.Identifier Name => name;

        public MethodDeclaration WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new MethodDeclaration(id, prefix, markers, attributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, newName, _parameters, body, methodType, _typeParameterConstraintClauses);
        }
        private readonly JContainer<Statement> _parameters = parameters;
        public IList<Statement> Parameters => _parameters.GetElements();

        public MethodDeclaration WithParameters(IList<Statement> newParameters)
        {
            return Padding.WithParameters(JContainer<Statement>.WithElements(_parameters, newParameters));
        }
        public Statement? Body => body;

        public MethodDeclaration WithBody(Statement? newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new MethodDeclaration(id, prefix, markers, attributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, newBody, methodType, _typeParameterConstraintClauses);
        }
        public JavaType.Method? MethodType => methodType;

        public MethodDeclaration WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == methodType ? this : new MethodDeclaration(id, prefix, markers, attributes, modifiers, _typeParameters, returnTypeExpression, _explicitInterfaceSpecifier, name, _parameters, body, newMethodType, _typeParameterConstraintClauses);
        }
        private readonly JContainer<Cs.TypeParameterConstraintClause> _typeParameterConstraintClauses = typeParameterConstraintClauses;
        public IList<Cs.TypeParameterConstraintClause> TypeParameterConstraintClauses => _typeParameterConstraintClauses.GetElements();

        public MethodDeclaration WithTypeParameterConstraintClauses(IList<Cs.TypeParameterConstraintClause> newTypeParameterConstraintClauses)
        {
            return Padding.WithTypeParameterConstraintClauses(JContainer<Cs.TypeParameterConstraintClause>.WithElements(_typeParameterConstraintClauses, newTypeParameterConstraintClauses));
        }
        public sealed record PaddingHelper(Cs.MethodDeclaration T)
        {
            public JContainer<Cs.TypeParameter>? TypeParameters => T._typeParameters;

            public Cs.MethodDeclaration WithTypeParameters(JContainer<Cs.TypeParameter>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new Cs.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, newTypeParameters, T.ReturnTypeExpression, T._explicitInterfaceSpecifier, T.Name, T._parameters, T.Body, T.MethodType, T._typeParameterConstraintClauses);
            }

            public JRightPadded<TypeTree>? ExplicitInterfaceSpecifier => T._explicitInterfaceSpecifier;

            public Cs.MethodDeclaration WithExplicitInterfaceSpecifier(JRightPadded<TypeTree>? newExplicitInterfaceSpecifier)
            {
                return T._explicitInterfaceSpecifier == newExplicitInterfaceSpecifier ? T : new Cs.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, newExplicitInterfaceSpecifier, T.Name, T._parameters, T.Body, T.MethodType, T._typeParameterConstraintClauses);
            }

            public JContainer<Statement> Parameters => T._parameters;

            public Cs.MethodDeclaration WithParameters(JContainer<Statement> newParameters)
            {
                return T._parameters == newParameters ? T : new Cs.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._explicitInterfaceSpecifier, T.Name, newParameters, T.Body, T.MethodType, T._typeParameterConstraintClauses);
            }

            public JContainer<Cs.TypeParameterConstraintClause> TypeParameterConstraintClauses => T._typeParameterConstraintClauses;

            public Cs.MethodDeclaration WithTypeParameterConstraintClauses(JContainer<Cs.TypeParameterConstraintClause> newTypeParameterConstraintClauses)
            {
                return T._typeParameterConstraintClauses == newTypeParameterConstraintClauses ? T : new Cs.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._explicitInterfaceSpecifier, T.Name, T._parameters, T.Body, T.MethodType, newTypeParameterConstraintClauses);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MethodDeclaration && other.Id == Id;
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}