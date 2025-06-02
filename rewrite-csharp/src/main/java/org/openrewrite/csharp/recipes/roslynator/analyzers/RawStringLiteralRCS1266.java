package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class RawStringLiteralRCS1266 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1266";
    }

    @Override
    public String getNugetPackageName() {
        return "Roslynator.Analyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "4.13.1";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Use raw string literal";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
