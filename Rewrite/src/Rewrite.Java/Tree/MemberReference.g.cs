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

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class MemberReference(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression> containing,
    JContainer<Expression>? typeParameters,
    JLeftPadded<Identifier> reference,
    JavaType? type,
    JavaType.Method? methodType,
    JavaType.Variable? variableType
    ) : J, TypedTree, MethodCall, Expression<MemberReference>, TypedTree<MemberReference>, J<MemberReference>, MutableTree<MemberReference>
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

        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitMemberReference(this, p);
        }

        public Guid Id { get;  set; } = id;

        public MemberReference WithId(Guid newId)
        {
            return newId == Id ? this : new MemberReference(newId, Prefix, Markers, _containing, _typeParameters, _reference, Type, MethodType, VariableType);
        }
        public Space Prefix { get;  set; } = prefix;

        public MemberReference WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new MemberReference(Id, newPrefix, Markers, _containing, _typeParameters, _reference, Type, MethodType, VariableType);
        }
        public Markers Markers { get;  set; } = markers;

        public MemberReference WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new MemberReference(Id, Prefix, newMarkers, _containing, _typeParameters, _reference, Type, MethodType, VariableType);
        }
        private JRightPadded<Expression> _containing = containing;
        public Expression Containing => _containing.Element;

        public MemberReference WithContaining(Expression newContaining)
        {
            return Padding.WithContaining(_containing.WithElement(newContaining));
        }
        private JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public MemberReference WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        private JLeftPadded<J.Identifier> _reference = reference;
        public J.Identifier Reference => _reference.Element;

        public MemberReference WithReference(J.Identifier newReference)
        {
            return Padding.WithReference(_reference.WithElement(newReference));
        }
        public JavaType? Type { get;  set; } = type;

        public MemberReference WithType(JavaType? newType)
        {
            return newType == Type ? this : new MemberReference(Id, Prefix, Markers, _containing, _typeParameters, _reference, newType, MethodType, VariableType);
        }
        public JavaType.Method? MethodType { get;  set; } = methodType;

        public MemberReference WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == MethodType ? this : new MemberReference(Id, Prefix, Markers, _containing, _typeParameters, _reference, Type, newMethodType, VariableType);
        }
        public JavaType.Variable? VariableType { get;  set; } = variableType;

        public MemberReference WithVariableType(JavaType.Variable? newVariableType)
        {
            return newVariableType == VariableType ? this : new MemberReference(Id, Prefix, Markers, _containing, _typeParameters, _reference, Type, MethodType, newVariableType);
        }
        public sealed record PaddingHelper(J.MemberReference T)
        {
            public JRightPadded<Expression> Containing { get => T._containing;  set => T._containing = value; }

            public J.MemberReference WithContaining(JRightPadded<Expression> newContaining)
            {
                return Containing == newContaining ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, newContaining, T._typeParameters, T._reference, T.Type, T.MethodType, T.VariableType);
            }

            public JContainer<Expression>? TypeParameters { get => T._typeParameters;  set => T._typeParameters = value; }

            public J.MemberReference WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return TypeParameters == newTypeParameters ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, T._containing, newTypeParameters, T._reference, T.Type, T.MethodType, T.VariableType);
            }

            public JLeftPadded<J.Identifier> Reference { get => T._reference;  set => T._reference = value; }

            public J.MemberReference WithReference(JLeftPadded<J.Identifier> newReference)
            {
                return Reference == newReference ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, T._containing, T._typeParameters, newReference, T.Type, T.MethodType, T.VariableType);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MemberReference && other.Id == Id;
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