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
    /// <summary>
    /// A tree node that represents an unparsed element.
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class Unknown(
    Guid id,
    Space prefix,
    Markers markers,
    Unknown.Source unknownSource
    ) : J, Statement, Expression, TypeTree, Expression<Unknown>, TypedTree<Unknown>, J<Unknown>, TypeTree<Unknown>, MutableTree<Unknown>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitUnknown(this, p);
        }

        public Guid Id => id;

        public Unknown WithId(Guid newId)
        {
            return newId == id ? this : new Unknown(newId, prefix, markers, unknownSource);
        }
        public Space Prefix => prefix;

        public Unknown WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Unknown(id, newPrefix, markers, unknownSource);
        }
        public Markers Markers => markers;

        public Unknown WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Unknown(id, prefix, newMarkers, unknownSource);
        }
        public Source UnknownSource => unknownSource;

        public Unknown WithUnknownSource(Source newUnknownSource)
        {
            return ReferenceEquals(newUnknownSource, unknownSource) ? this : new Unknown(id, prefix, markers, newUnknownSource);
        }
        /// <summary>
        /// This class only exists to clean up the printed results from `SearchResult` markers.
        /// Without the marker the comments will print before the LST prefix.
        /// </summary>
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class Source(
    Guid id,
    Space prefix,
    Markers markers,
    string text
        ) : J, J<Source>, MutableTree<Source>
        {
            public J? AcceptJava<P>(JavaVisitor<P> v, P p)
            {
                return v.VisitUnknownSource(this, p);
            }

            public Guid Id => id;

            public Source WithId(Guid newId)
            {
                return newId == id ? this : new Source(newId, prefix, markers, text);
            }
            public Space Prefix => prefix;

            public Source WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Source(id, newPrefix, markers, text);
            }
            public Markers Markers => markers;

            public Source WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Source(id, prefix, newMarkers, text);
            }
            public string Text => text;

            public Source WithText(string newText)
            {
                return newText == text ? this : new Source(id, prefix, markers, newText);
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Source && other.Id == Id;
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Unknown && other.Id == Id;
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