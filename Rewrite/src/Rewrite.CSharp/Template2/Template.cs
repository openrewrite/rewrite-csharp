using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Format;
using Rewrite.RewriteJava.Tree;


namespace Rewrite.RewriteCSharp.Template2
{
    using Expression = System.Linq.Expressions.Expression;

    public class Template
    {
        private readonly ILogger _logger;
        private readonly InterpolatedTemplate _template;
        private readonly ISet<string> _usings;

        public Template(ILogger<Template> logger, InterpolatedTemplate template) : this(logger, template, new HashSet<string>())
        {
        }

        public Template(ILogger<Template> logger, InterpolatedTemplate template, ISet<string> usings)
        {
            _logger = logger;
            _template = template;
            _usings = usings;
            _logger.LogDebug($"Interpolated template: \n {template}");
        }



        public T? Apply<T>(Cursor scope, CSharpCoordinates coordinates) where T : J
        {
            var result = Apply(scope, coordinates);
            if (result is null)
                return (T?)result;
            if (result is not T t)
                throw new InvalidOperationException($"Expected {typeof(T)} but got {result.GetType()}");
            return t;
        }


        // public J? Apply<T>(Cursor<T> scope, System.Linq.Expressions.Expression<Func<T, object>> property)
        // {
        //
        // }


        public J? Apply(Cursor scope, CSharpCoordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public class Builder
        {
            public static Builder WithReference(string dllPath) => throw new NotImplementedException();
        }

        class TypeHintsApplier : CSharpSyntaxRewriter
        {
            public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (IsSubstituted(node))
                {
                    node = node.WithAdditionalAnnotations(Annotation.Placeholder);
                }

                return base.VisitIdentifierName(node);
            }

            bool IsSubstituted(IdentifierNameSyntax node) => Regex.IsMatch(node.Identifier.ValueText, "^__p[0-9]+$");
        }
    }

    public static class TemplateExtensions
    {
        public static T ReplaceTypeParameters<T>(this Cursor<T> cursor, InterpolatedTemplate template) where T : Cs.ClassDeclaration
            => (T)cursor.Apply(template, x => x.TypeParameters);
        
        public static J.MethodInvocation ReplaceArguments(this Cursor<J.MethodInvocation> cursor, InterpolatedTemplate template)
            => (J.MethodInvocation)cursor.Apply(template, x => x.Arguments);
        
        public static J.MethodInvocation InsertAfter(this Cursor<J.MethodInvocation> cursor, Cs.Argument argument, InterpolatedTemplate template) 
        {
            var insertIndex = cursor.Value!.Arguments.IndexOf(argument) + 1;
            return cursor.InsertAt(insertIndex, template);
        }
        
        public static J.MethodInvocation InsertBefore(this Cursor<J.MethodInvocation> cursor, Cs.Argument argument, InterpolatedTemplate template) 
        {
            var insertIndex = cursor.Value!.Arguments.IndexOf(argument);
            return cursor.InsertAt(insertIndex, template);
        }
        
        public static J.MethodInvocation InsertAt(this Cursor<J.MethodInvocation> cursor, int insertIndex, InterpolatedTemplate template) 
        {
            var newArguments = cursor.Value!.Arguments.ToList();
            newArguments.Insert(insertIndex, CSharpParser.ParseArgument(template.ToString()));
            var result = cursor.Value.WithArguments(newArguments);
            result = result.Format(cursor);
            return result;
        }

        public static J Apply<T>(this Cursor<T> cursor, InterpolatedTemplate template) where T : class, J
        {
            var result = cursor.ApplyCore(template, null);
            var newCursor = RebuildCursor(cursor, result);
            result = result.Format(newCursor);
            return result!; // todo: double check of what we wanna do on nullability
        }
        public static J Apply<T, F>(this Cursor<T> cursor, InterpolatedTemplate template, System.Linq.Expressions.Expression<Func<T, F>> location) where T : class, J
        {
            var result = cursor.ApplyCore(template, location);
            var newCursor = RebuildCursor(cursor, result);
            result = result.Format(newCursor);
            return result!; // todo: double check of what we wanna do on nullability
        }


