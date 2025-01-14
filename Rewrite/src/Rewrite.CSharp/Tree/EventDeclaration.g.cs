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
    /// Represents a C# event declaration.
    /// <br/>
    /// For example:
    /// <code>
    /// // Simple event declaration
    /// public event EventHandler OnClick;
    /// // With explicit add/remove accessors
    /// public event EventHandler OnChange {
    ///     add { handlers += value; }
    ///     remove { handlers -= value; }
    /// }
    /// // Generic event
    /// public event EventHandler<TEventArgs> OnDataChanged;
    /// // Custom delegate type
    /// public event MyCustomDelegate OnCustomEvent;
    /// // Static event
    /// public static event Action StaticEvent;
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class EventDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    IList<AttributeList> attributeLists,
    IList<J.Modifier> modifiers,
    JLeftPadded<TypeTree> typeExpression,
    JRightPadded<NameTree>? interfaceSpecifier,
    J.Identifier name,
    JContainer<Statement>? accessors
    ) : Cs, Statement, J<EventDeclaration>, MutableTree<EventDeclaration>
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
            return v.VisitEventDeclaration(this, p);
        }

        public Guid Id => id;

        public EventDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new EventDeclaration(newId, prefix, markers, attributeLists, modifiers, _typeExpression, _interfaceSpecifier, name, _accessors);
        }
        public Space Prefix => prefix;

        public EventDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new EventDeclaration(id, newPrefix, markers, attributeLists, modifiers, _typeExpression, _interfaceSpecifier, name, _accessors);
        }
        public Markers Markers => markers;

        public EventDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new EventDeclaration(id, prefix, newMarkers, attributeLists, modifiers, _typeExpression, _interfaceSpecifier, name, _accessors);
        }
        public IList<Cs.AttributeList> AttributeLists => attributeLists;

        public EventDeclaration WithAttributeLists(IList<Cs.AttributeList> newAttributeLists)
        {
            return newAttributeLists == attributeLists ? this : new EventDeclaration(id, prefix, markers, newAttributeLists, modifiers, _typeExpression, _interfaceSpecifier, name, _accessors);
        }
        public IList<J.Modifier> Modifiers => modifiers;

        public EventDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new EventDeclaration(id, prefix, markers, attributeLists, newModifiers, _typeExpression, _interfaceSpecifier, name, _accessors);
        }
        private readonly JLeftPadded<TypeTree> _typeExpression = typeExpression;
        public TypeTree TypeExpression => _typeExpression.Element;

        public EventDeclaration WithTypeExpression(TypeTree newTypeExpression)
        {
            return Padding.WithTypeExpression(_typeExpression.WithElement(newTypeExpression));
        }
        private readonly JRightPadded<NameTree>? _interfaceSpecifier = interfaceSpecifier;
        public NameTree? InterfaceSpecifier => _interfaceSpecifier?.Element;

        public EventDeclaration WithInterfaceSpecifier(NameTree? newInterfaceSpecifier)
        {
            return Padding.WithInterfaceSpecifier(JRightPadded<NameTree>.WithElement(_interfaceSpecifier, newInterfaceSpecifier));
        }
        public J.Identifier Name => name;

        public EventDeclaration WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new EventDeclaration(id, prefix, markers, attributeLists, modifiers, _typeExpression, _interfaceSpecifier, newName, _accessors);
        }
        private readonly JContainer<Statement>? _accessors = accessors;
        public IList<Statement>? Accessors => _accessors?.GetElements();

        public EventDeclaration WithAccessors(IList<Statement>? newAccessors)
        {
            return Padding.WithAccessors(JContainer<Statement>.WithElementsNullable(_accessors, newAccessors));
        }
        public sealed record PaddingHelper(Cs.EventDeclaration T)
        {
            public JLeftPadded<TypeTree> TypeExpression => T._typeExpression;

            public Cs.EventDeclaration WithTypeExpression(JLeftPadded<TypeTree> newTypeExpression)
            {
                return T._typeExpression == newTypeExpression ? T : new Cs.EventDeclaration(T.Id, T.Prefix, T.Markers, T.AttributeLists, T.Modifiers, newTypeExpression, T._interfaceSpecifier, T.Name, T._accessors);
            }

            public JRightPadded<NameTree>? InterfaceSpecifier => T._interfaceSpecifier;

            public Cs.EventDeclaration WithInterfaceSpecifier(JRightPadded<NameTree>? newInterfaceSpecifier)
            {
                return T._interfaceSpecifier == newInterfaceSpecifier ? T : new Cs.EventDeclaration(T.Id, T.Prefix, T.Markers, T.AttributeLists, T.Modifiers, T._typeExpression, newInterfaceSpecifier, T.Name, T._accessors);
            }

            public JContainer<Statement>? Accessors => T._accessors;

            public Cs.EventDeclaration WithAccessors(JContainer<Statement>? newAccessors)
            {
                return T._accessors == newAccessors ? T : new Cs.EventDeclaration(T.Id, T.Prefix, T.Markers, T.AttributeLists, T.Modifiers, T._typeExpression, T._interfaceSpecifier, T.Name, newAccessors);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is EventDeclaration && other.Id == Id;
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