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
    /// <summary>
    /// Represents a Java class declaration.
    /// <br/>Example:
    /// <code>{@code
    /// public class MyClass {
    /// }
    /// }</code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class ClassDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    IList<Annotation> leadingAnnotations,
    IList<Modifier> modifiers,
    ClassDeclaration.Kind declarationKind,
    Identifier name,
    JContainer<TypeParameter>? typeParameters,
    JContainer<Statement>? primaryConstructor,
    JLeftPadded<TypeTree>? extends,
    JContainer<TypeTree>? implements,
    JContainer<TypeTree>? permits,
    Block body,
    JavaType.FullyQualified? type
    ) : J,Statement,TypedTree    {
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
            return v.VisitClassDeclaration(this, p);
        }

        public Guid Id { get;  set; } = id;

        public ClassDeclaration WithId(Guid newId)
        {
            return newId == Id ? this : new ClassDeclaration(newId, Prefix, Markers, LeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public ClassDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new ClassDeclaration(Id, newPrefix, Markers, LeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public ClassDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new ClassDeclaration(Id, Prefix, newMarkers, LeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        public IList<J.Annotation> LeadingAnnotations { get;  set; } = leadingAnnotations;

        public ClassDeclaration WithLeadingAnnotations(IList<J.Annotation> newLeadingAnnotations)
        {
            return newLeadingAnnotations == LeadingAnnotations ? this : new ClassDeclaration(Id, Prefix, Markers, newLeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        public IList<J.Modifier> Modifiers { get;  set; } = modifiers;

        public ClassDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == Modifiers ? this : new ClassDeclaration(Id, Prefix, Markers, LeadingAnnotations, newModifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        private Kind _declarationKind = declarationKind;

        public ClassDeclaration WithDeclarationKind(Kind newDeclarationKind)
        {
            return ReferenceEquals(newDeclarationKind, DeclarationKind) ? this : new ClassDeclaration(Id, Prefix, Markers, LeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        public J.Identifier Name { get;  set; } = name;

        public ClassDeclaration WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, Name) ? this : new ClassDeclaration(Id, Prefix, Markers, LeadingAnnotations, Modifiers, _declarationKind, newName, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, Type);
        }
        private JContainer<J.TypeParameter>? _typeParameters = typeParameters;
        public IList<J.TypeParameter>? TypeParameters => _typeParameters?.GetElements();

        public ClassDeclaration WithTypeParameters(IList<J.TypeParameter>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<J.TypeParameter>.WithElementsNullable(_typeParameters, newTypeParameters));
        }
        private JContainer<Statement>? _primaryConstructor = primaryConstructor;
        public IList<Statement>? PrimaryConstructor => _primaryConstructor?.GetElements();

        public ClassDeclaration WithPrimaryConstructor(IList<Statement>? newPrimaryConstructor)
        {
            return Padding.WithPrimaryConstructor(JContainer<Statement>.WithElementsNullable(_primaryConstructor, newPrimaryConstructor));
        }
        private JLeftPadded<TypeTree>? _extends = extends;
        public TypeTree? Extends => _extends?.Element;

        public ClassDeclaration WithExtends(TypeTree? newExtends)
        {
            return Padding.WithExtends(JLeftPadded<TypeTree>.WithElement(_extends, newExtends));
        }
        private JContainer<TypeTree>? _implements = implements;
        public IList<TypeTree>? Implements => _implements?.GetElements();

        public ClassDeclaration WithImplements(IList<TypeTree>? newImplements)
        {
            return Padding.WithImplements(JContainer<TypeTree>.WithElementsNullable(_implements, newImplements));
        }
        private JContainer<TypeTree>? _permits = permits;
        public IList<TypeTree>? Permits => _permits?.GetElements();

        public ClassDeclaration WithPermits(IList<TypeTree>? newPermits)
        {
            return Padding.WithPermits(JContainer<TypeTree>.WithElementsNullable(_permits, newPermits));
        }
        public J.Block Body { get;  set; } = body;

        public ClassDeclaration WithBody(J.Block newBody)
        {
            return ReferenceEquals(newBody, Body) ? this : new ClassDeclaration(Id, Prefix, Markers, LeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, newBody, Type);
        }
        public JavaType.FullyQualified? Type { get;  set; } = type;

        public ClassDeclaration WithType(JavaType.FullyQualified? newType)
        {
            return newType == Type ? this : new ClassDeclaration(Id, Prefix, Markers, LeadingAnnotations, Modifiers, _declarationKind, Name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, Body, newType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class Kind(
    Guid id,
    Space prefix,
    Markers markers,
    IList<J.Annotation> annotations,
    Kind.Types kindType
        ) : J, MutableTree
        {
            public J? AcceptJava<P>(JavaVisitor<P> v, P p)
            {
                return v.VisitClassDeclarationKind(this, p);
            }

            public Guid Id { get;  set; } = id;

            public Kind WithId(Guid newId)
            {
                return newId == Id ? this : new Kind(newId, Prefix, Markers, Annotations, KindType);
            }
            public Space Prefix { get;  set; } = prefix;

            public Kind WithPrefix(Space newPrefix)
            {
                return newPrefix == Prefix ? this : new Kind(Id, newPrefix, Markers, Annotations, KindType);
            }
            public Markers Markers { get;  set; } = markers;

            public Kind WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, Markers) ? this : new Kind(Id, Prefix, newMarkers, Annotations, KindType);
            }
            public IList<J.Annotation> Annotations { get;  set; } = annotations;

            public Kind WithAnnotations(IList<J.Annotation> newAnnotations)
            {
                return newAnnotations == Annotations ? this : new Kind(Id, Prefix, Markers, newAnnotations, KindType);
            }
            public Types KindType { get;  set; } = kindType;

            public Kind WithKindType(Types newKindType)
            {
                return newKindType == KindType ? this : new Kind(Id, Prefix, Markers, Annotations, newKindType);
            }
            public enum Types
            {
                Class,
                Enum,
                Interface,
                Annotation,
                Record,
                Value,
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Kind && other.Id == Id;
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        public sealed record PaddingHelper(J.ClassDeclaration T)
        {
            public J.ClassDeclaration.Kind DeclarationKind { get => T._declarationKind;  set => T._declarationKind = value; }

            public J.ClassDeclaration WithDeclarationKind(J.ClassDeclaration.Kind newDeclarationKind)
            {
                return DeclarationKind == newDeclarationKind ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, newDeclarationKind, T.Name, T._typeParameters, T._primaryConstructor, T._extends, T._implements, T._permits, T.Body, T.Type);
            }

            public JContainer<J.TypeParameter>? TypeParameters { get => T._typeParameters;  set => T._typeParameters = value; }

            public J.ClassDeclaration WithTypeParameters(JContainer<J.TypeParameter>? newTypeParameters)
            {
                return TypeParameters == newTypeParameters ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, newTypeParameters, T._primaryConstructor, T._extends, T._implements, T._permits, T.Body, T.Type);
            }

            public JContainer<Statement>? PrimaryConstructor { get => T._primaryConstructor;  set => T._primaryConstructor = value; }

            public J.ClassDeclaration WithPrimaryConstructor(JContainer<Statement>? newPrimaryConstructor)
            {
                return PrimaryConstructor == newPrimaryConstructor ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, newPrimaryConstructor, T._extends, T._implements, T._permits, T.Body, T.Type);
            }

            public JLeftPadded<TypeTree>? Extends { get => T._extends;  set => T._extends = value; }

            public J.ClassDeclaration WithExtends(JLeftPadded<TypeTree>? newExtends)
            {
                return Extends == newExtends ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, T._primaryConstructor, newExtends, T._implements, T._permits, T.Body, T.Type);
            }

            public JContainer<TypeTree>? Implements { get => T._implements;  set => T._implements = value; }

            public J.ClassDeclaration WithImplements(JContainer<TypeTree>? newImplements)
            {
                return Implements == newImplements ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, T._primaryConstructor, T._extends, newImplements, T._permits, T.Body, T.Type);
            }

            public JContainer<TypeTree>? Permits { get => T._permits;  set => T._permits = value; }

            public J.ClassDeclaration WithPermits(JContainer<TypeTree>? newPermits)
            {
                return Permits == newPermits ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, T._primaryConstructor, T._extends, T._implements, newPermits, T.Body, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ClassDeclaration && other.Id == Id;
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