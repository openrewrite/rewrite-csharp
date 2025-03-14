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
    /// <summary>
    /// Represents a C# delegate declaration which defines a type that can reference methods.
    /// Delegates act as type-safe function pointers and provide the foundation for events in C#.
    /// <br/>
    /// For example:
    /// <code>
    /// // Simple non-generic delegate with single parameter
    /// public delegate void Logger(string message);
    /// // Generic delegate
    /// public delegate T Factory<T>() where T : class, new();
    /// // Delegate with multiple parameters and constraint
    /// public delegate TResult Convert<T, TResult>(T input)
    ///     where T : struct
    ///     where TResult : class;
    /// // Static delegate (C# 11+)
    /// public static delegate int StaticHandler(string msg);
    /// // Protected access
    /// protected delegate bool Validator<T>(T item);
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class DelegateDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    IList<AttributeList> attributes,
    IList<J.Modifier> modifiers,
    JLeftPadded<TypeTree> returnType,
    J.Identifier identifier,
    JContainer<TypeParameter>? typeParameters,
    JContainer<Statement> parameters,
    JContainer<TypeParameterConstraintClause>? typeParameterConstraintClauses
    ) : Cs, Statement, J<DelegateDeclaration>, MutableTree<DelegateDeclaration>
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
            return v.VisitDelegateDeclaration(this, p);
        }

        public Guid Id => id;

        public DelegateDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new DelegateDeclaration(newId, prefix, markers, attributes, modifiers, _returnType, identifier, _typeParameters, _parameters, _typeParameterConstraintClauses);
        }
        public Space Prefix => prefix;

        public DelegateDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new DelegateDeclaration(id, newPrefix, markers, attributes, modifiers, _returnType, identifier, _typeParameters, _parameters, _typeParameterConstraintClauses);
        }
        public Markers Markers => markers;

        public DelegateDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new DelegateDeclaration(id, prefix, newMarkers, attributes, modifiers, _returnType, identifier, _typeParameters, _parameters, _typeParameterConstraintClauses);
        }
        public IList<Cs.AttributeList> Attributes => attributes;

        public DelegateDeclaration WithAttributes(IList<Cs.AttributeList> newAttributes)
        {
            return newAttributes == attributes ? this : new DelegateDeclaration(id, prefix, markers, newAttributes, modifiers, _returnType, identifier, _typeParameters, _parameters, _typeParameterConstraintClauses);
        }
        public IList<J.Modifier> Modifiers => modifiers;

        public DelegateDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new DelegateDeclaration(id, prefix, markers, attributes, newModifiers, _returnType, identifier, _typeParameters, _parameters, _typeParameterConstraintClauses);
        }
        private readonly JLeftPadded<TypeTree> _returnType = returnType;
        public TypeTree ReturnType => _returnType.Element;

        public DelegateDeclaration WithReturnType(TypeTree newReturnType)
        {
            return Padding.WithReturnType(_returnType.WithElement(newReturnType));
        }
        public J.Identifier Identifier => identifier;

        public DelegateDeclaration WithIdentifier(J.Identifier newIdentifier)
        {
            return ReferenceEquals(newIdentifier, identifier) ? this : new DelegateDeclaration(id, prefix, markers, attributes, modifiers, _returnType, newIdentifier, _typeParameters, _parameters, _typeParameterConstraintClauses);
        }
        private readonly JContainer<Cs.TypeParameter>? _typeParameters = typeParameters;
        public IList<Cs.TypeParameter>? TypeParameters => _typeParameters?.GetElements();

        public DelegateDeclaration WithTypeParameters(IList<Cs.TypeParameter>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Cs.TypeParameter>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        private readonly JContainer<Statement> _parameters = parameters;
        public IList<Statement> Parameters => _parameters.GetElements();

        public DelegateDeclaration WithParameters(IList<Statement> newParameters)
        {
            return Padding.WithParameters(JContainer<Statement>.WithElements(_parameters, newParameters));
        }
        private readonly JContainer<Cs.TypeParameterConstraintClause>? _typeParameterConstraintClauses = typeParameterConstraintClauses;
        public IList<Cs.TypeParameterConstraintClause>? TypeParameterConstraintClauses => _typeParameterConstraintClauses?.GetElements();

        public DelegateDeclaration WithTypeParameterConstraintClauses(IList<Cs.TypeParameterConstraintClause>? newTypeParameterConstraintClauses)
        {
            return Padding.WithTypeParameterConstraintClauses(JContainer<Cs.TypeParameterConstraintClause>.WithElementsNullable(_typeParameterConstraintClauses, newTypeParameterConstraintClauses));
        }
        public sealed record PaddingHelper(Cs.DelegateDeclaration T)
        {
            public JLeftPadded<TypeTree> ReturnType => T._returnType;

            public Cs.DelegateDeclaration WithReturnType(JLeftPadded<TypeTree> newReturnType)
            {
                return T._returnType == newReturnType ? T : new Cs.DelegateDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, newReturnType, T.Identifier, T._typeParameters, T._parameters, T._typeParameterConstraintClauses);
            }

            public JContainer<Cs.TypeParameter>? TypeParameters => T._typeParameters;

            public Cs.DelegateDeclaration WithTypeParameters(JContainer<Cs.TypeParameter>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new Cs.DelegateDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, T._returnType, T.Identifier, newTypeParameters, T._parameters, T._typeParameterConstraintClauses);
            }

            public JContainer<Statement> Parameters => T._parameters;

            public Cs.DelegateDeclaration WithParameters(JContainer<Statement> newParameters)
            {
                return T._parameters == newParameters ? T : new Cs.DelegateDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, T._returnType, T.Identifier, T._typeParameters, newParameters, T._typeParameterConstraintClauses);
            }

            public JContainer<Cs.TypeParameterConstraintClause>? TypeParameterConstraintClauses => T._typeParameterConstraintClauses;

            public Cs.DelegateDeclaration WithTypeParameterConstraintClauses(JContainer<Cs.TypeParameterConstraintClause>? newTypeParameterConstraintClauses)
            {
                return T._typeParameterConstraintClauses == newTypeParameterConstraintClauses ? T : new Cs.DelegateDeclaration(T.Id, T.Prefix, T.Markers, T.Attributes, T.Modifiers, T._returnType, T.Identifier, T._typeParameters, T._parameters, newTypeParameterConstraintClauses);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is DelegateDeclaration && other.Id == Id;
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