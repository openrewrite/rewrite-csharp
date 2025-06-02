package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SealInternalTypesCA1852 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1852";
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
        return "Seal internal types";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "When a type is not accessible outside its assembly and has no subtypes within its containing assembly, it can be safely sealed. Sealing types can improve performance.";
    }
}
