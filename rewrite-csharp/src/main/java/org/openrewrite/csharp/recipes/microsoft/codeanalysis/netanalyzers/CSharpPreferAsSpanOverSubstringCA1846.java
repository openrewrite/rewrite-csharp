package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferAsSpanOverSubstringCA1846 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1846";
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
        return "Prefer 'AsSpan' over 'Substring'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'AsSpan' is more efficient than 'Substring'. 'Substring' performs an O(n) string copy, while 'AsSpan' does not and has a constant cost.";
    }
}
