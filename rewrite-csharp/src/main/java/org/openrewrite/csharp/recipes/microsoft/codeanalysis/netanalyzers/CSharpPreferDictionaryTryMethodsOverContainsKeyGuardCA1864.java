package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferDictionaryTryMethodsOverContainsKeyGuardCA1864 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1864";
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
        return "Prefer the 'IDictionary.TryAdd(TKey, TValue)' method";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Prefer a 'TryAdd' call over an 'Add' call guarded by a 'ContainsKey' check. 'TryAdd' behaves the same as 'Add', except that when the specified key already exists, it returns 'false' instead of throwing an exception.";
    }
}
