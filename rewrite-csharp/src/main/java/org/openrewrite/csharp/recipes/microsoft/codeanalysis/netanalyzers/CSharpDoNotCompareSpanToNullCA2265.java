package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotCompareSpanToNullCA2265 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2265";
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
        return "Do not compare Span<T> to 'null' or 'default'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Comparing a span to 'null' or 'default' might not do what you intended. 'default' and the 'null' literal are implicitly converted to 'Span<T>.Empty'. Remove the redundant comparison or make the code more explicit by using 'IsEmpty'.";
    }
}
