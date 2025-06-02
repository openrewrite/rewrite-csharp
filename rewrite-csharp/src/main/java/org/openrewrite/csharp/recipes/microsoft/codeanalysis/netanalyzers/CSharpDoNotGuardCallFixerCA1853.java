package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotGuardCallFixerCA1853 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1853";
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
        return "Unnecessary call to 'Dictionary.ContainsKey(key)'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Do not guard 'Dictionary.Remove(key)' with 'Dictionary.ContainsKey(key)'. The former already checks whether the key exists, and will not throw if it does not.";
    }
}
