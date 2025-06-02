package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUsePropertyInsteadOfCountMethodWhenAvailableCA1829 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1829";
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
        return "Use Length/Count property instead of Count() when available";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Enumerable.Count() potentially enumerates the sequence while a Length/Count property is a direct access.";
    }
}
