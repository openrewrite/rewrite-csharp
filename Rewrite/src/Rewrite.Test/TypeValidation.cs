namespace Rewrite.Test;

public record TypeValidation(bool Unknowns)
{
    public static TypeValidation All()
    {
        return new TypeValidation(true);
    }

    public static TypeValidation None()
    {
        return new TypeValidation(false);
    }

    public static TypeValidation Before(RecipeSpec testMethodSpec, RecipeSpec testClassSpec)
    {
        return testMethodSpec.TypeValidation ?? testClassSpec.TypeValidation ?? All();
    }
}