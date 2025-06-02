package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpMarkAllNonSerializableFieldsFixerCA2235 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2235";
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
        return "Mark all non-serializable fields";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "An instance field of a type that is not serializable is declared in a type that is serializable.";
    }
}
