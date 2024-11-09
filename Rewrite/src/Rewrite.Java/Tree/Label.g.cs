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
    public partial class Label(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Identifier> name,
    Statement statement
    ) : J, Statement, MutableTree<Label>
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
            return v.VisitLabel(this, p);
        }

        public Guid Id => id;

        public Label WithId(Guid newId)
        {
            return newId == id ? this : new Label(newId, prefix, markers, _name, statement);
        }
        public Space Prefix => prefix;

        public Label WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Label(id, newPrefix, markers, _name, statement);
        }
        public Markers Markers => markers;

        public Label WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Label(id, prefix, newMarkers, _name, statement);
        }
        private readonly JRightPadded<J.Identifier> _name = name;
        public J.Identifier Name => _name.Element;

        public Label WithName(J.Identifier newName)
        {
            return Padding.WithName(_name.WithElement(newName));
        }
        public Statement Statement => statement;

        public Label WithStatement(Statement newStatement)
        {
            return ReferenceEquals(newStatement, statement) ? this : new Label(id, prefix, markers, _name, newStatement);
        }
        public sealed record PaddingHelper(J.Label T)
        {
            public JRightPadded<J.Identifier> Name => T._name;

            public J.Label WithName(JRightPadded<J.Identifier> newName)
            {
                return T._name == newName ? T : new J.Label(T.Id, T.Prefix, T.Markers, newName, T.Statement);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Label && other.Id == Id;
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