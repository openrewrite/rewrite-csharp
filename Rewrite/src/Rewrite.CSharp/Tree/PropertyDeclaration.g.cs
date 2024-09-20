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
    public partial class PropertyDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    IList<AttributeList> attributeLists,
    IList<J.Modifier> modifiers,
    TypeTree typeExpression,
    JRightPadded<NameTree>? interfaceSpecifier,
    J.Identifier name,
    J.Block accessors,
    JLeftPadded<Expression>? initializer
    ) : Cs, Statement, TypedTree, MutableTree<PropertyDeclaration>
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
            return v.VisitPropertyDeclaration(this, p);
        }

        public Guid Id => id;

        public PropertyDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new PropertyDeclaration(newId, prefix, markers, attributeLists, modifiers, typeExpression, _interfaceSpecifier, name, accessors, _initializer);
        }
        public Space Prefix => prefix;

        public PropertyDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new PropertyDeclaration(id, newPrefix, markers, attributeLists, modifiers, typeExpression, _interfaceSpecifier, name, accessors, _initializer);
        }
        public Markers Markers => markers;

        public PropertyDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new PropertyDeclaration(id, prefix, newMarkers, attributeLists, modifiers, typeExpression, _interfaceSpecifier, name, accessors, _initializer);
        }
        public IList<Cs.AttributeList> AttributeLists => attributeLists;

        public PropertyDeclaration WithAttributeLists(IList<Cs.AttributeList> newAttributeLists)
        {
            return newAttributeLists == attributeLists ? this : new PropertyDeclaration(id, prefix, markers, newAttributeLists, modifiers, typeExpression, _interfaceSpecifier, name, accessors, _initializer);
        }
        public IList<J.Modifier> Modifiers => modifiers;

        public PropertyDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new PropertyDeclaration(id, prefix, markers, attributeLists, newModifiers, typeExpression, _interfaceSpecifier, name, accessors, _initializer);
        }
        public TypeTree TypeExpression => typeExpression;

        public PropertyDeclaration WithTypeExpression(TypeTree newTypeExpression)
        {
            return ReferenceEquals(newTypeExpression, typeExpression) ? this : new PropertyDeclaration(id, prefix, markers, attributeLists, modifiers, newTypeExpression, _interfaceSpecifier, name, accessors, _initializer);
        }
        private readonly JRightPadded<NameTree>? _interfaceSpecifier = interfaceSpecifier;
        public NameTree? InterfaceSpecifier => _interfaceSpecifier?.Element;

        public PropertyDeclaration WithInterfaceSpecifier(NameTree? newInterfaceSpecifier)
        {
            return Padding.WithInterfaceSpecifier(JRightPadded<NameTree>.WithElement(_interfaceSpecifier, newInterfaceSpecifier));
        }
        public J.Identifier Name => name;

        public PropertyDeclaration WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new PropertyDeclaration(id, prefix, markers, attributeLists, modifiers, typeExpression, _interfaceSpecifier, newName, accessors, _initializer);
        }
        public J.Block Accessors => accessors;

        public PropertyDeclaration WithAccessors(J.Block newAccessors)
        {
            return ReferenceEquals(newAccessors, accessors) ? this : new PropertyDeclaration(id, prefix, markers, attributeLists, modifiers, typeExpression, _interfaceSpecifier, name, newAccessors, _initializer);
        }
        private readonly JLeftPadded<Expression>? _initializer = initializer;
        public Expression? Initializer => _initializer?.Element;

        public PropertyDeclaration WithInitializer(Expression? newInitializer)
        {
            return Padding.WithInitializer(JLeftPadded<Expression>.WithElement(_initializer, newInitializer));
        }
        public sealed record PaddingHelper(Cs.PropertyDeclaration T)
        {
            public JRightPadded<NameTree>? InterfaceSpecifier => T._interfaceSpecifier;

            public Cs.PropertyDeclaration WithInterfaceSpecifier(JRightPadded<NameTree>? newInterfaceSpecifier)
            {
                return T._interfaceSpecifier == newInterfaceSpecifier ? T : new Cs.PropertyDeclaration(T.Id, T.Prefix, T.Markers, T.AttributeLists, T.Modifiers, T.TypeExpression, newInterfaceSpecifier, T.Name, T.Accessors, T._initializer);
            }

            public JLeftPadded<Expression>? Initializer => T._initializer;

            public Cs.PropertyDeclaration WithInitializer(JLeftPadded<Expression>? newInitializer)
            {
                return T._initializer == newInitializer ? T : new Cs.PropertyDeclaration(T.Id, T.Prefix, T.Markers, T.AttributeLists, T.Modifiers, T.TypeExpression, T._interfaceSpecifier, T.Name, T.Accessors, newInitializer);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is PropertyDeclaration && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}