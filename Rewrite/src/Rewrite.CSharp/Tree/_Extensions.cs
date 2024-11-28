﻿using Rewrite.Core;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

public partial interface Cs
{
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
        public CasePatternSwitchLabel WithType(JavaType? type) => WithPattern(Pattern.WithType(type));
    }
    public partial class SwitchExpression : Expression<SwitchExpression>
    {
        public JavaType? Type => Expression.Type;
        public SwitchExpression WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }

    public partial interface VariableDesignation<T> : VariableDesignation, Expression<T> where T : VariableDesignation
    {
        VariableDesignation VariableDesignation.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
        Expression Expression.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    }

    public partial interface VariableDesignation : Expression
    {
        public new VariableDesignation WithType(JavaType? type);
        Expression Expression.WithType(JavaType? type) => WithType(type);
    }
    public partial interface Pattern<T> : Pattern, Expression<T> where T : Pattern
    {
        Expression Expression.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
        Pattern Pattern.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    }
    public partial interface Pattern : Expression
    {
        public new Pattern WithType(JavaType? type);
        Expression Expression.WithType(JavaType? type) => WithType(type);
    }

    public partial class ConstantPattern : Pattern<ConstantPattern>
    {
        public JavaType? Type => Value.Type;
        public ConstantPattern WithType(JavaType? type) => WithValue(Value.WithType(type));

    }

    public partial class BinaryPattern : Pattern<BinaryPattern>
    {
        public JavaType? Type => null;
        public BinaryPattern WithType(JavaType? type) => this;
    }

    public partial class RelationalPattern : Pattern<RelationalPattern>
    {
        public JavaType? Type => this.Value.Type;
        public RelationalPattern WithType(JavaType? type) => WithValue(Value.WithType(type));
    }

    public partial class TypePattern : Pattern<TypePattern>
    {
        public JavaType? Type => this.TypeIdentifier.Type;
        public TypePattern WithType(JavaType? type) => WithTypeIdentifier(TypeIdentifier.WithType(type));
    }

    public partial class VarPattern : Pattern<VarPattern>
    {
        public JavaType? Type => this.Designation.Type;
        public VarPattern WithType(JavaType? type) => this.WithDesignation(Designation.WithType(type));
    }

    public partial class SlicePattern : Pattern<SlicePattern>
    {
        public JavaType? Type => null;
        public SlicePattern WithType(JavaType? type) => this;
    }

    public partial class RecursivePattern : Pattern<RecursivePattern>
    {
        public JavaType? Type => TypeQualifier?.Type;
        public RecursivePattern WithType(JavaType? type) => TypeQualifier != null ? WithTypeQualifier(TypeQualifier.WithType(type)) : this;
    }

    public partial class ListPattern : Pattern<ListPattern>
    {
        public JavaType? Type => Patterns is [{ } singleValue] ? singleValue.Type : null;
        public ListPattern WithType(JavaType? type) => Patterns is [{ } singleValue] ? Padding.WithPatterns(Padding.Patterns.WithElements([singleValue])) : this;
    }

    public partial class ParenthesizedPattern : Pattern<ParenthesizedPattern>
    {
        public JavaType? Type => Pattern is [{ } singleValue] ? singleValue.Type : null;
        public ParenthesizedPattern WithType(JavaType? type) => Pattern is [{ } singleValue] ? Padding.WithPattern(Padding.Pattern.WithElements([singleValue])) : this;
    }

    public partial class DiscardPattern : Pattern<DiscardPattern>
    {
    }

    public partial class UnaryPattern : Pattern<UnaryPattern>
    {
        public JavaType? Type => Pattern.Type;
        public UnaryPattern WithType(JavaType? type) => WithPattern(Pattern.WithType(type));
    }
    public partial class IsPattern
    {
        public JavaType? Type => Pattern.Type;
        public IsPattern WithType(JavaType? type) => WithPattern(Pattern.WithType(type));
    }
    public partial class NewClass
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

    public partial class Lambda
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

    public partial class ParenthesizedVariableDesignation : VariableDesignation<ParenthesizedVariableDesignation>
    {
    }

    public partial class SingleVariableDesignation : VariableDesignation<SingleVariableDesignation>
    {
        public JavaType? Type => Name.Type;
        public SingleVariableDesignation WithType(JavaType? type) => WithName(Name.WithType(type));
    }

    public partial class DiscardVariableDesignation : VariableDesignation<DiscardVariableDesignation>
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