        private static J ApplyCore<T, F>(this Cursor<T> cursor, InterpolatedTemplate template, System.Linq.Expressions.Expression<Func<T, F>> location) where T : class, J
        {
            var (path, propertyType, propertySelector) = GetPropertyPath(location);
            Func<T, object?> untypedSelector = x => propertySelector(x);
            return ApplyCore(cursor, template, path, propertyType, untypedSelector);
        }

        /// <summary>
        /// Updates the current cursor value with new value, and returns new cursor with correct parent stack
        /// </summary>
        internal static Cursor RebuildCursor(this Cursor cursor, object newValue)
        {
            var cu = cursor.FirstEnclosingOrThrow<Cs.CompilationUnit>();
            var newCu = cu.ReplaceNode((J)cursor.Value!, (J)newValue);
            var newCursor = newCu.Find(x => ReferenceEquals(x, newValue)) ?? throw new InvalidOperationException("Value in cursor was not visited");
            // var visitor = new RebuildCursorVisitor(cursor.Value!, newValue);
            // visitor.Visit(newCu, new object());
            // var newCursor = visitor.Result;
            return newCursor;
        }
        //
        // class RebuildCursorVisitor(object oldValue) : CSharpVisitor<object>
        // {
        //     private readonly object _oldValue = oldValue;
        //     private bool _stop;
        //
        //     public Cursor Result { get; private set; } = null!;
        //
        //     public override J? Visit(Core.Tree? node, object p)
        //     {
        //         return IsFound(node) ? (J?)node : base.Visit(node, p);
        //     }
        //
        //     public override JLeftPadded<T>? VisitLeftPadded<T>(JLeftPadded<T>? node, JLeftPadded.Location loc, object p)
        //     {
        //         return IsFound(node) ? node : base.VisitLeftPadded(node, loc, p);
        //     }
        //
        //     public override JRightPadded<T>? VisitRightPadded<T>(JRightPadded<T>? node, JRightPadded.Location loc, object p)
        //     {
        //         return IsFound(node) ? node : base.VisitRightPadded(node, loc, p);
        //     }
        //
        //     public override JContainer<T>? VisitContainer<T>(JContainer<T>? node, JContainer.Location loc, object p)
        //     {
        //         return IsFound(node) ? node : base.VisitContainer(node, loc, p);
        //     }
        //
        //     private bool IsFound(object? node)
        //     {
        //         if (ReferenceEquals(node, _oldValue))
        //         {
        //             Result = new Cursor;
        //             _stop = true;
        //         }
        //
        //         if (_stop)
        //         {
        //             return true;
        //         }
        //     }
        // }

