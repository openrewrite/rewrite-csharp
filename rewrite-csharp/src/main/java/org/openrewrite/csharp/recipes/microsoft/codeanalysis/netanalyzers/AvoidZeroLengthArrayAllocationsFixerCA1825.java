package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class AvoidZeroLengthArrayAllocationsFixerCA1825 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1825";
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
        return "Avoid zero-length array allocations";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
