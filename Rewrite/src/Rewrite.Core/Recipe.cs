using System.Runtime.CompilerServices;
using Rewrite.Core.Config;

namespace Rewrite.Core;

public abstract class Recipe
{
    private RecipeDescriptor? _descriptor;

    public RecipeDescriptor Descriptor
    {
        get
        {
            return _descriptor ??= new RecipeDescriptor(GetType().FullName!, GetType().FullName!, "",
                new HashSet<string>(), null,
                GetType().GetConstructors()[0].GetParameters().Select(parameterInfo =>
                {
                    return new OptionDescriptor(parameterInfo.Name!, parameterInfo.ParameterType.FullName!, null,
                        null,
                        null,
                        null,
                        !(parameterInfo.IsOptional || parameterInfo.HasDefaultValue ||
                          parameterInfo.CustomAttributes.FirstOrDefault(data =>
                              data.AttributeType == typeof(NullableAttribute)) != null),
                        parameterInfo.DefaultValue
                    );
                }).ToList(),
                [],
                new Uri($"recipe://{GetType().FullName}")
            );
        }

        set => _descriptor = value;
    }

    public static Recipe Noop()
    {
        return new NoopRecipe();
    }

    private class NoopRecipe : Recipe
    {
        public override ITreeVisitor<Tree, ExecutionContext> GetVisitor()
        {
            return ITreeVisitor<Tree, ExecutionContext>.Noop();
        }
    }

    public virtual ITreeVisitor<Tree, ExecutionContext> GetVisitor()
    {
        return ITreeVisitor<Tree, ExecutionContext>.Noop();
    }

    public IList<Validated<object>> ValidateAll(ExecutionContext ctx, IList<Validated<object>> acc)
    {
        acc.Add(Validate(ctx));
        // for (Recipe recipe : getRecipeList()) {
        //     recipe.validateAll(ctx, acc);
        // }
        return acc;
    }

    public Validated<object> Validate(ExecutionContext ctx)
    {
        var validated = Validate();

        // for (Recipe recipe : getRecipeList()) {
        //     validated = validated.and(recipe.validate(ctx));
        // }
        return validated;
    }

    public Validated<object> Validate()
    {
        var validated = Validated<object>.None();
        // Class<? extends Recipe> clazz = this.getClass();
        // List<Field> requiredFields = NullUtils.findNonNullFields(clazz);
        // for (Field field : requiredFields) {
        //     try {
        //         validated = validated.and(Validated.required(clazz.getSimpleName() + '.' + field.getName(), field.get(this)));
        //     } catch (IllegalAccessException e) {
        //         validated = Validated.invalid(field.getName(), null, "Unable to access " + clazz.getName() + "." + field.getName(), e);
        //     }
        // }
        // for (Recipe recipe : getRecipeList()) {
        //     validated = validated.and(recipe.validate());
        // }
        return validated;
    }
}