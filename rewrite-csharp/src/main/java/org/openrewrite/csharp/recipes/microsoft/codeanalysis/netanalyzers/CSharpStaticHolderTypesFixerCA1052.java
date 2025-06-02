package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpStaticHolderTypesFixerCA1052 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1052";
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
        return "Static holder types should be Static or NotInheritable";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
