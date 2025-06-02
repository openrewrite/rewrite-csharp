package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUseStringContainsCharOverloadWithSingleCharactersCA1847 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1847";
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
        return "Use char literal for a single character lookup";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'string.Contains(char)' is available as a better performing overload for single char lookup.";
    }
}
