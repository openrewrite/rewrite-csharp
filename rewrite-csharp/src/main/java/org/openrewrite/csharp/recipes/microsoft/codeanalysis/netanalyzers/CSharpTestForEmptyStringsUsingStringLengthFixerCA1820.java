package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpTestForEmptyStringsUsingStringLengthFixerCA1820 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1820";
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
        return "Test for empty strings using string length";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Comparing strings by using the String.Length property or the String.IsNullOrEmpty method is significantly faster than using Equals.";
    }
}
