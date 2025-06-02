package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferDictionaryContainsMethodsCA1841 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1841";
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
        return "Prefer Dictionary.Contains methods";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'ContainsKey' is usually O(1), while 'Keys.Contains' may be O(n) in some cases. Additionally, many dictionary implementations lazily initialize the Keys collection to cut back on allocations.";
    }
}
