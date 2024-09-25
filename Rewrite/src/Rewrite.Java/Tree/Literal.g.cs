//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
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
    public partial class Literal(
    Guid id,
    Space prefix,
    Markers markers,
    object? value,
    string? valueSource,
    IList<Literal.UnicodeEscape>? unicodeEscapes,
    JavaType.Primitive type
    ) : J, Expression, TypedTree, MutableTree<Literal>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitLiteral(this, p);
        }

        public Guid Id => id;

        public Literal WithId(Guid newId)
        {
            return newId == id ? this : new Literal(newId, prefix, markers, value, valueSource, unicodeEscapes, type);
        }
        public Space Prefix => prefix;

        public Literal WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Literal(id, newPrefix, markers, value, valueSource, unicodeEscapes, type);
        }
        public Markers Markers => markers;

        public Literal WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Literal(id, prefix, newMarkers, value, valueSource, unicodeEscapes, type);
        }
        public object? Value => value;

        public Literal WithValue(object? newValue)
        {
            return newValue == value ? this : new Literal(id, prefix, markers, newValue, valueSource, unicodeEscapes, type);
        }
        public string? ValueSource => valueSource;

        public Literal WithValueSource(string? newValueSource)
        {
            return newValueSource == valueSource ? this : new Literal(id, prefix, markers, value, newValueSource, unicodeEscapes, type);
        }
        public IList<UnicodeEscape>? UnicodeEscapes => unicodeEscapes;

        public Literal WithUnicodeEscapes(IList<UnicodeEscape>? newUnicodeEscapes)
        {
            return newUnicodeEscapes == unicodeEscapes ? this : new Literal(id, prefix, markers, value, valueSource, newUnicodeEscapes, type);
        }
        public JavaType.Primitive Type => type;

        public Literal WithType(JavaType.Primitive newType)
        {
            return newType == type ? this : new Literal(id, prefix, markers, value, valueSource, unicodeEscapes, newType);
        }
        public sealed record UnicodeEscape(
    int valueSourceIndex,
    string codePoint
        )
        {
            public int ValueSourceIndex => valueSourceIndex;

            public UnicodeEscape WithValueSourceIndex(int newValueSourceIndex)
            {
                return newValueSourceIndex == valueSourceIndex ? this : new UnicodeEscape(newValueSourceIndex, codePoint);
            }
            public string CodePoint => codePoint;

            public UnicodeEscape WithCodePoint(string newCodePoint)
            {
                return newCodePoint == codePoint ? this : new UnicodeEscape(valueSourceIndex, newCodePoint);
            }
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Literal && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}