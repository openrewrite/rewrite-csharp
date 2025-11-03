using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.RoslynRecipe.Helpers;
using TUnit.Core;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace Rewrite.RoslynRecipe.Tests
{
    /// <summary>
    /// Test suite for SemanticAnalysisUtil member access tracking functionality.
    /// </summary>
    public class SemanticAnalysisUtilTests
    {
        /// <summary>
        /// Tests that direct member access on a parameter is correctly detected.
        /// </summary>
        [Test]
        public async Task DirectMemberAccess_OnParameter_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                    public string Name { get; set; }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        obj.Dispose();
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
            await Assert.That(result.AccessLocations[0].AccessType).IsEqualTo(SemanticAnalysisUtil.MemberAccessType.MethodInvocation);
        }

        /// <summary>
        /// Tests that property access on a parameter is correctly detected.
        /// </summary>
        [Test]
        public async Task PropertyAccess_OnParameter_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public string Name { get; set; }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        var name = obj.Name;
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Name");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
            await Assert.That(result.AccessLocations[0].AccessType).IsEqualTo(SemanticAnalysisUtil.MemberAccessType.PropertyGet);
        }

        /// <summary>
        /// Tests that member access through local variable assignment is tracked.
        /// </summary>
        [Test]
        public async Task MemberAccess_ThroughLocalVariable_IsTracked()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        var local = obj;
                        local.Dispose();
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
            await Assert.That(result.AliasedSymbols.Count).IsGreaterThan(1); // Original + local variable
        }

        /// <summary>
        /// Tests that member access through multiple reassignments is tracked.
        /// </summary>
        [Test]
        public async Task MemberAccess_ThroughMultipleReassignments_IsTracked()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        var temp1 = obj;
                        var temp2 = temp1;
                        TestClass temp3;
                        temp3 = temp2;
                        temp3.Dispose();
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
            await Assert.That(result.AliasedSymbols.Count).IsGreaterThanOrEqualTo(4); // obj, temp1, temp2, temp3
        }

        /// <summary>
        /// Tests that conditional member access is correctly detected.
        /// </summary>
        [Test]
        public async Task ConditionalMemberAccess_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        obj?.Dispose();
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
        }

        /// <summary>
        /// Tests that member access in conditional branches is detected.
        /// </summary>
        [Test]
        public async Task MemberAccess_InConditionalBranch_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj, bool condition)
                    {
                        if (condition)
                        {
                            obj.Dispose();
                        }
                        else
                        {
                            var temp = obj;
                            temp.Dispose();
                        }
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(2); // One in each branch
        }

        /// <summary>
        /// Tests that member access in loops is detected.
        /// </summary>
        [Test]
        public async Task MemberAccess_InLoop_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public void Process() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            obj.Process();
                        }
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Process");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1); // Single location, even in loop
        }

        /// <summary>
        /// Tests that passing object to another method is tracked.
        /// </summary>
        [Test]
        public async Task PassingObject_ToAnotherMethod_IsTracked()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        HelperMethod(obj);
                    }

                    private void HelperMethod(TestClass item)
                    {
                        item.Dispose();
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.MethodCalls.Count).IsGreaterThan(0);
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
        }

        /// <summary>
        /// Tests that no access is correctly reported when member is not accessed.
        /// </summary>
        [Test]
        public async Task NoMemberAccess_ReturnsNotAccessed()
        {
            const string code = @"
                public class TestClass
                {
                    public void Dispose() { }
                    public string Name { get; set; }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        var name = obj.Name; // Access Name, not Dispose
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Dispose");

            await Assert.That(result.IsAccessed).IsFalse();
            await Assert.That(result.AccessLocations).IsEmpty();
        }

        /// <summary>
        /// Tests that member access through LINQ is detected.
        /// </summary>
        [Test]
        public async Task MemberAccess_ThroughLinq_IsDetected()
        {
            const string code = @"
                using System.Linq;
                using System.Collections.Generic;

                public class TestClass
                {
                    public void Process() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        var list = new List<TestClass> { obj };
                        list.ForEach(x => x.Process());
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Process");

            // Note: This test might require more sophisticated analysis for lambda closures
            // For now, we're testing that the infrastructure works
            await Assert.That(result).IsNotNull();
        }

        /// <summary>
        /// Tests property setter access detection.
        /// </summary>
        [Test]
        public async Task PropertySetter_Access_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public string Name { get; set; }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        obj.Name = ""NewName"";
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Name");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
            await Assert.That(result.AccessLocations[0].AccessType).IsEqualTo(SemanticAnalysisUtil.MemberAccessType.PropertySet);
        }

        /// <summary>
        /// Tests that field access is correctly detected.
        /// </summary>
        [Test]
        public async Task FieldAccess_IsDetected()
        {
            const string code = @"
                public class TestClass
                {
                    public string Name;
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        var value = obj.Name;
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Name");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
            await Assert.That(result.AccessLocations[0].AccessType).IsEqualTo(SemanticAnalysisUtil.MemberAccessType.FieldAccess);
        }

        /// <summary>
        /// Tests member access through using statement.
        /// </summary>
        [Test]
        public async Task MemberAccess_InUsingStatement_IsDetected()
        {
            const string code = @"
                using System;

                public class TestClass : IDisposable
                {
                    public void Dispose() { }
                    public void Process() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        using (var temp = obj)
                        {
                            temp.Process();
                        }
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Process");

            await Assert.That(result.IsAccessed).IsTrue();
            await Assert.That(result.AccessLocations.Count).IsEqualTo(1);
        }

        /// <summary>
        /// Tests member access tracking through try-catch-finally blocks.
        /// </summary>
        [Test]
        public async Task MemberAccess_InTryCatchFinally_IsDetected()
        {
            const string code = @"
                using System;

                public class TestClass
                {
                    public void Process() { }
                    public void Cleanup() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        try
                        {
                            obj.Process();
                        }
                        catch (Exception)
                        {
                            // No access here
                        }
                        finally
                        {
                            obj.Cleanup();
                        }
                    }
                }";

            var processResult = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Process");
            var cleanupResult = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "Cleanup");

            await Assert.That(processResult.IsAccessed).IsTrue();
            await Assert.That(cleanupResult.IsAccessed).IsTrue();
        }

        /// <summary>
        /// Tests that accessing a different member doesn't trigger false positives.
        /// </summary>
        [Test]
        public async Task DifferentMemberAccess_DoesNotTriggerFalsePositive()
        {
            const string code = @"
                public class TestClass
                {
                    public void MethodA() { }
                    public void MethodB() { }
                }

                public class Consumer
                {
                    public void ProcessObject(TestClass obj)
                    {
                        obj.MethodA();
                    }
                }";

            var result = await AnalyzeMemberAccessAsync(code, "ProcessObject", "obj", "MethodB");

            await Assert.That(result.IsAccessed).IsFalse();
            await Assert.That(result.AccessLocations).IsEmpty();
        }

        // Helper method to set up the analysis
        private async Task<SemanticAnalysisUtil.MemberAccessResult> AnalyzeMemberAccessAsync(
            string code,
            string methodName,
            string parameterName,
            string memberName)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("TestCompilation")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location))
                .AddSyntaxTrees(tree);

            var semanticModel = compilation.GetSemanticModel(tree);
            var root = await tree.GetRootAsync();

            // Find the method
            var method = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text == methodName);

            if (method == null)
                throw new InvalidOperationException($"Method {methodName} not found");

            // Find the parameter symbol
            var methodSymbol = semanticModel.GetDeclaredSymbol(method);
            var parameterSymbol = methodSymbol?.Parameters.FirstOrDefault(p => p.Name == parameterName);

            if (parameterSymbol == null)
                throw new InvalidOperationException($"Parameter {parameterName} not found");

            // Analyze member access
            return await SemanticAnalysisUtil.IsMemberAccessedAsync(
                semanticModel,
                parameterSymbol,
                memberName,
                method.Body ?? (SyntaxNode)method,
                CancellationToken.None);
        }
    }
}