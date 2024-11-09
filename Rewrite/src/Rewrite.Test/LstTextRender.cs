using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Rewrite.Core;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Socolin.ANSITerminalColor;

namespace Rewrite.Test;

public static class LstTextRender
{
    const bool StringWhitespace = true;

    public static string RenderLstTree(this Tree tree)
    {

        var walker = new Walker();
        var results = new Dictionary<Guid, OutputLine>();
        walker.Visit(tree, results);

        Column columnTree = new("Tree");
        Column columnRender = new("Render");
        Column columnParentProperty = new("Parent property");
        var compilationRoot = results.Values.First();
        PrintNode(compilationRoot, "", false, columnTree, columnRender, columnParentProperty);
        var table = RenderTable(columnTree, columnRender, columnParentProperty);
        return table;
    }

    private class Column(string name)
    {
        public List<string> Lines { get; } = new();
        public string Name { get; set; } = name;
    }

    static string RenderTable(params Column[] columns)
    {
        foreach (var column in columns)
        {
            PadRight(column);
        }

        var sb = new StringBuilder();


        var header = string.Join(" | ", columns.Select(column => column.Name));
        sb.AppendLine(header);


        sb.AppendLine(new string('=', header.GetVisibleLength()));
        for (int lineNum = 0; lineNum < columns[0].Lines.Count; lineNum++)
        {
            //columns.Select(column => column.Lines[lineNum].Length).ToList();
            var tableLine = string.Join(" | ", columns.Select(column => column.Lines[lineNum]));
            sb.AppendLine(tableLine);
        }

        return sb.ToString();
    }
    static int GetVisibleLength(this string input)
    {
        // Regular expression pattern to match ANSI escape codes
        string ansiPattern = @"\x1B\[[0-9;]*[A-Za-z]";

        // Remove ANSI escape codes
        string cleanString = Regex.Replace(input, ansiPattern, string.Empty);

        // Return the length of the cleaned string
        return cleanString.Length;
    }
    static void PadRight(Column column)
    {
        var col1Size = column.Lines.Select(x => x?.GetVisibleLength() ?? 0).Max();
        for (int i = 0; i < column.Lines.Count; i++)
        {
            var val = column.Lines[i] ?? "";
            var padded = val.PadRightIgnoringAnsi(col1Size);
            //padded.GetVisibleLength().Dump();
            column.Lines[i] = padded;
        }

        column.Name = column.Name.PadRightIgnoringAnsi(col1Size);
        //column.Name.GetVisibleLength().Dump("header");
    }
    private static readonly Regex AnsiRegex = new Regex(@"\x1B\[[0-9;]*[mGKH]", RegexOptions.Compiled);

    public static string TrimIgnoreAnsiCodes(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        input = input.Trim();

        var pattern = @"^(?<start>\x1B\[[0-9;]*[mK])?(?<body>.*?)(?<end>\x1B\[[0-9;]*[mK])?$";

        var match = Regex.Match(input, pattern, RegexOptions.Singleline);

        if (!match.Success)
            return input;

        var leadingCodes = match.Groups["start"].Value;  // Group 1: Leading ANSI codes
        var bodyText = match.Groups["body"].Value;      // Group 2: Main text content
        var trailingCodes = match.Groups["end"].Value; // Group 3: Trailing ANSI codes

        return leadingCodes + bodyText.Trim() + trailingCodes;
    }

    public static string PadRightIgnoringAnsi(this string input, int totalWidth, char paddingChar = ' ')
    {
        // Strip ANSI codes temporarily to calculate the actual visible length
        string strippedString = AnsiRegex.Replace(input, "");

        // Calculate padding based on visible length without ANSI codes
        int paddingNeeded = Math.Max(0, totalWidth - strippedString.Length);

        // Pad the original string with padding character
        return input + new string(paddingChar, paddingNeeded);
    }

    static void PrintNode(OutputLine tree, String indent, bool last, Column columnTree, Column columnRender, Column columnParentProperty)
    {
        columnTree.Lines.Add(indent + "+-" + tree.Node);
        indent += last ? "  " : "| ";
        var render = tree.Render;
        if (StringWhitespace)
        {
            render = Regex.Replace(render, @"[\r\n]+", " ", RegexOptions.Multiline);
            render = Regex.Replace(render, @"\s{2,}", " ");
            render = Regex.Replace(render, $" ({AnsiRegex} )", "$1");
            render = render.TrimIgnoreAnsiCodes();
            render = TrimString(render, 100);
        }

        columnRender.Lines.Add(render);
        columnParentProperty.Lines.Add(tree.Property ?? "");
        for (int i = 0; i < tree.Children.Count; i++)
        {
            PrintNode(tree.Children[i], indent, i == tree.Children.Count - 1, columnTree, columnRender, columnParentProperty);
        }
    }

