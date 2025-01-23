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
    public partial class EnumValueSet(
    Guid id,
    Space prefix,
    Markers markers,
    IList<JRightPadded<EnumValue>> enums,
    bool terminatedWithSemicolon
    ) : J, Statement, J<EnumValueSet>, MutableTree<EnumValueSet>
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
            return v.VisitEnumValueSet(this, p);
        }

        public Guid Id => id;

        public EnumValueSet WithId(Guid newId)
        {
            return newId == id ? this : new EnumValueSet(newId, prefix, markers, _enums, terminatedWithSemicolon);
        }
        public Space Prefix => prefix;

        public EnumValueSet WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new EnumValueSet(id, newPrefix, markers, _enums, terminatedWithSemicolon);
        }
        public Markers Markers => markers;

        public EnumValueSet WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new EnumValueSet(id, prefix, newMarkers, _enums, terminatedWithSemicolon);
        }
        private readonly IList<JRightPadded<J.EnumValue>> _enums = enums;
        public IList<J.EnumValue> Enums => _enums.Elements();

        public EnumValueSet WithEnums(IList<J.EnumValue> newEnums)
        {
            return Padding.WithEnums(_enums.WithElements(newEnums));
        }
        public bool TerminatedWithSemicolon => terminatedWithSemicolon;

        public EnumValueSet WithTerminatedWithSemicolon(bool newTerminatedWithSemicolon)
        {
            return newTerminatedWithSemicolon == terminatedWithSemicolon ? this : new EnumValueSet(id, prefix, markers, _enums, newTerminatedWithSemicolon);
        }
        public sealed record PaddingHelper(J.EnumValueSet T)
        {
            public IList<JRightPadded<J.EnumValue>> Enums => T._enums;

            public J.EnumValueSet WithEnums(IList<JRightPadded<J.EnumValue>> newEnums)
            {
                return T._enums == newEnums ? T : new J.EnumValueSet(T.Id, T.Prefix, T.Markers, newEnums, T.TerminatedWithSemicolon);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is EnumValueSet && other.Id == Id;
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