        private static J ApplyCore<T>(this Cursor<T> cursor, InterpolatedTemplate template, string? path = null, Type? propertyType = null,  Func<T, object?>? propertySelector = null) where T : class, J
        {
            var parser = new CSharpParserVisitor();
            // do we need to support cursor being on a padding node?
            T node = cursor.Value ?? throw new InvalidOperationException("Cursor current value must not be null");
            J j = (J)node;
            J result = j;
            
            if (node is Cs.Argument)
            {
                var roslynArguments = SyntaxFactory.ParseArgumentList(template.ToString()) ?? throw new InvalidOperationException("Template is not an argument");
                var newArgument = parser.VisitArgument(roslynArguments.Arguments[0]);
                return newArgument.WithPrefix(Space.SINGLE_SPACE);
            }

            if (node is J.MethodInvocation invocation && invocation.Arguments.GetType().IsAssignableTo(propertyType))
            {
                var roslynArguments = SyntaxFactory.ParseArgumentList(template.ToString()) ?? throw new InvalidOperationException("Template is not an expression");
                var lstArguments = parser.VisitArgumentList(roslynArguments);
                return invocation.WithArguments(lstArguments.GetElements().Cast<Rewrite.RewriteJava.Tree.Expression>().ToList());
            }

            if (j.IsStatement() || j is J.VariableDeclarations && cursor.GetParent(2)?.Value is J.Block or Cs.CompilationUnit)
            {
                var roslynStatement = SyntaxFactory.ParseStatement(template.ToString()) ?? throw new InvalidOperationException("Template is not an statement");
                var statement = (Statement)parser.Visit(roslynStatement)!;
                return statement;
            }
            
            if (node.IsExpression())
            {
                var roslynStatement = SyntaxFactory.ParseExpression(template.ToString()) ?? throw new InvalidOperationException("Template is not an expression");
                result = (RewriteJava.Tree.Expression)parser.Visit(roslynStatement)!;
                result = result.WithPrefix(Space.SINGLE_SPACE);
                if (propertySelector != null)
                {
                    var targetNode = (J?)propertySelector(node)!;
                    result = node.ReplaceNode(targetNode, result);
                }
                return result;
            }

            //     if (j is Cs.VariableDeclarations && cursor.Parent?.Value is Cs.MethodDeclaration or J.MethodDeclaration) // method parameter
            //     {
            //         parsedNode = SyntaxFactory.ParseParameterList(template.ToString()) ?? throw new InvalidOperationException("Template is not a member declaration");
            //     }
            //     else if (j.GetSyntaxType() == SyntaxType.MemberDeclaration)
            //     {
            //         parsedNode = SyntaxFactory.ParseMemberDeclaration(template.ToString()) ?? throw new InvalidOperationException("Template is not a member declaration");
            //     }
            //
            //     else if (j.GetSyntaxType() == SyntaxType.Expression)
            //     {
            //         parsedNode = SyntaxFactory.ParseExpression(template.ToString()) ?? throw new InvalidOperationException("Template is not an expression");
            //     }
            // }
            //
            // if (lst.GetSyntaxType() is J.MethodInvocation method)
            // {
            //     parsedNode = SyntaxFactory.ParseExpression(_template.ToString()) ?? throw new InvalidOperationException($"Cannot parse template as Invocation Expression. Should take form of 'MyMethod(p1, p2)'");
            //     if (parsedNode is not InvocationExpressionSyntax templateInvocation)
            //         throw new InvalidOperationException($"Expected template to parse as InvocationExpression, but instead found {parsedNode.GetType().Name}");
            // }
            //
            // if (lst is J.VariableDeclarations statement)
            // {
            //     parsedNode = SyntaxFactory.ParseStatement(_template.ToString()) ?? throw new InvalidOperationException($"Cannot parse template as Invocation Expression. Should take form of 'MyMethod(p1, p2)'");
            //     if (parsedNode is not LocalDeclarationStatementSyntax templateInvocation)
            //         throw new InvalidOperationException($"Expected template to parse as LocalDeclarationStatementSyntax, but instead found {parsedNode.GetType().Name}");
            // }
            //

            // return (J?)templateReplacementLst;

            throw new NotImplementedException();
        }



        public static (string Path, Type FinalType, Func<TIn, TOut> PropertySelector) GetPropertyPath<TIn, TOut>(System.Linq.Expressions.Expression<Func<TIn, TOut>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            Expression body = expression.Body;

            // Unwrap boxing if needed
            if (body.NodeType == ExpressionType.Convert || body.NodeType == ExpressionType.ConvertChecked)
            {
                var unary = (UnaryExpression)body;
                body = unary.Operand;
            }

            var visitor = new PropertyPathVisitor();
            visitor.Visit(body);

            if (!visitor.IsValid)
                throw new ArgumentException("Expression must be a property accessor", nameof(expression));

            var propertyGetter = expression.Compile();

            return (string.Join(".", visitor.PathComponents), visitor.FinalType ?? throw new InvalidOperationException("Failed to resolve final type"), propertyGetter);
        }

        private class PropertyPathVisitor : ExpressionVisitor
        {
            public List<string> PathComponents { get; } = new();
            public bool IsValid { get; private set; } = true;
            public Type? FinalType { get; private set; }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member is not PropertyInfo property)
                {
                    IsValid = false;
                    return node;
                }

                Visit(node.Expression);
                PathComponents.Add(property.Name);

                FinalType = property.PropertyType; // update final type as we recurse
                return node;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node == null)
                {
                    IsValid = false;
                    return node;
                }

                return base.Visit(node);
            }
        }

    }


}