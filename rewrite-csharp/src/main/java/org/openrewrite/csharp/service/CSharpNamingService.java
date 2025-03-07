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
package org.openrewrite.csharp.service;

import org.openrewrite.internal.NameCaseConvention;
import org.openrewrite.internal.NamingService;

import java.util.regex.Pattern;

public class CSharpNamingService implements NamingService {

    private static final Pattern STANDARD_METHOD_NAME = Pattern.compile("^[A-Z][a-zA-Z0-9]*$");
    private static final Pattern SNAKE_CASE = Pattern.compile("^[a-zA-Z0-9]+_\\w+$");

    @Override
    public String standardizeMethodName(String oldMethodName) {
        if (!STANDARD_METHOD_NAME.matcher(oldMethodName).matches()) {
            StringBuilder result = new StringBuilder();
            if (SNAKE_CASE.matcher(oldMethodName).matches()) {
                result.append(NameCaseConvention.format(NameCaseConvention.UPPER_CAMEL, oldMethodName));
            } else {
                int nameLength = oldMethodName.length();
                for (int i = 0; i < nameLength; i++) {
                    char c = oldMethodName.charAt(i);
                    if (i == 0) {
                        // the java specification requires identifiers to start with [a-zA-Z$_]
                        if (c != '$' && c != '_') {
                            result.append(Character.toUpperCase(c));
                        }
                    } else {
                        if (!Character.isLetterOrDigit(c)) {
                            while ((!Character.isLetterOrDigit(c) || c > 'z')) {
                                i++;
                                if (i < nameLength) {
                                    c = oldMethodName.charAt(i);
                                } else {
                                    break;
                                }
                            }
                            if (i < nameLength) {
                                result.append(Character.toUpperCase(c));
                            }
                        } else {
                            result.append(c);
                        }
                    }
                }
            }
            return result.toString();
        }
        return oldMethodName;
    }
}
