using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteProperties.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.RewriteProperties;

public class AddProperty(string property, string value) : Recipe
{
    public override ITreeVisitor<Core.Tree, ExecutionContext> GetVisitor()
    {
        return new AddPropertyVisitor(property, value);
    }

    private class AddPropertyVisitor(string property, string value) : PropertiesVisitor<ExecutionContext>
    {
        public override Properties VisitFile(Properties.File file, ExecutionContext p)
        {
            var first = file.Content.FirstOrDefault(e => e is Properties.Entry entry && entry.Key == property);
            if (first == null)
            {
                var entry = new Properties.Entry(Core.Tree.RandomId(), "\n", Markers.EMPTY, property,
                    " ", Properties.Entry.Delimiter.EQUALS,
                    new Properties.Value(Core.Tree.RandomId(), " ", Markers.EMPTY, value));
                return file.WithContent(ListUtils.Concat(file.Content, entry).ToList());
            }
            else
            {
                return file.WithContent(ListUtils.Map(file.Content, e =>
                {
                    if (e is Properties.Entry entry && entry.Key == property)
                    {
                        return entry.Value.Text.Equals(value) ? entry : entry.WithValue(entry.Value.WithText(value));
                    }
                    return e;
                }));
            }
        }
    }
}
