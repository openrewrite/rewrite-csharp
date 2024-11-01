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
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Cs : J
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class ConstructorInitializer(
    Guid id,
    Space prefix,
    Markers markers,
    Keyword keyword,
    JContainer<Argument> arguments
    ) : Cs, MutableTree<ConstructorInitializer>
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

        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitConstructorInitializer(this, p);
        }

        public Guid Id => id;

        public ConstructorInitializer WithId(Guid newId)
        {
            return newId == id ? this : new ConstructorInitializer(newId, prefix, markers, keyword, _arguments);
        }
        public Space Prefix => prefix;

        public ConstructorInitializer WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ConstructorInitializer(id, newPrefix, markers, keyword, _arguments);
        }
        public Markers Markers => markers;

        public ConstructorInitializer WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ConstructorInitializer(id, prefix, newMarkers, keyword, _arguments);
        }
        public Cs.Keyword Keyword => keyword;

        public ConstructorInitializer WithKeyword(Cs.Keyword newKeyword)
        {
            return ReferenceEquals(newKeyword, keyword) ? this : new ConstructorInitializer(id, prefix, markers, newKeyword, _arguments);
        }
        private readonly JContainer<Cs.Argument> _arguments = arguments;
        public IList<Cs.Argument> Arguments => _arguments.GetElements();

        public ConstructorInitializer WithArguments(IList<Cs.Argument> newArguments)
        {
            return Padding.WithArguments(JContainer<Cs.Argument>.WithElements(_arguments, newArguments));
        }
        public sealed record PaddingHelper(Cs.ConstructorInitializer T)
        {
            public JContainer<Cs.Argument> Arguments => T._arguments;

            public Cs.ConstructorInitializer WithArguments(JContainer<Cs.Argument> newArguments)
            {
                return T._arguments == newArguments ? T : new Cs.ConstructorInitializer(T.Id, T.Prefix, T.Markers, T.Keyword, newArguments);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ConstructorInitializer && other.Id == Id;
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