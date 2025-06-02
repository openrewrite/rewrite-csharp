package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ImplementStandardExceptionConstructorsFixerCA1032 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1032";
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
        return "Implement standard exception constructors";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Failure to provide the full set of constructors can make it difficult to correctly handle exceptions.";
    }
}
