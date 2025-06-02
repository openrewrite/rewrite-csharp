package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DoNotCallToImmutableCollectionOnAnImmutableCollectionValueCA2009 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2009";
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
        return "Do not call ToImmutableCollection on an ImmutableCollection value";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
