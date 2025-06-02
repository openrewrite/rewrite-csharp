package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class EquatableFixerCA1066 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1066";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Implement IEquatable when overriding Object.Equals";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "When a type T overrides Object.Equals(object), the implementation must cast the object argument to the correct type T before performing the comparison. If the type implements IEquatable<T>, and therefore offers the method T.Equals(T), and if the argument is known at compile time to be of type T, then the compiler can call IEquatable<T>.Equals(T) instead of Object.Equals(object), and no cast is necessary, improving performance.";
    }
}
