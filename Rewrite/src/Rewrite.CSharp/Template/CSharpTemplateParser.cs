// using System.Diagnostics;
// using System.Text;
// using Rewrite.RewriteCSharp.Tree;
//
// namespace Rewrite.RewriteCSharp;
//
// public class CSharpTemplateParser
// {
//     private static readonly PropertyPlaceholderHelper placeholderHelper = new PropertyPlaceholderHelper("#{", "}", null);
//
//     private static readonly string TEMPLATE_CACHE_MESSAGE_KEY = "__org.openrewrite.csharp.internal.template.CSharpTemplateParser.cache__";
//
//     private static readonly string PACKAGE_STUB = "namespace #{}; class $Template {}";
//     private static readonly string PARAMETER_STUB = "abstract class $Template { abstract void $template(#{}); }";
//     private static readonly string LAMBDA_PARAMETER_STUB = "object o = (#{}) => {};";
//     private static readonly string BASE_STUB = "class $Template : #{} {}";
//     private static readonly string IMPLEMENTS_STUB = "class $Template : #{} {}";
//     private static readonly string THROWS_STUB = "abstract class $Template { abstract void $template() throws #{}; }";
//     private static readonly string TYPE_PARAMS_STUB = "class $Template<#{}> {}";
//
//     private static readonly string SUBSTITUTED_ANNOTATION = "[System.ComponentModel.Description] public interface SubAnnotation { int Value { get; } }";
//
//     private readonly Parser.Builder parser;
//     private readonly Action<string> onAfterVariableSubstitution;
//     private readonly Action<string> onBeforeParseTemplate;
//     private readonly HashSet<string> imports;
//     private readonly bool contextSensitive;
//     private readonly BlockStatementTemplateGenerator statementTemplateGenerator;
//     private readonly AnnotationTemplateGenerator annotationTemplateGenerator;
//
//     public CSharpTemplateParser(bool contextSensitive, Parser.Builder parser, Action<string> onAfterVariableSubstitution, Action<string> onBeforeParseTemplate, HashSet<string> imports)
//     {
//         this.parser = parser;
//         this.onAfterVariableSubstitution = onAfterVariableSubstitution;
//         this.onBeforeParseTemplate = onBeforeParseTemplate;
//         this.imports = imports;
//         this.contextSensitive = contextSensitive;
//         this.statementTemplateGenerator = new BlockStatementTemplateGenerator(imports, contextSensitive);
//         this.annotationTemplateGenerator = new AnnotationTemplateGenerator(imports);
//     }
//
//     protected CSharpTemplateParser(Parser.Builder parser, Action<string> onAfterVariableSubstitution, Action<string> onBeforeParseTemplate, HashSet<string> imports, bool contextSensitive, BlockStatementTemplateGenerator statementTemplateGenerator, AnnotationTemplateGenerator annotationTemplateGenerator)
//     {
//         this.parser = parser;
//         this.onAfterVariableSubstitution = onAfterVariableSubstitution;
//         this.onBeforeParseTemplate = onBeforeParseTemplate;
//         this.imports = imports;
//         this.contextSensitive = contextSensitive;
//         this.statementTemplateGenerator = statementTemplateGenerator;
//         this.annotationTemplateGenerator = annotationTemplateGenerator;
//     }
//
//     public IList<Statement> ParseParameters(Cursor cursor, string template)
//     {
//         string stub = AddImports(Substitute(PARAMETER_STUB, template));
//         onBeforeParseTemplate(stub);
//         return Cache(cursor, stub, () =>
//         {
//             CSharpSourceFile cu = CompileTemplate(stub);
//             var m = cu.Descendents().OfType<Cs.MethodDeclaration>().First();
//             return m.Parameters;
//         });
//     }
//
//     public J.Lambda.Parameters ParseLambdaParameters(Cursor cursor, string template)
//     {
//         string stub = AddImports(Substitute(LAMBDA_PARAMETER_STUB, template));
//         onBeforeParseTemplate(stub);
//
//         return (J.Lambda.Parameters)Cache(cursor, stub, () =>
//         {
//             CSharpSourceFile cu = CompileTemplate(stub);
//
//             J.Lambda l = cu.Descendents().OfType<J.Lambda>().First();;
//             Debug.Assert(l != null);
//             return new List<J>() { l.Params };
//         })[0];
//     }
//
//     public J ParseExpression(Cursor cursor, string template, Space.Location location)
//     {
//         return CacheIfContextFree(cursor, new ContextFreeCacheKey(template, typeof(Expression), imports),
//                 tmpl => statementTemplateGenerator.Template(cursor, tmpl, location, CSharpCoordinates.Mode.REPLACEMENT),
//                 stub =>
//                 {
//                     onBeforeParseTemplate(stub);
//                     CSharpSourceFile cu = CompileTemplate(stub);
//                     return statementTemplateGenerator.ListTemplatedTrees(cu, typeof(Expression));
//                 })[0];
//     }
//
//     // public TypeTree ParseExtends(Cursor cursor, string template)
//     // {
//     //     string stub = AddImports(Substitute(BASE_STUB, template));
//     //     onBeforeParseTemplate(stub);
//     //
//     //     return (TypeTree)Cache(cursor, stub, () =>
//     //     {
//     //         CSharpSourceFile cu = CompileTemplate(stub);
//     //         TypeTree anExtends = cu.Descendents().OfType<Cs.ClassDeclaration>().First().Implementings;
//     //         Debug.Assert(anExtends != null);
//     //         return new List<J>() { anExtends };
//     //     })[0];
//     // }
//
//     public IList<TypeTree> ParseImplements(Cursor cursor, string template)
//     {
//         string stub = AddImports(Substitute(BASE_STUB, template));
//         onBeforeParseTemplate(stub);
//         return Cache(cursor, stub, () =>
//         {
//             CSharpSourceFile cu = CompileTemplate(stub);
//             IList<TypeTree> anImplements = cu.Descendents().OfType<Cs.ClassDeclaration>().First().Implementings ?? [];
//             Debug.Assert(anImplements != null);
//             return anImplements;
//         });
//     }
//     
//
//     public IList<Cs.TypeParameter> ParseTypeParameters(Cursor cursor, string template)
//     {
//         string stub = AddImports(Substitute(TYPE_PARAMS_STUB, template));
//         onBeforeParseTemplate(stub);
//         return Cache(cursor, stub, () =>
//         {
//             CSharpSourceFile cu = CompileTemplate(stub);
//             
//             IList<Cs.TypeParameter> tps = cu.Descendents().OfType<Cs.ClassDeclaration>().First().TypeParameters ?? [];
//             Debug.Assert(tps != null);
//             return tps;
//         });
//     }
//
//     public IList<T> ParseBlockStatements<T>(Cursor cursor, 
//                                         string template,
//                                         Space.Location location,
//                                         CSharpCoordinates.Mode mode) where T : J
//     {
//         return CacheIfContextFree(cursor,
//                 new ContextFreeCacheKey(template, typeof(T), imports),
//                 tmpl => statementTemplateGenerator.Template(cursor, tmpl, location, mode),
//                 stub =>
//                 {
//                     onBeforeParseTemplate(stub);
//                     CSharpSourceFile cu = CompileTemplate(stub);
//                     return statementTemplateGenerator.ListTemplatedTrees(cu, typeof(T));
//                 });
//     }
//
//     public J.MethodInvocation ParseMethod(Cursor cursor, string template, Space.Location location)
//     {
//         J.MethodInvocation method = cursor.GetValue<J.MethodInvocation>();
//         string methodWithReplacedNameAndArgs;
//         if (method.Select == null)
//         {
//             methodWithReplacedNameAndArgs = template;
//         }
//         else
//         {
//             methodWithReplacedNameAndArgs = method.Select.Print(cursor) + "." + template;
//         }
//         // TODO: The stub string includes the scoped elements of each original AST, and therefore is not a good
//         //       cache key. There are virtual no cases where a stub key will result in re-use. If we can come up with
//         //       a safe, reusable key, we can consider using the cache for block statements.
//         [Language("csharp")] string stub = statementTemplateGenerator.Template(cursor, methodWithReplacedNameAndArgs, location, CSharpCoordinates.Mode.REPLACEMENT);
//         onBeforeParseTemplate(stub);
//         CSharpSourceFile cu = CompileTemplate(stub);
//         return (J.MethodInvocation)statementTemplateGenerator
//                 .ListTemplatedTrees(cu, typeof(Statement))[0];
//     }
//
//     public J.MethodInvocation ParseMethodArguments(Cursor cursor, string template, Space.Location location)
//     {
//         J.MethodInvocation method = cursor.GetValue<J.MethodInvocation>();
//         string methodWithReplacementArgs = method.WithArguments(new List<Expression>()).PrintTrimmed(cursor.GetParentOrThrow())
//                 .Replace(")$", template + ")");
//         // TODO: The stub string includes the scoped elements of each original AST, and therefore is not a good
//         //       cache key. There are virtual no cases where a stub key will result in re-use. If we can come up with
//         //       a safe, reusable key, we can consider using the cache for block statements.
//         [Language("csharp")] string stub = statementTemplateGenerator.Template(cursor, methodWithReplacementArgs, location, CSharpCoordinates.Mode.REPLACEMENT);
//         onBeforeParseTemplate(stub);
//         CSharpSourceFile cu = CompileTemplate(stub);
//         return (J.MethodInvocation)statementTemplateGenerator
//                 .ListTemplatedTrees(cu, typeof(Statement))[0];
//     }
//
//     public IList<J.Annotation> ParseAnnotations(Cursor cursor, string template)
//     {
//         string cacheKey = AddImports(annotationTemplateGenerator.CacheKey(cursor, template));
//         return Cache(cursor, cacheKey, () =>
//         {
//             [Language("csharp")] string stub = annotationTemplateGenerator.Template(cursor, template);
//             onBeforeParseTemplate(stub);
//             CSharpSourceFile cu = CompileTemplate(stub);
//             return annotationTemplateGenerator.ListAnnotations(cu);
//         });
//     }
//
//     public Expression ParseNamespace(Cursor cursor, string template)
//     {
//         [Language("csharp")] string stub = Substitute(PACKAGE_STUB, template);
//         onBeforeParseTemplate(stub);
//
//         return (Expression)Cache(cursor, stub, () =>
//         {
//             CSharpSourceFile cu = CompileTemplate(stub);
//             Expression expression = cu.GetNamespaceDeclaration().Expression;
//             return new List<J>() { expression };
//         })[0];
//     }
//
//     private string Substitute(string stub, string template)
//     {
//         string beforeParse = placeholderHelper.ReplacePlaceholders(stub, k => template);
//         onAfterVariableSubstitution(beforeParse);
//         return beforeParse;
//     }
//
//     private string AddImports(string stub)
//     {
//         if (imports.Count > 0)
//         {
//             StringBuilder withImports = new StringBuilder();
//             foreach (string anImport in imports)
//             {
//                 withImports.Append(anImport);
//             }
//             withImports.Append(stub);
//             return withImports.ToString();
//         }
//         return stub;
//     }
//
//     private CSharpSourceFile CompileTemplate([Language("csharp")] string stub)
//     {
//         ExecutionContext ctx = new InMemoryExecutionContext();
//         ctx.PutMessage(CSharpParser.SKIP_SOURCE_SET_TYPE_GENERATION, true);
//         ctx.PutMessage(ExecutionContext.REQUIRE_PRINT_EQUALS_INPUT, false);
//         Parser cp = parser.Build();
//         return (stub.Contains("SubAnnotation") ?
//                 cp.Reset().Parse(ctx, stub, SUBSTITUTED_ANNOTATION) :
//                 cp.Reset().Parse(ctx, stub))
//                 .FirstOrDefault(s => s is CSharpSourceFile) // Filters out ParseErrors
//                 ?.Cast<CSharpSourceFile>()
//                 ?? throw new ArgumentException("Could not parse as C#");
//     }
//
//     /// <summary>
//     /// Return the result of parsing the stub.
//     /// Cache the LST elements parsed from stub only if the stub is context free.
//     /// 
//     /// For a stub to be context free nothing about its meaning can be changed by the context in which it is parsed.
//     /// For example, the statement `int i = 0;` is context free because it will always be parsed as a variable
//     /// The statement `i++;` cannot be context free because it cannot be parsed without a preceding declaration of i.
//     /// The statement `class A{}` is typically not context free because it
//     /// </summary>
//     /// <param name="cursor">indicates whether the stub is context free or not</param>
//     /// <param name="key">cache key for the stub</param>
//     /// <param name="stubMapper">maps the template to a stub</param>
//     /// <param name="treeMapper">supplies the LST elements produced from the stub</param>
//     /// <returns>result of parsing the stub into LST elements</returns>
//     private IList<T> CacheIfContextFree<T>(Cursor cursor, ContextFreeCacheKey key,
//                                         Func<string, string> stubMapper,
//                                         Func<string, IList<T>> treeMapper) where T : J
//     {
//         if (cursor.Parent == null)
//         {
//             throw new ArgumentException("Expecting `cursor` to have a parent element");
//         }
//         if (!contextSensitive)
//         {
//             return Cache<T>(cursor, key, () => treeMapper(stubMapper(key.Template)));
//         }
//         
//         return treeMapper(stubMapper(key.Template)).Cast<T>().ToList();
//     }
//
//     private IList<T> Cache<T>(Cursor cursor, object key, Func<IList<T>> ifAbsent) where T : J
//     {
//         IList<T> js = null;
//
//         Timer.Sample sample = Timer.Start();
//         Cursor root = cursor.GetRoot();
//         Dictionary<object, IList<T>> cache = root.GetMessage<Dictionary<object, IList<T>>>(TEMPLATE_CACHE_MESSAGE_KEY);
//         if (cache == null)
//         {
//             cache = new Dictionary<object, IList<T>>();
//             root.PutMessage(TEMPLATE_CACHE_MESSAGE_KEY, cache);
//         }
//         else
//         {
//             cache.TryGetValue(key, out js);
//         }
//
//         if (js == null)
//         {
//             js = ifAbsent().Cast<T>().ToList();
//             cache[key] = js;
//             sample.Stop(Timer.Builder("rewrite.template.cache").Tag("result", "miss")
//                     .Register(Metrics.GlobalRegistry));
//         }
//         else
//         {
//             sample.Stop(Timer.Builder("rewrite.template.cache").Tag("result", "hit")
//                     .Register(Metrics.GlobalRegistry));
//         }
//
//         return js.Select(j => (T)new RandomizeIdVisitor<int>().Visit(j, 0)).ToList();
//     }
//
//     private class ContextFreeCacheKey
//     {
//         public string Template { get; }
//         public Type Expected { get; }
//         public HashSet<string> Imports { get; }
//
//         public ContextFreeCacheKey(string template, Type expected, HashSet<string> imports)
//         {
//             Template = template;
//             Expected = expected;
//             Imports = imports;
//         }
//
//         public override bool Equals(object obj)
//         {
//             if (obj is ContextFreeCacheKey other)
//             {
//                 return Template == other.Template &&
//                        Expected == other.Expected &&
//                        Imports.SetEquals(other.Imports);
//             }
//             return false;
//         }
//
//         public override int GetHashCode()
//         {
//             int hash = 17;
//             hash = hash * 31 + Template.GetHashCode();
//             hash = hash * 31 + Expected.GetHashCode();
//             foreach (var import in Imports.OrderBy(i => i))
//             {
//                 hash = hash * 31 + import.GetHashCode();
//             }
//             return hash;
//         }
//     }
// }