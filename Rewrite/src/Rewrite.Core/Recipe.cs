using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rewrite.Core.Config;

namespace Rewrite.Core;
public abstract class Recipe
{
    private static Recipe _noop = new NoopRecipe();

    public static Recipe Noop() => _noop;
    [DisplayName("NoOp")]
    [Description("Does nothing")]
    private class NoopRecipe : Recipe
    {
        public override ITreeVisitor<Tree, IExecutionContext> GetVisitor()
        {
            return ITreeVisitor<Tree, IExecutionContext>.Noop();
        }
    }

    public virtual ITreeVisitor<Tree, IExecutionContext> GetVisitor()
    {
        return ITreeVisitor<Tree, IExecutionContext>.Noop();
    }

}

