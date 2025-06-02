package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseNameOfInPlaceOfStringFixerCA1507 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1507";
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
        return "Use nameof to express symbol names";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Using nameof helps keep your code valid when refactoring.";
    }
}
