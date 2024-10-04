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
    public partial class Unknown(
    Guid id,
    Space prefix,
    Markers markers,
    Unknown.Source unknownSource
    ) : J, Statement, Expression, TypeTree, MutableTree<Unknown>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitUnknown(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public Unknown WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
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
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class Source(
    Guid id,
    Space prefix,
    Markers markers,
    string text
        ) : J, MutableTree<Source>
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
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Source && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Unknown && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}