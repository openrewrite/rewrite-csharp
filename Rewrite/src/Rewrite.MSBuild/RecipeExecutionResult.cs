using Microsoft.CodeAnalysis;

namespace Rewrite.MSBuild;

public record RecipeExecutionResult(string SolutionFile, TimeSpan SolutionLoadTime, TimeSpan ExecutionTime, List<IssueFixResult> FixedIssues);

public record IssueFixResult(string IssueId, TimeSpan ExecutionTime, List<DocumentFixResult> Fixes);

public record DocumentFixResult(string FileName, List<int> LineNumbers);