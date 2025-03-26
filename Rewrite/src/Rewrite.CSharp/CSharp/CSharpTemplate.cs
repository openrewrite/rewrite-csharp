// namespace Rewrite.RewriteCSharp;
//
// public class CSharpTemplate : SourceTemplate<J, CSharpCoordinates>
// {
//     private static Path TEMPLATE_CLASSPATH_DIR;
//
//     protected static Path GetTemplateClasspathDir()
//     {
//         if (TEMPLATE_CLASSPATH_DIR == null)
//         {
//             try
//             {
//                 TEMPLATE_CLASSPATH_DIR = Files.CreateTempDirectory("csharp-template");
//                 Path templateDir = Files.CreateDirectories(TEMPLATE_CLASSPATH_DIR.Resolve("org/openrewrite/csharp/internal/template"));
//                 Path mClass = templateDir.Resolve("__M__.class");
//                 Path pClass = templateDir.Resolve("__P__.class");
//
//                 // Delete in reverse order to avoid issues with non-empty directories
//                 foreach (Path path in new Path[]
//                 {
//                     TEMPLATE_CLASSPATH_DIR,
//                     TEMPLATE_CLASSPATH_DIR.Resolve("org"),
//                     TEMPLATE_CLASSPATH_DIR.Resolve("org/openrewrite"),
//                     TEMPLATE_CLASSPATH_DIR.Resolve("org/openrewrite/csharp"),
//                     TEMPLATE_CLASSPATH_DIR.Resolve("org/openrewrite/csharp/internal"),
//                     templateDir, mClass, pClass
//                 })
//                 {
//                     path.ToFile().DeleteOnExit();
//                 }
//
//                 using (Stream inStream = CSharpTemplateParser.Class.GetClassLoader().GetResourceAsStream("org/openrewrite/csharp/internal/template/__M__.class"))
//                 {
//                     Debug.Assert(inStream != null);
//                     Files.Copy(inStream, mClass);
//                 }
//                 using (Stream inStream = CSharpTemplateParser.Class.GetClassLoader().GetResourceAsStream("org/openrewrite/csharp/internal/template/__P__.class"))
//                 {
//                     Debug.Assert(inStream != null);
//                     Files.Copy(inStream, pClass);
//                 }
//             }
//             catch (IOException e)
//             {
//                 throw new RuntimeException(e);
//             }
//         }
//         return TEMPLATE_CLASSPATH_DIR;
//     }
//
//     // Property instead of getter
//     public string Code { get; }
//
//     private readonly Action<string> onAfterVariableSubstitution;
//     private readonly CSharpTemplateParser templateParser;
//
//     private CSharpTemplate(bool contextSensitive, CSharpParser.Builder<?, ?> parser, string code, HashSet<string> imports,
//                          Action<string> onAfterVariableSubstitution, Action<string> onBeforeParseTemplate)
//     {
//         this(code, onAfterVariableSubstitution, new CSharpTemplateParser(contextSensitive, AugmentClasspath(parser), onAfterVariableSubstitution, onBeforeParseTemplate, imports));
//     }
//
//     private static CSharpParser.Builder<?, ?> AugmentClasspath(CSharpParser.Builder<?, ?> parserBuilder)
//     {
//         return parserBuilder.AddClasspathEntry(GetTemplateClasspathDir());
//     }
//
//     protected CSharpTemplate(string code, Action<string> onAfterVariableSubstitution, CSharpTemplateParser templateParser)
//     {
//         this.Code = code;
//         this.onAfterVariableSubstitution = onAfterVariableSubstitution;
//         this.templateParser = templateParser;
//     }
//
//     /// <inheritdoc/>
//     public override J2 Apply<J2>(Cursor scope, CSharpCoordinates coordinates, params object[] parameters)
//     {
//         if (!(scope.Value is J))
//         {
//             throw new ArgumentException("`scope` must point to a J instance.");
//         }
//
//         Substitutions substitutions = Substitutions(parameters);
//         string substitutedTemplate = substitutions.Substitute();
//         onAfterVariableSubstitution(substitutedTemplate);
//
//         //noinspection ConstantConditions
//         return (J2)new CSharpTemplateCSharpExtension(templateParser, substitutions, substitutedTemplate, coordinates)
//                 .GetMixin()
//                 .Visit(scope.Value, 0, scope.GetParentOrThrow());
//     }
//
//     protected Substitutions Substitutions(object[] parameters)
//     {
//         return new Substitutions(Code, parameters);
//     }
//
//     /// <summary>
//     /// Incubating since = "8.0.0"
//     /// </summary>
//     public static bool Matches(string template, Cursor cursor)
//     {
//         return CSharpTemplate.Builder(template).Build().Matches(cursor);
//     }
//
//     /// <summary>
//     /// Incubating since = "7.38.0"
//     /// </summary>
//     public bool Matches(Cursor cursor)
//     {
//         return Matcher(cursor).Find();
//     }
//
//     /// <summary>
//     /// Incubating since = "7.38.0"
//     /// </summary>
//     public Matcher Matcher(Cursor cursor)
//     {
//         return new Matcher(cursor);
//     }
//
//     /// <summary>
//     /// Incubating since = "7.38.0"
//     /// </summary>
//     public class Matcher
//     {
//         public Cursor Cursor { get; }
//
//         private CSharpTemplateSemanticallyEqual.TemplateMatchResult matchResult;
//
//         public Matcher(Cursor cursor)
//         {
//             this.Cursor = cursor;
//         }
//
//         public bool Find()
//         {
//             matchResult = CSharpTemplateSemanticallyEqual.MatchesTemplate(CSharpTemplate.this, Cursor);
//             return matchResult.IsMatch();
//         }
//
//         public J Parameter(int i)
//         {
//             return matchResult.GetMatchedParameters()[i];
//         }
//     }
//
//     public static J2 Apply<J2>(string template, Cursor scope, CSharpCoordinates coordinates, params object[] parameters) where J2 : J
//     {
//         return Builder(template).Build().Apply<J2>(scope, coordinates, parameters);
//     }
//
//     public static Builder Builder(string code)
//     {
//         return new Builder(code);
//     }
//
//     public class Builder
//     {
//         private readonly string code;
//         private readonly HashSet<string> imports = new HashSet<string>();
//
//         private bool contextSensitive;
//
//         private CSharpParser.Builder<?, ?> parser = org.openrewrite.csharp.CSharpParser.FromCSharpVersion();
//
//         private Action<string> onAfterVariableSubstitution = s => { };
//         private Action<string> onBeforeParseTemplate = s => { };
//
//         protected Builder(string code)
//         {
//             this.code = code.Trim();
//         }
//
//         /// <summary>
//         /// A template snippet is context-sensitive when it refers to the class, variables, methods, or other symbols
//         /// visible from its insertion scope. When a template is completely self-contained, it is not context-sensitive.
//         /// Context-free template snippets can be cached, since it does not matter where the resulting LST elements will
//         /// be inserted. Since the LST elements in a context-sensitive snippet vary depending on where they are inserted
//         /// the resulting LST elements cannot be reused between different insertion points and are not cached.
//         /// <para>
//         /// An example of a context-free snippet might be something like this, to be used as a local variable declaration:
//         /// <code>int i = 1</code>;
//         /// </para>
//         /// <para>
//         /// An example of a context-sensitive snippet is:
//         /// <code>int i = a</code>;
//         /// This cannot be made sense of without the surrounding scope which includes the declaration of "a".
//         /// </para>
//         /// </summary>
//         public Builder ContextSensitive()
//         {
//             this.contextSensitive = true;
//             return this;
//         }
//
//         public Builder Imports(params string[] fullyQualifiedTypeNames)
//         {
//             foreach (string typeName in fullyQualifiedTypeNames)
//             {
//                 ValidateImport(typeName);
//                 this.imports.Add("using " + typeName + ";\n");
//             }
//             return this;
//         }
//
//         public Builder StaticImports(params string[] fullyQualifiedMemberTypeNames)
//         {
//             foreach (string typeName in fullyQualifiedMemberTypeNames)
//             {
//                 ValidateImport(typeName);
//                 this.imports.Add("using static " + typeName + ";\n");
//             }
//             return this;
//         }
//
//         private void ValidateImport(string typeName)
//         {
//             if (string.IsNullOrWhiteSpace(typeName))
//             {
//                 throw new ArgumentException("Imports must not be blank");
//             }
//             else if (typeName.StartsWith("using ") || typeName.StartsWith("static "))
//             {
//                 throw new ArgumentException("Imports are expressed as fully-qualified names and should not include a \"using \" or \"static \" prefix");
//             }
//             else if (typeName.EndsWith(";") || typeName.EndsWith("\n"))
//             {
//                 throw new ArgumentException("Imports are expressed as fully-qualified names and should not include a suffixed terminator");
//             }
//         }
//
//         public Builder CSharpParser(CSharpParser.Builder<?, ?> parser)
//         {
//             this.parser = parser;
//             return this;
//         }
//
//         public Builder DoAfterVariableSubstitution(Action<string> afterVariableSubstitution)
//         {
//             this.onAfterVariableSubstitution = afterVariableSubstitution;
//             return this;
//         }
//
//         public Builder DoBeforeParseTemplate(Action<string> beforeParseTemplate)
//         {
//             this.onBeforeParseTemplate = beforeParseTemplate;
//             return this;
//         }
//
//         public CSharpTemplate Build()
//         {
//             return new CSharpTemplate(contextSensitive, parser.Clone(), code, imports,
//                     onAfterVariableSubstitution, onBeforeParseTemplate);
//         }
//     }
// }
