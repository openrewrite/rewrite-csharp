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
    /// <summary>
    /// Represents a C# enum member declaration, including optional attributes and initializer.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Simple enum member
    ///     Red,
    ///     // Member with initializer
    ///     Green = 2,
    ///     // Member with attributes and expression initializer
    ///     [Obsolete]
    ///     Blue = Red | Green,
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class EnumMemberDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    IList<AttributeList> attributeLists,
    J.Identifier name,
    JLeftPadded<Expression>? initializer
    ) : Cs, Expression, Expression<EnumMemberDeclaration>, J<EnumMemberDeclaration>, MutableTree<EnumMemberDeclaration>
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
            return v.VisitEnumMemberDeclaration(this, p);
        }

        public Guid Id => id;

        public EnumMemberDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new EnumMemberDeclaration(newId, prefix, markers, attributeLists, name, _initializer);
        }
        public Space Prefix => prefix;

        public EnumMemberDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new EnumMemberDeclaration(id, newPrefix, markers, attributeLists, name, _initializer);
        }
        public Markers Markers => markers;

        public EnumMemberDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new EnumMemberDeclaration(id, prefix, newMarkers, attributeLists, name, _initializer);
        }
        public IList<Cs.AttributeList> AttributeLists => attributeLists;

        public EnumMemberDeclaration WithAttributeLists(IList<Cs.AttributeList> newAttributeLists)
        {
            return newAttributeLists == attributeLists ? this : new EnumMemberDeclaration(id, prefix, markers, newAttributeLists, name, _initializer);
        }
        public J.Identifier Name => name;

        public EnumMemberDeclaration WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new EnumMemberDeclaration(id, prefix, markers, attributeLists, newName, _initializer);
        }
        private readonly JLeftPadded<Expression>? _initializer = initializer;
        public Expression? Initializer => _initializer?.Element;

        public EnumMemberDeclaration WithInitializer(Expression? newInitializer)
        {
            return Padding.WithInitializer(JLeftPadded<Expression>.WithElement(_initializer, newInitializer));
        }
        public sealed record PaddingHelper(Cs.EnumMemberDeclaration T)
        {
            public JLeftPadded<Expression>? Initializer => T._initializer;

            public Cs.EnumMemberDeclaration WithInitializer(JLeftPadded<Expression>? newInitializer)
            {
                return T._initializer == newInitializer ? T : new Cs.EnumMemberDeclaration(T.Id, T.Prefix, T.Markers, T.AttributeLists, T.Name, newInitializer);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is EnumMemberDeclaration && other.Id == Id;
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