package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferGenericOverloadsCA2263 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2263";
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
        return "Prefer generic overload when type is known";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Using a generic overload is preferable to the 'System.Type' overload when the type is known, promoting cleaner and more type-safe code with improved compile-time checks.";
    }
}
