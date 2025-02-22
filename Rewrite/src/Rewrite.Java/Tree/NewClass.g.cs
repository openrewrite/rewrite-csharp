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
    public partial class NewClass(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression>? enclosing,
    Space @new,
    TypeTree? clazz,
    JContainer<Expression> arguments,
    Block? body,
    JavaType.Method? constructorType
    ) : J, Statement, TypedTree, MethodCall, Expression<NewClass>, TypedTree<NewClass>, J<NewClass>, MutableTree<NewClass>
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
            return v.VisitNewClass(this, p);
        }

        public Guid Id => id;

        public NewClass WithId(Guid newId)
        {
            return newId == id ? this : new NewClass(newId, prefix, markers, _enclosing, @new, clazz, _arguments, body, constructorType);
        }
        public Space Prefix => prefix;

        public NewClass WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NewClass(id, newPrefix, markers, _enclosing, @new, clazz, _arguments, body, constructorType);
        }
        public Markers Markers => markers;

        public NewClass WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NewClass(id, prefix, newMarkers, _enclosing, @new, clazz, _arguments, body, constructorType);
        }
        private readonly JRightPadded<Expression>? _enclosing = enclosing;
        public Expression? Enclosing => _enclosing?.Element;

        public NewClass WithEnclosing(Expression? newEnclosing)
        {
            return Padding.WithEnclosing(JRightPadded<Expression>.WithElement(_enclosing, newEnclosing));
        }
        public Space New => @new;

        public NewClass WithNew(Space newNew)
        {
            return newNew == @new ? this : new NewClass(id, prefix, markers, _enclosing, newNew, clazz, _arguments, body, constructorType);
        }
        public TypeTree? Clazz => clazz;

        public NewClass WithClazz(TypeTree? newClazz)
        {
            return ReferenceEquals(newClazz, clazz) ? this : new NewClass(id, prefix, markers, _enclosing, @new, newClazz, _arguments, body, constructorType);
        }
        private readonly JContainer<Expression> _arguments = arguments;
        public IList<Expression> Arguments => _arguments.GetElements();

        public NewClass WithArguments(IList<Expression> newArguments)
        {
            return Padding.WithArguments(JContainer<Expression>.WithElements(_arguments, newArguments));
        }
        public J.Block? Body => body;

        public NewClass WithBody(J.Block? newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new NewClass(id, prefix, markers, _enclosing, @new, clazz, _arguments, newBody, constructorType);
        }
        public JavaType.Method? ConstructorType => constructorType;

        public NewClass WithConstructorType(JavaType.Method? newConstructorType)
        {
            return newConstructorType == constructorType ? this : new NewClass(id, prefix, markers, _enclosing, @new, clazz, _arguments, body, newConstructorType);
        }
        public sealed record PaddingHelper(J.NewClass T)
        {
            public JRightPadded<Expression>? Enclosing => T._enclosing;

            public J.NewClass WithEnclosing(JRightPadded<Expression>? newEnclosing)
            {
                return T._enclosing == newEnclosing ? T : new J.NewClass(T.Id, T.Prefix, T.Markers, newEnclosing, T.New, T.Clazz, T._arguments, T.Body, T.ConstructorType);
            }

            public JContainer<Expression> Arguments => T._arguments;

            public J.NewClass WithArguments(JContainer<Expression> newArguments)
            {
                return T._arguments == newArguments ? T : new J.NewClass(T.Id, T.Prefix, T.Markers, T._enclosing, T.New, T.Clazz, newArguments, T.Body, T.ConstructorType);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NewClass && other.Id == Id;
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