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
    public partial class FileScopeNamespaceDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression> name,
    IList<JRightPadded<ExternAlias>> externs,
    IList<JRightPadded<UsingDirective>> usings,
    IList<JRightPadded<Statement>> members
    ) : Cs, Statement, J<FileScopeNamespaceDeclaration>, MutableTree<FileScopeNamespaceDeclaration>
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
            return v.VisitFileScopeNamespaceDeclaration(this, p);
        }

        public Guid Id => id;

        public FileScopeNamespaceDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new FileScopeNamespaceDeclaration(newId, prefix, markers, _name, _externs, _usings, _members);
        }
        public Space Prefix => prefix;

        public FileScopeNamespaceDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new FileScopeNamespaceDeclaration(id, newPrefix, markers, _name, _externs, _usings, _members);
        }
        public Markers Markers => markers;

        public FileScopeNamespaceDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new FileScopeNamespaceDeclaration(id, prefix, newMarkers, _name, _externs, _usings, _members);
        }
        private readonly JRightPadded<Expression> _name = name;
        public Expression Name => _name.Element;

        public FileScopeNamespaceDeclaration WithName(Expression newName)
        {
            return Padding.WithName(_name.WithElement(newName));
        }
        private readonly IList<JRightPadded<Cs.ExternAlias>> _externs = externs;
        public IList<Cs.ExternAlias> Externs => _externs.Elements();

        public FileScopeNamespaceDeclaration WithExterns(IList<Cs.ExternAlias> newExterns)
        {
            return Padding.WithExterns(_externs.WithElements(newExterns));
        }
        private readonly IList<JRightPadded<Cs.UsingDirective>> _usings = usings;
        public IList<Cs.UsingDirective> Usings => _usings.Elements();

        public FileScopeNamespaceDeclaration WithUsings(IList<Cs.UsingDirective> newUsings)
        {
            return Padding.WithUsings(_usings.WithElements(newUsings));
        }
        private readonly IList<JRightPadded<Statement>> _members = members;
        public IList<Statement> Members => _members.Elements();

        public FileScopeNamespaceDeclaration WithMembers(IList<Statement> newMembers)
        {
            return Padding.WithMembers(_members.WithElements(newMembers));
        }
        public sealed record PaddingHelper(Cs.FileScopeNamespaceDeclaration T)
        {
            public JRightPadded<Expression> Name => T._name;

            public Cs.FileScopeNamespaceDeclaration WithName(JRightPadded<Expression> newName)
            {
                return T._name == newName ? T : new Cs.FileScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, newName, T._externs, T._usings, T._members);
            }

            public IList<JRightPadded<Cs.ExternAlias>> Externs => T._externs;

            public Cs.FileScopeNamespaceDeclaration WithExterns(IList<JRightPadded<Cs.ExternAlias>> newExterns)
            {
                return T._externs == newExterns ? T : new Cs.FileScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, T._name, newExterns, T._usings, T._members);
            }

            public IList<JRightPadded<Cs.UsingDirective>> Usings => T._usings;

            public Cs.FileScopeNamespaceDeclaration WithUsings(IList<JRightPadded<Cs.UsingDirective>> newUsings)
            {
                return T._usings == newUsings ? T : new Cs.FileScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, T._name, T._externs, newUsings, T._members);
            }

            public IList<JRightPadded<Statement>> Members => T._members;

            public Cs.FileScopeNamespaceDeclaration WithMembers(IList<JRightPadded<Statement>> newMembers)
            {
                return T._members == newMembers ? T : new Cs.FileScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, T._name, T._externs, T._usings, newMembers);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is FileScopeNamespaceDeclaration && other.Id == Id;
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