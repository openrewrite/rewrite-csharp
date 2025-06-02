package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseStringEqualsOverStringCompareCA2251 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2251";
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
        return "Use 'string.Equals'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "It is both clearer and likely faster to use 'string.Equals' instead of comparing the result of 'string.Compare' to zero.";
    }
}
