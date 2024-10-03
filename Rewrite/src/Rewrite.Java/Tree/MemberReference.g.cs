//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
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
    ) : J, TypedTree, MethodCall, MutableTree<MemberReference>
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

        public Guid Id => id;

        public MemberReference WithId(Guid newId)
        {
            return newId == id ? this : new MemberReference(newId, prefix, markers, _containing, _typeParameters, _reference, type, methodType, variableType);
        }
        public Space Prefix => prefix;

        public MemberReference WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MemberReference(id, newPrefix, markers, _containing, _typeParameters, _reference, type, methodType, variableType);
        }
        public Markers Markers => markers;

        public MemberReference WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MemberReference(id, prefix, newMarkers, _containing, _typeParameters, _reference, type, methodType, variableType);
        }
        private readonly JRightPadded<Expression> _containing = containing;
        public Expression Containing => _containing.Element;

        public MemberReference WithContaining(Expression newContaining)
        {
            return Padding.WithContaining(_containing.WithElement(newContaining));
        }
        private readonly JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public MemberReference WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        private readonly JLeftPadded<J.Identifier> _reference = reference;
        public J.Identifier Reference => _reference.Element;

        public MemberReference WithReference(J.Identifier newReference)
        {
            return Padding.WithReference(_reference.WithElement(newReference));
        }
        public JavaType? Type => type;

        public MemberReference WithType(JavaType? newType)
        {
            return newType == type ? this : new MemberReference(id, prefix, markers, _containing, _typeParameters, _reference, newType, methodType, variableType);
        }
        public JavaType.Method? MethodType => methodType;

        public MemberReference WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == methodType ? this : new MemberReference(id, prefix, markers, _containing, _typeParameters, _reference, type, newMethodType, variableType);
        }
        public JavaType.Variable? VariableType => variableType;

        public MemberReference WithVariableType(JavaType.Variable? newVariableType)
        {
            return newVariableType == variableType ? this : new MemberReference(id, prefix, markers, _containing, _typeParameters, _reference, type, methodType, newVariableType);
        }
        public sealed record PaddingHelper(J.MemberReference T)
        {
            public JRightPadded<Expression> Containing => T._containing;

            public J.MemberReference WithContaining(JRightPadded<Expression> newContaining)
            {
                return T._containing == newContaining ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, newContaining, T._typeParameters, T._reference, T.Type, T.MethodType, T.VariableType);
            }

            public JContainer<Expression>? TypeParameters => T._typeParameters;

            public J.MemberReference WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, T._containing, newTypeParameters, T._reference, T.Type, T.MethodType, T.VariableType);
            }

            public JLeftPadded<J.Identifier> Reference => T._reference;

            public J.MemberReference WithReference(JLeftPadded<J.Identifier> newReference)
            {
                return T._reference == newReference ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, T._containing, T._typeParameters, newReference, T.Type, T.MethodType, T.VariableType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MemberReference && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}