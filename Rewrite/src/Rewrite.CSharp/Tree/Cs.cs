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
public interface Cs : J
{
    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(CSharpVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptCSharp(v.Adapt<J, CSharpVisitor<P>>(), p);
    }

    J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
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
        IList<JRightPadded<ExternAlias>> externs,
        IList<JRightPadded<UsingDirective>> usings,
        IList<AttributeList> attributeLists,
        IList<JRightPadded<Statement>> members,
        Space eof
    ) : Cs, MutableSourceFile<CompilationUnit>, MutableTree<CompilationUnit>
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
            return v.VisitCompilationUnit(this, p);
        }

        public Guid Id => id;

        public CompilationUnit WithId(Guid newId)
        {
            return newId == id ? this : new CompilationUnit(newId, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public Space Prefix => prefix;

        public CompilationUnit WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new CompilationUnit(id, newPrefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public Markers Markers => markers;

        public CompilationUnit WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new CompilationUnit(id, prefix, newMarkers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public string SourcePath => sourcePath;

        public CompilationUnit WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new CompilationUnit(id, prefix, markers, newSourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public FileAttributes? FileAttributes => fileAttributes;

        public CompilationUnit WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new CompilationUnit(id, prefix, markers, sourcePath, newFileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public string? CharsetName => charsetName;

        public CompilationUnit WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, newCharsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public bool CharsetBomMarked => charsetBomMarked;

        public CompilationUnit WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, newCharsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }

        public Checksum? Checksum => checksum;

        public CompilationUnit WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, newChecksum, _externs, _usings, attributeLists, _members, eof);
        }

        private readonly IList<JRightPadded<Cs.ExternAlias>> _externs = externs;
        public IList<Cs.ExternAlias> Externs => _externs.Elements();

        public CompilationUnit WithExterns(IList<Cs.ExternAlias> newExterns)
        {
            return Padding.WithExterns(_externs.WithElements(newExterns));
        }

        private readonly IList<JRightPadded<Cs.UsingDirective>> _usings = usings;
        public IList<Cs.UsingDirective> Usings => _usings.Elements();

        public CompilationUnit WithUsings(IList<Cs.UsingDirective> newUsings)
        {
            return Padding.WithUsings(_usings.WithElements(newUsings));
        }

        public IList<Cs.AttributeList> AttributeLists => attributeLists;

        public CompilationUnit WithAttributeLists(IList<Cs.AttributeList> newAttributeLists)
        {
            return newAttributeLists == attributeLists ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, newAttributeLists, _members, eof);
        }

        private readonly IList<JRightPadded<Statement>> _members = members;
        public IList<Statement> Members => _members.Elements();

        public CompilationUnit WithMembers(IList<Statement> newMembers)
        {
            return Padding.WithMembers(_members.WithElements(newMembers));
        }

        public Space Eof => eof;

        public CompilationUnit WithEof(Space newEof)
        {
            return newEof == eof ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, newEof);
        }

        public sealed record PaddingHelper(Cs.CompilationUnit T)
        {
            public IList<JRightPadded<Cs.ExternAlias>> Externs => T._externs;

            public Cs.CompilationUnit WithExterns(IList<JRightPadded<Cs.ExternAlias>> newExterns)
            {
                return T._externs == newExterns ? T : new Cs.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, newExterns, T._usings, T.AttributeLists, T._members, T.Eof);
            }

            public IList<JRightPadded<Cs.UsingDirective>> Usings => T._usings;

            public Cs.CompilationUnit WithUsings(IList<JRightPadded<Cs.UsingDirective>> newUsings)
            {
                return T._usings == newUsings ? T : new Cs.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, T._externs, newUsings, T.AttributeLists, T._members, T.Eof);
            }

            public IList<JRightPadded<Statement>> Members => T._members;

            public Cs.CompilationUnit WithMembers(IList<JRightPadded<Statement>> newMembers)
            {
                return T._members == newMembers ? T : new Cs.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, T._externs, T._usings, T.AttributeLists, newMembers, T.Eof);
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

    public class AnnotatedStatement(
        Guid id,
        Space prefix,
        Markers markers,
        IList<AttributeList> attributeLists,
        Statement statement
    ) : Cs, Statement, MutableTree<AnnotatedStatement>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitAnnotatedStatement(this, p);
        }

        public Guid Id => id;

        public AnnotatedStatement WithId(Guid newId)
        {
            return newId == id ? this : new AnnotatedStatement(newId, prefix, markers, attributeLists, statement);
        }

        public Space Prefix => prefix;

        public AnnotatedStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AnnotatedStatement(id, newPrefix, markers, attributeLists, statement);
        }

        public Markers Markers => markers;

        public AnnotatedStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AnnotatedStatement(id, prefix, newMarkers, attributeLists, statement);
        }

        public IList<Cs.AttributeList> AttributeLists => attributeLists;

        public AnnotatedStatement WithAttributeLists(IList<Cs.AttributeList> newAttributeLists)
        {
            return newAttributeLists == attributeLists ? this : new AnnotatedStatement(id, prefix, markers, newAttributeLists, statement);
        }

        public Statement Statement => statement;

        public AnnotatedStatement WithStatement(Statement newStatement)
        {
            return ReferenceEquals(newStatement, statement) ? this : new AnnotatedStatement(id, prefix, markers, attributeLists, newStatement);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AnnotatedStatement && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ArrayRankSpecifier(
        Guid id,
        Space prefix,
        Markers markers,
        JContainer<Expression> sizes
    ) : Cs, Expression, MutableTree<ArrayRankSpecifier>
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
            return v.VisitArrayRankSpecifier(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public ArrayRankSpecifier WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public ArrayRankSpecifier WithId(Guid newId)
        {
            return newId == id ? this : new ArrayRankSpecifier(newId, prefix, markers, _sizes);
        }

        public Space Prefix => prefix;

        public ArrayRankSpecifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayRankSpecifier(id, newPrefix, markers, _sizes);
        }

        public Markers Markers => markers;

        public ArrayRankSpecifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayRankSpecifier(id, prefix, newMarkers, _sizes);
        }

        private readonly JContainer<Expression> _sizes = sizes;
        public IList<Expression> Sizes => _sizes.GetElements();

        public ArrayRankSpecifier WithSizes(IList<Expression> newSizes)
        {
            return Padding.WithSizes(JContainer<Expression>.WithElements(_sizes, newSizes));
        }

        public sealed record PaddingHelper(Cs.ArrayRankSpecifier T)
        {
            public JContainer<Expression> Sizes => T._sizes;

            public Cs.ArrayRankSpecifier WithSizes(JContainer<Expression> newSizes)
            {
                return T._sizes == newSizes ? T : new Cs.ArrayRankSpecifier(T.Id, T.Prefix, T.Markers, newSizes);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayRankSpecifier && other.Id == Id;
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
        JLeftPadded<AssignmentOperation.OperatorType> @operator,
        Expression assignment,
        JavaType? type
    ) : Cs, Statement, Expression, TypedTree, MutableTree<AssignmentOperation>
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
            return v.VisitAssignmentOperation(this, p);
        }

        public Guid Id => id;

        public AssignmentOperation WithId(Guid newId)
        {
            return newId == id ? this : new AssignmentOperation(newId, prefix, markers, variable, _operator, assignment, type);
        }

        public Space Prefix => prefix;

        public AssignmentOperation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AssignmentOperation(id, newPrefix, markers, variable, _operator, assignment, type);
        }

        public Markers Markers => markers;

        public AssignmentOperation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AssignmentOperation(id, prefix, newMarkers, variable, _operator, assignment, type);
        }

        public Expression Variable => variable;

        public AssignmentOperation WithVariable(Expression newVariable)
        {
            return ReferenceEquals(newVariable, variable) ? this : new AssignmentOperation(id, prefix, markers, newVariable, _operator, assignment, type);
        }

        private readonly JLeftPadded<OperatorType> _operator = @operator;
        public OperatorType Operator => _operator.Element;

        public AssignmentOperation WithOperator(OperatorType newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }

        public Expression Assignment => assignment;

        public AssignmentOperation WithAssignment(Expression newAssignment)
        {
            return ReferenceEquals(newAssignment, assignment) ? this : new AssignmentOperation(id, prefix, markers, variable, _operator, newAssignment, type);
        }

        public JavaType? Type => type;

        public AssignmentOperation WithType(JavaType? newType)
        {
            return newType == type ? this : new AssignmentOperation(id, prefix, markers, variable, _operator, assignment, newType);
        }

        public enum OperatorType
        {
            NullCoalescing,

        }

        public sealed record PaddingHelper(Cs.AssignmentOperation T)
        {
            public JLeftPadded<Cs.AssignmentOperation.OperatorType> Operator => T._operator;

            public Cs.AssignmentOperation WithOperator(JLeftPadded<Cs.AssignmentOperation.OperatorType> newOperator)
            {
                return T._operator == newOperator ? T : new Cs.AssignmentOperation(T.Id, T.Prefix, T.Markers, T.Variable, newOperator, T.Assignment, T.Type);
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

    public class AttributeList(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<J.Identifier>? target,
        IList<JRightPadded<J.Annotation>> attributes
    ) : Cs, MutableTree<AttributeList>
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
            return v.VisitAttributeList(this, p);
        }

        public Guid Id => id;

        public AttributeList WithId(Guid newId)
        {
            return newId == id ? this : new AttributeList(newId, prefix, markers, _target, _attributes);
        }

        public Space Prefix => prefix;

        public AttributeList WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AttributeList(id, newPrefix, markers, _target, _attributes);
        }

        public Markers Markers => markers;

        public AttributeList WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AttributeList(id, prefix, newMarkers, _target, _attributes);
        }

        private readonly JRightPadded<J.Identifier>? _target = target;
        public J.Identifier? Target => _target?.Element;

        public AttributeList WithTarget(J.Identifier? newTarget)
        {
            return Padding.WithTarget(JRightPadded<J.Identifier>.WithElement(_target, newTarget));
        }

        private readonly IList<JRightPadded<J.Annotation>> _attributes = attributes;
        public IList<J.Annotation> Attributes => _attributes.Elements();

        public AttributeList WithAttributes(IList<J.Annotation> newAttributes)
        {
            return Padding.WithAttributes(_attributes.WithElements(newAttributes));
        }

        public sealed record PaddingHelper(Cs.AttributeList T)
        {
            public JRightPadded<J.Identifier>? Target => T._target;

            public Cs.AttributeList WithTarget(JRightPadded<J.Identifier>? newTarget)
            {
                return T._target == newTarget ? T : new Cs.AttributeList(T.Id, T.Prefix, T.Markers, newTarget, T._attributes);
            }

            public IList<JRightPadded<J.Annotation>> Attributes => T._attributes;

            public Cs.AttributeList WithAttributes(IList<JRightPadded<J.Annotation>> newAttributes)
            {
                return T._attributes == newAttributes ? T : new Cs.AttributeList(T.Id, T.Prefix, T.Markers, T._target, newAttributes);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AttributeList && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class AwaitExpression(
        Guid id,
        Space prefix,
        Markers markers,
        Expression expression,
        JavaType? type
    ) : Cs, Expression, MutableTree<AwaitExpression>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitAwaitExpression(this, p);
        }

        public Guid Id => id;

        public AwaitExpression WithId(Guid newId)
        {
            return newId == id ? this : new AwaitExpression(newId, prefix, markers, expression, type);
        }

        public Space Prefix => prefix;

        public AwaitExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AwaitExpression(id, newPrefix, markers, expression, type);
        }

        public Markers Markers => markers;

        public AwaitExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AwaitExpression(id, prefix, newMarkers, expression, type);
        }

        public Expression Expression => expression;

        public AwaitExpression WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new AwaitExpression(id, prefix, markers, newExpression, type);
        }

        public JavaType? Type => type;

        public AwaitExpression WithType(JavaType? newType)
        {
            return newType == type ? this : new AwaitExpression(id, prefix, markers, expression, newType);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AwaitExpression && other.Id == Id;
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
        JLeftPadded<Binary.OperatorType> @operator,
        Expression right,
        JavaType? type
    ) : Cs, Expression, TypedTree, MutableTree<Binary>
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
            return v.VisitBinary(this, p);
        }

        public Guid Id => id;

        public Binary WithId(Guid newId)
        {
            return newId == id ? this : new Binary(newId, prefix, markers, left, _operator, right, type);
        }

        public Space Prefix => prefix;

        public Binary WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Binary(id, newPrefix, markers, left, _operator, right, type);
        }

        public Markers Markers => markers;

        public Binary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Binary(id, prefix, newMarkers, left, _operator, right, type);
        }

        public Expression Left => left;

        public Binary WithLeft(Expression newLeft)
        {
            return ReferenceEquals(newLeft, left) ? this : new Binary(id, prefix, markers, newLeft, _operator, right, type);
        }

        private readonly JLeftPadded<OperatorType> _operator = @operator;
        public OperatorType Operator => _operator.Element;

        public Binary WithOperator(OperatorType newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }

        public Expression Right => right;

        public Binary WithRight(Expression newRight)
        {
            return ReferenceEquals(newRight, right) ? this : new Binary(id, prefix, markers, left, _operator, newRight, type);
        }

        public JavaType? Type => type;

        public Binary WithType(JavaType? newType)
        {
            return newType == type ? this : new Binary(id, prefix, markers, left, _operator, right, newType);
        }

        public enum OperatorType
        {
            As,
            NullCoalescing,

        }

        public sealed record PaddingHelper(Cs.Binary T)
        {
            public JLeftPadded<Cs.Binary.OperatorType> Operator => T._operator;

            public Cs.Binary WithOperator(JLeftPadded<Cs.Binary.OperatorType> newOperator)
            {
                return T._operator == newOperator ? T : new Cs.Binary(T.Id, T.Prefix, T.Markers, T.Left, newOperator, T.Right, T.Type);
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

    public class BlockScopeNamespaceDeclaration(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> name,
        IList<JRightPadded<ExternAlias>> externs,
        IList<JRightPadded<UsingDirective>> usings,
        IList<JRightPadded<Statement>> members,
        Space end
    ) : Cs, Statement, MutableTree<BlockScopeNamespaceDeclaration>
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
            return v.VisitBlockScopeNamespaceDeclaration(this, p);
        }

        public Guid Id => id;

        public BlockScopeNamespaceDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new BlockScopeNamespaceDeclaration(newId, prefix, markers, _name, _externs, _usings, _members, end);
        }

        public Space Prefix => prefix;

        public BlockScopeNamespaceDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new BlockScopeNamespaceDeclaration(id, newPrefix, markers, _name, _externs, _usings, _members, end);
        }

        public Markers Markers => markers;

        public BlockScopeNamespaceDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new BlockScopeNamespaceDeclaration(id, prefix, newMarkers, _name, _externs, _usings, _members, end);
        }

        private readonly JRightPadded<Expression> _name = name;
        public Expression Name => _name.Element;

        public BlockScopeNamespaceDeclaration WithName(Expression newName)
        {
            return Padding.WithName(_name.WithElement(newName));
        }

        private readonly IList<JRightPadded<Cs.ExternAlias>> _externs = externs;
        public IList<Cs.ExternAlias> Externs => _externs.Elements();

        public BlockScopeNamespaceDeclaration WithExterns(IList<Cs.ExternAlias> newExterns)
        {
            return Padding.WithExterns(_externs.WithElements(newExterns));
        }

        private readonly IList<JRightPadded<Cs.UsingDirective>> _usings = usings;
        public IList<Cs.UsingDirective> Usings => _usings.Elements();

        public BlockScopeNamespaceDeclaration WithUsings(IList<Cs.UsingDirective> newUsings)
        {
            return Padding.WithUsings(_usings.WithElements(newUsings));
        }

        private readonly IList<JRightPadded<Statement>> _members = members;
        public IList<Statement> Members => _members.Elements();

        public BlockScopeNamespaceDeclaration WithMembers(IList<Statement> newMembers)
        {
            return Padding.WithMembers(_members.WithElements(newMembers));
        }

        public Space End => end;

        public BlockScopeNamespaceDeclaration WithEnd(Space newEnd)
        {
            return newEnd == end ? this : new BlockScopeNamespaceDeclaration(id, prefix, markers, _name, _externs, _usings, _members, newEnd);
        }

        public sealed record PaddingHelper(Cs.BlockScopeNamespaceDeclaration T)
        {
            public JRightPadded<Expression> Name => T._name;

            public Cs.BlockScopeNamespaceDeclaration WithName(JRightPadded<Expression> newName)
            {
                return T._name == newName ? T : new Cs.BlockScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, newName, T._externs, T._usings, T._members, T.End);
            }

            public IList<JRightPadded<Cs.ExternAlias>> Externs => T._externs;

            public Cs.BlockScopeNamespaceDeclaration WithExterns(IList<JRightPadded<Cs.ExternAlias>> newExterns)
            {
                return T._externs == newExterns ? T : new Cs.BlockScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, T._name, newExterns, T._usings, T._members, T.End);
            }

            public IList<JRightPadded<Cs.UsingDirective>> Usings => T._usings;

            public Cs.BlockScopeNamespaceDeclaration WithUsings(IList<JRightPadded<Cs.UsingDirective>> newUsings)
            {
                return T._usings == newUsings ? T : new Cs.BlockScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, T._name, T._externs, newUsings, T._members, T.End);
            }

            public IList<JRightPadded<Statement>> Members => T._members;

            public Cs.BlockScopeNamespaceDeclaration WithMembers(IList<JRightPadded<Statement>> newMembers)
            {
                return T._members == newMembers ? T : new Cs.BlockScopeNamespaceDeclaration(T.Id, T.Prefix, T.Markers, T._name, T._externs, T._usings, newMembers, T.End);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is BlockScopeNamespaceDeclaration && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CollectionExpression(
        Guid id,
        Space prefix,
        Markers markers,
        IList<JRightPadded<Expression>> elements,
        JavaType type
    ) : Cs, Expression, TypedTree, MutableTree<CollectionExpression>
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
            return v.VisitCollectionExpression(this, p);
        }

        public Guid Id => id;

        public CollectionExpression WithId(Guid newId)
        {
            return newId == id ? this : new CollectionExpression(newId, prefix, markers, _elements, type);
        }

        public Space Prefix => prefix;

        public CollectionExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new CollectionExpression(id, newPrefix, markers, _elements, type);
        }

        public Markers Markers => markers;

        public CollectionExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new CollectionExpression(id, prefix, newMarkers, _elements, type);
        }

        private readonly IList<JRightPadded<Expression>> _elements = elements;
        public IList<Expression> Elements => _elements.Elements();

        public CollectionExpression WithElements(IList<Expression> newElements)
        {
            return Padding.WithElements(_elements.WithElements(newElements));
        }

        public JavaType Type => type;

        public CollectionExpression WithType(JavaType newType)
        {
            return newType == type ? this : new CollectionExpression(id, prefix, markers, _elements, newType);
        }

        public sealed record PaddingHelper(Cs.CollectionExpression T)
        {
            public IList<JRightPadded<Expression>> Elements => T._elements;

            public Cs.CollectionExpression WithElements(IList<JRightPadded<Expression>> newElements)
            {
                return T._elements == newElements ? T : new Cs.CollectionExpression(T.Id, T.Prefix, T.Markers, newElements, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is CollectionExpression && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ExpressionStatement(
        Guid id,
        Space prefix,
        Markers markers,
        Expression expression
    ) : Cs, Statement, MutableTree<ExpressionStatement>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitExpressionStatement(this, p);
        }

        public Guid Id => id;

        public ExpressionStatement WithId(Guid newId)
        {
            return newId == id ? this : new ExpressionStatement(newId, prefix, markers, expression);
        }

        public Space Prefix => prefix;

        public ExpressionStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ExpressionStatement(id, newPrefix, markers, expression);
        }

        public Markers Markers => markers;

        public ExpressionStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ExpressionStatement(id, prefix, newMarkers, expression);
        }

        public Expression Expression => expression;

        public ExpressionStatement WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new ExpressionStatement(id, prefix, markers, newExpression);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ExpressionStatement && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ExternAlias(
        Guid id,
        Space prefix,
        Markers markers,
        JLeftPadded<J.Identifier> identifier
    ) : Cs, Statement, MutableTree<ExternAlias>
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
            return v.VisitExternAlias(this, p);
        }

        public Guid Id => id;

        public ExternAlias WithId(Guid newId)
        {
            return newId == id ? this : new ExternAlias(newId, prefix, markers, _identifier);
        }

        public Space Prefix => prefix;

        public ExternAlias WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ExternAlias(id, newPrefix, markers, _identifier);
        }

        public Markers Markers => markers;

        public ExternAlias WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ExternAlias(id, prefix, newMarkers, _identifier);
        }

        private readonly JLeftPadded<J.Identifier> _identifier = identifier;
        public J.Identifier Identifier => _identifier.Element;

        public ExternAlias WithIdentifier(J.Identifier newIdentifier)
        {
            return Padding.WithIdentifier(_identifier.WithElement(newIdentifier));
        }

        public sealed record PaddingHelper(Cs.ExternAlias T)
        {
            public JLeftPadded<J.Identifier> Identifier => T._identifier;

            public Cs.ExternAlias WithIdentifier(JLeftPadded<J.Identifier> newIdentifier)
            {
                return T._identifier == newIdentifier ? T : new Cs.ExternAlias(T.Id, T.Prefix, T.Markers, newIdentifier);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ExternAlias && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class FileScopeNamespaceDeclaration(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> name,
        IList<JRightPadded<ExternAlias>> externs,
        IList<JRightPadded<UsingDirective>> usings,
        IList<JRightPadded<Statement>> members
    ) : Cs, Statement, MutableTree<FileScopeNamespaceDeclaration>
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is FileScopeNamespaceDeclaration && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class InterpolatedString(
        Guid id,
        Space prefix,
        Markers markers,
        string start,
        IList<JRightPadded<Expression>> parts,
        string end
    ) : Cs, Expression, MutableTree<InterpolatedString>
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
            return v.VisitInterpolatedString(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public InterpolatedString WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public InterpolatedString WithId(Guid newId)
        {
            return newId == id ? this : new InterpolatedString(newId, prefix, markers, start, _parts, end);
        }

        public Space Prefix => prefix;

        public InterpolatedString WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new InterpolatedString(id, newPrefix, markers, start, _parts, end);
        }

        public Markers Markers => markers;

        public InterpolatedString WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new InterpolatedString(id, prefix, newMarkers, start, _parts, end);
        }

        public string Start => start;

        public InterpolatedString WithStart(string newStart)
        {
            return newStart == start ? this : new InterpolatedString(id, prefix, markers, newStart, _parts, end);
        }

        private readonly IList<JRightPadded<Expression>> _parts = parts;
        public IList<Expression> Parts => _parts.Elements();

        public InterpolatedString WithParts(IList<Expression> newParts)
        {
            return Padding.WithParts(_parts.WithElements(newParts));
        }

        public string End => end;

        public InterpolatedString WithEnd(string newEnd)
        {
            return newEnd == end ? this : new InterpolatedString(id, prefix, markers, start, _parts, newEnd);
        }

        public sealed record PaddingHelper(Cs.InterpolatedString T)
        {
            public IList<JRightPadded<Expression>> Parts => T._parts;

            public Cs.InterpolatedString WithParts(IList<JRightPadded<Expression>> newParts)
            {
                return T._parts == newParts ? T : new Cs.InterpolatedString(T.Id, T.Prefix, T.Markers, T.Start, newParts, T.End);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is InterpolatedString && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Interpolation(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> expression,
        JRightPadded<Expression>? alignment,
        JRightPadded<Expression>? format
    ) : Cs, Expression, MutableTree<Interpolation>
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
            return v.VisitInterpolation(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public Interpolation WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public Interpolation WithId(Guid newId)
        {
            return newId == id ? this : new Interpolation(newId, prefix, markers, _expression, _alignment, _format);
        }

        public Space Prefix => prefix;

        public Interpolation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Interpolation(id, newPrefix, markers, _expression, _alignment, _format);
        }

        public Markers Markers => markers;

        public Interpolation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Interpolation(id, prefix, newMarkers, _expression, _alignment, _format);
        }

        private readonly JRightPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public Interpolation WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }

        private readonly JRightPadded<Expression>? _alignment = alignment;
        public Expression? Alignment => _alignment?.Element;

        public Interpolation WithAlignment(Expression? newAlignment)
        {
            return Padding.WithAlignment(JRightPadded<Expression>.WithElement(_alignment, newAlignment));
        }

        private readonly JRightPadded<Expression>? _format = format;
        public Expression? Format => _format?.Element;

        public Interpolation WithFormat(Expression? newFormat)
        {
            return Padding.WithFormat(JRightPadded<Expression>.WithElement(_format, newFormat));
        }

        public sealed record PaddingHelper(Cs.Interpolation T)
        {
            public JRightPadded<Expression> Expression => T._expression;

            public Cs.Interpolation WithExpression(JRightPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.Interpolation(T.Id, T.Prefix, T.Markers, newExpression, T._alignment, T._format);
            }

            public JRightPadded<Expression>? Alignment => T._alignment;

            public Cs.Interpolation WithAlignment(JRightPadded<Expression>? newAlignment)
            {
                return T._alignment == newAlignment ? T : new Cs.Interpolation(T.Id, T.Prefix, T.Markers, T._expression, newAlignment, T._format);
            }

            public JRightPadded<Expression>? Format => T._format;

            public Cs.Interpolation WithFormat(JRightPadded<Expression>? newFormat)
            {
                return T._format == newFormat ? T : new Cs.Interpolation(T.Id, T.Prefix, T.Markers, T._expression, T._alignment, newFormat);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Interpolation && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class NullSafeExpression(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<Expression> expression
    ) : Cs, Expression, MutableTree<NullSafeExpression>
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
            return v.VisitNullSafeExpression(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public NullSafeExpression WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public NullSafeExpression WithId(Guid newId)
        {
            return newId == id ? this : new NullSafeExpression(newId, prefix, markers, _expression);
        }

        public Space Prefix => prefix;

        public NullSafeExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NullSafeExpression(id, newPrefix, markers, _expression);
        }

        public Markers Markers => markers;

        public NullSafeExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NullSafeExpression(id, prefix, newMarkers, _expression);
        }

        private readonly JRightPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public NullSafeExpression WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }

        public sealed record PaddingHelper(Cs.NullSafeExpression T)
        {
            public JRightPadded<Expression> Expression => T._expression;

            public Cs.NullSafeExpression WithExpression(JRightPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.NullSafeExpression(T.Id, T.Prefix, T.Markers, newExpression);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NullSafeExpression && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StatementExpression(
        Guid id,
        Space prefix,
        Markers markers,
        Statement statement
    ) : Cs, Expression, MutableTree<StatementExpression>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitStatementExpression(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public StatementExpression WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public StatementExpression WithId(Guid newId)
        {
            return newId == id ? this : new StatementExpression(newId, prefix, markers, statement);
        }

        public Space Prefix => prefix;

        public StatementExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new StatementExpression(id, newPrefix, markers, statement);
        }

        public Markers Markers => markers;

        public StatementExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new StatementExpression(id, prefix, newMarkers, statement);
        }

        public Statement Statement => statement;

        public StatementExpression WithStatement(Statement newStatement)
        {
            return ReferenceEquals(newStatement, statement) ? this : new StatementExpression(id, prefix, markers, newStatement);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is StatementExpression && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class UsingDirective(
        Guid id,
        Space prefix,
        Markers markers,
        JRightPadded<bool> global,
        JLeftPadded<bool> @static,
        JLeftPadded<bool> @unsafe,
        JRightPadded<J.Identifier>? alias,
        TypeTree namespaceOrType
    ) : Cs, Statement, MutableTree<UsingDirective>
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is UsingDirective && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PropertyDeclaration(
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
