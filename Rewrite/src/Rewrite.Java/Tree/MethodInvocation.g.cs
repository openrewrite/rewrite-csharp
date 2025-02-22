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
    public partial class MethodInvocation(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression>? select,
    JContainer<Expression>? typeParameters,
    Identifier name,
    JContainer<Expression> arguments,
    JavaType.Method? methodType
    ) : J, Statement, TypedTree, MethodCall, Expression<MethodInvocation>, TypedTree<MethodInvocation>, J<MethodInvocation>, MutableTree<MethodInvocation>
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
            return v.VisitMethodInvocation(this, p);
        }

        public Guid Id => id;

        public MethodInvocation WithId(Guid newId)
        {
            return newId == id ? this : new MethodInvocation(newId, prefix, markers, _select, _typeParameters, name, _arguments, methodType);
        }
        public Space Prefix => prefix;

        public MethodInvocation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MethodInvocation(id, newPrefix, markers, _select, _typeParameters, name, _arguments, methodType);
        }
        public Markers Markers => markers;

        public MethodInvocation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MethodInvocation(id, prefix, newMarkers, _select, _typeParameters, name, _arguments, methodType);
        }
        private readonly JRightPadded<Expression>? _select = select;
        public Expression? Select => _select?.Element;

        public MethodInvocation WithSelect(Expression? newSelect)
        {
            return Padding.WithSelect(JRightPadded<Expression>.WithElement(_select, newSelect));
        }
        private readonly JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public MethodInvocation WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        public J.Identifier Name => name;

        public MethodInvocation WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new MethodInvocation(id, prefix, markers, _select, _typeParameters, newName, _arguments, methodType);
        }
        private readonly JContainer<Expression> _arguments = arguments;
        public IList<Expression> Arguments => _arguments.GetElements();

        public MethodInvocation WithArguments(IList<Expression> newArguments)
        {
            return Padding.WithArguments(JContainer<Expression>.WithElements(_arguments, newArguments));
        }
        public JavaType.Method? MethodType => methodType;

        public MethodInvocation WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == methodType ? this : new MethodInvocation(id, prefix, markers, _select, _typeParameters, name, _arguments, newMethodType);
        }
        public sealed record PaddingHelper(J.MethodInvocation T)
        {
            public JRightPadded<Expression>? Select => T._select;

            public J.MethodInvocation WithSelect(JRightPadded<Expression>? newSelect)
            {
                return T._select == newSelect ? T : new J.MethodInvocation(T.Id, T.Prefix, T.Markers, newSelect, T._typeParameters, T.Name, T._arguments, T.MethodType);
            }

            public JContainer<Expression>? TypeParameters => T._typeParameters;

            public J.MethodInvocation WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.MethodInvocation(T.Id, T.Prefix, T.Markers, T._select, newTypeParameters, T.Name, T._arguments, T.MethodType);
            }

            public JContainer<Expression> Arguments => T._arguments;

            public J.MethodInvocation WithArguments(JContainer<Expression> newArguments)
            {
                return T._arguments == newArguments ? T : new J.MethodInvocation(T.Id, T.Prefix, T.Markers, T._select, T._typeParameters, T.Name, newArguments, T.MethodType);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MethodInvocation && other.Id == Id;
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