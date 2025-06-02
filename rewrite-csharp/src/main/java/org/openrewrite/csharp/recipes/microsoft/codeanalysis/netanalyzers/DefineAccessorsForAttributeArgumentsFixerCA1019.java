package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DefineAccessorsForAttributeArgumentsFixerCA1019 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1019";
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
        return "Define accessors for attribute arguments";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
