package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpIdentifiersShouldNotContainUnderscoresFixerCA1707 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1707";
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
        return "Identifiers should not contain underscores";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "By convention, identifier names do not contain the underscore (_) character. This rule checks namespaces, types, members, and parameters.";
    }
}
