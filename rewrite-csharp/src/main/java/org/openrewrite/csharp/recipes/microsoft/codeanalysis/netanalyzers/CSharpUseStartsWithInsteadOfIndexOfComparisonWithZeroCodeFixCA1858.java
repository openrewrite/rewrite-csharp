package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUseStartsWithInsteadOfIndexOfComparisonWithZeroCodeFixCA1858 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1858";
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
        return "Use 'StartsWith' instead of 'IndexOf'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "It is both clearer and faster to use 'StartsWith' instead of comparing the result of 'IndexOf' to zero.";
    }
}