    class SyntaxHighlighter(Tree targetNode) : CSharpPrinter<int>
    {
        public override J? PreVisit(Tree? tree, PrintOutputCapture<int> p)
        {

            if (tree?.Id == targetNode.Id)
            {
                p.Append(AnsiColor.Foreground(Terminal256ColorCodes.GreenC2).ToEscapeSequence());
            }
            return tree as J;
        }
        public override J PostVisit(Tree tree, PrintOutputCapture<int> p)
        {
            if (tree.Id == targetNode.Id)
            {
                p.Append(AnsiColor.Reset.ToEscapeSequence());
            }
            return (J)tree;
        }
        public static string Highlight(Tree root, Tree node)
        {
            PrintOutputCapture<int> p = new(1);
            var visitor = new SyntaxHighlighter(node).Visit(root, p);
            return p.Out.ToString();
        }
    }

    class Walker : CSharpVisitor<Dictionary<Guid, OutputLine>>
    {
        string GetParentQualifiedName(Type type)
        {
            var segments = new List<string>();
            var current = type;
            do
            {
                segments.Add(current.Name);
                current = current.DeclaringType;
            } while (current != null);

            segments.Reverse();
            return string.Join(".", segments);
        }
        public override J? PreVisit(Tree? tree, Dictionary<Guid, OutputLine> p)
        {

            if (tree == null)
                return null;
            var (parentType, parentProperty) = FindTrueParent();
            var varType = tree.GetType();
            var typeName = GetParentQualifiedName(varType);
            var root = Cursor.GetRoot().GetValue<Tree>() ?? tree;
            var render = SyntaxHighlighter.Highlight(root, tree);
            var line = new OutputLine
            {
                Node = typeName,
                Render = render,
                Property = parentProperty,
                Parent = parentType?.GetType().Name!
            };
            p[tree.Id] = line;
            if (parentType != null)
            {
                p[parentType.Id].Children.Add(line);
            }

            return base.PreVisit(tree, p);
        }

        private (J? Parent, string? Property) FindTrueParent()
        {
            var current = Cursor;
            while (!current.Parent?.IsRoot ?? false)
            {
                //current.Parent.Value.Dump();
                if (current.Parent?.Value is not JRightPadded and not JContainer and not JLeftPadded)
                    return (current.Parent?.Value as J, FindParentProperty(current));
                current = current.Parent;
            }

            return (null, null);
        }

        private string? FindParentProperty(Cursor cursor)
        {
            if (!cursor.TryFindParent(x => x != null, out var parent))
                return null;
            var properties = parent!.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var valueType = cursor.Value?.GetType();
            //var underlyingType = valueType;
            if (cursor.Value is JRightPadded or JContainer or JLeftPadded)
            {
                parent = ((dynamic)parent).Padding;
                properties = parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //underlyingType = valueType.GetGenericArguments()[0];
            }


            var parentProp = properties.FirstOrDefault(x => ReferenceEquals(x.GetValue(parent), cursor.Value));
            //var parentPropName = properties.FirstOrDefault(x => ReferenceEquals(x.GetValue(parent), cursor.Value))?.Name;
            if (parentProp == null)
            {
                // try to find inside collections
                var compatibleCollections = properties.Where(x => x.PropertyType.IsAssignableTo(typeof(IEnumerable)) && x.PropertyType.IsGenericType && x.PropertyType.GenericTypeArguments[0].IsAssignableFrom(valueType))
                    .ToList();
                var value = cursor.Value;
                var matchingCollection = compatibleCollections.FirstOrDefault(x => x.GetValue(parent) is IList list && list.Contains(value));
                if (matchingCollection != null)
                {
                    parentProp = matchingCollection;
                }
            }
            //var result = parentProp.Name == underlyingType.Name ? parentProp.
            return parentProp?.Name;
        }
    }

    public static string TrimString(string? input, int maxLength)
    {
        if (input == null || maxLength <= 0)
        {
            return string.Empty;
        }

        if (input.GetVisibleLength() > maxLength)
        {
            return input.Substring(0, maxLength) + "..." + AnsiColor.Reset.ToEscapeSequence();
        }

        return input;
    }
    class OutputLine
    {
        public required string Node { get; init; }
        public string Render { get; set; } = "";
        public string? Parent { get; set; }
        public string? Property { get; set; }
        public List<OutputLine> Children { get; } = new();
    }

}
public static class Extensions
{
    public static bool TryFindParent(this Cursor current, Func<object?, bool> predicate, out object? match)
    {
        var success = TryFindParent(current, cursor => predicate(cursor.Value), out var cursorMatch);
        match = cursorMatch?.Value;
        return success;
    }

    public static bool TryFindParent(this Cursor? current, Func<Cursor, bool> predicate, out Cursor? match)
    {
        match = null;
        if (current == null)
            return false;
        while (current is { IsRoot: false })
        {
            current = current.Parent;
            if (current is { IsRoot: false } && predicate(current))
            {
                match = current;
                return true;
            }

            ;
        }

        return false;
    }
}
