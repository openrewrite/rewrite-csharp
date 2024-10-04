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
    public partial class NewArray(
    Guid id,
    Space prefix,
    Markers markers,
    TypeTree? typeExpression,
    IList<ArrayDimension> dimensions,
    JContainer<Expression>? initializer,
    JavaType? type
    ) : J, Expression, TypedTree, MutableTree<NewArray>
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
            return v.VisitNewArray(this, p);
        }

        public Guid Id => id;

        public NewArray WithId(Guid newId)
        {
            return newId == id ? this : new NewArray(newId, prefix, markers, typeExpression, dimensions, _initializer, type);
        }
        public Space Prefix => prefix;

        public NewArray WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NewArray(id, newPrefix, markers, typeExpression, dimensions, _initializer, type);
        }
        public Markers Markers => markers;

        public NewArray WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NewArray(id, prefix, newMarkers, typeExpression, dimensions, _initializer, type);
        }
        public TypeTree? TypeExpression => typeExpression;

        public NewArray WithTypeExpression(TypeTree? newTypeExpression)
        {
            return ReferenceEquals(newTypeExpression, typeExpression) ? this : new NewArray(id, prefix, markers, newTypeExpression, dimensions, _initializer, type);
        }
        public IList<J.ArrayDimension> Dimensions => dimensions;

        public NewArray WithDimensions(IList<J.ArrayDimension> newDimensions)
        {
            return newDimensions == dimensions ? this : new NewArray(id, prefix, markers, typeExpression, newDimensions, _initializer, type);
        }
        private readonly JContainer<Expression>? _initializer = initializer;
        public IList<Expression>? Initializer => _initializer?.GetElements();

        public NewArray WithInitializer(IList<Expression>? newInitializer)
        {
            return Padding.WithInitializer(JContainer<Expression>.WithElementsNullable(_initializer, newInitializer));
        }
        public JavaType? Type => type;

        public NewArray WithType(JavaType? newType)
        {
            return newType == type ? this : new NewArray(id, prefix, markers, typeExpression, dimensions, _initializer, newType);
        }
        public sealed record PaddingHelper(J.NewArray T)
        {
            public JContainer<Expression>? Initializer => T._initializer;

            public J.NewArray WithInitializer(JContainer<Expression>? newInitializer)
            {
                return T._initializer == newInitializer ? T : new J.NewArray(T.Id, T.Prefix, T.Markers, T.TypeExpression, T.Dimensions, newInitializer, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NewArray && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}