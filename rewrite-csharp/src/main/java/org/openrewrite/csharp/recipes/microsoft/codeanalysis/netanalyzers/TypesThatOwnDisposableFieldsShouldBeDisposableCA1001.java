package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class TypesThatOwnDisposableFieldsShouldBeDisposableCA1001 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1001";
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
        return "Types that own disposable fields should be disposable";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "A class declares and implements an instance field that is a System.IDisposable type, and the class does not implement IDisposable. A class that declares an IDisposable field indirectly owns an unmanaged resource and should implement the IDisposable interface.";
    }
}
