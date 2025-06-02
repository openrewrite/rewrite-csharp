package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class AvoidLengthCalculationWhenSlicingToEndCA1514 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1514";
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
        return "Avoid redundant length argument";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "An explicit length calculation can be error-prone and can be avoided when slicing to end of the buffer.";
    }
}
