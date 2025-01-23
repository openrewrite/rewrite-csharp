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
    public partial class UsingDirective(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<bool> global,
    JLeftPadded<bool> @static,
    JLeftPadded<bool> @unsafe,
    JRightPadded<J.Identifier>? alias,
    TypeTree namespaceOrType
    ) : Cs, Statement, J<UsingDirective>, MutableTree<UsingDirective>
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
            return v.VisitUsingDirective(this, p);
        }

        public Guid Id => id;

        public UsingDirective WithId(Guid newId)
        {
            return newId == id ? this : new UsingDirective(newId, prefix, markers, _global, _static, _unsafe, _alias, namespaceOrType);
        }
        public Space Prefix => prefix;

        public UsingDirective WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new UsingDirective(id, newPrefix, markers, _global, _static, _unsafe, _alias, namespaceOrType);
        }
        public Markers Markers => markers;

        public UsingDirective WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new UsingDirective(id, prefix, newMarkers, _global, _static, _unsafe, _alias, namespaceOrType);
        }
        private readonly JRightPadded<bool> _global = global;
        public bool Global => _global.Element;

        public UsingDirective WithGlobal(bool newGlobal)
        {
            return Padding.WithGlobal(_global.WithElement(newGlobal));
        }
        private readonly JLeftPadded<bool> _static = @static;
        public bool Static => _static.Element;

        public UsingDirective WithStatic(bool newStatic)
        {
            return Padding.WithStatic(_static.WithElement(newStatic));
        }
        private readonly JLeftPadded<bool> _unsafe = @unsafe;
        public bool Unsafe => _unsafe.Element;

        public UsingDirective WithUnsafe(bool newUnsafe)
        {
            return Padding.WithUnsafe(_unsafe.WithElement(newUnsafe));
        }
        private readonly JRightPadded<J.Identifier>? _alias = alias;
        public J.Identifier? Alias => _alias?.Element;

        public UsingDirective WithAlias(J.Identifier? newAlias)
        {
            return Padding.WithAlias(JRightPadded<J.Identifier>.WithElement(_alias, newAlias));
        }
        public TypeTree NamespaceOrType => namespaceOrType;

        public UsingDirective WithNamespaceOrType(TypeTree newNamespaceOrType)
        {
            return ReferenceEquals(newNamespaceOrType, namespaceOrType) ? this : new UsingDirective(id, prefix, markers, _global, _static, _unsafe, _alias, newNamespaceOrType);
        }
        public sealed record PaddingHelper(Cs.UsingDirective T)
        {
            public JRightPadded<bool> Global => T._global;

            public Cs.UsingDirective WithGlobal(JRightPadded<bool> newGlobal)
            {
                return T._global == newGlobal ? T : new Cs.UsingDirective(T.Id, T.Prefix, T.Markers, newGlobal, T._static, T._unsafe, T._alias, T.NamespaceOrType);
            }

            public JLeftPadded<bool> Static => T._static;

            public Cs.UsingDirective WithStatic(JLeftPadded<bool> newStatic)
            {
                return T._static == newStatic ? T : new Cs.UsingDirective(T.Id, T.Prefix, T.Markers, T._global, newStatic, T._unsafe, T._alias, T.NamespaceOrType);
            }

            public JLeftPadded<bool> Unsafe => T._unsafe;

            public Cs.UsingDirective WithUnsafe(JLeftPadded<bool> newUnsafe)
            {
                return T._unsafe == newUnsafe ? T : new Cs.UsingDirective(T.Id, T.Prefix, T.Markers, T._global, T._static, newUnsafe, T._alias, T.NamespaceOrType);
            }

            public JRightPadded<J.Identifier>? Alias => T._alias;

            public Cs.UsingDirective WithAlias(JRightPadded<J.Identifier>? newAlias)
            {
                return T._alias == newAlias ? T : new Cs.UsingDirective(T.Id, T.Prefix, T.Markers, T._global, T._static, T._unsafe, newAlias, T.NamespaceOrType);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is UsingDirective && other.Id == Id;
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