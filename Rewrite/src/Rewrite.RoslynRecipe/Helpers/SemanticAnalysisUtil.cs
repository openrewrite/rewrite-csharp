using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Operations;

namespace Rewrite.RoslynRecipe.Helpers
{
    /// <summary>
    /// Provides utilities for semantic analysis
    /// </summary>
    public static class SemanticAnalysisUtil
    {
        
        public static bool IsSymbolOneOf(this SyntaxNode node, SemanticModel semanticModel, params IEnumerable<string> symbolNames)
        {
            var symbolInfo = semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;
            if (symbol == null)
            {
                return false;
            }

            // Use GetDocumentationCommentId for precise identification when available
            var symbolKey = symbol.GetDocumentationCommentId();
            if(symbolKey == null)
                return false;

            if (symbolNames.Contains(symbolKey))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Determines if a specific member of a type is accessed from a given location,
        /// tracking the object through control flow, data flow, reassignments, and method calls.
        /// </summary>
        /// <param name="semanticModel">The semantic model for analysis.</param>
        /// <param name="objectSymbol">The symbol representing the object to track (parameter, variable, field, etc.).</param>
        /// <param name="memberName">The name of the member to check for access (method or property name).</param>
        /// <param name="searchScope">The syntax node defining the scope to search within (method body, class, etc.).</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A result containing whether the member is accessed and details about the access locations.</returns>
        /// <example>
        /// Check if a parameter's Dispose method is called:
        /// <code>
        /// var parameter = method.Parameters[0];
        /// var result = await SemanticAnalysisUtil.IsMemberAccessedAsync(
        ///     semanticModel,
        ///     parameter,
        ///     "Dispose",
        ///     methodBody,
        ///     cancellationToken);
        /// </code>
        /// </example>
        public static async Task<MemberAccessResult> IsMemberAccessedAsync(
            SemanticModel semanticModel,
            ISymbol objectSymbol,
            string memberName,
            SyntaxNode searchScope,
            CancellationToken cancellationToken = default)
        {
            if (semanticModel == null) throw new ArgumentNullException(nameof(semanticModel));
            if (objectSymbol == null) throw new ArgumentNullException(nameof(objectSymbol));
            if (string.IsNullOrEmpty(memberName)) throw new ArgumentException("Member name cannot be null or empty.", nameof(memberName));
            if (searchScope == null) throw new ArgumentNullException(nameof(searchScope));

            var tracker = new ObjectFlowTracker(semanticModel, objectSymbol, memberName, cancellationToken);
            return await tracker.AnalyzeAsync(searchScope);
        }

        /// <summary>
        /// Represents the result of member access analysis including all access locations and paths.
        /// </summary>
        public class MemberAccessResult
        {
            /// <summary>
            /// Gets whether the member is accessed anywhere in the analyzed scope.
            /// </summary>
            public bool IsAccessed { get; }

            /// <summary>
            /// Gets the locations where the member is directly accessed.
            /// </summary>
            public IReadOnlyList<MemberAccessLocation> AccessLocations { get; }

            /// <summary>
            /// Gets the symbols that alias or reference the tracked object.
            /// </summary>
            public IReadOnlyCollection<ISymbol> AliasedSymbols { get; }

            /// <summary>
            /// Gets methods where the object is passed as an argument.
            /// </summary>
            public IReadOnlyList<MethodCallInfo> MethodCalls { get; }

            public MemberAccessResult(
                bool isAccessed,
                IReadOnlyList<MemberAccessLocation> accessLocations,
                IReadOnlyCollection<ISymbol> aliasedSymbols,
                IReadOnlyList<MethodCallInfo> methodCalls)
            {
                IsAccessed = isAccessed;
                AccessLocations = accessLocations ?? Array.Empty<MemberAccessLocation>();
                AliasedSymbols = aliasedSymbols ?? new HashSet<ISymbol>(SymbolEqualityComparer.Default);
                MethodCalls = methodCalls ?? Array.Empty<MethodCallInfo>();
            }
        }

        /// <summary>
        /// Represents a location where a member is accessed.
        /// </summary>
        public class MemberAccessLocation
        {
            /// <summary>
            /// Gets the syntax node where the access occurs.
            /// </summary>
            public SyntaxNode AccessNode { get; }

            /// <summary>
            /// Gets the type of access (method invocation, property get/set).
            /// </summary>
            public MemberAccessType AccessType { get; }

            /// <summary>
            /// Gets the symbol through which the member was accessed (could be alias).
            /// </summary>
            public ISymbol AccessedThrough { get; }

            /// <summary>
            /// Gets the file path and line number.
            /// </summary>
            public FileLinePositionSpan Location { get; }

            public MemberAccessLocation(
                SyntaxNode accessNode,
                MemberAccessType accessType,
                ISymbol accessedThrough,
                FileLinePositionSpan location)
            {
                AccessNode = accessNode;
                AccessType = accessType;
                AccessedThrough = accessedThrough;
                Location = location;
            }
        }

        /// <summary>
        /// Represents information about a method call where the tracked object is passed.
        /// </summary>
        public class MethodCallInfo
        {
            /// <summary>
            /// Gets the method being called.
            /// </summary>
            public IMethodSymbol Method { get; }

            /// <summary>
            /// Gets the parameter position where the object is passed.
            /// </summary>
            public int ParameterIndex { get; }

            /// <summary>
            /// Gets the invocation syntax node.
            /// </summary>
            public InvocationExpressionSyntax InvocationNode { get; }

            public MethodCallInfo(IMethodSymbol method, int parameterIndex, InvocationExpressionSyntax invocationNode)
            {
                Method = method;
                ParameterIndex = parameterIndex;
                InvocationNode = invocationNode;
            }
        }

        /// <summary>
        /// Types of member access.
        /// </summary>
        public enum MemberAccessType
        {
            MethodInvocation,
            PropertyGet,
            PropertySet,
            EventAccess,
            FieldAccess
        }

        /// <summary>
        /// Internal class that performs the actual tracking of object flow and member access.
        /// </summary>
        private class ObjectFlowTracker
        {
            private readonly SemanticModel _semanticModel;
            private readonly ISymbol _originalSymbol;
            private readonly string _memberName;
            private readonly CancellationToken _cancellationToken;
            private readonly HashSet<ISymbol> _trackedSymbols;
            private readonly HashSet<SyntaxNode> _visitedNodes;
            private readonly List<MemberAccessLocation> _accessLocations;
            private readonly List<MethodCallInfo> _methodCalls;

            public ObjectFlowTracker(
                SemanticModel semanticModel,
                ISymbol originalSymbol,
                string memberName,
                CancellationToken cancellationToken)
            {
                _semanticModel = semanticModel;
                _originalSymbol = originalSymbol;
                _memberName = memberName;
                _cancellationToken = cancellationToken;
                _trackedSymbols = new HashSet<ISymbol>(SymbolEqualityComparer.Default) { originalSymbol };
                _visitedNodes = new HashSet<SyntaxNode>();
                _accessLocations = new List<MemberAccessLocation>();
                _methodCalls = new List<MethodCallInfo>();
            }

            public async Task<MemberAccessResult> AnalyzeAsync(SyntaxNode searchScope)
            {
                // Start analysis from the search scope
                await AnalyzeNodeAsync(searchScope);

                // Check if we need to analyze method calls where the object is passed
                foreach (var methodCall in _methodCalls.ToList())
                {
                    await AnalyzeMethodCallAsync(methodCall);
                }

                return new MemberAccessResult(
                    _accessLocations.Any(),
                    _accessLocations,
                    _trackedSymbols,
                    _methodCalls);
            }

            private async Task AnalyzeNodeAsync(SyntaxNode node)
            {
                if (node == null || _visitedNodes.Contains(node))
                    return;

                _visitedNodes.Add(node);
                _cancellationToken.ThrowIfCancellationRequested();

                // Check for assignments that create aliases
                if (node is AssignmentExpressionSyntax assignment)
                {
                    await AnalyzeAssignmentAsync(assignment);
                }
                // Check for variable declarations that create aliases
                else if (node is VariableDeclaratorSyntax variableDeclarator)
                {
                    await AnalyzeVariableDeclaratorAsync(variableDeclarator);
                }
                // Check for member access
                else if (node is MemberAccessExpressionSyntax memberAccess)
                {
                    await AnalyzeMemberAccessAsync(memberAccess);
                }
                // Check for invocations where the object might be passed
                else if (node is InvocationExpressionSyntax invocation)
                {
                    await AnalyzeInvocationAsync(invocation);
                }
                // Check for conditional access
                else if (node is ConditionalAccessExpressionSyntax conditionalAccess)
                {
                    await AnalyzeConditionalAccessAsync(conditionalAccess);
                }

                // Recursively analyze child nodes
                foreach (var child in node.ChildNodes())
                {
                    await AnalyzeNodeAsync(child);
                }
            }

            private Task AnalyzeAssignmentAsync(AssignmentExpressionSyntax assignment)
            {
                var rightSymbol = GetSymbol(assignment.Right);
                var leftSymbol = GetSymbol(assignment.Left);

                // If the right side is one of our tracked symbols, track the left side too
                if (rightSymbol != null && _trackedSymbols.Contains(rightSymbol) && leftSymbol != null)
                {
                    _trackedSymbols.Add(leftSymbol);
                }
                // If the left side is one of our tracked symbols, check if right side accesses the member
                else if (leftSymbol != null && _trackedSymbols.Contains(leftSymbol))
                {
                    // This could be a reassignment, need to handle appropriately
                    // For now, we continue tracking the left symbol
                }

                return Task.CompletedTask;
            }

            private Task AnalyzeVariableDeclaratorAsync(VariableDeclaratorSyntax declarator)
            {
                if (declarator.Initializer == null)
                    return Task.CompletedTask;

                var initializerSymbol = GetSymbol(declarator.Initializer.Value);
                if (initializerSymbol != null && _trackedSymbols.Contains(initializerSymbol))
                {
                    var declaredSymbol = _semanticModel.GetDeclaredSymbol(declarator);
                    if (declaredSymbol != null)
                    {
                        _trackedSymbols.Add(declaredSymbol);
                    }
                }

                return Task.CompletedTask;
            }

            private Task AnalyzeMemberAccessAsync(MemberAccessExpressionSyntax memberAccess)
            {
                var expressionSymbol = GetSymbol(memberAccess.Expression);
                if (expressionSymbol != null && _trackedSymbols.Contains(expressionSymbol))
                {
                    if (memberAccess.Name.Identifier.Text == _memberName)
                    {
                        // Found a member access!
                        var accessType = DetermineAccessType(memberAccess);
                        var location = memberAccess.GetLocation().GetLineSpan();

                        _accessLocations.Add(new MemberAccessLocation(
                            memberAccess,
                            accessType,
                            expressionSymbol,
                            location));
                    }
                }

                return Task.CompletedTask;
            }

            private async Task AnalyzeInvocationAsync(InvocationExpressionSyntax invocation)
            {
                var argumentList = invocation.ArgumentList;
                if (argumentList == null)
                    return;

                for (int i = 0; i < argumentList.Arguments.Count; i++)
                {
                    var argument = argumentList.Arguments[i];
                    var argumentSymbol = GetSymbol(argument.Expression);

                    if (argumentSymbol != null && _trackedSymbols.Contains(argumentSymbol))
                    {
                        // The tracked object is being passed to a method
                        var methodSymbol = _semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
                        if (methodSymbol != null)
                        {
                            _methodCalls.Add(new MethodCallInfo(methodSymbol, i, invocation));
                        }
                    }
                }

                // Also check if this is a member invocation on our tracked object
                if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
                {
                    await AnalyzeMemberAccessAsync(memberAccess);
                }
            }

            private Task AnalyzeConditionalAccessAsync(ConditionalAccessExpressionSyntax conditionalAccess)
            {
                var expressionSymbol = GetSymbol(conditionalAccess.Expression);
                if (expressionSymbol != null && _trackedSymbols.Contains(expressionSymbol))
                {
                    // Check if the when-not-null part accesses our member
                    if (conditionalAccess.WhenNotNull is MemberBindingExpressionSyntax memberBinding)
                    {
                        if (memberBinding.Name.Identifier.Text == _memberName)
                        {
                            var accessType = DetermineAccessType(conditionalAccess);
                            var location = conditionalAccess.GetLocation().GetLineSpan();

                            _accessLocations.Add(new MemberAccessLocation(
                                conditionalAccess,
                                accessType,
                                expressionSymbol,
                                location));
                        }
                    }
                }

                return Task.CompletedTask;
            }

            private async Task AnalyzeMethodCallAsync(MethodCallInfo methodCall)
            {
                // Try to get the method declaration to analyze its body
                var methodDeclaration = await GetMethodDeclarationAsync(methodCall.Method);
                if (methodDeclaration != null && methodCall.ParameterIndex < methodCall.Method.Parameters.Length)
                {
                    var parameter = methodCall.Method.Parameters[methodCall.ParameterIndex];
                    _trackedSymbols.Add(parameter);

                    // Analyze the method body
                    var bodyNode = methodDeclaration.Body ?? (SyntaxNode?)methodDeclaration.ExpressionBody;
                    if (bodyNode != null)
                    {
                        await AnalyzeNodeAsync(bodyNode);
                    }
                }
            }

            private async Task<MethodDeclarationSyntax?> GetMethodDeclarationAsync(IMethodSymbol method)
            {
                if (method.DeclaringSyntaxReferences.Length > 0)
                {
                    var syntaxRef = method.DeclaringSyntaxReferences[0];
                    var syntax = await syntaxRef.GetSyntaxAsync(_cancellationToken);
                    return syntax as MethodDeclarationSyntax;
                }
                return null;
            }

            private ISymbol? GetSymbol(SyntaxNode node)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(node, _cancellationToken);
                return symbolInfo.Symbol ?? _semanticModel.GetDeclaredSymbol(node, _cancellationToken);
            }

            private MemberAccessType DetermineAccessType(SyntaxNode node)
            {
                var parent = node.Parent;

                // Check if it's an invocation
                if (parent is InvocationExpressionSyntax)
                    return MemberAccessType.MethodInvocation;

                // Check if it's an assignment
                if (parent is AssignmentExpressionSyntax assignment)
                {
                    if (assignment.Left == node)
                        return MemberAccessType.PropertySet;
                    else
                        return MemberAccessType.PropertyGet;
                }

                // Default to property get for member access
                if (node is MemberAccessExpressionSyntax || node is ConditionalAccessExpressionSyntax)
                {
                    var symbolInfo = _semanticModel.GetSymbolInfo(node);
                    if (symbolInfo.Symbol is IPropertySymbol)
                        return MemberAccessType.PropertyGet;
                    if (symbolInfo.Symbol is IFieldSymbol)
                        return MemberAccessType.FieldAccess;
                    if (symbolInfo.Symbol is IEventSymbol)
                        return MemberAccessType.EventAccess;
                }

                return MemberAccessType.PropertyGet;
            }
        }
    }
}