// using Rewrite.Core.Config;
//
// namespace Rewrite.RewriteCSharp;
//
// public class CSharpTemplate : SourceTemplate<J, CSharpCoordinates>
// {
//     private static Path TEMPLATE_CLASSPATH_DIR;
//
//     public string Code { get; }
//
//     private readonly Action<string> onAfterVariableSubstitution;
//     private readonly CSharpTemplateParser templateParser;
//
//     private CSharpTemplate(
//         bool contextSensitive, 
//         CSharpParser.Builder parser, 
//         string template, 
//         HashSet<string> imports, 
//         Action<string> onAfterVariableSubstitution, 
//         Action<string> onBeforeParseTemplate)
//         : this(template, onAfterVariableSubstitution, new CSharpTemplateParser(contextSensitive, AugmentClasspath(parser), onAfterVariableSubstitution, onBeforeParseTemplate, imports))
//     {
//         
//     }
//
//     protected CSharpTemplate(string code, Action<string> onAfterVariableSubstitution, CSharpTemplateParser templateParser)
//     {
//         this.Code = code;
//         this.onAfterVariableSubstitution = onAfterVariableSubstitution;
//         this.templateParser = templateParser;
//     }
//
//     private static CSharpParser.Builder AugmentClasspath(CSharpParser.Builder parserBuilder)
//     {
//         return parserBuilder.AddClasspathEntry(GetTemplateClasspathDir());
//     }
//
//     /// <inheritdoc/>
//     public J2 Apply<J2>(Cursor scope, CSharpCoordinates coordinates, params object[] parameters) where J2 : J
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
//         return Create(template).Build().Apply<J2>(scope, coordinates, parameters);
//     }
//
//     public static Builder Create(string code)
//     {
//         return new Builder(code);
//     }
//
//     public class Builder
//     {
//         private readonly string code;
//         private readonly HashSet<string> _usings = new HashSet<string>();
//
//         private bool contextSensitive;
//
//         private CSharpParser.Builder parser = new();
//
//         private Action<string> onAfterVariableSubstitution = s => { };
//         private Action<string> onBeforeParseTemplate = s => { };
//
//         internal Builder(string code)
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
//         public Builder AddUsing(string namespaceName)
//         {
//             this._usings.Add(namespaceName);
//             return this;
//         }
//
//
//         public Builder CSharpParser(CSharpParser.Builder parser)
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
//             return new CSharpTemplate(contextSensitive, parser, code, _usings, onAfterVariableSubstitution, onBeforeParseTemplate);
//         }
//     }
// }
