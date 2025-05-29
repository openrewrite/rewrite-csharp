using Rewrite.Core;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

partial interface CsContainer
{
    partial record Location
    {
    }
}


public partial interface Cs
{

    public partial class EnumMemberDeclaration
    {
        JavaType? TypedTree.Type => this.Name.Type;
        public EnumMemberDeclaration WithType(JavaType? type) => WithName(Name.WithType(type));
    }
    public partial class CheckedExpression
    {
        JavaType? TypedTree.Type => this.Expression.Type;
        public CheckedExpression WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }
    public partial class PointerType
    {
        JavaType? TypedTree.Type => this.ElementType.Type;
        public PointerType WithType(JavaType? type) => WithElementType(ElementType.WithType(type));
    }
    public partial class RefExpression
    {
        JavaType? TypedTree.Type => this.Expression.Type;
        public RefExpression WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }
    public partial class StackAllocExpression
    {
        JavaType? TypedTree.Type => this.Expression.Type;
        public StackAllocExpression WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }
    public partial class Subpattern : Expression<Subpattern>
    {
        JavaType? TypedTree.Type => this.Pattern.Type;
        public Subpattern WithType(JavaType? type) => WithPattern((Pattern)Pattern.WithType(type));
    }
    public partial class AliasQualifiedName : Expression<AliasQualifiedName>
    {
        JavaType? TypedTree.Type => this.Name.Type;
        public AliasQualifiedName WithType(JavaType? type) => WithName(Name.WithType(type));
    }
    public new partial class ClassDeclaration : Expression<ClassDeclaration>
    {
        public J.ClassDeclaration.Kind Kind => _kind;
        JavaType? TypedTree.Type => Type;
        public ClassDeclaration WithType(JavaType? type) => WithType((JavaType.FullyQualified?)type);
        public CoordinateBuilder.ClassDeclaration Coordinates => new (this);
        CoordinateBuilder.Statement Statement.Coordinates => Coordinates;
    }
    public new partial class MethodDeclaration : Expression<MethodDeclaration>
    {
        public bool IsAbstract => this.Modifiers.Any(x => x.Keyword == "abstract");
        public JavaType? Type => this.MethodType;
        public MethodDeclaration WithType(JavaType? type) => WithMethodType(type as JavaType.Method);
        public CoordinateBuilder.MethodDeclaration Coordinates => new (this);
        CoordinateBuilder.Statement Statement.Coordinates => Coordinates;
    }
    public partial class IndexerDeclaration : Expression<IndexerDeclaration>
    {
        public JavaType? Type => this.TypeExpression.Type;
        public IndexerDeclaration WithType(JavaType? type) => WithTypeExpression(TypeExpression.WithType(type));

    }
    public partial class QueryExpression : Expression<QueryExpression>
    {
        public JavaType? Type => this.FromClause.Type;
        public QueryExpression WithType(JavaType? type) => WithFromClause(FromClause.WithType(type));
    }
    public partial class FromClause : Expression<FromClause>
    {
        public JavaType? Type => this.Expression.Type;
        public FromClause WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }
    public partial class RangeExpression : Expression<RangeExpression>
    {
        public JavaType? Type => Start?.Type ?? End?.Type;
        public RangeExpression WithType(JavaType? type) => this;
    }
    public partial class DefaultSwitchLabel : Expression<DefaultSwitchLabel>
    {
        public JavaType? Type => null;
        public DefaultSwitchLabel WithType(JavaType? type) => this;
    }

    public partial class CasePatternSwitchLabel : Expression<CasePatternSwitchLabel>
    {
        public JavaType? Type => Pattern.Type;
        public CasePatternSwitchLabel WithType(JavaType? type) => WithPattern((Pattern)Pattern.WithType(type));
    }
    public new partial class SwitchExpression : Expression<SwitchExpression>
    {
        public JavaType? Type => Expression.Type;
        public SwitchExpression WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }

    // public partial interface VariableDesignation<T> : VariableDesignation, Expression<T> where T : VariableDesignation
    // {
    //     VariableDesignation VariableDesignation.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    //     Expression Expression.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    // }

    // public partial interface VariableDesignation : Expression
    // {
    //     public new VariableDesignation WithType(JavaType? type);
    //     Expression Expression.WithType(JavaType? type) => WithType(type);
    // }
    // public partial interface Pattern<out T> : Pattern, Expression<T> where T : Pattern
    // {
    //     // Expression Expression.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    //     // Pattern Pattern.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    // }
    // public partial interface Pattern : Expression
    // {
    //     // public new Pattern WithType(JavaType? type);
    //     // Expression Expression.WithType(JavaType? type) => WithType(type);
    // }

    public partial class ConstantPattern
    {
        public JavaType? Type => Value.Type;
        public ConstantPattern WithType(JavaType? type) => WithValue(Value.WithType(type));

    }

    public partial class BinaryPattern
    {
        public JavaType? Type => null;
        public BinaryPattern WithType(JavaType? type) => this;
    }

