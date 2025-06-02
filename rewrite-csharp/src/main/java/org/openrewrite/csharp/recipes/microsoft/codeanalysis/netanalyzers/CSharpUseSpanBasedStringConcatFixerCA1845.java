package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUseSpanBasedStringConcatFixerCA1845 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1845";
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
        return "Use span-based 'string.Concat'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "It is more efficient to use 'AsSpan' and 'string.Concat', instead of 'Substring' and a concatenation operator.";
    }
}
