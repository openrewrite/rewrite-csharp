using FluentAssertions;
using Rewrite.Core;
using Rewrite.Core.Config;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.CSharp.Tests;

public class RecipeOptionsTest
{
    [Fact]
    public void testDescriptorResolution()
    {
        var findClass = new TestRecipe();

        var findClassDescriptor = findClass.Descriptor;
        findClassDescriptor.Should().BeEquivalentTo(new RecipeDescriptor(
            "TestRecipe",
            "Test Recipe",
            "Test Recipe Description",
            new HashSet<string>(),
            TimeSpan.FromMinutes(5),
            [
                new OptionDescriptor(
                    "description",
                    typeof(string).ToString(),
                    "Description",
                    "Found class marker",
                    "~~>",
                    null,
                    false,
                    null
                )
            ],
            [],
            new Uri($"recipe://{nameof(TestRecipe)}")
        ));
    }


    public class TestRecipe : Recipe
    {
        public TestRecipe([Option(displayName: "Description", "Found class marker", "~~>")] string? description = null)
        {
        }

        public override string DisplayName => "Test Recipe";
        public override string Description => "Test Recipe Description";

        public override ITreeVisitor<Core.Tree, ExecutionContext> GetVisitor()
        {
            return null!;
        }
    }
}
