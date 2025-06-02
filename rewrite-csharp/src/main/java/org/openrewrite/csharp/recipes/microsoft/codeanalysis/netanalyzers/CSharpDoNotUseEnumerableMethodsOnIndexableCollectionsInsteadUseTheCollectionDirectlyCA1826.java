package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotUseEnumerableMethodsOnIndexableCollectionsInsteadUseTheCollectionDirectlyCA1826 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1826";
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
        return "Do not use Enumerable methods on indexable collections";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "This collection is directly indexable. Going through LINQ here causes unnecessary allocations and CPU work.";
    }
}
