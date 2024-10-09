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
    public partial class VariableDeclarations(
    Guid id,
    Space prefix,
    Markers markers,
    IList<Annotation> leadingAnnotations,
    IList<Modifier> modifiers,
    TypeTree? typeExpression,
    Space? varargs,
    IList<JLeftPadded<Space>> dimensionsBeforeName,
    IList<JRightPadded<VariableDeclarations.NamedVariable>> variables
    ) : J, Statement, TypedTree, MutableTree<VariableDeclarations>
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
            return v.VisitVariableDeclarations(this, p);
        }

        public Guid Id => id;

        public VariableDeclarations WithId(Guid newId)
        {
            return newId == id ? this : new VariableDeclarations(newId, prefix, markers, leadingAnnotations, modifiers, typeExpression, varargs, dimensionsBeforeName, _variables);
        }
        public Space Prefix => prefix;

        public VariableDeclarations WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new VariableDeclarations(id, newPrefix, markers, leadingAnnotations, modifiers, typeExpression, varargs, dimensionsBeforeName, _variables);
        }
        public Markers Markers => markers;

        public VariableDeclarations WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new VariableDeclarations(id, prefix, newMarkers, leadingAnnotations, modifiers, typeExpression, varargs, dimensionsBeforeName, _variables);
        }
        public IList<J.Annotation> LeadingAnnotations => leadingAnnotations;

        public VariableDeclarations WithLeadingAnnotations(IList<J.Annotation> newLeadingAnnotations)
        {
            return newLeadingAnnotations == leadingAnnotations ? this : new VariableDeclarations(id, prefix, markers, newLeadingAnnotations, modifiers, typeExpression, varargs, dimensionsBeforeName, _variables);
        }
        public IList<J.Modifier> Modifiers => modifiers;

        public VariableDeclarations WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new VariableDeclarations(id, prefix, markers, leadingAnnotations, newModifiers, typeExpression, varargs, dimensionsBeforeName, _variables);
        }
        public TypeTree? TypeExpression => typeExpression;

        public VariableDeclarations WithTypeExpression(TypeTree? newTypeExpression)
        {
            return ReferenceEquals(newTypeExpression, typeExpression) ? this : new VariableDeclarations(id, prefix, markers, leadingAnnotations, modifiers, newTypeExpression, varargs, dimensionsBeforeName, _variables);
        }
        public Space? Varargs => varargs;

        public VariableDeclarations WithVarargs(Space? newVarargs)
        {
            return newVarargs == varargs ? this : new VariableDeclarations(id, prefix, markers, leadingAnnotations, modifiers, typeExpression, newVarargs, dimensionsBeforeName, _variables);
        }
        public IList<JLeftPadded<Space>> DimensionsBeforeName => dimensionsBeforeName;

        public VariableDeclarations WithDimensionsBeforeName(IList<JLeftPadded<Space>> newDimensionsBeforeName)
        {
            return newDimensionsBeforeName == dimensionsBeforeName ? this : new VariableDeclarations(id, prefix, markers, leadingAnnotations, modifiers, typeExpression, varargs, newDimensionsBeforeName, _variables);
        }
        private readonly IList<JRightPadded<NamedVariable>> _variables = variables;
        public IList<NamedVariable> Variables => _variables.Elements();

        public VariableDeclarations WithVariables(IList<NamedVariable> newVariables)
        {
            return Padding.WithVariables(_variables.WithElements(newVariables));
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class NamedVariable(
    Guid id,
    Space prefix,
    Markers markers,
    J.Identifier name,
    IList<JLeftPadded<Space>> dimensionsAfterName,
    JLeftPadded<Expression>? initializer,
    JavaType.Variable? variableType
        ) : J, NameTree, MutableTree<NamedVariable>
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
                return v.VisitVariable(this, p);
            }

            public Guid Id => id;

            public NamedVariable WithId(Guid newId)
            {
                return newId == id ? this : new NamedVariable(newId, prefix, markers, name, dimensionsAfterName, _initializer, variableType);
            }
            public Space Prefix => prefix;

            public NamedVariable WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new NamedVariable(id, newPrefix, markers, name, dimensionsAfterName, _initializer, variableType);
            }
            public Markers Markers => markers;

            public NamedVariable WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new NamedVariable(id, prefix, newMarkers, name, dimensionsAfterName, _initializer, variableType);
            }
            public J.Identifier Name => name;

            public NamedVariable WithName(J.Identifier newName)
            {
                return ReferenceEquals(newName, name) ? this : new NamedVariable(id, prefix, markers, newName, dimensionsAfterName, _initializer, variableType);
            }
            public IList<JLeftPadded<Space>> DimensionsAfterName => dimensionsAfterName;

            public NamedVariable WithDimensionsAfterName(IList<JLeftPadded<Space>> newDimensionsAfterName)
            {
                return newDimensionsAfterName == dimensionsAfterName ? this : new NamedVariable(id, prefix, markers, name, newDimensionsAfterName, _initializer, variableType);
            }
            private readonly JLeftPadded<Expression>? _initializer = initializer;
            public Expression? Initializer => _initializer?.Element;

            public NamedVariable WithInitializer(Expression? newInitializer)
            {
                return Padding.WithInitializer(JLeftPadded<Expression>.WithElement(_initializer, newInitializer));
            }
            public JavaType.Variable? VariableType => variableType;

            public NamedVariable WithVariableType(JavaType.Variable? newVariableType)
            {
                return newVariableType == variableType ? this : new NamedVariable(id, prefix, markers, name, dimensionsAfterName, _initializer, newVariableType);
            }
            public sealed record PaddingHelper(J.VariableDeclarations.NamedVariable T)
            {
                public JLeftPadded<Expression>? Initializer => T._initializer;

                public J.VariableDeclarations.NamedVariable WithInitializer(JLeftPadded<Expression>? newInitializer)
                {
                    return T._initializer == newInitializer ? T : new J.VariableDeclarations.NamedVariable(T.Id, T.Prefix, T.Markers, T.Name, T.DimensionsAfterName, newInitializer, T.VariableType);
                }

            }

            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is NamedVariable && other.Id == Id;
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        public sealed record PaddingHelper(J.VariableDeclarations T)
        {
            public IList<JRightPadded<J.VariableDeclarations.NamedVariable>> Variables => T._variables;

            public J.VariableDeclarations WithVariables(IList<JRightPadded<J.VariableDeclarations.NamedVariable>> newVariables)
            {
                return T._variables == newVariables ? T : new J.VariableDeclarations(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T.TypeExpression, T.Varargs, T.DimensionsBeforeName, newVariables);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is VariableDeclarations && other.Id == Id;
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