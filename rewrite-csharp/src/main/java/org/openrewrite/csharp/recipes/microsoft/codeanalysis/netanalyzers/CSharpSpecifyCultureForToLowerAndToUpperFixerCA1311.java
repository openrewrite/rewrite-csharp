package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpSpecifyCultureForToLowerAndToUpperFixerCA1311 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1311";
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
        return "Specify a culture or use an invariant version";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Specify culture to help avoid accidental implicit dependency on current culture. Using an invariant version yields consistent results regardless of the culture of an application.";
    }
}
