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
    public partial class ParameterizedType(
    Guid id,
    Space prefix,
    Markers markers,
    NameTree clazz,
    JContainer<Expression>? typeParameters,
    JavaType? type
    ) : J,TypeTree,Expression    {
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
            return v.VisitParameterizedType(this, p);
        }

        public Guid Id { get;  set; } = id;

        public ParameterizedType WithId(Guid newId)
        {
            return newId == Id ? this : new ParameterizedType(newId, Prefix, Markers, Clazz, _typeParameters, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public ParameterizedType WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new ParameterizedType(Id, newPrefix, Markers, Clazz, _typeParameters, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public ParameterizedType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new ParameterizedType(Id, Prefix, newMarkers, Clazz, _typeParameters, Type);
        }
        public NameTree Clazz { get;  set; } = clazz;

        public ParameterizedType WithClazz(NameTree newClazz)
        {
            return ReferenceEquals(newClazz, Clazz) ? this : new ParameterizedType(Id, Prefix, Markers, newClazz, _typeParameters, Type);
        }
        private JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public ParameterizedType WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        public JavaType? Type { get;  set; } = type;

        public ParameterizedType WithType(JavaType? newType)
        {
            return newType == Type ? this : new ParameterizedType(Id, Prefix, Markers, Clazz, _typeParameters, newType);
        }
        public sealed record PaddingHelper(J.ParameterizedType T)
        {
            public JContainer<Expression>? TypeParameters { get => T._typeParameters;  set => T._typeParameters = value; }

            public J.ParameterizedType WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return TypeParameters == newTypeParameters ? T : new J.ParameterizedType(T.Id, T.Prefix, T.Markers, T.Clazz, newTypeParameters, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ParameterizedType && other.Id == Id;
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