package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class EqualityShouldBeCorrectlyImplementedMA0077 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0077";
    }

    @Override
    public String getNugetPackageName() {
        return "Meziantou.Analyzer";
    }

    @Override
    public String getNugetPackageVersion() {
        return "2.0.201";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "A class that provides Equals(T) should implement IEquatable<T>";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
