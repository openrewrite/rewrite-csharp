using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzer to detect usage of DriveInfo.DriveFormat which has behavioral changes in .NET 10 on Linux.
    /// On Linux, DriveInfo.DriveFormat now returns actual Linux kernel filesystem type strings (e.g., "ext3", "ext4")
    /// instead of mapped constants, and some filesystem names have changed (e.g., "cgroupfs" -> "cgroup", "selinux" -> "selinuxfs").
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DriveInfoDriveFormatAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ORNETX0001";

        private static readonly LocalizableString Title = "DriveInfo.DriveFormat has behavioral changes in .NET 10 on Linux";
        private static readonly LocalizableString MessageFormat = "DriveInfo.DriveFormat returns different values in .NET 10 on Linux. Review usage to ensure compatibility with new filesystem type strings.";
        private static readonly LocalizableString Description = "In .NET 10, DriveInfo.DriveFormat on Linux returns actual kernel filesystem type strings (e.g., 'ext3', 'ext4') instead of mapped constants. " +
            "Filesystem names for cgroup ('cgroup'/'cgroup2' instead of 'cgroupfs'/'cgroup2fs') and SELinux ('selinuxfs' instead of 'selinux') have also changed. " +
            "Check and update string comparisons to include these new filesystem type strings. Reference /proc/self/mountinfo for actual values.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://github.com/dotnet/docs/blob/main/docs/core/compatibility/core-libraries/10.0/driveinfo-driveformat-linux.md");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register for member access expressions to catch DriveInfo.DriveFormat usage
            context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
        }

        private void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
        {
            var memberAccess = (MemberAccessExpressionSyntax)context.Node;
            if(!memberAccess.IsSymbolOneOf(context.SemanticModel, "P:System.IO.DriveInfo.DriveFormat"))
                return;

            // Report diagnostic
            var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
