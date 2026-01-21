/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Moderne Source Available License (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://docs.moderne.io/licensing/moderne-source-available-license
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
/*
 * -------------------THIS FILE IS AUTO GENERATED--------------------------
 * Changes to this file may cause incorrect behavior and will be lost if
 * the code is regenerated.
*/

package org.openrewrite.csharp.recipes.microsoft;

import org.openrewrite.csharp.RoslynRecipe;

import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class DoNotUseInsecureDtdProcessingAnalyzerCA3075 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA3075";
    }

    @Override
    public boolean getRunCodeFixup() {
        return false;
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "10.0.102";
    }

    @Override
    public String getDisplayName() {
        return "Analysis: Insecure DTD processing in XML";
    }

    @Override
    public String getDescription() {
        return "This is a reporting only recipe. Using XmlTextReader.Load(), creating an insecure XmlReaderSettings instance when invoking XmlReader.Create(), setting the InnerXml property of the XmlDocument and enabling DTD processing using XmlUrlResolver insecurely can lead to information disclosure. Replace it with a call to the Load() method overload that takes an XmlReader instance, use XmlReader.Create() to accept XmlReaderSettings arguments or consider explicitly setting secure values. The DataViewSettingCollectionString property of DataViewManager should always be assigned from a trusted source, the DtdProcessing property should be set to false, and the XmlResolver property should be changed to XmlSecureResolver or null.";
    }

    @Override
    public Set<String> getTags() {
        return Stream.of("roslyn", "analyzer", "CA3075", "microsoft", "csharp", "dotnet", "c#").collect(Collectors.toSet());
    }
    }
