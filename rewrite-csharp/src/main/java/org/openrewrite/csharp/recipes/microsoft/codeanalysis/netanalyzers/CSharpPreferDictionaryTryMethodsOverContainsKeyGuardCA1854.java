package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferDictionaryTryMethodsOverContainsKeyGuardCA1854 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1854";
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
        return "Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Prefer a 'TryGetValue' call over a Dictionary indexer access guarded by a 'ContainsKey' check. 'ContainsKey' and the indexer both would lookup the key under the hood, so using 'TryGetValue' removes the extra lookup.";
    }
}
