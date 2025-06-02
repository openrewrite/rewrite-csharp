package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUseStringMethodCharOverloadWithSingleCharactersFixerCA1865 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1865";
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
        return "Use char overload";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "The char overload is a better performing overload than a string with a single char.";
    }
}
