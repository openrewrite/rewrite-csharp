// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Microsoft.CodeAnalysis.Text;
// using System.Collections.Immutable;
// using System.Linq;
// using System.Text;
// using Microsoft.CodeAnalysis;
//
// [Generator]
// public class AsyncTreeVisitorGenerator : IIncrementalGenerator
// {
//     public void Initialize(IncrementalGeneratorInitializationContext context)
//     {
//         // Find all classes that inherit from TreeVisitor or its async equivalent
//         var treeVisitorClasses = context.SyntaxProvider
//             .CreateSyntaxProvider(
//                 predicate: static (s, _) => IsClassDeclaration(s),
//                 transform: static (ctx, _) => GetClassInfo(ctx))
//             .Where(static m => m is not null);
//
//         // Combine with compilation to get semantic information
//         var compilationAndClasses = context.CompilationProvider.Combine(treeVisitorClasses.Collect());
//
//         context.RegisterSourceOutput(compilationAndClasses,
//             static (spc, source) => Execute(source.Left, source.Right, spc));
//     }
//
//     private static bool IsClassDeclaration(SyntaxNode node)
//     {
//         return node is ClassDeclarationSyntax classDecl && 
//                classDecl.BaseList?.Types.Any() == true;
//     }
//
//     private static ClassInfo? GetClassInfo(GeneratorSyntaxContext context)
//     {
//         var classDecl = (ClassDeclarationSyntax)context.Node;
//         var semanticModel = context.SemanticModel;
//         
//         var classSymbol = semanticModel.GetDeclaredSymbol(classDecl);
//         if (classSymbol == null) return null;
//
//         // Check if it inherits from TreeVisitor (directly or indirectly)
//         if (!InheritsFromTreeVisitor(classSymbol))
//             return null;
//
//         return new ClassInfo(
//             classDecl,
//             classSymbol,
//             GetNamespaceName(classDecl),
//             GetDirectBaseClass(classSymbol)
//         );
//     }
//
//     private static bool InheritsFromTreeVisitor(INamedTypeSymbol classSymbol)
//     {
//         var current = classSymbol.BaseType;
//         while (current != null)
//         {
//             if (current.Name == "TreeVisitor" || current.Name == "TreeVisitorAsync")
//                 return true;
//             current = current.BaseType;
//         }
//         return false;
//     }
//
//     private static string GetDirectBaseClass(INamedTypeSymbol classSymbol)
//     {
//         return classSymbol.BaseType?.Name ?? string.Empty;
//     }
//
//     private static string GetNamespaceName(ClassDeclarationSyntax classDecl)
//     {
//         var namespaceDecl = classDecl.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
//         if (namespaceDecl != null)
//             return namespaceDecl.Name.ToString();
//             
//         var fileScopedNamespace = classDecl.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>();
//         if (fileScopedNamespace != null)
//             return fileScopedNamespace.Name.ToString();
//             
//         return string.Empty;
//     }
//
//     private static void Execute(Compilation compilation, ImmutableArray<ClassInfo?> classes, SourceProductionContext context)
//     {
//         foreach (var classInfo in classes)
//         {
//             if (classInfo == null) continue;
//
//             // Skip if already an async class
//             if (classInfo.ClassSymbol.Name.EndsWith("Async"))
//                 continue;
//
//             var source = GenerateAsyncClass(classInfo);
//             context.AddSource($"{classInfo.ClassSymbol.Name}Async.g.cs", SourceText.From(source, Encoding.UTF8));
//         }
//     }
//
//     private static string GenerateAsyncClass(ClassInfo classInfo)
//     {
//         var sb = new StringBuilder();
//         var className = classInfo.ClassSymbol.Name;
//         var asyncClassName = $"{className}Async";
//         
//         // Determine base class
//         var baseClass = classInfo.DirectBaseClass == "TreeVisitor" 
//             ? "TreeVisitorAsync" 
//             : $"{classInfo.DirectBaseClass}Async";
//
//         // Add using statements
//         sb.AppendLine("using System;");
//         sb.AppendLine("using System.Threading.Tasks;");
//         sb.AppendLine("using Rewrite.Core;");
//         sb.AppendLine();
//
//         // Add namespace
//         if (!string.IsNullOrEmpty(classInfo.NamespaceName))
//         {
//             sb.AppendLine($"namespace {classInfo.NamespaceName};");
//             sb.AppendLine();
//         }
//
//         // Get generic parameters from original class
//         var genericParams = GetGenericParameters(classInfo.ClassDeclaration);
//         var genericConstraints = GetGenericConstraints(classInfo.ClassDeclaration);
//
//         // Class declaration
//         sb.AppendLine($"public partial class {asyncClassName}{genericParams}");
//         if (!string.IsNullOrEmpty(baseClass))
//         {
//             sb.AppendLine($"    : {baseClass}{genericParams}");
//         }
//         sb.AppendLine("{");
//
//         // Generate async methods
//         foreach (var method in classInfo.ClassDeclaration.Members.OfType<MethodDeclarationSyntax>())
//         {
//             if (ShouldGenerateAsyncMethod(method))
//             {
//                 GenerateAsyncMethod(sb, method);
//             }
//         }
//
//         // Generate properties with transformed types
//         foreach (var property in classInfo.ClassDeclaration.Members.OfType<PropertyDeclarationSyntax>())
//         {
//             if (ShouldTransformProperty(property))
//             {
//                 GenerateTransformedProperty(sb, property);
//             }
//         }
//
//         // Generate fields with transformed types
//         foreach (var field in classInfo.ClassDeclaration.Members.OfType<FieldDeclarationSyntax>())
//         {
//             if (ShouldTransformField(field))
//             {
//                 GenerateTransformedField(sb, field);
//             }
//         }
//
//         // Add generic constraints if any
//         if (!string.IsNullOrEmpty(genericConstraints))
//         {
//             sb.AppendLine(genericConstraints);
//         }
//
//         sb.AppendLine("}");
//
//         return sb.ToString();
//     }
//
//     private static bool ShouldGenerateAsyncMethod(MethodDeclarationSyntax method)
//     {
//         // Generate async versions for public and protected virtual/override/abstract methods
//         var modifiers = method.Modifiers;
//         return (modifiers.Any(SyntaxKind.PublicKeyword) || modifiers.Any(SyntaxKind.ProtectedKeyword)) &&
//                (modifiers.Any(SyntaxKind.VirtualKeyword) || 
//                 modifiers.Any(SyntaxKind.OverrideKeyword) || 
//                 modifiers.Any(SyntaxKind.AbstractKeyword));
//     }
//
//     private static void GenerateAsyncMethod(StringBuilder sb, MethodDeclarationSyntax originalMethod)
//     {
//         var methodName = originalMethod.Identifier.ValueText;
//         var returnType = GetAsyncReturnType(originalMethod.ReturnType);
//         var parameters = originalMethod.ParameterList.ToString();
//         var genericParams = originalMethod.TypeParameterList?.ToString() ?? "";
//         var constraints = GetMethodConstraints(originalMethod);
//
//         // Method signature
//         sb.Append("    ");
//         foreach (var modifier in originalMethod.Modifiers)
//         {
//             if (modifier.IsKind(SyntaxKind.OverrideKeyword))
//                 sb.Append("override ");
//             else if (modifier.IsKind(SyntaxKind.VirtualKeyword))
//                 sb.Append("virtual ");
//             else if (modifier.IsKind(SyntaxKind.PublicKeyword))
//                 sb.Append("public ");
//             else if (modifier.IsKind(SyntaxKind.ProtectedKeyword))
//                 sb.Append("protected ");
//             else if (modifier.IsKind(SyntaxKind.AbstractKeyword))
//                 sb.Append("abstract ");
//         }
//
//         sb.AppendLine($"async {returnType} {methodName}{genericParams}{parameters}");
//         
//         if (!string.IsNullOrEmpty(constraints))
//         {
//             sb.AppendLine($"        {constraints}");
//         }
//
//         // Don't generate body for abstract methods
//         if (originalMethod.Modifiers.Any(SyntaxKind.AbstractKeyword))
//         {
//             sb.AppendLine("    ;");
//         }
//         else
//         {
//             sb.AppendLine("    {");
//             GenerateAsyncMethodBody(sb, originalMethod, methodName);
//             sb.AppendLine("    }");
//         }
//         
//         sb.AppendLine();
//     }
//
//     private static void GenerateAsyncMethodBody(StringBuilder sb, MethodDeclarationSyntax originalMethod, string methodName)
//     {
//         var parameters = string.Join(", ", originalMethod.ParameterList.Parameters.Select(p => p.Identifier.ValueText));
//         var returnType = originalMethod.ReturnType.ToString();
//         
//         if (originalMethod.Body != null)
//         {
//             // Transform the existing method body to async
//             var transformedBody = TransformMethodBodyToAsync(originalMethod.Body);
//             sb.AppendLine($"        {transformedBody}");
//         }
//         else if (originalMethod.Modifiers.Any(SyntaxKind.OverrideKeyword))
//         {
//             // For override methods without body, call base implementation
//             if (returnType == "void")
//             {
//                 sb.AppendLine($"        await base.{methodName}({parameters});");
//             }
//             else if (IsTaskType(returnType))
//             {
//                 sb.AppendLine($"        return await base.{methodName}({parameters});");
//             }
//             else
//             {
//                 sb.AppendLine($"        return await base.{methodName}({parameters});");
//             }
//         }
//         else
//         {
//             // For virtual methods, provide default implementation
//             if (returnType != "void" && !IsTaskType(returnType))
//             {
//                 sb.AppendLine($"        return await Task.FromResult(default({returnType}));");
//             }
//             else if (returnType == "void")
//             {
//                 sb.AppendLine("        await Task.CompletedTask;");
//             }
//             else
//             {
//                 sb.AppendLine("        await Task.CompletedTask;");
//             }
//         }
//     }
//
//     private static string TransformMethodBodyToAsync(BlockSyntax body)
//     {
//         var visitor = new AsyncMethodBodyTransformer();
//         var transformedBody = visitor.Visit(body);
//         return transformedBody?.ToString() ?? "{ await Task.CompletedTask; }";
//     }
//
//     private static string GetAsyncReturnType(TypeSyntax returnType)
//     {
//         var returnTypeText = returnType.ToString();
//         
//         if (returnTypeText == "void")
//             return "Task";
//         
//         if (IsTaskType(returnTypeText))
//             return returnTypeText;
//             
//         return $"Task<{returnTypeText}>";
//     }
//
//     private static bool IsTaskType(string typeName)
//     {
//         return typeName == "Task" || typeName.StartsWith("Task<") || 
//                typeName == "ValueTask" || typeName.StartsWith("ValueTask<");
//     }
//
//     private static string GetGenericParameters(ClassDeclarationSyntax classDecl)
//     {
//         return classDecl.TypeParameterList?.ToString() ?? "";
//     }
//
//     private static string GetGenericConstraints(ClassDeclarationSyntax classDecl)
//     {
//         if (classDecl.ConstraintClauses.Count == 0)
//             return "";
//             
//         var constraints = classDecl.ConstraintClauses.Select(c => c.ToString());
//         return string.Join(" ", constraints);
//     }
//
//     private static string GetMethodConstraints(MethodDeclarationSyntax method)
//     {
//         if (method.ConstraintClauses.Count == 0)
//             return "";
//             
//         var constraints = method.ConstraintClauses.Select(c => c.ToString());
//         return string.Join(" ", constraints);
//     }
//
//     private static bool ShouldTransformProperty(PropertyDeclarationSyntax property)
//     {
//         var typeName = property.Type.ToString();
//         return ContainsTreeVisitorType(typeName);
//     }
//
//     private static bool ShouldTransformField(FieldDeclarationSyntax field)
//     {
//         var typeName = field.Declaration.Type.ToString();
//         return ContainsTreeVisitorType(typeName);
//     }
//
//     private static bool ContainsTreeVisitorType(string typeName)
//     {
//         return typeName.Contains("Visitor") && !typeName.Contains("Async");
//     }
//
//     private static void GenerateTransformedProperty(StringBuilder sb, PropertyDeclarationSyntax property)
//     {
//         var transformer = new AsyncMethodBodyTransformer();
//         var transformedType = transformer.Visit(property.Type);
//         var transformedProperty = property.WithType((TypeSyntax)transformedType!);
//         
//         sb.AppendLine($"    {transformedProperty}");
//     }
//
//     private static void GenerateTransformedField(StringBuilder sb, FieldDeclarationSyntax field)
//     {
//         var transformer = new AsyncMethodBodyTransformer();
//         var transformedType = transformer.Visit(field.Declaration.Type);
//         var transformedDeclaration = field.Declaration.WithType((TypeSyntax)transformedType!);
//         var transformedField = field.WithDeclaration(transformedDeclaration);
//         
//         sb.AppendLine($"    {transformedField}");
//     }
//
//     private record ClassInfo(
//         ClassDeclarationSyntax ClassDeclaration,
//         INamedTypeSymbol ClassSymbol,
//         string NamespaceName,
//         string DirectBaseClass
//     );
// }
//
// public class AsyncMethodBodyTransformer : CSharpSyntaxRewriter
// {
//     private readonly HashSet<string> _treeVisitorMethods = new()
//     {
//         "Visit", "VisitAndCast", "VisitAndCastForward", "DefaultValue",
//         "Accept", "AcceptJava", "AcceptCSharp", "VisitCompilationUnit",
//         "VisitClassDeclaration", "VisitMethodDeclaration", "VisitBlock",
//         "VisitStatement", "VisitExpression", "VisitLiteral", "VisitIdentifier",
//         "VisitBinary", "VisitUnary", "VisitFieldAccess", "VisitMethodInvocation"
//     };
//
//     public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node)
//     {
//         var memberAccess = node.Expression as MemberAccessExpressionSyntax;
//         if (memberAccess != null)
//         {
//             var methodName = memberAccess.Name.Identifier.ValueText;
//             
//             // Check if this is a method call that should be async
//             if (ShouldBeAsyncCall(memberAccess, methodName))
//             {
//                 // Transform to await call
//                 var awaitExpression = SyntaxFactory.AwaitExpression(node);
//                 return awaitExpression;
//             }
//         }
//         
//         // Check for direct method calls (like Visit methods)
//         if (node.Expression is IdentifierNameSyntax identifier)
//         {
//             var methodName = identifier.Identifier.ValueText;
//             if (ShouldBeAsyncCall(null, methodName))
//             {
//                 var awaitExpression = SyntaxFactory.AwaitExpression(node);
//                 return awaitExpression;
//             }
//         }
//
//         return base.VisitInvocationExpression(node);
//     }
//
//     public override SyntaxNode? VisitReturnStatement(ReturnStatementSyntax node)
//     {
//         if (node.Expression is InvocationExpressionSyntax invocation)
//         {
//             var transformedInvocation = VisitInvocationExpression(invocation);
//             if (transformedInvocation is AwaitExpressionSyntax)
//             {
//                 return node.WithExpression((ExpressionSyntax)transformedInvocation);
//             }
//         }
//
//         return base.VisitReturnStatement(node);
//     }
//
//     public override SyntaxNode? VisitExpressionStatement(ExpressionStatementSyntax node)
//     {
//         if (node.Expression is InvocationExpressionSyntax invocation)
//         {
//             var transformedInvocation = VisitInvocationExpression(invocation);
//             if (transformedInvocation is AwaitExpressionSyntax)
//             {
//                 return node.WithExpression((ExpressionSyntax)transformedInvocation);
//             }
//         }
//         else if (node.Expression is AssignmentExpressionSyntax assignment)
//         {
//             var transformedAssignment = VisitAssignmentExpression(assignment);
//             if (transformedAssignment != assignment)
//             {
//                 return node.WithExpression((ExpressionSyntax)transformedAssignment!);
//             }
//         }
//
//         return base.VisitExpressionStatement(node);
//     }
//
//     public override SyntaxNode? VisitAssignmentExpression(AssignmentExpressionSyntax node)
//     {
//         if (node.Right is InvocationExpressionSyntax invocation)
//         {
//             var transformedInvocation = VisitInvocationExpression(invocation);
//             if (transformedInvocation is AwaitExpressionSyntax)
//             {
//                 return node.WithRight((ExpressionSyntax)transformedInvocation);
//             }
//         }
//
//         return base.VisitAssignmentExpression(node);
//     }
//
//     public override SyntaxNode? VisitVariableDeclaration(VariableDeclarationSyntax node)
//     {
//         var newVariables = new List<VariableDeclaratorSyntax>();
//         bool hasChanges = false;
//         
//         foreach (var variable in node.Variables)
//         {
//             if (variable.Initializer?.Value is InvocationExpressionSyntax invocation)
//             {
//                 var transformedInvocation = VisitInvocationExpression(invocation);
//                 if (transformedInvocation is AwaitExpressionSyntax)
//                 {
//                     var newInitializer = variable.Initializer.WithValue((ExpressionSyntax)transformedInvocation);
//                     newVariables.Add(variable.WithInitializer(newInitializer));
//                     hasChanges = true;
//                     continue;
//                 }
//             }
//             newVariables.Add(variable);
//         }
//
//         if (hasChanges)
//         {
//             return node.WithVariables(SyntaxFactory.SeparatedList(newVariables));
//         }
//
//         return base.VisitVariableDeclaration(node);
//     }
//
//     public override SyntaxNode? VisitConditionalExpression(ConditionalExpressionSyntax node)
//     {
//         // Handle ternary expressions with async calls
//         var whenTrue = node.WhenTrue;
//         var whenFalse = node.WhenFalse;
//         bool hasChanges = false;
//
//         if (whenTrue is InvocationExpressionSyntax trueInvocation)
//         {
//             var transformed = VisitInvocationExpression(trueInvocation);
//             if (transformed is AwaitExpressionSyntax)
//             {
//                 whenTrue = (ExpressionSyntax)transformed;
//                 hasChanges = true;
//             }
//         }
//
//         if (whenFalse is InvocationExpressionSyntax falseInvocation)
//         {
//             var transformed = VisitInvocationExpression(falseInvocation);
//             if (transformed is AwaitExpressionSyntax)
//             {
//                 whenFalse = (ExpressionSyntax)transformed;
//                 hasChanges = true;
//             }
//         }
//
//         if (hasChanges)
//         {
//             return node.WithWhenTrue(whenTrue).WithWhenFalse(whenFalse);
//         }
//
//         return base.VisitConditionalExpression(node);
//     }
//
//     private bool ShouldBeAsyncCall(MemberAccessExpressionSyntax? memberAccess, string methodName)
//     {
//         // Check if this is a TreeVisitor method that should be async
//         if (_treeVisitorMethods.Any(m => methodName.StartsWith(m)))
//         {
//             return true;
//         }
//
//         // Check if the target object has "Visitor" in its type or name
//         if (memberAccess?.Expression is IdentifierNameSyntax targetIdentifier)
//         {
//             var targetName = targetIdentifier.Identifier.ValueText;
//             if (targetName.Contains("Visitor") || targetName.Contains("visitor"))
//             {
//                 return true;
//             }
//         }
//
//         // Check for method calls on objects that end with "Async"
//         if (memberAccess?.Expression!.ToString().EndsWith("Async") == true)
//         {
//             return true;
//         }
//
//         // Check for specific method patterns
//         if (methodName.StartsWith("Visit") || methodName == "Accept" || 
//             methodName == "DefaultValue" || methodName.StartsWith("Transform"))
//         {
//             return true;
//         }
//
//         return false;
//     }
//
//     public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
//     {
//         var identifier = node.Identifier.ValueText;
//         
//         // Transform TreeVisitor type references to TreeVisitorAsync
//         if (identifier.EndsWith("Visitor") && !identifier.EndsWith("Async") && !identifier.EndsWith("VisitorAsync"))
//         {
//             var asyncIdentifier = $"{identifier}Async";
//             return node.WithIdentifier(SyntaxFactory.Identifier(asyncIdentifier));
//         }
//
//         return base.VisitIdentifierName(node);
//     }
//
//     public override SyntaxNode? VisitGenericName(GenericNameSyntax node)
//     {
//         var identifier = node.Identifier.ValueText;
//         
//         // Transform generic TreeVisitor type references
//         if (identifier.EndsWith("Visitor") && !identifier.EndsWith("Async"))
//         {
//             var asyncIdentifier = $"{identifier}Async";
//             return node.WithIdentifier(SyntaxFactory.Identifier(asyncIdentifier));
//         }
//
//         return base.VisitGenericName(node);
//     }
// }