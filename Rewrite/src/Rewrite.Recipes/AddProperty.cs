using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteProperties;

namespace Rewrite.Recipes.Properties;

[DisplayName("Add Property Recipe")]
[Description("Full description")]
public class AddProperty : Recipe
{
    [DisplayName("Property Name")]
    [Description("A name of the property to add")]
    public required string PropertyName { get; init; } = "blah";
    
    [DisplayName("Value")]
    [Description("Value to specify for the given property name")]
    public string PropertyValue { get; } = "blah";
    public override ITreeVisitor<Tree, IExecutionContext> GetVisitor()
    {
        return new AddPropertyVisitor(PropertyName, PropertyValue);
    }

    private class AddPropertyVisitor(string property, string value) : PropertiesVisitor<IExecutionContext>
    {
        public override RewriteProperties.Tree.Properties VisitFile(RewriteProperties.Tree.Properties.File file, IExecutionContext p)
        {
            var first = file.Content.FirstOrDefault(e => e is RewriteProperties.Tree.Properties.Entry entry && entry.Key == property);
            if (first == null)
            {
                var entry = new RewriteProperties.Tree.Properties.Entry(Tree.RandomId(), "\n", Markers.EMPTY, property,
                    " ", RewriteProperties.Tree.Properties.Entry.Delimiter.EQUALS,
                    new RewriteProperties.Tree.Properties.Value(Tree.RandomId(), " ", Markers.EMPTY, value));
                return file.WithContent(ListUtils.Concat(file.Content, entry).ToList());
            }
            else
            {
                return file.WithContent(ListUtils.Map(file.Content, e =>
                {
                    if (e is RewriteProperties.Tree.Properties.Entry entry && entry.Key == property)
                    {
                        return entry.Value.Text.Equals(value) ? entry : entry.WithValue(entry.Value.WithText(value));
                    }

                    return e;
                }));
            }
        }
    }
}