    public partial class RelationalPattern
    {
        public JavaType? Type => this.Value.Type;
        public RelationalPattern WithType(JavaType? type) => WithValue(Value.WithType(type));
    }

    public partial class TypePattern
    {
        public JavaType? Type => this.TypeIdentifier.Type;
        public TypePattern WithType(JavaType? type) => WithTypeIdentifier(TypeIdentifier.WithType(type));
    }

    public partial class VarPattern
    {
        public JavaType? Type => this.Designation.Type;
        public VarPattern WithType(JavaType? type) => this.WithDesignation((VariableDesignation)Designation.WithType(type));
    }

    public partial class SlicePattern
    {
        public JavaType? Type => null;
        public SlicePattern WithType(JavaType? type) => this;
    }

    public partial class RecursivePattern
    {
        public JavaType? Type => TypeQualifier?.Type;
        public RecursivePattern WithType(JavaType? type) => TypeQualifier != null ? WithTypeQualifier(TypeQualifier.WithType(type)) : this;
    }

    public partial class ListPattern
    {
        public JavaType? Type => Patterns is [{ } singleValue] ? singleValue.Type : null;
        public ListPattern WithType(JavaType? type) => Patterns is [{ } singleValue] ? Padding.WithPatterns(Padding.Patterns.WithElements([singleValue])) : this;
    }

    public partial class ParenthesizedPattern
    {
        public JavaType? Type => Pattern is [{ } singleValue] ? singleValue.Type : null;
        public ParenthesizedPattern WithType(JavaType? type) => Pattern is [{ } singleValue] ? Padding.WithPattern(Padding.Pattern.WithElements([singleValue])) : this;
    }

    public partial class DiscardPattern
    {
    }

    public partial class UnaryPattern
    {
        public JavaType? Type => Pattern.Type;
        public UnaryPattern WithType(JavaType? type) => WithPattern((Pattern)Pattern.WithType(type));
    }
    public partial class IsPattern
    {
        public JavaType? Type => Pattern.Type;
        public IsPattern WithType(JavaType? type)
        {
            return WithPattern((Pattern)Pattern.WithType(type));
        }
    }
    public new partial class NewClass
    {
        public JavaType? Type => NewClassCore.Type;
        public NewClass WithType(JavaType? type) => WithNewClassCore(NewClassCore.WithType(type));
    }

    public partial class InitializerExpression
    {
        public JavaType? Type => null;
        public InitializerExpression WithType(JavaType? type) => this;
    }

    public partial class DeclarationExpression
    {
        public JavaType? Type => null;
        public DeclarationExpression WithType(JavaType? type) => this;
    }

    public new partial class Lambda
    {
        public JavaType? Type => LambdaExpression.Type;
        public Lambda WithType(JavaType? type) => WithLambdaExpression(LambdaExpression.WithType(type));
        
    }

    public partial class StatementExpression
    {
        public JavaType? Type => null;
        public StatementExpression WithType(JavaType? type) => this;
    }

    public partial class NullSafeExpression
    {
        public JavaType? Type => Expression.Type;
        public NullSafeExpression WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }

    public partial class InterpolatedString
    {
        public JavaType? Type => new JavaType.Primitive(JavaType.Primitive.PrimitiveType.String);
        public InterpolatedString WithType(JavaType? type) => this;
    }

    public partial class Interpolation
    {
        public JavaType? Type => Type;
        public Interpolation WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }

    public partial class ParenthesizedVariableDesignation : VariableDesignation
    {
    }

    public partial class SingleVariableDesignation : VariableDesignation
    {
        public JavaType? Type => Name.Type;
        public SingleVariableDesignation WithType(JavaType? type) => WithName(Name.WithType(type));
    }

    public partial class DiscardVariableDesignation : VariableDesignation
    {
        public JavaType? Type => Discard.Type;
        public DiscardVariableDesignation WithType(JavaType? type) => WithDiscard(Discard.WithType(type));
    }

    public partial class TupleExpression
    {
        public JavaType? Type => null;
        public TupleExpression WithType(JavaType? type) => this;
    }

    public partial class ImplicitElementAccess
    {
        public JavaType? Type => null;
        public ImplicitElementAccess WithType(JavaType? type) => this;
    }

    public partial class PropertyDeclaration
    {
        public JavaType? Type => TypeExpression.Type;
        public PropertyDeclaration WithType(JavaType? type) =>  WithTypeExpression(TypeExpression.WithType(type));
    }

    public partial class TypeConstraint
    {
        public JavaType? Type => TypeExpression.Type;
        public TypeConstraint WithType(JavaType? type) =>  WithTypeExpression(TypeExpression.WithType(type));
    }

    public partial class DefaultExpression
    {
        //todo: add proper type attestation
        public JavaType? Type => TypeOperator?.FirstOrDefault()?.Type;
        public DefaultExpression WithType(JavaType? type) => this;
    }
}

