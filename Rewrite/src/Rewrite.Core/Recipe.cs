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
            return _descriptor ??= new RecipeDescriptor(
                Name,
                DisplayName,
                Description,
                Tags,
                EstimatedEffortPerOccurrence,
                GetType().GetConstructors()[0].GetParameters().Select(parameterInfo =>
                {
                    var option = ((OptionAttribute[])parameterInfo.GetCustomAttributes(typeof(OptionAttribute), false)).FirstOrDefault();
                    return new OptionDescriptor(parameterInfo.Name!, parameterInfo.ParameterType.FullName!, option?.DisplayName ?? parameterInfo.Name, option?.Description ?? null,
                        option?.Example,
                        null,
                        !(parameterInfo.IsOptional || parameterInfo.HasDefaultValue ||
                          parameterInfo.ParameterType.CustomAttributes.FirstOrDefault(data =>
                              data.AttributeType == typeof(NullableAttribute)) != null),
                        parameterInfo.DefaultValue
                    );
                }).ToList(),
                [],
                new Uri($"recipe://{GetType().Name}")
            );
        }

        set => _descriptor = value;
    }

    public string Name => GetType().Name!;

    public string InstanceName => GetType().FullName!;

    public abstract string DisplayName { get; }

    public abstract string Description { get; }

    public ISet<string> Tags => new HashSet<string>();

    public TimeSpan? EstimatedEffortPerOccurrence => TimeSpan.FromMinutes(5);

    public static Recipe Noop()
    {
        return new NoopRecipe();
    }

    private class NoopRecipe : Recipe
    {
        public override string DisplayName => "NoopRecipe";

        public override string Description => "NoopRecipe";

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
