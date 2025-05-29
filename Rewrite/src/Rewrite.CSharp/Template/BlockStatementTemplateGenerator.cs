// using System.Text;
// using Microsoft.CodeAnalysis;
// using Rewrite.RewriteCSharp;
// using Rewrite.RewriteCSharp.Tree;
// using Rewrite.RewriteJava;
// using Rewrite.Template;
//
// namespace Rewrite.RewriteCSharp;
//
// /// <summary>
// /// Generates a stub containing enough variable, method, and class scope
// /// for the insertion of a statement in any block scope.
// /// </summary>
// public class BlockStatementTemplateGenerator
// {
//     private static readonly string TEMPLATE_COMMENT = "__TEMPLATE__";
//     private static readonly string STOP_COMMENT = "__TEMPLATE_STOP__";
//
//     protected static readonly string TEMPLATE_INTERNAL_IMPORTS = """
//                                                                  using org.openrewrite.java.@internal.template.__M__;
//                                                                  using org.openrewrite.java.@internal.template.__P__;
//
//                                                                  """;
//
//     protected HashSet<string> _imports;
//     private readonly bool contextSensitive;
//
//     public BlockStatementTemplateGenerator(HashSet<string> imports, bool contextSensitive)
//     {
//         _imports = imports;
//         this.contextSensitive = contextSensitive;
//     }
//
//     public string Template(Cursor cursor, string template, Space.Location location, CSharpCoordinates.Mode mode)
//     {
//
//         var before = new StringBuilder();
//         var after = new StringBuilder();
//
//         // for CoordinateBuilder.MethodDeclaration#replaceBody()
//         if (cursor.TryGetValue<Cs.MethodDeclaration>(out var method) && location == Space.Location.BLOCK_PREFIX)
//         {
//             var m = method!.WithBody(null).WithAttributes(new List<Cs.AttributeList>()).WithPrefix(Space.EMPTY);
//             before.Insert(0, m.PrintTrimmed(cursor.GetParentOrThrow()).Trim() + '{');
//             after.Append('}');
//         }
//
//         if (contextSensitive)
//         {
//             ContextTemplate(Next(cursor), cursor.GetValue<J>()!, before, after, cursor.GetValue<J>()!, mode);
//         }
//         else
//         {
//             ContextFreeTemplate(Next(cursor), cursor.GetValue<J>()!, before, after);
//         }
//
//         return before.ToString().Trim() +
//                "\n/*" + TEMPLATE_COMMENT + "*/" +
//                template +
//                "/*" + STOP_COMMENT + "*/\n" +
//                after.ToString().Trim();
//
//     }
//
//     private class ListTemplatedVisitor<J2> : CSharpIsoVisitor<List<J2>> where J2 : J<J2>
//     {
//         private bool done = false;
//
//         private J.Block? blockEnclosingTemplateComment = null;
//
//         public override Cs.ClassDeclaration? VisitClassDeclaration(Cs.ClassDeclaration classDecl, List<J2> js)
//         {
//             if (Cursor.GetParentTreeCursor().Value is SourceFile &&
//                 (classDecl.Name.SimpleName.Equals("__P__") || classDecl.Name.SimpleName.Equals("__M__")))
//             {
//                 // don't visit the __P__ and __M__ classes declaring stubs
//                 return classDecl;
//             }
//
//             return base.VisitClassDeclaration(classDecl, js);
//         }
//
//
//         public override J.Block? VisitBlock(J.Block block, List<J2> js)
//         {
//             var b = base.VisitBlock(block, js);
//             if (b == blockEnclosingTemplateComment)
//             {
//                 done = true;
//             }
//
//             return b;
//         }
//
//         public override JRightPadded<T>? VisitRightPadded<T>(JRightPadded<T>? right, JRightPadded.Location loc, List<J2> js)
//         {
//             right = base.VisitRightPadded(right, loc, js);
//             //noinspection ConstantValue
//             if (right != null)
//             {
//                 foreach (var comment in right.After.Comments)
//                 {
//                     if (IsTemplateStopComment(comment))
//                     {
//                         done = true;
//                         break;
//                     }
//                 }
//             }
//
//             return right;
//         }
//
//         public override JLeftPadded<T>? VisitLeftPadded<T>(JLeftPadded<T>? left, JLeftPadded.Location loc, List<J2> js)
//         {
//             left = base.VisitLeftPadded(left, loc, js);
//             //noinspection ConstantValue
//             if (left != null)
//             {
//                 foreach (var comment in left.Before.Comments)
//                 {
//                     if (IsTemplateStopComment(comment))
//                     {
//                         done = true;
//                         break;
//                     }
//                 }
//             }
//
//             return left;
//         }
//
//         public override J? Visit(Core.Tree? tree, List<J2> js)
//         {
//             if (done)
//             {
//                 return tree as J;
//             }
//
//             if (tree is J2)
//             {
//                 var t = (J2)tree;
//
//                 if (blockEnclosingTemplateComment != null)
//                 {
//                     foreach (var comment in t.Comments)
//                     {
//                         if (IsTemplateStopComment(comment))
//                         {
//                             done = true;
//                             break;
//                         }
//                     }
//
//                     var trimmed = done ? default(J2) : (J2?)TemplatedTreeTrimmer.TrimTree(t);
//                     if (trimmed != null)
//                     {
//                         js.Add(trimmed);
//                     }
//
//                     return t;
//                 }
//
//                 var comments = t.Prefix.Comments;
//                 for (var i = 0; i < comments.Count; i++)
//                 {
//                     var comment = comments[i];
//                     if (comment is TextComment && ((TextComment)comment).Text.Equals(TEMPLATE_COMMENT))
//                     {
//                         blockEnclosingTemplateComment = Cursor.FirstEnclosing<J.Block>();
//                         //noinspection unchecked
//                         var trimmed = (J2?)TemplatedTreeTrimmer.TrimTree(t);
//                         if (!object.ReferenceEquals(t, trimmed))
//                         {
//                             done = true;
//                         }
//
//                         if (trimmed != null)
//                         {
//                             var skip = i + 1;
//                             var take = comments.Count - (i + 1) - skip;
//                             js.Add(trimmed.WithPrefix(trimmed.Prefix.WithComments(comments.Skip(i + 1).Take(take).ToList())));
//                         }
//
//                         return t;
//                     }
//                 }
//             }
//             // Catch any trees having a STOP_COMMENT that are not an instance of `expected`
//             else if (tree != null && js.Count > 0)
//             {
//                 //noinspection unchecked
//                 var trimmed = (J2?)TemplatedTreeTrimmer.TrimTree((J)tree);
//                 if (!object.ReferenceEquals(trimmed, tree))
//                 {
//                     done = true;
//                 }
//             }
//
//             return base.Visit(tree, js);
//         }
//
//         private bool IsTemplateStopComment(Comment comment)
//         {
//             return comment is TextComment && ((TextComment)comment).Text.Equals(STOP_COMMENT);
//         }
//     }
//
//     public IList<J2> ListTemplatedTrees<J2>(CSharpSourceFile cu) where J2 : J<J2>
//     {
//         var js = new List<J2>();
//
//         new ListTemplatedVisitor<J2>().Visit(cu, js);
//
//         return js;
//     }
//
//     protected void ContextFreeTemplate(Cursor cursor, J j, StringBuilder before, StringBuilder after)
//     {
//         if (j is J.Lambda)
//         {
//             throw new ArgumentException(
//                 "Templating a lambda requires a cursor so that it can be properly parsed and type-attributed. " +
//                 "Mark this template as context-sensitive by calling JavaTemplate.Builder#contextSensitive().");
//         }
//         else if (j is J.MemberReference)
//         {
//             throw new ArgumentException(
//                 "Templating a method reference requires a cursor so that it can be properly parsed and type-attributed. " +
//                 "Mark this template as context-sensitive by calling JavaTemplate.Builder#contextSensitive().");
//         }
//         else if (j is Cs.MethodInvocation methodInvocation)
//         {
//             before.Insert(0, "class Template {{\n");
//             var methodType = methodInvocation.MethodType;
//             if (methodType == null || methodType.ReturnType != JavaType.Primitive.Void)
//             {
//                 before.Append("object o = ");
//             }
//
//             after.Append(";\n}}");
//         }
//         else if (j is Expression && !(j is J.Assignment))
//         {
//             before.Insert(0, "class Template {\n");
//             before.Append("object o = ");
//             after.Append(";\n}");
//         }
//         else if ((j is Cs.MethodDeclaration || j is J.VariableDeclarations || j is J.Block || j is Cs.ClassDeclaration) &&
//                  cursor.Value is J.Block &&
//                  (cursor.Parent?.Value is Cs.ClassDeclaration || cursor.Parent?.Value is J.NewClass))
//         {
//             before.Insert(0, "class Template {\n");
//             after.Append("\n}");
//         }
//         else if (j is Cs.ClassDeclaration)
//         {
//             // While not impossible to handle, reaching this point is likely to be a mistake.
//             // Without context a class declaration can include no imports, package, or outer class.
//             // It is a rare class that is deliberately in the root package with no imports.
//             // In the more likely case omission of these things is unintentional, the resulting type metadata would be
//             // incorrect, and it would not be obvious to the recipe author why.
//             throw new ArgumentException(
//                 "Templating a class declaration requires context from which package declaration and imports may be reached. " +
//                 "Mark this template as context-sensitive by calling JavaTemplate.Builder#contextSensitive().");
//         }
//         else if (j is Statement && !(j is J.Import) && !(j is J.Package))
//         {
//             before.Insert(0, "class Template {{\n");
//             after.Append("\n}}");
//         }
//
//         before.Insert(0, TEMPLATE_INTERNAL_IMPORTS);
//         foreach (var anImport in this._imports)
//         {
//             before.Insert(0, anImport);
//         }
//     }
//
//     private void ContextTemplate(Cursor cursor, J prior, StringBuilder before, StringBuilder after, J insertionPoint, CSharpCoordinates.Mode mode)
//     {
//         var j = cursor.GetValue<J>();
//         if (j is CSharpSourceFile)
//         {
//             before.Insert(0, TEMPLATE_INTERNAL_IMPORTS);
//
//             var cu = (CSharpSourceFile)j;
//             foreach (var anImport in cu.Usings)
//             {
//                 before.Insert(0, anImport.WithPrefix(Space.EMPTY).PrintTrimmed(cursor) + ";\n");
//             }
//
//             foreach (var anImport in _imports)
//             {
//                 before.Insert(0, anImport);
//             }
//
//             var fileScopeNamespaceDeclaration = cu.Descendents().OfType<Cs.FileScopeNamespaceDeclaration>().FirstOrDefault();
//             if (fileScopeNamespaceDeclaration != null)
//             {
//                 before.Insert(0, fileScopeNamespaceDeclaration.WithPrefix(Space.EMPTY).PrintTrimmed(cursor) + ";\n");
//             }
//
//             var blockScopeNamespaceDeclaration = cu.Descendents().OfType<Cs.BlockScopeNamespaceDeclaration>().FirstOrDefault();
//             if (blockScopeNamespaceDeclaration != null)
//             {
//                 before.Insert(0, blockScopeNamespaceDeclaration.WithPrefix(Space.EMPTY).PrintTrimmed(cursor) + ";\n");
//             }
//
//             return;
//         }
//         else if (j is J.Block)
//         {
//             var parent = Next(cursor).Value;
//             if (parent is Cs.ClassDeclaration)
//             {
//                 var c = (Cs.ClassDeclaration)parent;
//                 ClassDeclaration(prior, c, before, after, cursor, mode);
//             }
//             else if (parent is Cs.MethodDeclaration)
//             {
//                 var m = (Cs.MethodDeclaration)parent;
//
//                 // variable declarations up to the point of insertion
//                 var body = m.Body as J.Block;
//                 if (body != null)
//                 {
//                     AddLeadingVariableDeclarations(cursor, prior, body, before, insertionPoint);
//                 }
//
//                 if (m.ReturnTypeExpression.Type != null && JavaType.Primitive.Void != m.ReturnTypeExpression.Type)
//                 {
//                     before.Insert(0, "if(true) {");
//                     after.Append("}\nreturn ")
//                         .Append(ValueOfType(m.ReturnTypeExpression.Type))
//                         .Append(";\n");
//                 }
//
//                 before.Insert(0, m.WithBody(null)
//                     .WithAttributes([])
//                     .WithPrefix(Space.EMPTY)
//                     .PrintTrimmed(cursor).Trim() + '{');
//             }
//             else
//             {
//                 var b = (J.Block)j;
//
//                 // variable declarations up to the point of insertion
//                 AddLeadingVariableDeclarations(cursor, prior, b, before, insertionPoint);
//
//                 before.Insert(0, "{\n");
//             }
//
//             if (ReferenceEquals(prior, insertionPoint) && prior is Expression)
//             {
//                 // the template represents an expression, so we need to wrap it in a statement
//                 after.Append(';');
//             }
//
//             after.Append('}');
//         }
//         else if (j is J.Annotation)
//         {
//             var annotation = (J.Annotation)j;
//             var arg = annotation.Arguments?.Where(a => ReferenceEquals(a, prior)).FirstOrDefault();
//             if (arg != null)
//             {
//                 var beforeBuffer = new StringBuilder();
//                 var name = annotation.Type is JavaType.Class ? ((JavaType.Class)annotation.Type).FullyQualifiedName :
//                     annotation.Type is JavaType.FullyQualified ? ((JavaType.FullyQualified)annotation.Type).FullyQualifiedName :
//                     annotation.SimpleName;
//                 beforeBuffer.Append('[').Append(name).Append('(');
//                 before.Insert(0, beforeBuffer);
//                 after.Append(")]").Append('\n');
//
//                 var parent = Next(cursor).GetValue<J>();
//                 if (parent is Cs.ClassDeclaration classDeclaration)
//                 {
//                     after.Append(classDeclaration.WithBody(null).WithPrefix(Space.EMPTY).PrintTrimmed(cursor).Trim()).Append("{}");
//                 }
//                 else if (parent is Cs.MethodDeclaration)
//                 {
//                     var md = (Cs.MethodDeclaration)parent;
//                     after.Append(md.WithBody(null)
//                         .WithPrefix(Space.EMPTY)
//                         .PrintTrimmed(cursor).Trim()).Append("{}");
//                 }
//                 // TODO cover more cases where annotations can appear, ideally not by "inlining" template for parent
//             }
//         }
//         
//         else if (j is J.DoWhileLoop)
//         {
//             var dw = (J.DoWhileLoop)j;
//             if (ReferToSameElement(prior, dw.WhileCondition))
//             {
//                 before.Insert(0, "object __b" + cursor.Count() + "__ =");
//                 after.Append(";");
//             }
//         }
//         else if (j is J.NewArray)
//         {
//             var n = (J.NewArray)j;
//             if (n.Initializer != null && n.Initializer.Any(arg => ReferToSameElement(prior, arg)))
//             {
//                 before.Insert(0, n.WithInitializer(null).PrintTrimmed(cursor) + "{\n");
//                 after.Append("\n}");
//             }
//             else
//             {
//                 // no initializer
//                 before.Insert(0, "__M__.any(");
//                 after.Append(");");
//             }
//         }
//         else if (j is J.NewClass)
//         {
//             var n = (J.NewClass)j;
//             string newClassString;
//             var constructorTypeClass = n.ConstructorType != null ? n.ConstructorType?.ReturnType as JavaType.Class : null;
//             var isEnum = constructorTypeClass != null && JavaType.FullyQualified.TypeKind.Enum == constructorTypeClass.Kind;
//             if (n.Clazz != null)
//             {
//                 newClassString = "new " + n.Clazz.PrintTrimmed(cursor);
//             }
//             else if (constructorTypeClass != null)
//             {
//                 // enum definitions with anonymous class initializers or a J.NewClass with a null clazz and valid constructor type.
//                 newClassString = isEnum ? "" : "new " + constructorTypeClass.FullyQualifiedName;
//             }
//             else
//             {
//                 throw new InvalidOperationException("Unable to template a J.NewClass instance having a null clazz and constructor type.");
//             }
//
//             if (n.Arguments.Any(arg => ReferToSameElement(prior, arg)))
//             {
//                 var beforeSegments = new StringBuilder();
//                 var afterSegments = new StringBuilder();
//                 beforeSegments.Append(newClassString).Append("(");
//                 var priorFound = false;
//                 foreach (var arg in n.Arguments)
//                 {
//                     if (!priorFound)
//                     {
//                         if (ReferToSameElement(prior, arg))
//                         {
//                             priorFound = true;
//                             continue;
//                         }
//
//                         beforeSegments.Append(ValueOfType(arg.Type)).Append(",");
//                     }
//                     else
//                     {
//                         afterSegments.Append(",/*" + STOP_COMMENT + "*/").Append(ValueOfType(arg.Type));
//                     }
//                 }
//
//                 afterSegments.Append(")");
//                 if (priorFound && !afterSegments.ToString().Contains(STOP_COMMENT))
//                 {
//                     if (isEnum)
//                     {
//                         afterSegments.Append(";");
//                     }
//
//                     afterSegments.Append("/*" + STOP_COMMENT + "*/");
//                 }
//
//                 before.Insert(0, beforeSegments);
//                 after.Append(afterSegments);
//                 if (Next(cursor).Value is J.Block)
//                 {
//                     after.Append(";");
//                 }
//             }
//             else
//             {
//                 n = n.WithBody(null).WithPrefix(Space.EMPTY);
//                 before.Insert(0, n.PrintTrimmed(cursor.GetParentOrThrow()).Trim());
//                 if (!(Next(cursor).Value is MethodCall))
//                 {
//                     after.Append(';');
//                 }
//             }
//         }
//         else if (j is J.ForLoop.Control)
//         {
//             var c = (J.ForLoop.Control)j;
//             if (ReferToSameElement(prior, c.Condition))
//             {
//                 before.Insert(0, "for (" + c.Init[0].PrintTrimmed(cursor).Trim() + ";");
//                 after.Append(";) {}");
//             }
//         }
//         else if (j is J.ForLoop)
//         {
//             var f = (J.ForLoop)j;
//             if (ReferToSameElement(prior, f.Body))
//             {
//                 InsertControlWithBlock(f.Body, before, after, () => before.Insert(0,
//                     f.WithBody(new J.Block(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY, JRightPadded.Create(false), [], Space.EMPTY))
//                         .WithPrefix(Space.EMPTY)
//                         .WithLoopControl(f.LoopControl.WithCondition(new J.Empty(Core.Tree.RandomId(), Space.EMPTY, Markers.EMPTY)).WithUpdate([]))
//                         .PrintTrimmed(cursor).Trim()));
//             }
//         }
//         else if (j is J.ForEachLoop.Control)
//         {
//             var c = (J.ForEachLoop.Control)j;
//             if (ReferToSameElement(prior, c.Variable))
//             {
//                 after.Append(" = /*" + STOP_COMMENT + "*/").Append(c.Iterable.PrintTrimmed(cursor));
//             }
//             else if (ReferToSameElement(prior, c.Iterable))
//             {
//                 before.Insert(0, "object __b" + cursor.Count() + "__ =");
//                 after.Append(";");
//             }
//         }
//         else if (j is J.ForEachLoop)
//         {
//             var f = (J.ForEachLoop)j;
//             if (!ReferToSameElement(prior, f.LoopControl))
//             {
//                 InsertControlWithBlock(f.Body, before, after, () => before.Insert(0,
//                     f.WithBody(J.Empty.Create()).WithPrefix(Space.EMPTY).PrintTrimmed(cursor).Trim()));
//             }
//         }
//         else if (j is J.Lambda)
//         {
//             var l = (J.Lambda)j;
//             if (l.Body is Expression)
//             {
//                 before.Insert(0, "return ");
//                 after.Append(";");
//             }
//
//             before.Insert(0, l.WithBody(J.Empty.Create()).WithPrefix(Space.EMPTY).PrintTrimmed(cursor.GetParentOrThrow()).Trim() + "{ if(true) {");
//
//             after.Append("}\n");
//             var mt = FindSingleAbstractMethod(l.Type);
//             if (mt == null)
//             {
//                 // Missing type information, but usually the Java compiler can soldier on anyway
//                 after.Append("return null;\n");
//             }
//             else if (mt.ReturnType != JavaType.Primitive.Void)
//             {
//                 after.Append("return ").Append(ValueOfType(mt.ReturnType)).Append(";\n");
//             }
//
//             after.Append("}");
//             if (Next(cursor).Value is J.Block)
//             {
//                 after.Append(";");
//             }
//         }
//         else if (j is J.VariableDeclarations)
//         {
//             if (prior is J.Annotation)
//             {
//                 after.Append(Variable((J.VariableDeclarations)j, false, cursor))
//                     .Append('=')
//                     .Append(ValueOfType(((J.VariableDeclarations)j).Type));
//             }
//             else
//             {
//                 before.Insert(0, Variable((J.VariableDeclarations)j, false, cursor) + '=');
//             }
//
//             after.Append(";");
//         }
//         else if (j is Cs.MethodInvocation)
//         {
//             // If prior is an argument, wrap in __M__.any(prior)
//             // If prior is a type parameter, wrap in __M__.anyT<prior>()
//             // For anything else, ignore the invocation
//             var m = (Cs.MethodInvocation)j;
//             var firstEnclosing = cursor.GetParentOrThrow().FirstEnclosing<J>();
//             if (m.Arguments.Any(arg => ReferToSameElement(prior, arg)))
//             {
//                 before.Insert(0, "__M__.any(");
//                 if (firstEnclosing is J.Block || firstEnclosing is J.Case ||
//                     firstEnclosing is J.If || firstEnclosing is J.If.Else)
//                 {
//                     after.Append(");");
//                 }
//                 else
//                 {
//                     after.Append(")");
//                 }
//             }
//             else if (m.TypeParameters != null && m.TypeParameters.Any(tp => ReferToSameElement(prior, tp)))
//             {
//                 before.Insert(0, "__M__.anyT<");
//                 if (firstEnclosing is J.Block || firstEnclosing is J.Case)
//                 {
//                     after.Append(">();");
//                 }
//                 else
//                 {
//                     after.Append(">()");
//                 }
//             }
//             else if (m.Select == prior)
//             {
//                 var comments = new List<Comment>(1);
//                 comments.Add(new TextComment(true, STOP_COMMENT, "", Markers.EMPTY));
//                 after.Append(".").Append(m.WithSelect(null).WithComments(comments).PrintTrimmed(cursor.GetParentOrThrow()));
//                 if (firstEnclosing is J.Block || firstEnclosing is J.Case)
//                 {
//                     after.Append(";");
//                 }
//             }
//         }
//         else if (j is J.Return)
//         {
//             before.Insert(0, "return ");
//             after.Append(";");
//         }
//         else if (j is J.Throw)
//         {
//             before.Insert(0, "throw ");
//             after.Append(";");
//         }
//         else if (j?.GetType().GetGenericTypeDefinition() == typeof(J.Parentheses<>))
//         {
//             before.Insert(0, '(');
//             after.Append(')');
//         }
//         else if (j is J.If)
//         {
//             var iff = (J.If)j;
//             if (ReferToSameElement(prior, iff.IfCondition))
//             {
//                 string condition = PatternVariables.SimplifiedPatternVariableCondition(iff.IfCondition.Tree, insertionPoint);
//                 int toReplaceIdx;
//                 if (condition != null && (toReplaceIdx = condition.IndexOf('§')) != -1)
//                 {
//                     before.Insert(0, "if (" + condition.Substring(0, toReplaceIdx) + '(');
//                     after.Append(')').Append(condition.Substring(toReplaceIdx + 1)).Append(") {}");
//                 }
//                 else
//                 {
//                     before.Insert(0, "object __b" + cursor.Count() + "__ =");
//                     after.Append(";");
//                 }
//             }
//             else
//             {
//                 string condition = PatternVariables.SimplifiedPatternVariableCondition(iff.IfCondition.Tree, insertionPoint);
//                 if (condition != null)
//                 {
//                     if (ReferToSameElement(prior, iff.ThenPart))
//                     {
//                         InsertControlWithBlock(iff.ThenPart, before, after, () =>
//                             before.Insert(0, "if (" + condition + ") "));
//                     }
//                     else if (ReferToSameElement(prior, iff.ElsePart))
//                     {
//                         InsertControlWithBlock(iff.ElsePart!.Body, before, after, () =>
//                             before.Insert(0, "if (" + condition + ") {} else "));
//                     }
//                 }
//             }
//         }
//         else if (j is J.Ternary)
//         {
//             var ternary = (J.Ternary)j;
//             string condition = PatternVariables.SimplifiedPatternVariableCondition(ternary.Condition, insertionPoint);
//             if (condition != null)
//             {
//                 if (ReferToSameElement(prior, ternary.Condition))
//                 {
//                     var splitIdx = condition.IndexOf('§');
//                     before.Insert(0, condition.Substring(0, splitIdx) + '(');
//                     after.Append(')').Append(condition.Substring(splitIdx + 1))
//                         .Append(" ? ").Append(ternary.TruePart.PrintTrimmed(cursor).Trim())
//                         .Append(" : ").Append(ternary.FalsePart.PrintTrimmed(cursor).Trim());
//                 }
//                 else if (ReferToSameElement(prior, ternary.TruePart))
//                 {
//                     before.Insert(0, (condition == null ? "true" : condition) + " ? ");
//                     after.Append(" : ").Append(ternary.FalsePart.PrintTrimmed(cursor).Trim());
//                 }
//                 else if (ReferToSameElement(prior, ternary.FalsePart))
//                 {
//                     before.Insert(0, (condition == null ? "true" : condition) + " ? " + ternary.TruePart.PrintTrimmed(cursor).Trim() + " : ");
//                 }
//             }
//         }
//         else if (j is J.WhileLoop)
//         {
//             var wl = (J.WhileLoop)j;
//             if (ReferToSameElement(prior, wl.Condition))
//             {
//                 before.Insert(0, "object __b" + cursor.Count() + "__ =");
//                 after.Append(";");
//             }
//         }
//         else if (j is J.Assignment)
//         {
//             var @as = (J.Assignment)j;
//             if (ReferToSameElement(prior, @as.Expression))
//             {
//                 before.Insert(0, @as.Variable + " = ");
//                 var parent = Next(cursor).Value;
//                 if (!(parent is J.Annotation))
//                 {
//                     after.Append(";");
//                 }
//             }
//         }
//         else if (j is J.AssignmentOperation)
//         {
//             var as_ = (J.AssignmentOperation)j;
//             if (ReferToSameElement(prior, as_.Assignment))
//             {
//                 before.Insert(0, "object __b" + cursor.Count() + "__ = ");
//                 after.Append(";");
//             }
//         }
//         else if (j is J.EnumValue)
//         {
//             var ev = (J.EnumValue)j;
//             before.Insert(0, ev.Name);
//         }
//         else if (j is J.EnumValueSet)
//         {
//             after.Append(";");
//         }
//         else if (j is J.Case)
//         {
//             after.Append(";");
//         }
//
//         ContextTemplate(Next(cursor), j, before, after, insertionPoint, CSharpCoordinates.Mode.REPLACEMENT);
//     }
//
//     private void AddLeadingVariableDeclarations(Cursor cursor, J current, J.Block containingBlock, StringBuilder before, J insertionPoint)
//     {
//         for (var index = 0; index < containingBlock.Statements.Count; index++)
//         {
//             var statement = containingBlock.Statements[index];
//             if (ReferToSameElement(current, statement))
//             {
//                 break;
//             }
//
//             if (statement is J.Label label)
//             {
//                 statement = label.Statement;
//             }
//
//             if (statement is J.VariableDeclarations varDecl)
//             {
//                 before.Insert(0, "\n" +
//                                  Variable(varDecl, true, cursor) +
//                                  ";\n");
//             }
//             else if (statement is J.If iff)
//             {
//                 string condition = PatternVariables.SimplifiedPatternVariableCondition(iff.IfCondition.Tree, insertionPoint);
//                 if (condition != null)
//                 {
//                     bool thenNeverCompletesNormally = PatternVariables.NeverCompletesNormally(iff.ThenPart);
//                     var elseNeverCompletesNormally = iff.ElsePart != null && PatternVariables.NeverCompletesNormally(iff.ElsePart.Body);
//                     if (thenNeverCompletesNormally || elseNeverCompletesNormally)
//                     {
//                         var ifStatement = new StringBuilder("if (").Append(condition).Append(") {");
//                         ifStatement.Append(thenNeverCompletesNormally ? " throw new RuntimeException(); }" : " }");
//                         ifStatement.Append(elseNeverCompletesNormally ? " else { throw new RuntimeException(); }" : " else { }");
//                         before.Insert(0, ifStatement);
//                     }
//                 }
//             }
//         }
//     }
//
//     private void InsertControlWithBlock(J body, StringBuilder before, StringBuilder after, Action insertion)
//     {
//         if (!(body is J.Block))
//         {
//             before.Insert(0, "{");
//         }
//
//         insertion();
//         if (!(body is J.Block))
//         {
//             after.Append("}");
//         }
//     }
//
//     private void ClassDeclaration(J? prior, Cs.ClassDeclaration cd, StringBuilder before, StringBuilder after, Cursor cursor, CSharpCoordinates.Mode mode)
//     {
//         var beforeBuffer = prior == null ? null : new StringBuilder();
//         var appendBuffer = prior == null ? after : beforeBuffer!;
//
//         appendBuffer.Append(cd.WithBody(null).WithAttributeList([]).WithPrefix(Space.EMPTY).PrintTrimmed(cursor).Trim()).Append('{');
//
//         var statements = cd.Body?.Statements ?? [];
//         foreach (var statement in statements)
//         {
//             if (ReferToSameElement(statement, prior))
//             {
//                 if (mode != CSharpCoordinates.Mode.AFTER)
//                 {
//                     appendBuffer = after;
//                     if (mode == CSharpCoordinates.Mode.REPLACEMENT)
//                     {
//                         continue;
//                     }
//                 }
//             }
//
//             if (statement is J.EnumValueSet enumValues)
//             {
//                 var enumStr = string.Join(',', enumValues.Enums.Select(x => x.Name.SimpleName));
//
//                 appendBuffer.Append(enumStr).Append(";\n");
//             }
//             else if (statement is J.VariableDeclarations varDecl)
//             {
//                 var variable = Variable(varDecl, false, cursor);
//                 appendBuffer.Append(variable).Append(";\n");
//             }
//             else if (statement is Cs.MethodDeclaration methodDecl)
//             {
//                 var m = Method(methodDecl, cursor);
//                 appendBuffer.Append(m);
//             }
//             else if (statement is Cs.ClassDeclaration classDecl)
//             {
//                 // this is a sibling class. we need declarations for all variables and methods.
//                 // setting prior to null will cause them all to be written.
//                 ClassDeclaration(null, classDecl, before, appendBuffer, cursor, CSharpCoordinates.Mode.REPLACEMENT);
//                 appendBuffer.Append('}');
//             }
//         }
//
//         if (beforeBuffer != null)
//         {
//             before.Insert(0, beforeBuffer);
//         }
//     }
//
//     private string Method(Cs.MethodDeclaration method, Cursor cursor)
//     {
//         if (method.IsAbstract)
//         {
//             return "\n" + method.WithPrefix(Space.EMPTY).PrintTrimmed(cursor).Trim() + ";\n";
//         }
//
//         var methodBuilder = new StringBuilder("\n");
//         var m = method.WithBody(null).WithAttributes([]).WithPrefix(Space.EMPTY);
//         methodBuilder.Append(m.PrintTrimmed(cursor).Trim()).Append('{');
//         if (method.ReturnTypeExpression != null && JavaType.Primitive.Void != method.ReturnTypeExpression.Type)
//         {
//             methodBuilder.Append("\nreturn ")
//                 .Append(ValueOfType(method.ReturnTypeExpression.Type))
//                 .Append(";\n");
//         }
//
//         methodBuilder.Append("}\n");
//         return methodBuilder.ToString();
//     }
//
//     private string Variable(J.VariableDeclarations variable, bool initializer, Cursor cursor)
//     {
//         var varBuilder = new StringBuilder();
//         foreach (var modifier in variable.Modifiers)
//         {
//             varBuilder.Append(modifier.ModifierType.ToString().ToLower()).Append(' ');
//         }
//
//         var variables = variable.Variables;
//         for (int i = 0, variablesSize = variables.Count; i < variablesSize; i++)
//         {
//             var nv = variables[i];
//             if (i == 0)
//             {
//                 if (variable.TypeExpression != null)
//                 {
//                     varBuilder.Append(variable.TypeExpression.WithPrefix(Space.EMPTY).PrintTrimmed(cursor));
//                 }
//
//                 if (nv.Type is JavaType.Array)
//                 {
//                     if (nv.Initializer is J.NewArray na && na.Dimensions.Count > 0)
//                     {
//                         foreach (var d in na.Dimensions)
//                         {
//                             varBuilder.Append("[]");
//                         }
//                     }
//                     else
//                     {
//                         varBuilder.Append("[]");
//                     }
//                 }
//
//                 varBuilder.Append(" ");
//             }
//
//             varBuilder.Append(nv.Name.SimpleName);
//
//             var type = nv.Type;
//             if (initializer && type != null)
//             {
//                 varBuilder.Append('=').Append(ValueOfType(type));
//             }
//
//             if (i < variables.Count - 1)
//             {
//                 varBuilder.Append(',');
//             }
//         }
//
//         return varBuilder.ToString();
//     }
//
//     private string ValueOfType(JavaType? type)
//     {
//         if (type is JavaType.Primitive primitive)
//         {
//             switch (primitive?.Kind)
//             {
//                 case JavaType.Primitive.PrimitiveType.Boolean:
//                     return "true";
//                 case JavaType.Primitive.PrimitiveType.Byte:
//                 case JavaType.Primitive.PrimitiveType.Char:
//                 case JavaType.Primitive.PrimitiveType.Int:
//                 case JavaType.Primitive.PrimitiveType.Double:
//                 case JavaType.Primitive.PrimitiveType.Float:
//                 case JavaType.Primitive.PrimitiveType.Long:
//                 case JavaType.Primitive.PrimitiveType.Short:
//                     return "0";
//                 case JavaType.Primitive.PrimitiveType.String:
//                 case JavaType.Primitive.PrimitiveType.Null:
//                     return "null";
//                 default:
//                     return "";
//             }
//         }
//
//         return "null";
//     }
//
//     private Cursor Next(Cursor c)
//     {
//         return c.GetParentTreeCursor();
//     }
//
//     private static bool ReferToSameElement(Core.Tree? t1, Core.Tree? t2)
//     {
//         return t1 == t2 || (t1 != null && t2 != null && t1.Id.Equals(t2.Id));
//     }
//
//     /// <summary>
//     /// Accepts a @FunctionalInterface and returns the single abstract method from it, or null if the single abstract
//     /// method cannot be found
//     /// </summary>
//     private static JavaType.Method? FindSingleAbstractMethod(JavaType? javaType)
//     {
//         if (javaType == null)
//         {
//             return null;
//         }
//
//         var fq = TypeUtils.AsFullyQualified(javaType);
//         if (fq == null)
//         {
//             return null;
//         }
//
//         return fq.Methods.FirstOrDefault(method => method.HasFlags(Flag.Abstract));
//     }
//
// // Visitor for removing any trees having or following the `STOP_COMMENT`
//     private static class TemplatedTreeTrimmer
//     {
//         public static J? TrimTree(J j)
//         {
//             var trimmed = new TemplatedTreeTrimmerVisitor().Visit(j, 0);
//             if (trimmed == null || trimmed.Markers.FindFirst<RemoveTreeMarker>() != null)
//             {
//                 return null;
//             }
//
//             return trimmed;
//         }
//
//         private class RemoveTreeMarker : Core.Marker.Marker, IEquatable<RemoveTreeMarker>
//         {
//             public Guid Id { get; }
//
//             public RemoveTreeMarker With(Guid id)
//             {
//                 return new RemoveTreeMarker(id);
//             }
//
//             public RemoveTreeMarker(Guid id)
//             {
//                 Id = id;
//             }
//
//             public bool Equals(RemoveTreeMarker? other)
//             {
//                 if (other is null) return false;
//                 if (ReferenceEquals(this, other)) return true;
//                 return Id.Equals(other.Id);
//             }
//
//             bool IEquatable<Core.Marker.Marker>.Equals(Core.Marker.Marker? other) => Equals(other);
//
//             public override bool Equals(object? obj)
//             {
//                 if (obj is null) return false;
//                 if (ReferenceEquals(this, obj)) return true;
//                 if (obj.GetType() != GetType()) return false;
//                 return Equals((RemoveTreeMarker)obj);
//             }
//
//             public override int GetHashCode()
//             {
//                 return Id.GetHashCode();
//             }
//         }
//
//         private class TemplatedTreeTrimmerVisitor : JavaVisitor<int>
//         {
//             private bool StopCommentExists(J? j)
//             {
//                 return j != null && StopCommentExists(j.Comments);
//             }
//
//             private static bool StopCommentExists(IList<Comment> comments)
//             {
//                 foreach (var comment in comments)
//                 {
//                     if (comment is TextComment textComment && textComment.Text.Equals(STOP_COMMENT))
//                     {
//                         return true;
//                     }
//                 }
//
//                 return false;
//             }
//
//             public override J? Visit(Core.Tree? tree, int value)
//             {
//                 var j = base.Visit(tree, value);
//                 if (StopCommentExists(j))
//                 {
//                     var parent = Cursor.Parent;
//                     if (parent == null || !(parent.Value is J.MethodInvocation))
//                     {
//                         return j?.WithMarkers(j.Markers.AddIfAbsent(new RemoveTreeMarker(Core.Tree.RandomId())));
//                     }
//                 }
//
//                 return j;
//             }
//
//             public override J? VisitMethodInvocation(J.MethodInvocation method, int value)
//             {
//                 var mi = (J.MethodInvocation)base.VisitMethodInvocation(method, value)!;
//                 if (StopCommentExists(mi.Name))
//                 {
//                     return mi.Select;
//                 }
//
//                 if (method.TypeParameters != null)
//                 {
//                     // For method chains return `select` if `STOP_COMMENT` is found before `typeParameters`
//                     if (StopCommentExists(mi.Padding.TypeParameters?.Before.Comments ?? []))
//                     {
//                         return mi.Select;
//                     }
//                 }
//
//                 return mi;
//             }
//
//             public override J? VisitVariableDeclarations(J.VariableDeclarations multiVariable, int value)
//             {
//                 var variables = multiVariable.Variables;
//                 foreach (var variable in variables)
//                 {
//                     J.VariableDeclarations.NamedVariable.PaddingHelper padding = variable.Padding;
//                     if (padding.Initializer != null && StopCommentExists(padding.Initializer.Before.Comments))
//                     {
//                         // Split the variable declarations at the variable with the `STOP_COMMENT` & trim off initializer
//                         IList<J.VariableDeclarations.NamedVariable> vars = variables.Take(variables.IndexOf(variable) + 1).ToList();
//                         return multiVariable.WithVariables(ListUtils.MapLast(vars, v => v.WithInitializer(null))!);
//                     }
//                 }
//
//                 return base.VisitVariableDeclarations(multiVariable, value);
//             }
//         }
//     }
// }