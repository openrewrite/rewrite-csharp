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
public interface J : Rewrite.Core.Tree
{
    Space Prefix { get; }

    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(JavaVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptJava(v.Adapt<J, JavaVisitor<P>>(), p);
    }

    J? AcceptJava<P>(JavaVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
    }

    public class AnnotatedType(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        TypeTree typeExpression
    ) : J, Expression, TypeTree, MutableTree<AnnotatedType>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitAnnotatedType(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public AnnotatedType WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public AnnotatedType WithId(Guid newId)
        {
            return newId == id ? this : new AnnotatedType(newId, prefix, markers, annotations, typeExpression);
        }

        public Space Prefix => prefix;

        public AnnotatedType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AnnotatedType(id, newPrefix, markers, annotations, typeExpression);
        }

        public Markers Markers => markers;

        public AnnotatedType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AnnotatedType(id, prefix, newMarkers, annotations, typeExpression);
        }

        public IList<J.Annotation> Annotations => annotations;

        public AnnotatedType WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new AnnotatedType(id, prefix, markers, newAnnotations, typeExpression);
        }

        public TypeTree TypeExpression => typeExpression;

        public AnnotatedType WithTypeExpression(TypeTree newTypeExpression)
        {
            return ReferenceEquals(newTypeExpression, typeExpression) ? this : new AnnotatedType(id, prefix, markers, annotations, newTypeExpression);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AnnotatedType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Annotation(
        Guid id,
        Space prefix,
        Markers markers,
        NameTree annotationType,
        JContainer<Expression>? arguments
    ) : J, Expression, MutableTree<Annotation>
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
            return v.VisitAnnotation(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public Annotation WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public Annotation WithId(Guid newId)
        {
            return newId == id ? this : new Annotation(newId, prefix, markers, annotationType, _arguments);
        }

        public Space Prefix => prefix;

        public Annotation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Annotation(id, newPrefix, markers, annotationType, _arguments);
        }

        public Markers Markers => markers;

        public Annotation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Annotation(id, prefix, newMarkers, annotationType, _arguments);
        }

        public NameTree AnnotationType => annotationType;

        public Annotation WithAnnotationType(NameTree newAnnotationType)
        {
            return ReferenceEquals(newAnnotationType, annotationType) ? this : new Annotation(id, prefix, markers, newAnnotationType, _arguments);
        }

        private readonly JContainer<Expression>? _arguments = arguments;
        public IList<Expression>? Arguments => _arguments?.GetElements();

        public Annotation WithArguments(IList<Expression>? newArguments)
        {
            return Padding.WithArguments(JContainer<Expression>.WithElementsNullable(_arguments, newArguments));
        }

        public sealed record PaddingHelper(J.Annotation T)
        {
            public JContainer<Expression>? Arguments => T._arguments;

            public J.Annotation WithArguments(JContainer<Expression>? newArguments)
            {
                return T._arguments == newArguments ? T : new J.Annotation(T.Id, T.Prefix, T.Markers, T.AnnotationType, newArguments);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Annotation && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ArrayAccess(
        Guid id,
        Space prefix,
        Markers markers,
        Expression indexed,
        ArrayDimension dimension,
        JavaType? type
    ) : J, Expression, TypedTree, MutableTree<ArrayAccess>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitArrayAccess(this, p);
        }

        public Guid Id => id;

        public ArrayAccess WithId(Guid newId)
        {
            return newId == id ? this : new ArrayAccess(newId, prefix, markers, indexed, dimension, type);
        }

        public Space Prefix => prefix;

        public ArrayAccess WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayAccess(id, newPrefix, markers, indexed, dimension, type);
        }

        public Markers Markers => markers;

        public ArrayAccess WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayAccess(id, prefix, newMarkers, indexed, dimension, type);
        }

        public Expression Indexed => indexed;

        public ArrayAccess WithIndexed(Expression newIndexed)
        {
            return ReferenceEquals(newIndexed, indexed) ? this : new ArrayAccess(id, prefix, markers, newIndexed, dimension, type);
        }

        public J.ArrayDimension Dimension => dimension;

        public ArrayAccess WithDimension(J.ArrayDimension newDimension)
        {
            return ReferenceEquals(newDimension, dimension) ? this : new ArrayAccess(id, prefix, markers, indexed, newDimension, type);
        }

        public JavaType? Type => type;

        public ArrayAccess WithType(JavaType? newType)
        {
            return newType == type ? this : new ArrayAccess(id, prefix, markers, indexed, dimension, newType);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayAccess && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ArrayType(
        Guid id,
        Space prefix,
        Markers markers,
        TypeTree elementType,
        IList<Annotation>? annotations,
        JLeftPadded<Space>? dimension,
        JavaType type
    ) : J, TypeTree, Expression, MutableTree<ArrayType>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitArrayType(this, p);
        }

        public Guid Id => id;

        public ArrayType WithId(Guid newId)
        {
            return newId == id ? this : new ArrayType(newId, prefix, markers, elementType, annotations, dimension, type);
        }

        public Space Prefix => prefix;

        public ArrayType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayType(id, newPrefix, markers, elementType, annotations, dimension, type);
        }

        public Markers Markers => markers;

        public ArrayType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayType(id, prefix, newMarkers, elementType, annotations, dimension, type);
        }

        public TypeTree ElementType => elementType;

        public ArrayType WithElementType(TypeTree newElementType)
        {
            return ReferenceEquals(newElementType, elementType) ? this : new ArrayType(id, prefix, markers, newElementType, annotations, dimension, type);
        }

        public IList<J.Annotation>? Annotations => annotations;

        public ArrayType WithAnnotations(IList<J.Annotation>? newAnnotations)
        {
            return newAnnotations == annotations ? this : new ArrayType(id, prefix, markers, elementType, newAnnotations, dimension, type);
        }

        public JLeftPadded<Space>? Dimension => dimension;

        public ArrayType WithDimension(JLeftPadded<Space>? newDimension)
        {
            return newDimension == dimension ? this : new ArrayType(id, prefix, markers, elementType, annotations, newDimension, type);
        }

        public JavaType Type => type;

        public ArrayType WithType(JavaType newType)
        {
            return newType == type ? this : new ArrayType(id, prefix, markers, elementType, annotations, dimension, newType);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Assert(
        Guid id,
        Space prefix,
        Markers markers,
        Expression condition,
        JLeftPadded<Expression>? detail
    ) : J, Statement, MutableTree<Assert>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitAssert(this, p);
        }

        public Guid Id => id;

        public Assert WithId(Guid newId)
        {
            return newId == id ? this : new Assert(newId, prefix, markers, condition, detail);
        }

        public Space Prefix => prefix;

        public Assert WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Assert(id, newPrefix, markers, condition, detail);
        }

        public Markers Markers => markers;

        public Assert WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Assert(id, prefix, newMarkers, condition, detail);
        }

        public Expression Condition => condition;

        public Assert WithCondition(Expression newCondition)
        {
            return ReferenceEquals(newCondition, condition) ? this : new Assert(id, prefix, markers, newCondition, detail);
        }

        public JLeftPadded<Expression>? Detail => detail;

        public Assert WithDetail(JLeftPadded<Expression>? newDetail)
        {
            return newDetail == detail ? this : new Assert(id, prefix, markers, condition, newDetail);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Assert && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Assignment(
        Guid id,
        Space prefix,
        Markers markers,
        Expression variable,
        JLeftPadded<Expression> expression,
        JavaType? type
    ) : J, Statement, Expression, TypedTree, MutableTree<Assignment>
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
            return v.VisitAssignment(this, p);
        }

        public Guid Id => id;

        public Assignment WithId(Guid newId)
        {
            return newId == id ? this : new Assignment(newId, prefix, markers, variable, _expression, type);
        }

        public Space Prefix => prefix;

        public Assignment WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Assignment(id, newPrefix, markers, variable, _expression, type);
        }

        public Markers Markers => markers;

        public Assignment WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Assignment(id, prefix, newMarkers, variable, _expression, type);
        }

        public Expression Variable => variable;

        public Assignment WithVariable(Expression newVariable)
        {
            return ReferenceEquals(newVariable, variable) ? this : new Assignment(id, prefix, markers, newVariable, _expression, type);
        }

        private readonly JLeftPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public Assignment WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }

        public JavaType? Type => type;

        public Assignment WithType(JavaType? newType)
        {
            return newType == type ? this : new Assignment(id, prefix, markers, variable, _expression, newType);
        }

        public sealed record PaddingHelper(J.Assignment T)
        {
            public JLeftPadded<Expression> Expression => T._expression;

            public J.Assignment WithExpression(JLeftPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new J.Assignment(T.Id, T.Prefix, T.Markers, T.Variable, newExpression, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Assignment && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class AssignmentOperation(
        Guid id,
        Space prefix,
        Markers markers,
        Expression variable,
        JLeftPadded<AssignmentOperation.Type> @operator,
        Expression assignment,
        JavaType? javaType
    ) : J, Statement, Expression, TypedTree, MutableTree<AssignmentOperation>
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
            return v.VisitAssignmentOperation(this, p);
        }

        public Guid Id => id;

        public AssignmentOperation WithId(Guid newId)
        {
            return newId == id ? this : new AssignmentOperation(newId, prefix, markers, variable, _operator, assignment, javaType);
        }

        public Space Prefix => prefix;

        public AssignmentOperation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AssignmentOperation(id, newPrefix, markers, variable, _operator, assignment, javaType);
        }

        public Markers Markers => markers;

        public AssignmentOperation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AssignmentOperation(id, prefix, newMarkers, variable, _operator, assignment, javaType);
        }

        public Expression Variable => variable;

        public AssignmentOperation WithVariable(Expression newVariable)
        {
            return ReferenceEquals(newVariable, variable) ? this : new AssignmentOperation(id, prefix, markers, newVariable, _operator, assignment, javaType);
        }

        private readonly JLeftPadded<Type> _operator = @operator;
        public Type Operator => _operator.Element;

        public AssignmentOperation WithOperator(Type newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }

        public Expression Assignment => assignment;

        public AssignmentOperation WithAssignment(Expression newAssignment)
        {
            return ReferenceEquals(newAssignment, assignment) ? this : new AssignmentOperation(id, prefix, markers, variable, _operator, newAssignment, javaType);
        }

        public JavaType? JavaType => javaType;

        public AssignmentOperation WithJavaType(JavaType? newJavaType)
        {
            return newJavaType == javaType ? this : new AssignmentOperation(id, prefix, markers, variable, _operator, assignment, newJavaType);
        }

        public enum Type
        {
            Addition,
            BitAnd,
            BitOr,
            BitXor,
            Division,
            Exponentiation,
            FloorDivision,
            LeftShift,
            MatrixMultiplication,
            Modulo,
            Multiplication,
            RightShift,
            Subtraction,
            UnsignedRightShift,

        }

        public sealed record PaddingHelper(J.AssignmentOperation T)
        {
            public JLeftPadded<J.AssignmentOperation.Type> Operator => T._operator;

            public J.AssignmentOperation WithOperator(JLeftPadded<J.AssignmentOperation.Type> newOperator)
            {
                return T._operator == newOperator ? T : new J.AssignmentOperation(T.Id, T.Prefix, T.Markers, T.Variable, newOperator, T.Assignment, T.JavaType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AssignmentOperation && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Binary(
        Guid id,
        Space prefix,
        Markers markers,
        Expression left,
        JLeftPadded<Binary.Type> @operator,
        Expression right,
        JavaType? javaType
    ) : J, Expression, TypedTree, MutableTree<Binary>
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
            return v.VisitBinary(this, p);
        }

        public Guid Id => id;

        public Binary WithId(Guid newId)
        {
            return newId == id ? this : new Binary(newId, prefix, markers, left, _operator, right, javaType);
        }

        public Space Prefix => prefix;

        public Binary WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Binary(id, newPrefix, markers, left, _operator, right, javaType);
        }

        public Markers Markers => markers;

        public Binary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Binary(id, prefix, newMarkers, left, _operator, right, javaType);
        }

        public Expression Left => left;

        public Binary WithLeft(Expression newLeft)
        {
            return ReferenceEquals(newLeft, left) ? this : new Binary(id, prefix, markers, newLeft, _operator, right, javaType);
        }

        private readonly JLeftPadded<Type> _operator = @operator;
        public Type Operator => _operator.Element;

        public Binary WithOperator(Type newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }

        public Expression Right => right;

        public Binary WithRight(Expression newRight)
        {
            return ReferenceEquals(newRight, right) ? this : new Binary(id, prefix, markers, left, _operator, newRight, javaType);
        }

        public JavaType? JavaType => javaType;

        public Binary WithJavaType(JavaType? newJavaType)
        {
            return newJavaType == javaType ? this : new Binary(id, prefix, markers, left, _operator, right, newJavaType);
        }

        public enum Type
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Modulo,
            LessThan,
            GreaterThan,
            LessThanOrEqual,
            GreaterThanOrEqual,
            Equal,
            NotEqual,
            BitAnd,
            BitOr,
            BitXor,
            LeftShift,
            RightShift,
            UnsignedRightShift,
            Or,
            And,

        }

        public sealed record PaddingHelper(J.Binary T)
        {
            public JLeftPadded<J.Binary.Type> Operator => T._operator;

            public J.Binary WithOperator(JLeftPadded<J.Binary.Type> newOperator)
            {
                return T._operator == newOperator ? T : new J.Binary(T.Id, T.Prefix, T.Markers, T.Left, newOperator, T.Right, T.JavaType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Binary && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Block(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<bool> @static,
        IList<JRightPadded<Statement>> statements,
        Space end
    ) : J, Statement, MutableTree<Block>
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
            return v.VisitBlock(this, p);
        }

        public Guid Id => id;

        public Block WithId(Guid newId)
        {
            return newId == id ? this : new Block(newId, prefix, markers, _static, _statements, end);
        }

        public Space Prefix => prefix;

        public Block WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Block(id, newPrefix, markers, _static, _statements, end);
        }

        public Markers Markers => markers;

        public Block WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Block(id, prefix, newMarkers, _static, _statements, end);
        }

        private readonly JRightPadded<bool> _static = @static;
        public bool Static => _static.Element;

        public Block WithStatic(bool newStatic)
        {
            return Padding.WithStatic(_static.WithElement(newStatic));
        }

        private readonly IList<JRightPadded<Statement>> _statements = statements;
        public IList<Statement> Statements => _statements.Elements();

        public Block WithStatements(IList<Statement> newStatements)
        {
            return Padding.WithStatements(_statements.WithElements(newStatements));
        }

        public Space End => end;

        public Block WithEnd(Space newEnd)
        {
            return newEnd == end ? this : new Block(id, prefix, markers, _static, _statements, newEnd);
        }

        public sealed record PaddingHelper(J.Block T)
        {
            public JRightPadded<bool> Static => T._static;

            public J.Block WithStatic(JRightPadded<bool> newStatic)
            {
                return T._static == newStatic ? T : new J.Block(T.Id, T.Prefix, T.Markers, newStatic, T._statements, T.End);
            }

            public IList<JRightPadded<Statement>> Statements => T._statements;

            public J.Block WithStatements(IList<JRightPadded<Statement>> newStatements)
            {
                return T._statements == newStatements ? T : new J.Block(T.Id, T.Prefix, T.Markers, T._static, newStatements, T.End);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Block && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Break(
        Guid id,
        Space prefix,
        Markers markers,
        Identifier? label
    ) : J, Statement, MutableTree<Break>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitBreak(this, p);
        }

        public Guid Id => id;

        public Break WithId(Guid newId)
        {
            return newId == id ? this : new Break(newId, prefix, markers, label);
        }

        public Space Prefix => prefix;

        public Break WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Break(id, newPrefix, markers, label);
        }

        public Markers Markers => markers;

        public Break WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Break(id, prefix, newMarkers, label);
        }

        public J.Identifier? Label => label;

        public Break WithLabel(J.Identifier? newLabel)
        {
            return ReferenceEquals(newLabel, label) ? this : new Break(id, prefix, markers, newLabel);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Break && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Case(
        Guid id,
        Space prefix,
        Markers markers,
        Case.Type caseType,
        JContainer<Expression> expressions,
        JContainer<Statement> statements,
        JRightPadded<J>? body
    ) : J, Statement, MutableTree<Case>
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
            return v.VisitCase(this, p);
        }

        public Guid Id => id;

        public Case WithId(Guid newId)
        {
            return newId == id ? this : new Case(newId, prefix, markers, caseType, _expressions, _statements, _body);
        }

        public Space Prefix => prefix;

        public Case WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Case(id, newPrefix, markers, caseType, _expressions, _statements, _body);
        }

        public Markers Markers => markers;

        public Case WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Case(id, prefix, newMarkers, caseType, _expressions, _statements, _body);
        }

        public Type CaseType => caseType;

        public Case WithCaseType(Type newCaseType)
        {
            return newCaseType == caseType ? this : new Case(id, prefix, markers, newCaseType, _expressions, _statements, _body);
        }

        private readonly JContainer<Expression> _expressions = expressions;
        public IList<Expression> Expressions => _expressions.GetElements();

        public Case WithExpressions(IList<Expression> newExpressions)
        {
            return Padding.WithExpressions(JContainer<Expression>.WithElements(_expressions, newExpressions));
        }

        private readonly JContainer<Statement> _statements = statements;
        public IList<Statement> Statements => _statements.GetElements();

        public Case WithStatements(IList<Statement> newStatements)
        {
            return Padding.WithStatements(JContainer<Statement>.WithElements(_statements, newStatements));
        }

        private readonly JRightPadded<J>? _body = body;
        public J? Body => _body?.Element;

        public Case WithBody(J? newBody)
        {
            return Padding.WithBody(JRightPadded<J>.WithElement(_body, newBody));
        }

        public enum Type
        {
            Statement,
            Rule,

        }

        public sealed record PaddingHelper(J.Case T)
        {
            public JContainer<Expression> Expressions => T._expressions;

            public J.Case WithExpressions(JContainer<Expression> newExpressions)
            {
                return T._expressions == newExpressions ? T : new J.Case(T.Id, T.Prefix, T.Markers, T.CaseType, newExpressions, T._statements, T._body);
            }

            public JContainer<Statement> Statements => T._statements;

            public J.Case WithStatements(JContainer<Statement> newStatements)
            {
                return T._statements == newStatements ? T : new J.Case(T.Id, T.Prefix, T.Markers, T.CaseType, T._expressions, newStatements, T._body);
            }

            public JRightPadded<J>? Body => T._body;

            public J.Case WithBody(JRightPadded<J>? newBody)
            {
                return T._body == newBody ? T : new J.Case(T.Id, T.Prefix, T.Markers, T.CaseType, T._expressions, T._statements, newBody);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Case && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ClassDeclaration(
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
    ) : J, Statement, TypedTree, MutableTree<ClassDeclaration>
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
            return v.VisitClassDeclaration(this, p);
        }

        public Guid Id => id;

        public ClassDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new ClassDeclaration(newId, prefix, markers, leadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        public Space Prefix => prefix;

        public ClassDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ClassDeclaration(id, newPrefix, markers, leadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        public Markers Markers => markers;

        public ClassDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ClassDeclaration(id, prefix, newMarkers, leadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        public IList<J.Annotation> LeadingAnnotations => leadingAnnotations;

        public ClassDeclaration WithLeadingAnnotations(IList<J.Annotation> newLeadingAnnotations)
        {
            return newLeadingAnnotations == leadingAnnotations ? this : new ClassDeclaration(id, prefix, markers, newLeadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        public IList<J.Modifier> Modifiers => modifiers;

        public ClassDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new ClassDeclaration(id, prefix, markers, leadingAnnotations, newModifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        private readonly Kind _declarationKind = declarationKind;

        public ClassDeclaration WithDeclarationKind(Kind newDeclarationKind)
        {
            return ReferenceEquals(newDeclarationKind, _declarationKind) ? this : new ClassDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        public J.Identifier Name => name;

        public ClassDeclaration WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new ClassDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _declarationKind, newName, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, type);
        }

        private readonly JContainer<J.TypeParameter>? _typeParameters = typeParameters;
        public IList<J.TypeParameter>? TypeParameters => _typeParameters?.GetElements();

        public ClassDeclaration WithTypeParameters(IList<J.TypeParameter>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<J.TypeParameter>.WithElementsNullable(_typeParameters, newTypeParameters));
        }

        private readonly JContainer<Statement>? _primaryConstructor = primaryConstructor;
        public IList<Statement>? PrimaryConstructor => _primaryConstructor?.GetElements();

        public ClassDeclaration WithPrimaryConstructor(IList<Statement>? newPrimaryConstructor)
        {
            return Padding.WithPrimaryConstructor(JContainer<Statement>.WithElementsNullable(_primaryConstructor, newPrimaryConstructor));
        }

        private readonly JLeftPadded<TypeTree>? _extends = extends;
        public TypeTree? Extends => _extends?.Element;

        public ClassDeclaration WithExtends(TypeTree? newExtends)
        {
            return Padding.WithExtends(JLeftPadded<TypeTree>.WithElement(_extends, newExtends));
        }

        private readonly JContainer<TypeTree>? _implements = implements;
        public IList<TypeTree>? Implements => _implements?.GetElements();

        public ClassDeclaration WithImplements(IList<TypeTree>? newImplements)
        {
            return Padding.WithImplements(JContainer<TypeTree>.WithElementsNullable(_implements, newImplements));
        }

        private readonly JContainer<TypeTree>? _permits = permits;
        public IList<TypeTree>? Permits => _permits?.GetElements();

        public ClassDeclaration WithPermits(IList<TypeTree>? newPermits)
        {
            return Padding.WithPermits(JContainer<TypeTree>.WithElementsNullable(_permits, newPermits));
        }

        public J.Block Body => body;

        public ClassDeclaration WithBody(J.Block newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new ClassDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, newBody, type);
        }

        public JavaType.FullyQualified? Type => type;

        public ClassDeclaration WithType(JavaType.FullyQualified? newType)
        {
            return newType == type ? this : new ClassDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _declarationKind, name, _typeParameters, _primaryConstructor, _extends, _implements, _permits, body, newType);
        }

        public class Kind(
            Guid id,
            Space prefix,
            Markers markers,
            IList<J.Annotation> annotations,
            Kind.Type kindType
        ) : J, MutableTree<Kind>
        {
            public J? AcceptJava<P>(JavaVisitor<P> v, P p)
            {
                return v.VisitClassDeclarationKind(this, p);
            }

            public Guid Id => id;

            public Kind WithId(Guid newId)
            {
                return newId == id ? this : new Kind(newId, prefix, markers, annotations, kindType);
            }

            public Space Prefix => prefix;

            public Kind WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Kind(id, newPrefix, markers, annotations, kindType);
            }

            public Markers Markers => markers;

            public Kind WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Kind(id, prefix, newMarkers, annotations, kindType);
            }

            public IList<J.Annotation> Annotations => annotations;

            public Kind WithAnnotations(IList<J.Annotation> newAnnotations)
            {
                return newAnnotations == annotations ? this : new Kind(id, prefix, markers, newAnnotations, kindType);
            }

            public Type KindType => kindType;

            public Kind WithKindType(Type newKindType)
            {
                return newKindType == kindType ? this : new Kind(id, prefix, markers, annotations, newKindType);
            }

            public enum Type
            {
                Class,
                Enum,
                Interface,
                Annotation,
                Record,
                Value,

            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Kind && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public sealed record PaddingHelper(J.ClassDeclaration T)
        {
            public J.ClassDeclaration.Kind DeclarationKind => T._declarationKind;

            public J.ClassDeclaration WithDeclarationKind(J.ClassDeclaration.Kind newDeclarationKind)
            {
                return T._declarationKind == newDeclarationKind ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, newDeclarationKind, T.Name, T._typeParameters, T._primaryConstructor, T._extends, T._implements, T._permits, T.Body, T.Type);
            }

            public JContainer<J.TypeParameter>? TypeParameters => T._typeParameters;

            public J.ClassDeclaration WithTypeParameters(JContainer<J.TypeParameter>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, newTypeParameters, T._primaryConstructor, T._extends, T._implements, T._permits, T.Body, T.Type);
            }

            public JContainer<Statement>? PrimaryConstructor => T._primaryConstructor;

            public J.ClassDeclaration WithPrimaryConstructor(JContainer<Statement>? newPrimaryConstructor)
            {
                return T._primaryConstructor == newPrimaryConstructor ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, newPrimaryConstructor, T._extends, T._implements, T._permits, T.Body, T.Type);
            }

            public JLeftPadded<TypeTree>? Extends => T._extends;

            public J.ClassDeclaration WithExtends(JLeftPadded<TypeTree>? newExtends)
            {
                return T._extends == newExtends ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, T._primaryConstructor, newExtends, T._implements, T._permits, T.Body, T.Type);
            }

            public JContainer<TypeTree>? Implements => T._implements;

            public J.ClassDeclaration WithImplements(JContainer<TypeTree>? newImplements)
            {
                return T._implements == newImplements ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, T._primaryConstructor, T._extends, newImplements, T._permits, T.Body, T.Type);
            }

            public JContainer<TypeTree>? Permits => T._permits;

            public J.ClassDeclaration WithPermits(JContainer<TypeTree>? newPermits)
            {
                return T._permits == newPermits ? T : new J.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._declarationKind, T.Name, T._typeParameters, T._primaryConstructor, T._extends, T._implements, newPermits, T.Body, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ClassDeclaration && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CompilationUnit(
        Guid id,
        Space prefix,
        Markers markers,
        string sourcePath,
        FileAttributes? fileAttributes,
        string? charsetName,
        bool charsetBomMarked,
        Checksum? checksum,
        JRightPadded<Package>? packageDeclaration,
        IList<JRightPadded<Import>> imports,
        IList<ClassDeclaration> classes,
        Space eof
    ) : J, JavaSourceFile<CompilationUnit>, MutableSourceFile<CompilationUnit>, MutableTree<CompilationUnit>
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
            return v.VisitCompilationUnit(this, p);
        }

        public Guid Id => id;

        public CompilationUnit WithId(Guid newId)
        {
            return newId == id ? this : new CompilationUnit(newId, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public Space Prefix => prefix;

        public CompilationUnit WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new CompilationUnit(id, newPrefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public Markers Markers => markers;

        public CompilationUnit WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new CompilationUnit(id, prefix, newMarkers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public string SourcePath => sourcePath;

        public CompilationUnit WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new CompilationUnit(id, prefix, markers, newSourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public FileAttributes? FileAttributes => fileAttributes;

        public CompilationUnit WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new CompilationUnit(id, prefix, markers, sourcePath, newFileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public string? CharsetName => charsetName;

        public CompilationUnit WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, newCharsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public bool CharsetBomMarked => charsetBomMarked;

        public CompilationUnit WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, newCharsetBomMarked, checksum, _packageDeclaration, _imports, classes, eof);
        }

        public Checksum? Checksum => checksum;

        public CompilationUnit WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, newChecksum, _packageDeclaration, _imports, classes, eof);
        }

        private readonly JRightPadded<J.Package>? _packageDeclaration = packageDeclaration;
        public J.Package? PackageDeclaration => _packageDeclaration?.Element;

        public CompilationUnit WithPackageDeclaration(J.Package? newPackageDeclaration)
        {
            return Padding.WithPackageDeclaration(JRightPadded<J.Package>.WithElement(_packageDeclaration, newPackageDeclaration));
        }

        private readonly IList<JRightPadded<J.Import>> _imports = imports;
        public IList<J.Import> Imports => _imports.Elements();

        public CompilationUnit WithImports(IList<J.Import> newImports)
        {
            return Padding.WithImports(_imports.WithElements(newImports));
        }

        public IList<J.ClassDeclaration> Classes => classes;

        public CompilationUnit WithClasses(IList<J.ClassDeclaration> newClasses)
        {
            return newClasses == classes ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, newClasses, eof);
        }

        public Space Eof => eof;

        public CompilationUnit WithEof(Space newEof)
        {
            return newEof == eof ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _packageDeclaration, _imports, classes, newEof);
        }

        public sealed record PaddingHelper(J.CompilationUnit T)
        {
            public JRightPadded<J.Package>? PackageDeclaration => T._packageDeclaration;

            public J.CompilationUnit WithPackageDeclaration(JRightPadded<J.Package>? newPackageDeclaration)
            {
                return T._packageDeclaration == newPackageDeclaration ? T : new J.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, newPackageDeclaration, T._imports, T.Classes, T.Eof);
            }

            public IList<JRightPadded<J.Import>> Imports => T._imports;

            public J.CompilationUnit WithImports(IList<JRightPadded<J.Import>> newImports)
            {
                return T._imports == newImports ? T : new J.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, T._packageDeclaration, newImports, T.Classes, T.Eof);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is CompilationUnit && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public ITreeVisitor<Core.Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
        {
            return IPrinterFactory.Current()!.CreatePrinter<P>();
        }
    }

    public class Continue(
        Guid id,
        Space prefix,
        Markers markers,
        Identifier? label
    ) : J, Statement, MutableTree<Continue>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitContinue(this, p);
        }

        public Guid Id => id;

        public Continue WithId(Guid newId)
        {
            return newId == id ? this : new Continue(newId, prefix, markers, label);
        }

        public Space Prefix => prefix;

        public Continue WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Continue(id, newPrefix, markers, label);
        }

        public Markers Markers => markers;

        public Continue WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Continue(id, prefix, newMarkers, label);
        }

        public J.Identifier? Label => label;

        public Continue WithLabel(J.Identifier? newLabel)
        {
            return ReferenceEquals(newLabel, label) ? this : new Continue(id, prefix, markers, newLabel);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Continue && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DoWhileLoop(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Statement> body,
        JLeftPadded<J.ControlParentheses<Expression>> whileCondition
    ) : J, Loop, MutableTree<DoWhileLoop>
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
            return v.VisitDoWhileLoop(this, p);
        }

        public Guid Id => id;

        public DoWhileLoop WithId(Guid newId)
        {
            return newId == id ? this : new DoWhileLoop(newId, prefix, markers, _body, _whileCondition);
        }

        public Space Prefix => prefix;

        public DoWhileLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new DoWhileLoop(id, newPrefix, markers, _body, _whileCondition);
        }

        public Markers Markers => markers;

        public DoWhileLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new DoWhileLoop(id, prefix, newMarkers, _body, _whileCondition);
        }

        private readonly JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public DoWhileLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }

        private readonly JLeftPadded<J.ControlParentheses<Expression>> _whileCondition = whileCondition;
        public J.ControlParentheses<Expression> WhileCondition => _whileCondition.Element;

        public DoWhileLoop WithWhileCondition(J.ControlParentheses<Expression> newWhileCondition)
        {
            return Padding.WithWhileCondition(_whileCondition.WithElement(newWhileCondition));
        }

        public sealed record PaddingHelper(J.DoWhileLoop T)
        {
            public JRightPadded<Statement> Body => T._body;

            public J.DoWhileLoop WithBody(JRightPadded<Statement> newBody)
            {
                return T._body == newBody ? T : new J.DoWhileLoop(T.Id, T.Prefix, T.Markers, newBody, T._whileCondition);
            }

            public JLeftPadded<J.ControlParentheses<Expression>> WhileCondition => T._whileCondition;

            public J.DoWhileLoop WithWhileCondition(JLeftPadded<J.ControlParentheses<Expression>> newWhileCondition)
            {
                return T._whileCondition == newWhileCondition ? T : new J.DoWhileLoop(T.Id, T.Prefix, T.Markers, T._body, newWhileCondition);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is DoWhileLoop && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Empty(
        Guid id,
        Space prefix,
        Markers markers
    ) : J, Statement, Expression, TypeTree, MutableTree<Empty>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitEmpty(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public Empty WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public Empty WithId(Guid newId)
        {
            return newId == id ? this : new Empty(newId, prefix, markers);
        }

        public Space Prefix => prefix;

        public Empty WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Empty(id, newPrefix, markers);
        }

        public Markers Markers => markers;

        public Empty WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Empty(id, prefix, newMarkers);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Empty && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class EnumValue(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        Identifier name,
        NewClass? initializer
    ) : J, MutableTree<EnumValue>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitEnumValue(this, p);
        }

        public Guid Id => id;

        public EnumValue WithId(Guid newId)
        {
            return newId == id ? this : new EnumValue(newId, prefix, markers, annotations, name, initializer);
        }

        public Space Prefix => prefix;

        public EnumValue WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new EnumValue(id, newPrefix, markers, annotations, name, initializer);
        }

        public Markers Markers => markers;

        public EnumValue WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new EnumValue(id, prefix, newMarkers, annotations, name, initializer);
        }

        public IList<J.Annotation> Annotations => annotations;

        public EnumValue WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new EnumValue(id, prefix, markers, newAnnotations, name, initializer);
        }

        public J.Identifier Name => name;

        public EnumValue WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new EnumValue(id, prefix, markers, annotations, newName, initializer);
        }

        public J.NewClass? Initializer => initializer;

        public EnumValue WithInitializer(J.NewClass? newInitializer)
        {
            return ReferenceEquals(newInitializer, initializer) ? this : new EnumValue(id, prefix, markers, annotations, name, newInitializer);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is EnumValue && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class EnumValueSet(
        Guid id,
        Space prefix,
        Markers markers,
        IList<JRightPadded<EnumValue>> enums,
        bool terminatedWithSemicolon
    ) : J, Statement, MutableTree<EnumValueSet>
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
            return v.VisitEnumValueSet(this, p);
        }

        public Guid Id => id;

        public EnumValueSet WithId(Guid newId)
        {
            return newId == id ? this : new EnumValueSet(newId, prefix, markers, _enums, terminatedWithSemicolon);
        }

        public Space Prefix => prefix;

        public EnumValueSet WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new EnumValueSet(id, newPrefix, markers, _enums, terminatedWithSemicolon);
        }

        public Markers Markers => markers;

        public EnumValueSet WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new EnumValueSet(id, prefix, newMarkers, _enums, terminatedWithSemicolon);
        }

        private readonly IList<JRightPadded<J.EnumValue>> _enums = enums;
        public IList<J.EnumValue> Enums => _enums.Elements();

        public EnumValueSet WithEnums(IList<J.EnumValue> newEnums)
        {
            return Padding.WithEnums(_enums.WithElements(newEnums));
        }

        public bool TerminatedWithSemicolon => terminatedWithSemicolon;

        public EnumValueSet WithTerminatedWithSemicolon(bool newTerminatedWithSemicolon)
        {
            return newTerminatedWithSemicolon == terminatedWithSemicolon ? this : new EnumValueSet(id, prefix, markers, _enums, newTerminatedWithSemicolon);
        }

        public sealed record PaddingHelper(J.EnumValueSet T)
        {
            public IList<JRightPadded<J.EnumValue>> Enums => T._enums;

            public J.EnumValueSet WithEnums(IList<JRightPadded<J.EnumValue>> newEnums)
            {
                return T._enums == newEnums ? T : new J.EnumValueSet(T.Id, T.Prefix, T.Markers, newEnums, T.TerminatedWithSemicolon);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is EnumValueSet && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class FieldAccess(
        Guid id,
        Space prefix,
        Markers markers,
        Expression target,
        JLeftPadded<Identifier> name,
        JavaType? type
    ) : J, TypeTree, Expression, Statement, MutableTree<FieldAccess>
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
            return v.VisitFieldAccess(this, p);
        }

        public Guid Id => id;

        public FieldAccess WithId(Guid newId)
        {
            return newId == id ? this : new FieldAccess(newId, prefix, markers, target, _name, type);
        }

        public Space Prefix => prefix;

        public FieldAccess WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new FieldAccess(id, newPrefix, markers, target, _name, type);
        }

        public Markers Markers => markers;

        public FieldAccess WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new FieldAccess(id, prefix, newMarkers, target, _name, type);
        }

        public Expression Target => target;

        public FieldAccess WithTarget(Expression newTarget)
        {
            return ReferenceEquals(newTarget, target) ? this : new FieldAccess(id, prefix, markers, newTarget, _name, type);
        }

        private readonly JLeftPadded<J.Identifier> _name = name;
        public J.Identifier Name => _name.Element;

        public FieldAccess WithName(J.Identifier newName)
        {
            return Padding.WithName(_name.WithElement(newName));
        }

        public JavaType? Type => type;

        public FieldAccess WithType(JavaType? newType)
        {
            return newType == type ? this : new FieldAccess(id, prefix, markers, target, _name, newType);
        }

        public sealed record PaddingHelper(J.FieldAccess T)
        {
            public JLeftPadded<J.Identifier> Name => T._name;

            public J.FieldAccess WithName(JLeftPadded<J.Identifier> newName)
            {
                return T._name == newName ? T : new J.FieldAccess(T.Id, T.Prefix, T.Markers, T.Target, newName, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is FieldAccess && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ForEachLoop(
        Guid id,
        Space prefix,
        Markers markers,
        ForEachLoop.Control loopControl,
        JRightPadded<Statement> body
    ) : J, Loop, MutableTree<ForEachLoop>
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
            return v.VisitForEachLoop(this, p);
        }

        public Guid Id => id;

        public ForEachLoop WithId(Guid newId)
        {
            return newId == id ? this : new ForEachLoop(newId, prefix, markers, loopControl, _body);
        }

        public Space Prefix => prefix;

        public ForEachLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ForEachLoop(id, newPrefix, markers, loopControl, _body);
        }

        public Markers Markers => markers;

        public ForEachLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ForEachLoop(id, prefix, newMarkers, loopControl, _body);
        }

        public Control LoopControl => loopControl;

        public ForEachLoop WithLoopControl(Control newLoopControl)
        {
            return ReferenceEquals(newLoopControl, loopControl) ? this : new ForEachLoop(id, prefix, markers, newLoopControl, _body);
        }

        private readonly JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public ForEachLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }

        public class Control(
            Guid id,
            Space prefix,
            Markers markers,
            JRightPadded<J.VariableDeclarations> variable,
            JRightPadded<Expression> iterable
        ) : J, MutableTree<Control>
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
                return v.VisitForEachControl(this, p);
            }

            public Guid Id => id;

            public Control WithId(Guid newId)
            {
                return newId == id ? this : new Control(newId, prefix, markers, _variable, _iterable);
            }

            public Space Prefix => prefix;

            public Control WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Control(id, newPrefix, markers, _variable, _iterable);
            }

            public Markers Markers => markers;

            public Control WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Control(id, prefix, newMarkers, _variable, _iterable);
            }

            private readonly JRightPadded<J.VariableDeclarations> _variable = variable;
            public J.VariableDeclarations Variable => _variable.Element;

            public Control WithVariable(J.VariableDeclarations newVariable)
            {
                return Padding.WithVariable(_variable.WithElement(newVariable));
            }

            private readonly JRightPadded<Expression> _iterable = iterable;
            public Expression Iterable => _iterable.Element;

            public Control WithIterable(Expression newIterable)
            {
                return Padding.WithIterable(_iterable.WithElement(newIterable));
            }

            public sealed record PaddingHelper(J.ForEachLoop.Control T)
            {
                public JRightPadded<J.VariableDeclarations> Variable => T._variable;

                public J.ForEachLoop.Control WithVariable(JRightPadded<J.VariableDeclarations> newVariable)
                {
                    return T._variable == newVariable ? T : new J.ForEachLoop.Control(T.Id, T.Prefix, T.Markers, newVariable, T._iterable);
                }

                public JRightPadded<Expression> Iterable => T._iterable;

                public J.ForEachLoop.Control WithIterable(JRightPadded<Expression> newIterable)
                {
                    return T._iterable == newIterable ? T : new J.ForEachLoop.Control(T.Id, T.Prefix, T.Markers, T._variable, newIterable);
                }

            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Control && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public sealed record PaddingHelper(J.ForEachLoop T)
        {
            public JRightPadded<Statement> Body => T._body;

            public J.ForEachLoop WithBody(JRightPadded<Statement> newBody)
            {
                return T._body == newBody ? T : new J.ForEachLoop(T.Id, T.Prefix, T.Markers, T.LoopControl, newBody);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ForEachLoop && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ForLoop(
        Guid id,
        Space prefix,
        Markers markers,
        ForLoop.Control loopControl,
        JRightPadded<Statement> body
    ) : J, Loop, MutableTree<ForLoop>
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
            return v.VisitForLoop(this, p);
        }

        public Guid Id => id;

        public ForLoop WithId(Guid newId)
        {
            return newId == id ? this : new ForLoop(newId, prefix, markers, loopControl, _body);
        }

        public Space Prefix => prefix;

        public ForLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ForLoop(id, newPrefix, markers, loopControl, _body);
        }

        public Markers Markers => markers;

        public ForLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ForLoop(id, prefix, newMarkers, loopControl, _body);
        }

        public Control LoopControl => loopControl;

        public ForLoop WithLoopControl(Control newLoopControl)
        {
            return ReferenceEquals(newLoopControl, loopControl) ? this : new ForLoop(id, prefix, markers, newLoopControl, _body);
        }

        private readonly JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public ForLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }

        public class Control(
            Guid id,
            Space prefix,
            Markers markers,
            IList<JRightPadded<Statement>> init,
            JRightPadded<Expression> condition,
            IList<JRightPadded<Statement>> update
        ) : J, MutableTree<Control>
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
                return v.VisitForControl(this, p);
            }

            public Guid Id => id;

            public Control WithId(Guid newId)
            {
                return newId == id ? this : new Control(newId, prefix, markers, _init, _condition, _update);
            }

            public Space Prefix => prefix;

            public Control WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Control(id, newPrefix, markers, _init, _condition, _update);
            }

            public Markers Markers => markers;

            public Control WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Control(id, prefix, newMarkers, _init, _condition, _update);
            }

            private readonly IList<JRightPadded<Statement>> _init = init;
            public IList<Statement> Init => _init.Elements();

            public Control WithInit(IList<Statement> newInit)
            {
                return Padding.WithInit(_init.WithElements(newInit));
            }

            private readonly JRightPadded<Expression> _condition = condition;
            public Expression Condition => _condition.Element;

            public Control WithCondition(Expression newCondition)
            {
                return Padding.WithCondition(_condition.WithElement(newCondition));
            }

            private readonly IList<JRightPadded<Statement>> _update = update;
            public IList<Statement> Update => _update.Elements();

            public Control WithUpdate(IList<Statement> newUpdate)
            {
                return Padding.WithUpdate(_update.WithElements(newUpdate));
            }

            public sealed record PaddingHelper(J.ForLoop.Control T)
            {
                public IList<JRightPadded<Statement>> Init => T._init;

                public J.ForLoop.Control WithInit(IList<JRightPadded<Statement>> newInit)
                {
                    return T._init == newInit ? T : new J.ForLoop.Control(T.Id, T.Prefix, T.Markers, newInit, T._condition, T._update);
                }

                public JRightPadded<Expression> Condition => T._condition;

                public J.ForLoop.Control WithCondition(JRightPadded<Expression> newCondition)
                {
                    return T._condition == newCondition ? T : new J.ForLoop.Control(T.Id, T.Prefix, T.Markers, T._init, newCondition, T._update);
                }

                public IList<JRightPadded<Statement>> Update => T._update;

                public J.ForLoop.Control WithUpdate(IList<JRightPadded<Statement>> newUpdate)
                {
                    return T._update == newUpdate ? T : new J.ForLoop.Control(T.Id, T.Prefix, T.Markers, T._init, T._condition, newUpdate);
                }

            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Control && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public sealed record PaddingHelper(J.ForLoop T)
        {
            public JRightPadded<Statement> Body => T._body;

            public J.ForLoop WithBody(JRightPadded<Statement> newBody)
            {
                return T._body == newBody ? T : new J.ForLoop(T.Id, T.Prefix, T.Markers, T.LoopControl, newBody);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ForLoop && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class ParenthesizedTypeTree(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        J.Parentheses<TypeTree> parenthesizedType
    ) : J, TypeTree, Expression, MutableTree<ParenthesizedTypeTree>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitParenthesizedTypeTree(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public ParenthesizedTypeTree WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public ParenthesizedTypeTree WithId(Guid newId)
        {
            return newId == id ? this : new ParenthesizedTypeTree(newId, prefix, markers, annotations, parenthesizedType);
        }

        public Space Prefix => prefix;

        public ParenthesizedTypeTree WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ParenthesizedTypeTree(id, newPrefix, markers, annotations, parenthesizedType);
        }

        public Markers Markers => markers;

        public ParenthesizedTypeTree WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ParenthesizedTypeTree(id, prefix, newMarkers, annotations, parenthesizedType);
        }

        public IList<J.Annotation> Annotations => annotations;

        public ParenthesizedTypeTree WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new ParenthesizedTypeTree(id, prefix, markers, newAnnotations, parenthesizedType);
        }

        public J.Parentheses<TypeTree> ParenthesizedType => parenthesizedType;

        public ParenthesizedTypeTree WithParenthesizedType(J.Parentheses<TypeTree> newParenthesizedType)
        {
            return ReferenceEquals(newParenthesizedType, parenthesizedType) ? this : new ParenthesizedTypeTree(id, prefix, markers, annotations, newParenthesizedType);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ParenthesizedTypeTree && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class Identifier(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        string simpleName,
        JavaType? type,
        JavaType.Variable? fieldType
    ) : J, TypeTree, Expression, MutableTree<Identifier>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitIdentifier(this, p);
        }

        public Guid Id => id;

        public Identifier WithId(Guid newId)
        {
            return newId == id ? this : new Identifier(newId, prefix, markers, annotations, simpleName, type, fieldType);
        }

        public Space Prefix => prefix;

        public Identifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Identifier(id, newPrefix, markers, annotations, simpleName, type, fieldType);
        }

        public Markers Markers => markers;

        public Identifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Identifier(id, prefix, newMarkers, annotations, simpleName, type, fieldType);
        }

        public IList<J.Annotation> Annotations => annotations;

        public Identifier WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new Identifier(id, prefix, markers, newAnnotations, simpleName, type, fieldType);
        }

        public string SimpleName => simpleName;

        public Identifier WithSimpleName(string newSimpleName)
        {
            return newSimpleName == simpleName ? this : new Identifier(id, prefix, markers, annotations, newSimpleName, type, fieldType);
        }

        public JavaType? Type => type;

        public Identifier WithType(JavaType? newType)
        {
            return newType == type ? this : new Identifier(id, prefix, markers, annotations, simpleName, newType, fieldType);
        }

        public JavaType.Variable? FieldType => fieldType;

        public Identifier WithFieldType(JavaType.Variable? newFieldType)
        {
            return newFieldType == fieldType ? this : new Identifier(id, prefix, markers, annotations, simpleName, type, newFieldType);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Identifier && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class If(
        Guid id,
        Space prefix,
        Markers markers,
        J.ControlParentheses<Expression> ifCondition,
        JRightPadded<Statement> thenPart,
        If.Else? elsePart
    ) : J, Statement, MutableTree<If>
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
            return v.VisitIf(this, p);
        }

        public Guid Id => id;

        public If WithId(Guid newId)
        {
            return newId == id ? this : new If(newId, prefix, markers, ifCondition, _thenPart, elsePart);
        }

        public Space Prefix => prefix;

        public If WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new If(id, newPrefix, markers, ifCondition, _thenPart, elsePart);
        }

        public Markers Markers => markers;

        public If WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new If(id, prefix, newMarkers, ifCondition, _thenPart, elsePart);
        }

        public J.ControlParentheses<Expression> IfCondition => ifCondition;

        public If WithIfCondition(J.ControlParentheses<Expression> newIfCondition)
        {
            return ReferenceEquals(newIfCondition, ifCondition) ? this : new If(id, prefix, markers, newIfCondition, _thenPart, elsePart);
        }

        private readonly JRightPadded<Statement> _thenPart = thenPart;
        public Statement ThenPart => _thenPart.Element;

        public If WithThenPart(Statement newThenPart)
        {
            return Padding.WithThenPart(_thenPart.WithElement(newThenPart));
        }

        public Else? ElsePart => elsePart;

        public If WithElsePart(Else? newElsePart)
        {
            return ReferenceEquals(newElsePart, elsePart) ? this : new If(id, prefix, markers, ifCondition, _thenPart, newElsePart);
        }

        public class Else(
            Guid id,
            Space prefix,
            Markers markers,
            JRightPadded<Statement> body
        ) : J, MutableTree<Else>
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
                return v.VisitElse(this, p);
            }

            public Guid Id => id;

            public Else WithId(Guid newId)
            {
                return newId == id ? this : new Else(newId, prefix, markers, _body);
            }

            public Space Prefix => prefix;

            public Else WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Else(id, newPrefix, markers, _body);
            }

            public Markers Markers => markers;

            public Else WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Else(id, prefix, newMarkers, _body);
            }

            private readonly JRightPadded<Statement> _body = body;
            public Statement Body => _body.Element;

            public Else WithBody(Statement newBody)
            {
                return Padding.WithBody(_body.WithElement(newBody));
            }

            public sealed record PaddingHelper(J.If.Else T)
            {
                public JRightPadded<Statement> Body => T._body;

                public J.If.Else WithBody(JRightPadded<Statement> newBody)
                {
                    return T._body == newBody ? T : new J.If.Else(T.Id, T.Prefix, T.Markers, newBody);
                }

            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Else && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public sealed record PaddingHelper(J.If T)
        {
            public JRightPadded<Statement> ThenPart => T._thenPart;

            public J.If WithThenPart(JRightPadded<Statement> newThenPart)
            {
                return T._thenPart == newThenPart ? T : new J.If(T.Id, T.Prefix, T.Markers, T.IfCondition, newThenPart, T.ElsePart);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is If && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Import(
        Guid id,
        Space prefix,
        Markers markers,
        JLeftPadded<bool> @static,
        FieldAccess qualid,
        JLeftPadded<Identifier>? alias
    ) : Statement, MutableTree<Import>
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
            return v.VisitImport(this, p);
        }

        public Guid Id => id;

        public Import WithId(Guid newId)
        {
            return newId == id ? this : new Import(newId, prefix, markers, _static, qualid, _alias);
        }

        public Space Prefix => prefix;

        public Import WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Import(id, newPrefix, markers, _static, qualid, _alias);
        }

        public Markers Markers => markers;

        public Import WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Import(id, prefix, newMarkers, _static, qualid, _alias);
        }

        private readonly JLeftPadded<bool> _static = @static;
        public bool Static => _static.Element;

        public Import WithStatic(bool newStatic)
        {
            return Padding.WithStatic(_static.WithElement(newStatic));
        }

        public J.FieldAccess Qualid => qualid;

        public Import WithQualid(J.FieldAccess newQualid)
        {
            return ReferenceEquals(newQualid, qualid) ? this : new Import(id, prefix, markers, _static, newQualid, _alias);
        }

        private readonly JLeftPadded<J.Identifier>? _alias = alias;
        public J.Identifier? Alias => _alias?.Element;

        public Import WithAlias(J.Identifier? newAlias)
        {
            return Padding.WithAlias(JLeftPadded<J.Identifier>.WithElement(_alias, newAlias));
        }

        public sealed record PaddingHelper(J.Import T)
        {
            public JLeftPadded<bool> Static => T._static;

            public J.Import WithStatic(JLeftPadded<bool> newStatic)
            {
                return T._static == newStatic ? T : new J.Import(T.Id, T.Prefix, T.Markers, newStatic, T.Qualid, T._alias);
            }

            public JLeftPadded<J.Identifier>? Alias => T._alias;

            public J.Import WithAlias(JLeftPadded<J.Identifier>? newAlias)
            {
                return T._alias == newAlias ? T : new J.Import(T.Id, T.Prefix, T.Markers, T._static, T.Qualid, newAlias);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Import && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class InstanceOf(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> expression,
        J clazz,
        J? pattern,
        JavaType? type
    ) : J, Expression, TypedTree, MutableTree<InstanceOf>
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
            return v.VisitInstanceOf(this, p);
        }

        public Guid Id => id;

        public InstanceOf WithId(Guid newId)
        {
            return newId == id ? this : new InstanceOf(newId, prefix, markers, _expression, clazz, pattern, type);
        }

        public Space Prefix => prefix;

        public InstanceOf WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new InstanceOf(id, newPrefix, markers, _expression, clazz, pattern, type);
        }

        public Markers Markers => markers;

        public InstanceOf WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new InstanceOf(id, prefix, newMarkers, _expression, clazz, pattern, type);
        }

        private readonly JRightPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public InstanceOf WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }

        public J Clazz => clazz;

        public InstanceOf WithClazz(J newClazz)
        {
            return ReferenceEquals(newClazz, clazz) ? this : new InstanceOf(id, prefix, markers, _expression, newClazz, pattern, type);
        }

        public J? Pattern => pattern;

        public InstanceOf WithPattern(J? newPattern)
        {
            return ReferenceEquals(newPattern, pattern) ? this : new InstanceOf(id, prefix, markers, _expression, clazz, newPattern, type);
        }

        public JavaType? Type => type;

        public InstanceOf WithType(JavaType? newType)
        {
            return newType == type ? this : new InstanceOf(id, prefix, markers, _expression, clazz, pattern, newType);
        }

        public sealed record PaddingHelper(J.InstanceOf T)
        {
            public JRightPadded<Expression> Expression => T._expression;

            public J.InstanceOf WithExpression(JRightPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new J.InstanceOf(T.Id, T.Prefix, T.Markers, newExpression, T.Clazz, T.Pattern, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is InstanceOf && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class IntersectionType(
        Guid id,
        Space prefix,
        Markers markers,
        JContainer<TypeTree> bounds
    ) : J, TypeTree, Expression, MutableTree<IntersectionType>
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
            return v.VisitIntersectionType(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public IntersectionType WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public IntersectionType WithId(Guid newId)
        {
            return newId == id ? this : new IntersectionType(newId, prefix, markers, _bounds);
        }

        public Space Prefix => prefix;

        public IntersectionType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new IntersectionType(id, newPrefix, markers, _bounds);
        }

        public Markers Markers => markers;

        public IntersectionType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new IntersectionType(id, prefix, newMarkers, _bounds);
        }

        private readonly JContainer<TypeTree> _bounds = bounds;
        public IList<TypeTree> Bounds => _bounds.GetElements();

        public IntersectionType WithBounds(IList<TypeTree> newBounds)
        {
            return Padding.WithBounds(JContainer<TypeTree>.WithElements(_bounds, newBounds));
        }

        public sealed record PaddingHelper(J.IntersectionType T)
        {
            public JContainer<TypeTree> Bounds => T._bounds;

            public J.IntersectionType WithBounds(JContainer<TypeTree> newBounds)
            {
                return T._bounds == newBounds ? T : new J.IntersectionType(T.Id, T.Prefix, T.Markers, newBounds);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is IntersectionType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Label(
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Label && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Lambda(
        Guid id,
        Space prefix,
        Markers markers,
        Lambda.Parameters @params,
        Space arrow,
        J body,
        JavaType? type
    ) : J, Statement, Expression, TypedTree, MutableTree<Lambda>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitLambda(this, p);
        }

        public Guid Id => id;

        public Lambda WithId(Guid newId)
        {
            return newId == id ? this : new Lambda(newId, prefix, markers, @params, arrow, body, type);
        }

        public Space Prefix => prefix;

        public Lambda WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Lambda(id, newPrefix, markers, @params, arrow, body, type);
        }

        public Markers Markers => markers;

        public Lambda WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Lambda(id, prefix, newMarkers, @params, arrow, body, type);
        }

        public Parameters Params => @params;

        public Lambda WithParams(Parameters newParams)
        {
            return ReferenceEquals(newParams, @params) ? this : new Lambda(id, prefix, markers, newParams, arrow, body, type);
        }

        public Space Arrow => arrow;

        public Lambda WithArrow(Space newArrow)
        {
            return newArrow == arrow ? this : new Lambda(id, prefix, markers, @params, newArrow, body, type);
        }

        public J Body => body;

        public Lambda WithBody(J newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new Lambda(id, prefix, markers, @params, arrow, newBody, type);
        }

        public JavaType? Type => type;

        public Lambda WithType(JavaType? newType)
        {
            return newType == type ? this : new Lambda(id, prefix, markers, @params, arrow, body, newType);
        }

        public class Parameters(
            Guid id,
            Space prefix,
            Markers markers,
            bool parenthesized,
            IList<JRightPadded<J>> elements
        ) : J, MutableTree<Parameters>
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
                return v.VisitLambdaParameters(this, p);
            }

            public Guid Id => id;

            public Parameters WithId(Guid newId)
            {
                return newId == id ? this : new Parameters(newId, prefix, markers, parenthesized, _elements);
            }

            public Space Prefix => prefix;

            public Parameters WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Parameters(id, newPrefix, markers, parenthesized, _elements);
            }

            public Markers Markers => markers;

            public Parameters WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Parameters(id, prefix, newMarkers, parenthesized, _elements);
            }

            public bool Parenthesized => parenthesized;

            public Parameters WithParenthesized(bool newParenthesized)
            {
                return newParenthesized == parenthesized ? this : new Parameters(id, prefix, markers, newParenthesized, _elements);
            }

            private readonly IList<JRightPadded<J>> _elements = elements;
            public IList<J> Elements => _elements.Elements();

            public Parameters WithElements(IList<J> newElements)
            {
                return Padding.WithElements(_elements.WithElements(newElements));
            }

            public sealed record PaddingHelper(J.Lambda.Parameters T)
            {
                public IList<JRightPadded<J>> Elements => T._elements;

                public J.Lambda.Parameters WithElements(IList<JRightPadded<J>> newElements)
                {
                    return T._elements == newElements ? T : new J.Lambda.Parameters(T.Id, T.Prefix, T.Markers, T.Parenthesized, newElements);
                }

            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Parameters && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Lambda && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Literal(
        Guid id,
        Space prefix,
        Markers markers,
        object? value,
        string? valueSource,
        IList<Literal.UnicodeEscape>? unicodeEscapes,
        JavaType.Primitive type
    ) : J, Expression, TypedTree, MutableTree<Literal>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitLiteral(this, p);
        }

        public Guid Id => id;

        public Literal WithId(Guid newId)
        {
            return newId == id ? this : new Literal(newId, prefix, markers, value, valueSource, unicodeEscapes, type);
        }

        public Space Prefix => prefix;

        public Literal WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Literal(id, newPrefix, markers, value, valueSource, unicodeEscapes, type);
        }

        public Markers Markers => markers;

        public Literal WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Literal(id, prefix, newMarkers, value, valueSource, unicodeEscapes, type);
        }

        public object? Value => value;

        public Literal WithValue(object? newValue)
        {
            return newValue == value ? this : new Literal(id, prefix, markers, newValue, valueSource, unicodeEscapes, type);
        }

        public string? ValueSource => valueSource;

        public Literal WithValueSource(string? newValueSource)
        {
            return newValueSource == valueSource ? this : new Literal(id, prefix, markers, value, newValueSource, unicodeEscapes, type);
        }

        public IList<UnicodeEscape>? UnicodeEscapes => unicodeEscapes;

        public Literal WithUnicodeEscapes(IList<UnicodeEscape>? newUnicodeEscapes)
        {
            return newUnicodeEscapes == unicodeEscapes ? this : new Literal(id, prefix, markers, value, valueSource, newUnicodeEscapes, type);
        }

        public JavaType.Primitive Type => type;

        public Literal WithType(JavaType.Primitive newType)
        {
            return newType == type ? this : new Literal(id, prefix, markers, value, valueSource, unicodeEscapes, newType);
        }

        public sealed record UnicodeEscape(
            int valueSourceIndex,
            string codePoint
        )
        {
            public int ValueSourceIndex => valueSourceIndex;

            public UnicodeEscape WithValueSourceIndex(int newValueSourceIndex)
            {
                return newValueSourceIndex == valueSourceIndex ? this : new UnicodeEscape(newValueSourceIndex, codePoint);
            }

            public string CodePoint => codePoint;

            public UnicodeEscape WithCodePoint(string newCodePoint)
            {
                return newCodePoint == codePoint ? this : new UnicodeEscape(valueSourceIndex, newCodePoint);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Literal && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class MemberReference(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> containing,
        JContainer<Expression>? typeParameters,
        JLeftPadded<Identifier> reference,
        JavaType? type,
        JavaType.Method? methodType,
        JavaType.Variable? variableType
    ) : J, Expression, TypedTree, MethodCall, MutableTree<MemberReference>
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
            return v.VisitMemberReference(this, p);
        }

        public Guid Id => id;

        public MemberReference WithId(Guid newId)
        {
            return newId == id ? this : new MemberReference(newId, prefix, markers, _containing, _typeParameters, _reference, type, methodType, variableType);
        }

        public Space Prefix => prefix;

        public MemberReference WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MemberReference(id, newPrefix, markers, _containing, _typeParameters, _reference, type, methodType, variableType);
        }

        public Markers Markers => markers;

        public MemberReference WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MemberReference(id, prefix, newMarkers, _containing, _typeParameters, _reference, type, methodType, variableType);
        }

        private readonly JRightPadded<Expression> _containing = containing;
        public Expression Containing => _containing.Element;

        public MemberReference WithContaining(Expression newContaining)
        {
            return Padding.WithContaining(_containing.WithElement(newContaining));
        }

        private readonly JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public MemberReference WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }

        private readonly JLeftPadded<J.Identifier> _reference = reference;
        public J.Identifier Reference => _reference.Element;

        public MemberReference WithReference(J.Identifier newReference)
        {
            return Padding.WithReference(_reference.WithElement(newReference));
        }

        public JavaType? Type => type;

        public MemberReference WithType(JavaType? newType)
        {
            return newType == type ? this : new MemberReference(id, prefix, markers, _containing, _typeParameters, _reference, newType, methodType, variableType);
        }

        public JavaType.Method? MethodType => methodType;

        public MemberReference WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == methodType ? this : new MemberReference(id, prefix, markers, _containing, _typeParameters, _reference, type, newMethodType, variableType);
        }

        public JavaType.Variable? VariableType => variableType;

        public MemberReference WithVariableType(JavaType.Variable? newVariableType)
        {
            return newVariableType == variableType ? this : new MemberReference(id, prefix, markers, _containing, _typeParameters, _reference, type, methodType, newVariableType);
        }

        public sealed record PaddingHelper(J.MemberReference T)
        {
            public JRightPadded<Expression> Containing => T._containing;

            public J.MemberReference WithContaining(JRightPadded<Expression> newContaining)
            {
                return T._containing == newContaining ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, newContaining, T._typeParameters, T._reference, T.Type, T.MethodType, T.VariableType);
            }

            public JContainer<Expression>? TypeParameters => T._typeParameters;

            public J.MemberReference WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, T._containing, newTypeParameters, T._reference, T.Type, T.MethodType, T.VariableType);
            }

            public JLeftPadded<J.Identifier> Reference => T._reference;

            public J.MemberReference WithReference(JLeftPadded<J.Identifier> newReference)
            {
                return T._reference == newReference ? T : new J.MemberReference(T.Id, T.Prefix, T.Markers, T._containing, T._typeParameters, newReference, T.Type, T.MethodType, T.VariableType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MemberReference && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class MethodDeclaration(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> leadingAnnotations,
        IList<Modifier> modifiers,
        TypeParameters? typeParameters,
        TypeTree? returnTypeExpression,
        MethodDeclaration.IdentifierWithAnnotations name,
        JContainer<Statement> parameters,
        JContainer<NameTree>? throws,
        Block? body,
        JLeftPadded<Expression>? defaultValue,
        JavaType.Method? methodType
    ) : J, Statement, TypedTree, MutableTree<MethodDeclaration>
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

        [NonSerialized] private WeakReference<AnnotationsHelper>? _annotations;

        public AnnotationsHelper Annotations
        {
            get
            {
                AnnotationsHelper? p;
                if (_annotations == null)
                {
                    p = new AnnotationsHelper(this);
                    _annotations = new WeakReference<AnnotationsHelper>(p);
                }
                else
                {
                    _annotations.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new AnnotationsHelper(this);
                        _annotations.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitMethodDeclaration(this, p);
        }

        public Guid Id => id;

        public MethodDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new MethodDeclaration(newId, prefix, markers, leadingAnnotations, modifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, body, _defaultValue, methodType);
        }

        public Space Prefix => prefix;

        public MethodDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MethodDeclaration(id, newPrefix, markers, leadingAnnotations, modifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, body, _defaultValue, methodType);
        }

        public Markers Markers => markers;

        public MethodDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MethodDeclaration(id, prefix, newMarkers, leadingAnnotations, modifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, body, _defaultValue, methodType);
        }

        public IList<J.Annotation> LeadingAnnotations => leadingAnnotations;

        public MethodDeclaration WithLeadingAnnotations(IList<J.Annotation> newLeadingAnnotations)
        {
            return newLeadingAnnotations == leadingAnnotations ? this : new MethodDeclaration(id, prefix, markers, newLeadingAnnotations, modifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, body, _defaultValue, methodType);
        }

        public IList<J.Modifier> Modifiers => modifiers;

        public MethodDeclaration WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new MethodDeclaration(id, prefix, markers, leadingAnnotations, newModifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, body, _defaultValue, methodType);
        }

        private readonly J.TypeParameters? _typeParameters = typeParameters;

        public TypeTree? ReturnTypeExpression => returnTypeExpression;

        public MethodDeclaration WithReturnTypeExpression(TypeTree? newReturnTypeExpression)
        {
            return ReferenceEquals(newReturnTypeExpression, returnTypeExpression) ? this : new MethodDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _typeParameters, newReturnTypeExpression, _name, _parameters, _throws, body, _defaultValue, methodType);
        }

        private readonly IdentifierWithAnnotations _name = name;

        private readonly JContainer<Statement> _parameters = parameters;
        public IList<Statement> Parameters => _parameters.GetElements();

        public MethodDeclaration WithParameters(IList<Statement> newParameters)
        {
            return Padding.WithParameters(JContainer<Statement>.WithElements(_parameters, newParameters));
        }

        private readonly JContainer<NameTree>? _throws = throws;
        public IList<NameTree>? Throws => _throws?.GetElements();

        public MethodDeclaration WithThrows(IList<NameTree>? newThrows)
        {
            return Padding.WithThrows(JContainer<NameTree>.WithElementsNullable(_throws, newThrows));
        }

        public J.Block? Body => body;

        public MethodDeclaration WithBody(J.Block? newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new MethodDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, newBody, _defaultValue, methodType);
        }

        private readonly JLeftPadded<Expression>? _defaultValue = defaultValue;
        public Expression? DefaultValue => _defaultValue?.Element;

        public MethodDeclaration WithDefaultValue(Expression? newDefaultValue)
        {
            return Padding.WithDefaultValue(JLeftPadded<Expression>.WithElement(_defaultValue, newDefaultValue));
        }

        public JavaType.Method? MethodType => methodType;

        public MethodDeclaration WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == methodType ? this : new MethodDeclaration(id, prefix, markers, leadingAnnotations, modifiers, _typeParameters, returnTypeExpression, _name, _parameters, _throws, body, _defaultValue, newMethodType);
        }

        public sealed record IdentifierWithAnnotations(
            J.Identifier identifier,
            IList<J.Annotation> annotations
        )
        {
            public J.Identifier Identifier => identifier;

            public IdentifierWithAnnotations WithIdentifier(J.Identifier newIdentifier)
            {
                return ReferenceEquals(newIdentifier, identifier) ? this : new IdentifierWithAnnotations(newIdentifier, annotations);
            }

            public IList<J.Annotation> Annotations => annotations;

            public IdentifierWithAnnotations WithAnnotations(IList<J.Annotation> newAnnotations)
            {
                return newAnnotations == annotations ? this : new IdentifierWithAnnotations(identifier, newAnnotations);
            }

        }

        public sealed record PaddingHelper(J.MethodDeclaration T)
        {
            public J.TypeParameters? TypeParameters => T._typeParameters;

            public J.MethodDeclaration WithTypeParameters(J.TypeParameters? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, newTypeParameters, T.ReturnTypeExpression, T._name, T._parameters, T._throws, T.Body, T._defaultValue, T.MethodType);
            }

            public J.MethodDeclaration.IdentifierWithAnnotations Name => T._name;

            public J.MethodDeclaration WithName(J.MethodDeclaration.IdentifierWithAnnotations newName)
            {
                return T._name == newName ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, newName, T._parameters, T._throws, T.Body, T._defaultValue, T.MethodType);
            }

            public JContainer<Statement> Parameters => T._parameters;

            public J.MethodDeclaration WithParameters(JContainer<Statement> newParameters)
            {
                return T._parameters == newParameters ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._name, newParameters, T._throws, T.Body, T._defaultValue, T.MethodType);
            }

            public JContainer<NameTree>? Throws => T._throws;

            public J.MethodDeclaration WithThrows(JContainer<NameTree>? newThrows)
            {
                return T._throws == newThrows ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._name, T._parameters, newThrows, T.Body, T._defaultValue, T.MethodType);
            }

            public JLeftPadded<Expression>? DefaultValue => T._defaultValue;

            public J.MethodDeclaration WithDefaultValue(JLeftPadded<Expression>? newDefaultValue)
            {
                return T._defaultValue == newDefaultValue ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._name, T._parameters, T._throws, T.Body, newDefaultValue, T.MethodType);
            }

        }

        public sealed record AnnotationsHelper(J.MethodDeclaration T)
        {
            public J.TypeParameters? TypeParameters => T._typeParameters;

            public J.MethodDeclaration WithTypeParameters(J.TypeParameters? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, newTypeParameters, T.ReturnTypeExpression, T._name, T._parameters, T._throws, T.Body, T._defaultValue, T.MethodType);
            }

            public J.MethodDeclaration.IdentifierWithAnnotations Name => T._name;

            public J.MethodDeclaration WithName(J.MethodDeclaration.IdentifierWithAnnotations newName)
            {
                return T._name == newName ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, newName, T._parameters, T._throws, T.Body, T._defaultValue, T.MethodType);
            }

            public JContainer<Statement> Parameters => T._parameters;

            public J.MethodDeclaration WithParameters(JContainer<Statement> newParameters)
            {
                return T._parameters == newParameters ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._name, newParameters, T._throws, T.Body, T._defaultValue, T.MethodType);
            }

            public JContainer<NameTree>? Throws => T._throws;

            public J.MethodDeclaration WithThrows(JContainer<NameTree>? newThrows)
            {
                return T._throws == newThrows ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._name, T._parameters, newThrows, T.Body, T._defaultValue, T.MethodType);
            }

            public JLeftPadded<Expression>? DefaultValue => T._defaultValue;

            public J.MethodDeclaration WithDefaultValue(JLeftPadded<Expression>? newDefaultValue)
            {
                return T._defaultValue == newDefaultValue ? T : new J.MethodDeclaration(T.Id, T.Prefix, T.Markers, T.LeadingAnnotations, T.Modifiers, T._typeParameters, T.ReturnTypeExpression, T._name, T._parameters, T._throws, T.Body, newDefaultValue, T.MethodType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MethodDeclaration && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class MethodInvocation(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression>? select,
        JContainer<Expression>? typeParameters,
        Identifier name,
        JContainer<Expression> arguments,
        JavaType.Method? methodType
    ) : J, Statement, Expression, TypedTree, MethodCall, MutableTree<MethodInvocation>
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
            return v.VisitMethodInvocation(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public MethodInvocation WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public MethodInvocation WithId(Guid newId)
        {
            return newId == id ? this : new MethodInvocation(newId, prefix, markers, _select, _typeParameters, name, _arguments, methodType);
        }

        public Space Prefix => prefix;

        public MethodInvocation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MethodInvocation(id, newPrefix, markers, _select, _typeParameters, name, _arguments, methodType);
        }

        public Markers Markers => markers;

        public MethodInvocation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MethodInvocation(id, prefix, newMarkers, _select, _typeParameters, name, _arguments, methodType);
        }

        private readonly JRightPadded<Expression>? _select = select;
        public Expression? Select => _select?.Element;

        public MethodInvocation WithSelect(Expression? newSelect)
        {
            return Padding.WithSelect(JRightPadded<Expression>.WithElement(_select, newSelect));
        }

        private readonly JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public MethodInvocation WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }

        public J.Identifier Name => name;

        public MethodInvocation WithName(J.Identifier newName)
        {
            return Extensions.WithName(this, newName);
        }

        private readonly JContainer<Expression> _arguments = arguments;
        public IList<Expression> Arguments => _arguments.GetElements();

        public MethodInvocation WithArguments(IList<Expression> newArguments)
        {
            return Padding.WithArguments(JContainer<Expression>.WithElements(_arguments, newArguments));
        }

        public JavaType.Method? MethodType => methodType;

        public MethodInvocation WithMethodType(JavaType.Method? newMethodType)
        {
            return newMethodType == methodType ? this : new MethodInvocation(id, prefix, markers, _select, _typeParameters, name, _arguments, newMethodType);
        }

        public sealed record PaddingHelper(J.MethodInvocation T)
        {
            public JRightPadded<Expression>? Select => T._select;

            public J.MethodInvocation WithSelect(JRightPadded<Expression>? newSelect)
            {
                return T._select == newSelect ? T : new J.MethodInvocation(T.Id, T.Prefix, T.Markers, newSelect, T._typeParameters, T.Name, T._arguments, T.MethodType);
            }

            public JContainer<Expression>? TypeParameters => T._typeParameters;

            public J.MethodInvocation WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.MethodInvocation(T.Id, T.Prefix, T.Markers, T._select, newTypeParameters, T.Name, T._arguments, T.MethodType);
            }

            public JContainer<Expression> Arguments => T._arguments;

            public J.MethodInvocation WithArguments(JContainer<Expression> newArguments)
            {
                return T._arguments == newArguments ? T : new J.MethodInvocation(T.Id, T.Prefix, T.Markers, T._select, T._typeParameters, T.Name, newArguments, T.MethodType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MethodInvocation && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Modifier(
        Guid id,
        Space prefix,
        Markers markers,
        string? keyword,
        Modifier.Type modifierType,
        IList<Annotation> annotations
    ) : J, MutableTree<Modifier>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitModifier(this, p);
        }

        public Guid Id => id;

        public Modifier WithId(Guid newId)
        {
            return newId == id ? this : new Modifier(newId, prefix, markers, keyword, modifierType, annotations);
        }

        public Space Prefix => prefix;

        public Modifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Modifier(id, newPrefix, markers, keyword, modifierType, annotations);
        }

        public Markers Markers => markers;

        public Modifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Modifier(id, prefix, newMarkers, keyword, modifierType, annotations);
        }

        public string? Keyword => keyword;

        public Modifier WithKeyword(string? newKeyword)
        {
            return newKeyword == keyword ? this : new Modifier(id, prefix, markers, newKeyword, modifierType, annotations);
        }

        public Type ModifierType => modifierType;

        public Modifier WithModifierType(Type newModifierType)
        {
            return newModifierType == modifierType ? this : new Modifier(id, prefix, markers, keyword, newModifierType, annotations);
        }

        public IList<J.Annotation> Annotations => annotations;

        public Modifier WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new Modifier(id, prefix, markers, keyword, modifierType, newAnnotations);
        }

        public enum Type
        {
            Default,
            Public,
            Protected,
            Private,
            Abstract,
            Static,
            Final,
            Sealed,
            NonSealed,
            Transient,
            Volatile,
            Synchronized,
            Native,
            Strictfp,
            Async,
            Reified,
            Inline,
            LanguageExtension,

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Modifier && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class MultiCatch(
        Guid id,
        Space prefix,
        Markers markers,
        IList<JRightPadded<NameTree>> alternatives
    ) : J, TypeTree, MutableTree<MultiCatch>
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
            return v.VisitMultiCatch(this, p);
        }

        public Guid Id => id;

        public MultiCatch WithId(Guid newId)
        {
            return newId == id ? this : new MultiCatch(newId, prefix, markers, _alternatives);
        }

        public Space Prefix => prefix;

        public MultiCatch WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MultiCatch(id, newPrefix, markers, _alternatives);
        }

        public Markers Markers => markers;

        public MultiCatch WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MultiCatch(id, prefix, newMarkers, _alternatives);
        }

        private readonly IList<JRightPadded<NameTree>> _alternatives = alternatives;
        public IList<NameTree> Alternatives => _alternatives.Elements();

        public MultiCatch WithAlternatives(IList<NameTree> newAlternatives)
        {
            return Padding.WithAlternatives(_alternatives.WithElements(newAlternatives));
        }

        public sealed record PaddingHelper(J.MultiCatch T)
        {
            public IList<JRightPadded<NameTree>> Alternatives => T._alternatives;

            public J.MultiCatch WithAlternatives(IList<JRightPadded<NameTree>> newAlternatives)
            {
                return T._alternatives == newAlternatives ? T : new J.MultiCatch(T.Id, T.Prefix, T.Markers, newAlternatives);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MultiCatch && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class NewArray(
        Guid id,
        Space prefix,
        Markers markers,
        TypeTree? typeExpression,
        IList<ArrayDimension> dimensions,
        JContainer<Expression>? initializer,
        JavaType? type
    ) : J, Expression, TypedTree, MutableTree<NewArray>
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
            return v.VisitNewArray(this, p);
        }

        public Guid Id => id;

        public NewArray WithId(Guid newId)
        {
            return newId == id ? this : new NewArray(newId, prefix, markers, typeExpression, dimensions, _initializer, type);
        }

        public Space Prefix => prefix;

        public NewArray WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NewArray(id, newPrefix, markers, typeExpression, dimensions, _initializer, type);
        }

        public Markers Markers => markers;

        public NewArray WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NewArray(id, prefix, newMarkers, typeExpression, dimensions, _initializer, type);
        }

        public TypeTree? TypeExpression => typeExpression;

        public NewArray WithTypeExpression(TypeTree? newTypeExpression)
        {
            return ReferenceEquals(newTypeExpression, typeExpression) ? this : new NewArray(id, prefix, markers, newTypeExpression, dimensions, _initializer, type);
        }

        public IList<J.ArrayDimension> Dimensions => dimensions;

        public NewArray WithDimensions(IList<J.ArrayDimension> newDimensions)
        {
            return newDimensions == dimensions ? this : new NewArray(id, prefix, markers, typeExpression, newDimensions, _initializer, type);
        }

        private readonly JContainer<Expression>? _initializer = initializer;
        public IList<Expression>? Initializer => _initializer?.GetElements();

        public NewArray WithInitializer(IList<Expression>? newInitializer)
        {
            return Padding.WithInitializer(JContainer<Expression>.WithElementsNullable(_initializer, newInitializer));
        }

        public JavaType? Type => type;

        public NewArray WithType(JavaType? newType)
        {
            return newType == type ? this : new NewArray(id, prefix, markers, typeExpression, dimensions, _initializer, newType);
        }

        public sealed record PaddingHelper(J.NewArray T)
        {
            public JContainer<Expression>? Initializer => T._initializer;

            public J.NewArray WithInitializer(JContainer<Expression>? newInitializer)
            {
                return T._initializer == newInitializer ? T : new J.NewArray(T.Id, T.Prefix, T.Markers, T.TypeExpression, T.Dimensions, newInitializer, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NewArray && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ArrayDimension(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> index
    ) : J, MutableTree<ArrayDimension>
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
            return v.VisitArrayDimension(this, p);
        }

        public Guid Id => id;

        public ArrayDimension WithId(Guid newId)
        {
            return newId == id ? this : new ArrayDimension(newId, prefix, markers, _index);
        }

        public Space Prefix => prefix;

        public ArrayDimension WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayDimension(id, newPrefix, markers, _index);
        }

        public Markers Markers => markers;

        public ArrayDimension WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayDimension(id, prefix, newMarkers, _index);
        }

        private readonly JRightPadded<Expression> _index = index;
        public Expression Index => _index.Element;

        public ArrayDimension WithIndex(Expression newIndex)
        {
            return Padding.WithIndex(_index.WithElement(newIndex));
        }

        public sealed record PaddingHelper(J.ArrayDimension T)
        {
            public JRightPadded<Expression> Index => T._index;

            public J.ArrayDimension WithIndex(JRightPadded<Expression> newIndex)
            {
                return T._index == newIndex ? T : new J.ArrayDimension(T.Id, T.Prefix, T.Markers, newIndex);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayDimension && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class NewClass(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression>? enclosing,
        Space @new,
        TypeTree? clazz,
        JContainer<Expression> arguments,
        Block? body,
        JavaType.Method? constructorType
    ) : J, Statement, Expression, TypedTree, MethodCall, MutableTree<NewClass>
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

        public JavaType? Type => Extensions.GetJavaType(this);

        public NewClass WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NewClass && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class NullableType(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        JRightPadded<TypeTree> typeTree
    ) : J, TypeTree, Expression, MutableTree<NullableType>
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
            return v.VisitNullableType(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public NullableType WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public NullableType WithId(Guid newId)
        {
            return newId == id ? this : new NullableType(newId, prefix, markers, annotations, _typeTree);
        }

        public Space Prefix => prefix;

        public NullableType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NullableType(id, newPrefix, markers, annotations, _typeTree);
        }

        public Markers Markers => markers;

        public NullableType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NullableType(id, prefix, newMarkers, annotations, _typeTree);
        }

        public IList<J.Annotation> Annotations => annotations;

        public NullableType WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new NullableType(id, prefix, markers, newAnnotations, _typeTree);
        }

        private readonly JRightPadded<TypeTree> _typeTree = typeTree;
        public TypeTree TypeTree => _typeTree.Element;

        public NullableType WithTypeTree(TypeTree newTypeTree)
        {
            return Padding.WithTypeTree(_typeTree.WithElement(newTypeTree));
        }

        public sealed record PaddingHelper(J.NullableType T)
        {
            public JRightPadded<TypeTree> TypeTree => T._typeTree;

            public J.NullableType WithTypeTree(JRightPadded<TypeTree> newTypeTree)
            {
                return T._typeTree == newTypeTree ? T : new J.NullableType(T.Id, T.Prefix, T.Markers, T.Annotations, newTypeTree);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NullableType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Package(
        Guid id,
        Space prefix,
        Markers markers,
        Expression expression,
        IList<Annotation> annotations
    ) : Statement, J, MutableTree<Package>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitPackage(this, p);
        }

        public Guid Id => id;

        public Package WithId(Guid newId)
        {
            return newId == id ? this : new Package(newId, prefix, markers, expression, annotations);
        }

        public Space Prefix => prefix;

        public Package WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Package(id, newPrefix, markers, expression, annotations);
        }

        public Markers Markers => markers;

        public Package WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Package(id, prefix, newMarkers, expression, annotations);
        }

        public Expression Expression => expression;

        public Package WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new Package(id, prefix, markers, newExpression, annotations);
        }

        public IList<J.Annotation> Annotations => annotations;

        public Package WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new Package(id, prefix, markers, expression, newAnnotations);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Package && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ParameterizedType(
        Guid id,
        Space prefix,
        Markers markers,
        NameTree clazz,
        JContainer<Expression>? typeParameters,
        JavaType? type
    ) : J, TypeTree, Expression, MutableTree<ParameterizedType>
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
            return v.VisitParameterizedType(this, p);
        }

        public Guid Id => id;

        public ParameterizedType WithId(Guid newId)
        {
            return newId == id ? this : new ParameterizedType(newId, prefix, markers, clazz, _typeParameters, type);
        }

        public Space Prefix => prefix;

        public ParameterizedType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ParameterizedType(id, newPrefix, markers, clazz, _typeParameters, type);
        }

        public Markers Markers => markers;

        public ParameterizedType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ParameterizedType(id, prefix, newMarkers, clazz, _typeParameters, type);
        }

        public NameTree Clazz => clazz;

        public ParameterizedType WithClazz(NameTree newClazz)
        {
            return ReferenceEquals(newClazz, clazz) ? this : new ParameterizedType(id, prefix, markers, newClazz, _typeParameters, type);
        }

        private readonly JContainer<Expression>? _typeParameters = typeParameters;
        public IList<Expression>? TypeParameters => _typeParameters?.GetElements();

        public ParameterizedType WithTypeParameters(IList<Expression>? newTypeParameters)
        {
            return Padding.WithTypeParameters(JContainer<Expression>.WithElementsNullable(_typeParameters, newTypeParameters));
        }

        public JavaType? Type => type;

        public ParameterizedType WithType(JavaType? newType)
        {
            return newType == type ? this : new ParameterizedType(id, prefix, markers, clazz, _typeParameters, newType);
        }

        public sealed record PaddingHelper(J.ParameterizedType T)
        {
            public JContainer<Expression>? TypeParameters => T._typeParameters;

            public J.ParameterizedType WithTypeParameters(JContainer<Expression>? newTypeParameters)
            {
                return T._typeParameters == newTypeParameters ? T : new J.ParameterizedType(T.Id, T.Prefix, T.Markers, T.Clazz, newTypeParameters, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ParameterizedType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Parentheses<J2>(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<J2> tree
    ) : J, Expression, MutableTree<J.Parentheses<J2>> where J2 : J
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
            return v.VisitParentheses(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public J.Parentheses<J2> WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public J.Parentheses<J2> WithId(Guid newId)
        {
            return newId == id ? this : new J.Parentheses<J2>(newId, prefix, markers, _tree);
        }

        public Space Prefix => prefix;

        public J.Parentheses<J2> WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new J.Parentheses<J2>(id, newPrefix, markers, _tree);
        }

        public Markers Markers => markers;

        public J.Parentheses<J2> WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new J.Parentheses<J2>(id, prefix, newMarkers, _tree);
        }

        private readonly JRightPadded<J2> _tree = tree;
        public J2 Tree => _tree.Element;

        public J.Parentheses<J2> WithTree(J2 newTree)
        {
            return Padding.WithTree(_tree.WithElement(newTree));
        }

        public sealed record PaddingHelper(J.Parentheses<J2> T)
        {
            public JRightPadded<J2> Tree => T._tree;

            public J.Parentheses<J2> WithTree(JRightPadded<J2> newTree)
            {
                return T._tree == newTree ? T : new J.Parentheses<J2>(T.Id, T.Prefix, T.Markers, newTree);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is J.Parentheses<J2> && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ControlParentheses<J2>(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<J2> tree
    ) : J, Expression, MutableTree<J.ControlParentheses<J2>> where J2 : J
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
            return v.VisitControlParentheses(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public J.ControlParentheses<J2> WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public J.ControlParentheses<J2> WithId(Guid newId)
        {
            return newId == id ? this : new J.ControlParentheses<J2>(newId, prefix, markers, _tree);
        }

        public Space Prefix => prefix;

        public J.ControlParentheses<J2> WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new J.ControlParentheses<J2>(id, newPrefix, markers, _tree);
        }

        public Markers Markers => markers;

        public J.ControlParentheses<J2> WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new J.ControlParentheses<J2>(id, prefix, newMarkers, _tree);
        }

        private readonly JRightPadded<J2> _tree = tree;
        public J2 Tree => _tree.Element;

        public J.ControlParentheses<J2> WithTree(J2 newTree)
        {
            return Padding.WithTree(_tree.WithElement(newTree));
        }

        public sealed record PaddingHelper(J.ControlParentheses<J2> T)
        {
            public JRightPadded<J2> Tree => T._tree;

            public J.ControlParentheses<J2> WithTree(JRightPadded<J2> newTree)
            {
                return T._tree == newTree ? T : new J.ControlParentheses<J2>(T.Id, T.Prefix, T.Markers, newTree);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is J.ControlParentheses<J2> && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Primitive(
        Guid id,
        Space prefix,
        Markers markers,
        JavaType.Primitive type
    ) : J, TypeTree, Expression, MutableTree<Primitive>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitPrimitive(this, p);
        }

        public Guid Id => id;

        public Primitive WithId(Guid newId)
        {
            return newId == id ? this : new Primitive(newId, prefix, markers, type);
        }

        public Space Prefix => prefix;

        public Primitive WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Primitive(id, newPrefix, markers, type);
        }

        public Markers Markers => markers;

        public Primitive WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Primitive(id, prefix, newMarkers, type);
        }

        public JavaType.Primitive Type => type;

        public Primitive WithType(JavaType.Primitive newType)
        {
            return newType == type ? this : new Primitive(id, prefix, markers, newType);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Primitive && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Return(
        Guid id,
        Space prefix,
        Markers markers,
        Expression? expression
    ) : J, Statement, MutableTree<Return>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitReturn(this, p);
        }

        public Guid Id => id;

        public Return WithId(Guid newId)
        {
            return newId == id ? this : new Return(newId, prefix, markers, expression);
        }

        public Space Prefix => prefix;

        public Return WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Return(id, newPrefix, markers, expression);
        }

        public Markers Markers => markers;

        public Return WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Return(id, prefix, newMarkers, expression);
        }

        public Expression? Expression => expression;

        public Return WithExpression(Expression? newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new Return(id, prefix, markers, newExpression);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Return && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Switch(
        Guid id,
        Space prefix,
        Markers markers,
        J.ControlParentheses<Expression> selector,
        Block cases
    ) : J, Statement, MutableTree<Switch>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitSwitch(this, p);
        }

        public Guid Id => id;

        public Switch WithId(Guid newId)
        {
            return newId == id ? this : new Switch(newId, prefix, markers, selector, cases);
        }

        public Space Prefix => prefix;

        public Switch WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Switch(id, newPrefix, markers, selector, cases);
        }

        public Markers Markers => markers;

        public Switch WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Switch(id, prefix, newMarkers, selector, cases);
        }

        public J.ControlParentheses<Expression> Selector => selector;

        public Switch WithSelector(J.ControlParentheses<Expression> newSelector)
        {
            return ReferenceEquals(newSelector, selector) ? this : new Switch(id, prefix, markers, newSelector, cases);
        }

        public J.Block Cases => cases;

        public Switch WithCases(J.Block newCases)
        {
            return ReferenceEquals(newCases, cases) ? this : new Switch(id, prefix, markers, selector, newCases);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Switch && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class SwitchExpression(
        Guid id,
        Space prefix,
        Markers markers,
        J.ControlParentheses<Expression> selector,
        Block cases
    ) : J, Expression, TypedTree, MutableTree<SwitchExpression>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitSwitchExpression(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public SwitchExpression WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public SwitchExpression WithId(Guid newId)
        {
            return newId == id ? this : new SwitchExpression(newId, prefix, markers, selector, cases);
        }

        public Space Prefix => prefix;

        public SwitchExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new SwitchExpression(id, newPrefix, markers, selector, cases);
        }

        public Markers Markers => markers;

        public SwitchExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new SwitchExpression(id, prefix, newMarkers, selector, cases);
        }

        public J.ControlParentheses<Expression> Selector => selector;

        public SwitchExpression WithSelector(J.ControlParentheses<Expression> newSelector)
        {
            return ReferenceEquals(newSelector, selector) ? this : new SwitchExpression(id, prefix, markers, newSelector, cases);
        }

        public J.Block Cases => cases;

        public SwitchExpression WithCases(J.Block newCases)
        {
            return ReferenceEquals(newCases, cases) ? this : new SwitchExpression(id, prefix, markers, selector, newCases);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchExpression && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Synchronized(
        Guid id,
        Space prefix,
        Markers markers,
        J.ControlParentheses<Expression> @lock,
        Block body
    ) : J, Statement, MutableTree<Synchronized>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitSynchronized(this, p);
        }

        public Guid Id => id;

        public Synchronized WithId(Guid newId)
        {
            return newId == id ? this : new Synchronized(newId, prefix, markers, @lock, body);
        }

        public Space Prefix => prefix;

        public Synchronized WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Synchronized(id, newPrefix, markers, @lock, body);
        }

        public Markers Markers => markers;

        public Synchronized WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Synchronized(id, prefix, newMarkers, @lock, body);
        }

        public J.ControlParentheses<Expression> Lock => @lock;

        public Synchronized WithLock(J.ControlParentheses<Expression> newLock)
        {
            return ReferenceEquals(newLock, @lock) ? this : new Synchronized(id, prefix, markers, newLock, body);
        }

        public J.Block Body => body;

        public Synchronized WithBody(J.Block newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new Synchronized(id, prefix, markers, @lock, newBody);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Synchronized && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Ternary(
        Guid id,
        Space prefix,
        Markers markers,
        Expression condition,
        JLeftPadded<Expression> truePart,
        JLeftPadded<Expression> falsePart,
        JavaType? type
    ) : J, Expression, Statement, TypedTree, MutableTree<Ternary>
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
            return v.VisitTernary(this, p);
        }

        public Guid Id => id;

        public Ternary WithId(Guid newId)
        {
            return newId == id ? this : new Ternary(newId, prefix, markers, condition, _truePart, _falsePart, type);
        }

        public Space Prefix => prefix;

        public Ternary WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Ternary(id, newPrefix, markers, condition, _truePart, _falsePart, type);
        }

        public Markers Markers => markers;

        public Ternary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Ternary(id, prefix, newMarkers, condition, _truePart, _falsePart, type);
        }

        public Expression Condition => condition;

        public Ternary WithCondition(Expression newCondition)
        {
            return ReferenceEquals(newCondition, condition) ? this : new Ternary(id, prefix, markers, newCondition, _truePart, _falsePart, type);
        }

        private readonly JLeftPadded<Expression> _truePart = truePart;
        public Expression TruePart => _truePart.Element;

        public Ternary WithTruePart(Expression newTruePart)
        {
            return Padding.WithTruePart(_truePart.WithElement(newTruePart));
        }

        private readonly JLeftPadded<Expression> _falsePart = falsePart;
        public Expression FalsePart => _falsePart.Element;

        public Ternary WithFalsePart(Expression newFalsePart)
        {
            return Padding.WithFalsePart(_falsePart.WithElement(newFalsePart));
        }

        public JavaType? Type => type;

        public Ternary WithType(JavaType? newType)
        {
            return newType == type ? this : new Ternary(id, prefix, markers, condition, _truePart, _falsePart, newType);
        }

        public sealed record PaddingHelper(J.Ternary T)
        {
            public JLeftPadded<Expression> TruePart => T._truePart;

            public J.Ternary WithTruePart(JLeftPadded<Expression> newTruePart)
            {
                return T._truePart == newTruePart ? T : new J.Ternary(T.Id, T.Prefix, T.Markers, T.Condition, newTruePart, T._falsePart, T.Type);
            }

            public JLeftPadded<Expression> FalsePart => T._falsePart;

            public J.Ternary WithFalsePart(JLeftPadded<Expression> newFalsePart)
            {
                return T._falsePart == newFalsePart ? T : new J.Ternary(T.Id, T.Prefix, T.Markers, T.Condition, T._truePart, newFalsePart, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Ternary && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Throw(
        Guid id,
        Space prefix,
        Markers markers,
        Expression exception
    ) : J, Statement, MutableTree<Throw>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitThrow(this, p);
        }

        public Guid Id => id;

        public Throw WithId(Guid newId)
        {
            return newId == id ? this : new Throw(newId, prefix, markers, exception);
        }

        public Space Prefix => prefix;

        public Throw WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Throw(id, newPrefix, markers, exception);
        }

        public Markers Markers => markers;

        public Throw WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Throw(id, prefix, newMarkers, exception);
        }

        public Expression Exception => exception;

        public Throw WithException(Expression newException)
        {
            return ReferenceEquals(newException, exception) ? this : new Throw(id, prefix, markers, newException);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Throw && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Try(
        Guid id,
        Space prefix,
        Markers markers,
        JContainer<Try.Resource>? resources,
        Block body,
        IList<Try.Catch> catches,
        JLeftPadded<Block>? @finally
    ) : J, Statement, MutableTree<Try>
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
            return v.VisitTry(this, p);
        }

        public Guid Id => id;

        public Try WithId(Guid newId)
        {
            return newId == id ? this : new Try(newId, prefix, markers, _resources, body, catches, _finally);
        }

        public Space Prefix => prefix;

        public Try WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Try(id, newPrefix, markers, _resources, body, catches, _finally);
        }

        public Markers Markers => markers;

        public Try WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Try(id, prefix, newMarkers, _resources, body, catches, _finally);
        }

        private readonly JContainer<Resource>? _resources = resources;
        public IList<Resource>? Resources => _resources?.GetElements();

        public Try WithResources(IList<Resource>? newResources)
        {
            return Padding.WithResources(JContainer<Resource>.WithElementsNullable(_resources, newResources));
        }

        public J.Block Body => body;

        public Try WithBody(J.Block newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new Try(id, prefix, markers, _resources, newBody, catches, _finally);
        }

        public IList<Catch> Catches => catches;

        public Try WithCatches(IList<Catch> newCatches)
        {
            return newCatches == catches ? this : new Try(id, prefix, markers, _resources, body, newCatches, _finally);
        }

        private readonly JLeftPadded<J.Block>? _finally = @finally;
        public J.Block? Finally => _finally?.Element;

        public Try WithFinally(J.Block? newFinally)
        {
            return Padding.WithFinally(JLeftPadded<J.Block>.WithElement(_finally, newFinally));
        }

        public class Resource(
            Guid id,
            Space prefix,
            Markers markers,
            TypedTree variableDeclarations,
            bool terminatedWithSemicolon
        ) : J, MutableTree<Resource>
        {
            public J? AcceptJava<P>(JavaVisitor<P> v, P p)
            {
                return v.VisitTryResource(this, p);
            }

            public Guid Id => id;

            public Resource WithId(Guid newId)
            {
                return newId == id ? this : new Resource(newId, prefix, markers, variableDeclarations, terminatedWithSemicolon);
            }

            public Space Prefix => prefix;

            public Resource WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Resource(id, newPrefix, markers, variableDeclarations, terminatedWithSemicolon);
            }

            public Markers Markers => markers;

            public Resource WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Resource(id, prefix, newMarkers, variableDeclarations, terminatedWithSemicolon);
            }

            public TypedTree VariableDeclarations => variableDeclarations;

            public Resource WithVariableDeclarations(TypedTree newVariableDeclarations)
            {
                return ReferenceEquals(newVariableDeclarations, variableDeclarations) ? this : new Resource(id, prefix, markers, newVariableDeclarations, terminatedWithSemicolon);
            }

            public bool TerminatedWithSemicolon => terminatedWithSemicolon;

            public Resource WithTerminatedWithSemicolon(bool newTerminatedWithSemicolon)
            {
                return newTerminatedWithSemicolon == terminatedWithSemicolon ? this : new Resource(id, prefix, markers, variableDeclarations, newTerminatedWithSemicolon);
            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Resource && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public class Catch(
            Guid id,
            Space prefix,
            Markers markers,
            J.ControlParentheses<J.VariableDeclarations> parameter,
            J.Block body
        ) : J, MutableTree<Catch>
        {
            public J? AcceptJava<P>(JavaVisitor<P> v, P p)
            {
                return v.VisitCatch(this, p);
            }

            public Guid Id => id;

            public Catch WithId(Guid newId)
            {
                return newId == id ? this : new Catch(newId, prefix, markers, parameter, body);
            }

            public Space Prefix => prefix;

            public Catch WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Catch(id, newPrefix, markers, parameter, body);
            }

            public Markers Markers => markers;

            public Catch WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Catch(id, prefix, newMarkers, parameter, body);
            }

            public J.ControlParentheses<J.VariableDeclarations> Parameter => parameter;

            public Catch WithParameter(J.ControlParentheses<J.VariableDeclarations> newParameter)
            {
                return ReferenceEquals(newParameter, parameter) ? this : new Catch(id, prefix, markers, newParameter, body);
            }

            public J.Block Body => body;

            public Catch WithBody(J.Block newBody)
            {
                return ReferenceEquals(newBody, body) ? this : new Catch(id, prefix, markers, parameter, newBody);
            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Catch && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public sealed record PaddingHelper(J.Try T)
        {
            public JContainer<J.Try.Resource>? Resources => T._resources;

            public J.Try WithResources(JContainer<J.Try.Resource>? newResources)
            {
                return T._resources == newResources ? T : new J.Try(T.Id, T.Prefix, T.Markers, newResources, T.Body, T.Catches, T._finally);
            }

            public JLeftPadded<J.Block>? Finally => T._finally;

            public J.Try WithFinally(JLeftPadded<J.Block>? newFinally)
            {
                return T._finally == newFinally ? T : new J.Try(T.Id, T.Prefix, T.Markers, T._resources, T.Body, T.Catches, newFinally);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Try && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TypeCast(
        Guid id,
        Space prefix,
        Markers markers,
        J.ControlParentheses<TypeTree> clazz,
        Expression expression
    ) : J, Expression, TypedTree, MutableTree<TypeCast>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitTypeCast(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public TypeCast WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public TypeCast WithId(Guid newId)
        {
            return newId == id ? this : new TypeCast(newId, prefix, markers, clazz, expression);
        }

        public Space Prefix => prefix;

        public TypeCast WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TypeCast(id, newPrefix, markers, clazz, expression);
        }

        public Markers Markers => markers;

        public TypeCast WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TypeCast(id, prefix, newMarkers, clazz, expression);
        }

        public J.ControlParentheses<TypeTree> Clazz => clazz;

        public TypeCast WithClazz(J.ControlParentheses<TypeTree> newClazz)
        {
            return ReferenceEquals(newClazz, clazz) ? this : new TypeCast(id, prefix, markers, newClazz, expression);
        }

        public Expression Expression => expression;

        public TypeCast WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new TypeCast(id, prefix, markers, clazz, newExpression);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeCast && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TypeParameter(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        IList<Modifier> modifiers,
        Expression name,
        JContainer<TypeTree>? bounds
    ) : J, MutableTree<TypeParameter>
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
            return v.VisitTypeParameter(this, p);
        }

        public Guid Id => id;

        public TypeParameter WithId(Guid newId)
        {
            return newId == id ? this : new TypeParameter(newId, prefix, markers, annotations, modifiers, name, _bounds);
        }

        public Space Prefix => prefix;

        public TypeParameter WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TypeParameter(id, newPrefix, markers, annotations, modifiers, name, _bounds);
        }

        public Markers Markers => markers;

        public TypeParameter WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TypeParameter(id, prefix, newMarkers, annotations, modifiers, name, _bounds);
        }

        public IList<J.Annotation> Annotations => annotations;

        public TypeParameter WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new TypeParameter(id, prefix, markers, newAnnotations, modifiers, name, _bounds);
        }

        public IList<J.Modifier> Modifiers => modifiers;

        public TypeParameter WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new TypeParameter(id, prefix, markers, annotations, newModifiers, name, _bounds);
        }

        public Expression Name => name;

        public TypeParameter WithName(Expression newName)
        {
            return ReferenceEquals(newName, name) ? this : new TypeParameter(id, prefix, markers, annotations, modifiers, newName, _bounds);
        }

        private readonly JContainer<TypeTree>? _bounds = bounds;
        public IList<TypeTree>? Bounds => _bounds?.GetElements();

        public TypeParameter WithBounds(IList<TypeTree>? newBounds)
        {
            return Padding.WithBounds(JContainer<TypeTree>.WithElementsNullable(_bounds, newBounds));
        }

        public sealed record PaddingHelper(J.TypeParameter T)
        {
            public JContainer<TypeTree>? Bounds => T._bounds;

            public J.TypeParameter WithBounds(JContainer<TypeTree>? newBounds)
            {
                return T._bounds == newBounds ? T : new J.TypeParameter(T.Id, T.Prefix, T.Markers, T.Annotations, T.Modifiers, T.Name, newBounds);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeParameter && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TypeParameters(
        Guid id,
        Space prefix,
        Markers markers,
        IList<Annotation> annotations,
        IList<JRightPadded<TypeParameter>> parameters
    ) : J, MutableTree<TypeParameters>
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
            return v.VisitTypeParameters(this, p);
        }

        public Guid Id => id;

        public TypeParameters WithId(Guid newId)
        {
            return newId == id ? this : new TypeParameters(newId, prefix, markers, annotations, _parameters);
        }

        public Space Prefix => prefix;

        public TypeParameters WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TypeParameters(id, newPrefix, markers, annotations, _parameters);
        }

        public Markers Markers => markers;

        public TypeParameters WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TypeParameters(id, prefix, newMarkers, annotations, _parameters);
        }

        public IList<J.Annotation> Annotations => annotations;

        public TypeParameters WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new TypeParameters(id, prefix, markers, newAnnotations, _parameters);
        }

        private readonly IList<JRightPadded<J.TypeParameter>> _parameters = parameters;
        public IList<J.TypeParameter> Parameters => _parameters.Elements();

        public TypeParameters WithParameters(IList<J.TypeParameter> newParameters)
        {
            return Padding.WithParameters(_parameters.WithElements(newParameters));
        }

        public sealed record PaddingHelper(J.TypeParameters T)
        {
            public IList<JRightPadded<J.TypeParameter>> Parameters => T._parameters;

            public J.TypeParameters WithParameters(IList<JRightPadded<J.TypeParameter>> newParameters)
            {
                return T._parameters == newParameters ? T : new J.TypeParameters(T.Id, T.Prefix, T.Markers, T.Annotations, newParameters);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeParameters && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Unary(
        Guid id,
        Space prefix,
        Markers markers,
        JLeftPadded<Unary.Type> @operator,
        Expression expression,
        JavaType? javaType
    ) : J, Statement, Expression, TypedTree, MutableTree<Unary>
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
            return v.VisitUnary(this, p);
        }

        public Guid Id => id;

        public Unary WithId(Guid newId)
        {
            return newId == id ? this : new Unary(newId, prefix, markers, _operator, expression, javaType);
        }

        public Space Prefix => prefix;

        public Unary WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Unary(id, newPrefix, markers, _operator, expression, javaType);
        }

        public Markers Markers => markers;

        public Unary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Unary(id, prefix, newMarkers, _operator, expression, javaType);
        }

        private readonly JLeftPadded<Type> _operator = @operator;
        public Type Operator => _operator.Element;

        public Unary WithOperator(Type newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }

        public Expression Expression => expression;

        public Unary WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new Unary(id, prefix, markers, _operator, newExpression, javaType);
        }

        public JavaType? JavaType => javaType;

        public Unary WithJavaType(JavaType? newJavaType)
        {
            return newJavaType == javaType ? this : new Unary(id, prefix, markers, _operator, expression, newJavaType);
        }

        public enum Type
        {
            PreIncrement,
            PreDecrement,
            PostIncrement,
            PostDecrement,
            Positive,
            Negative,
            Complement,
            Not,

        }

        public sealed record PaddingHelper(J.Unary T)
        {
            public JLeftPadded<J.Unary.Type> Operator => T._operator;

            public J.Unary WithOperator(JLeftPadded<J.Unary.Type> newOperator)
            {
                return T._operator == newOperator ? T : new J.Unary(T.Id, T.Prefix, T.Markers, newOperator, T.Expression, T.JavaType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Unary && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class VariableDeclarations(
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

        public class NamedVariable(
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

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is NamedVariable && other.Id == Id;
            }

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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is VariableDeclarations && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class WhileLoop(
        Guid id,
        Space prefix,
        Markers markers,
        J.ControlParentheses<Expression> condition,
        JRightPadded<Statement> body
    ) : J, Loop, MutableTree<WhileLoop>
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
            return v.VisitWhileLoop(this, p);
        }

        public Guid Id => id;

        public WhileLoop WithId(Guid newId)
        {
            return newId == id ? this : new WhileLoop(newId, prefix, markers, condition, _body);
        }

        public Space Prefix => prefix;

        public WhileLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new WhileLoop(id, newPrefix, markers, condition, _body);
        }

        public Markers Markers => markers;

        public WhileLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new WhileLoop(id, prefix, newMarkers, condition, _body);
        }

        public J.ControlParentheses<Expression> Condition => condition;

        public WhileLoop WithCondition(J.ControlParentheses<Expression> newCondition)
        {
            return ReferenceEquals(newCondition, condition) ? this : new WhileLoop(id, prefix, markers, newCondition, _body);
        }

        private readonly JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public WhileLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }

        public sealed record PaddingHelper(J.WhileLoop T)
        {
            public JRightPadded<Statement> Body => T._body;

            public J.WhileLoop WithBody(JRightPadded<Statement> newBody)
            {
                return T._body == newBody ? T : new J.WhileLoop(T.Id, T.Prefix, T.Markers, T.Condition, newBody);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is WhileLoop && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Wildcard(
        Guid id,
        Space prefix,
        Markers markers,
        JLeftPadded<Wildcard.Bound>? wildcardBound,
        NameTree? boundedType
    ) : J, Expression, TypeTree, MutableTree<Wildcard>
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
            return v.VisitWildcard(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public Wildcard WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public Wildcard WithId(Guid newId)
        {
            return newId == id ? this : new Wildcard(newId, prefix, markers, _wildcardBound, boundedType);
        }

        public Space Prefix => prefix;

        public Wildcard WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Wildcard(id, newPrefix, markers, _wildcardBound, boundedType);
        }

        public Markers Markers => markers;

        public Wildcard WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Wildcard(id, prefix, newMarkers, _wildcardBound, boundedType);
        }

        private readonly JLeftPadded<Bound>? _wildcardBound = wildcardBound;
        public Bound? WildcardBound => _wildcardBound?.Element;

        public Wildcard WithWildcardBound(Bound newWildcardBound)
        {
            return Padding.WithWildcardBound(JLeftPadded<Bound>.WithElement(_wildcardBound, newWildcardBound));
        }

        public NameTree? BoundedType => boundedType;

        public Wildcard WithBoundedType(NameTree? newBoundedType)
        {
            return ReferenceEquals(newBoundedType, boundedType) ? this : new Wildcard(id, prefix, markers, _wildcardBound, newBoundedType);
        }

        public enum Bound
        {
            Extends,
            Super,

        }

        public sealed record PaddingHelper(J.Wildcard T)
        {
            public JLeftPadded<J.Wildcard.Bound>? WildcardBound => T._wildcardBound;

            public J.Wildcard WithWildcardBound(JLeftPadded<J.Wildcard.Bound>? newWildcardBound)
            {
                return T._wildcardBound == newWildcardBound ? T : new J.Wildcard(T.Id, T.Prefix, T.Markers, newWildcardBound, T.BoundedType);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Wildcard && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class Yield(
        Guid id,
        Space prefix,
        Markers markers,
        bool @implicit,
        Expression value
    ) : J, Statement, MutableTree<Yield>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitYield(this, p);
        }

        public Guid Id => id;

        public Yield WithId(Guid newId)
        {
            return newId == id ? this : new Yield(newId, prefix, markers, @implicit, value);
        }

        public Space Prefix => prefix;

        public Yield WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Yield(id, newPrefix, markers, @implicit, value);
        }

        public Markers Markers => markers;

        public Yield WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Yield(id, prefix, newMarkers, @implicit, value);
        }

        public bool Implicit => @implicit;

        public Yield WithImplicit(bool newImplicit)
        {
            return newImplicit == @implicit ? this : new Yield(id, prefix, markers, newImplicit, value);
        }

        public Expression Value => value;

        public Yield WithValue(Expression newValue)
        {
            return ReferenceEquals(newValue, value) ? this : new Yield(id, prefix, markers, @implicit, newValue);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Yield && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Unknown(
        Guid id,
        Space prefix,
        Markers markers,
        Unknown.Source unknownSource
    ) : J, Statement, Expression, TypeTree, MutableTree<Unknown>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitUnknown(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public Unknown WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public Unknown WithId(Guid newId)
        {
            return newId == id ? this : new Unknown(newId, prefix, markers, unknownSource);
        }

        public Space Prefix => prefix;

        public Unknown WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Unknown(id, newPrefix, markers, unknownSource);
        }

        public Markers Markers => markers;

        public Unknown WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Unknown(id, prefix, newMarkers, unknownSource);
        }

        public Source UnknownSource => unknownSource;

        public Unknown WithUnknownSource(Source newUnknownSource)
        {
            return ReferenceEquals(newUnknownSource, unknownSource) ? this : new Unknown(id, prefix, markers, newUnknownSource);
        }

        public class Source(
            Guid id,
            Space prefix,
            Markers markers,
            string text
        ) : J, MutableTree<Source>
        {
            public J? AcceptJava<P>(JavaVisitor<P> v, P p)
            {
                return v.VisitUnknownSource(this, p);
            }

            public Guid Id => id;

            public Source WithId(Guid newId)
            {
                return newId == id ? this : new Source(newId, prefix, markers, text);
            }

            public Space Prefix => prefix;

            public Source WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Source(id, newPrefix, markers, text);
            }

            public Markers Markers => markers;

            public Source WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Source(id, prefix, newMarkers, text);
            }

            public string Text => text;

            public Source WithText(string newText)
            {
                return newText == text ? this : new Source(id, prefix, markers, newText);
            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Source && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Unknown && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
