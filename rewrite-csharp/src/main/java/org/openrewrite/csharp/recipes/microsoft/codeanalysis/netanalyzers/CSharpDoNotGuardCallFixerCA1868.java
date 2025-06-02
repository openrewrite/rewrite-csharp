package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotGuardCallFixerCA1868 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1868";
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
        return "Unnecessary call to 'Contains(item)'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Do not guard 'Add(item)' or 'Remove(item)' with 'Contains(item)' for the set. The former two already check whether the item exists and will return if it was added or removed.";
    }
}
