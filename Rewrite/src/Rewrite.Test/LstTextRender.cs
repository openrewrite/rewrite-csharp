using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Text;
using NMica.Utils;
using Rewrite.Core;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Socolin.ANSITerminalColor;
using Spectre.Console;
using Spectre.Console.Rendering;
using Tree = Rewrite.Core.Tree;

namespace Rewrite.Test;

public static class LstTextRender
{
   
    public static Table RenderLstTree(this Tree tree)
    {
        var walker = new Walker();
        var graph = new Dictionary<Guid, OutputLine>();
        walker.Visit(tree, graph);

        var table = new Table()
            .AddColumn("Tree")
            .AddColumn("Render", c => c.NoWrap())
            .AddColumn("ParentProperty");
        var rowData = RenderTreeLines(graph.Values.First(), "", false).ToList();
        foreach (var item in rowData)
        {
            var codeSegmentSelector = item.Render.Segments.Where(x => x.SegmentType == MarkupString.SegmentType.Code).ToList();
            var firstCodeSegment = codeSegmentSelector.First();
            var lastCodeSegment = codeSegmentSelector.Last();
            firstCodeSegment.Value = firstCodeSegment.Value.TrimStart();
            lastCodeSegment.Value = lastCodeSegment.Value.TrimEnd();
        }
        foreach (var row in rowData)
        {
            var code = row.Render
                .Truncate(100)
                .ToString()
                .SingularSpace()
                .LineBreaksAsSpaces();
            table.AddRow(row.NodeTreeRender, code, row.Property ?? "");
        }

        return table;
    }

    static IEnumerable<OutputLine> RenderTreeLines(OutputLine tree, string indent, bool last)
    {
        tree.NodeTreeRender = indent + "+-" + tree.Node;
        yield return tree;

        indent += last ? "  " : "| ";

        for (int i = 0; i < tree.Children.Count; i++)
        {
            foreach (var child in RenderTreeLines(tree.Children[i], indent, i == tree.Children.Count - 1))
                yield return child;
        }
    }
    
   

    class SyntaxHighlighter(Tree targetNode) : CSharpPrinter<MarkupPrintCapture, object>
    {
        public override J? PreVisit(Tree? tree, MarkupPrintCapture p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {

            if (tree?.Id == targetNode.Id)
            {
                p.StartHighlight("green");
            }
            return tree as J;
        }
        public override J PostVisit(Tree tree, MarkupPrintCapture p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            if (tree.Id == targetNode.Id)
            {
                p.EndHighlight();
            }
            return (J)tree;
        }
        public static MarkupString Highlight(Tree root, Tree node)
        {
            MarkupPrintCapture p = new();
            var visitor = new SyntaxHighlighter(node).Visit(root, p);
            return new MarkupString(p.ToString(), p.Highlights);
        }

    }

    internal class MarkupPrintCapture() : PrintOutputCapture<object>(new())
    {
        private HighlightSpan? _currentHighlight;
        public List<HighlightSpan> Highlights { get; } = new();
        private StringBuilder _codeBuffer = new();


        public void StartHighlight(string metadata)
        {
            _currentHighlight = new(metadata, Length, 0);
        }

        public void EndHighlight()
        {
            var highlight = _currentHighlight! with { Length = Length - _currentHighlight.Start };
            Highlights.Add(highlight);
            _currentHighlight = null;
        }
       
    }

    public class MarkupString : HighlightString
    {
        public enum SegmentType
        {
            Code,
            Markup
        }

        public class Segment(SegmentType segmentType, string value)
        {
            public SegmentType SegmentType { get; set; } = segmentType;
            public string Value { get; set; } = value;

            public void Deconstruct(out SegmentType segmentType, out string value)
            {
                segmentType = this.SegmentType;
                value = this.Value;
            }
        }

        public List<Segment> Segments { get; } = new();
        public MarkupString(string text, List<HighlightSpan> highlights) : base(text, highlights)
        {
            ComputeSegments();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var segment in Segments)
            {
                if (segment.SegmentType == SegmentType.Code)
                    sb.Append(Markup.Escape(segment.Value));
                else
                    sb.Append(segment.Value);
            }
            return sb.ToString();
        }

        private void ComputeSegments()
        {
            // if (Highlights.Count == 0)
            //     return Text;

            var events = new List<(int Position, bool IsStart, HighlightSpan Span)>();

            foreach (var span in Highlights)
            {
                // Skip spans with no width
                if (span.Length <= 0)
                    continue;

                events.Add((span.Start, true, span));
                events.Add((span.End, false, span));
            }

            events.Sort((a, b) =>
            {
                int cmp = a.Position.CompareTo(b.Position);
                if (cmp != 0)
                    return cmp;

                // Close before open if at same position
                if (a.IsStart == b.IsStart)
                    return 0;

                return a.IsStart ? 1 : -1;
            });

            // var result = new System.Text.StringBuilder();
            int currentIndex = 0;
            var openTags = new Stack<string>();

            foreach (var (position, isStart, span) in events)
            {
                if (position > currentIndex)
                {
                    Segments.Add(new(SegmentType.Code, Text.Substring(currentIndex, position - currentIndex)));
                    currentIndex = position;
                }

                if (isStart)
                {
                    Segments.Add(new(SegmentType.Markup, $"[{span.HighlightMetadata}]"));
                    openTags.Push(span.HighlightMetadata);
                }
                else
                {
                    // Close only the matching tag
                    var tempStack = new Stack<string>();
                    while (openTags.Count > 0)
                    {
                        var tag = openTags.Pop();
                        Segments.Add(new(SegmentType.Markup, "[/]"));
                        if (tag == span.HighlightMetadata)
                            break;
                        tempStack.Push(tag);
                    }

                    while (tempStack.Count > 0)
                    {
                        var tag = tempStack.Pop();
                        Segments.Add(new(SegmentType.Markup, $"[{tag}]"));
                        openTags.Push(tag);
                    }
                }
            }

            if (currentIndex < Text.Length)
                Segments.Add(new(SegmentType.Code, Text.Substring(currentIndex)));

            while (openTags.Count > 0)
            {
                Segments.Add(new(SegmentType.Markup, "[/]"));
                openTags.Pop();
            }

        }

    }

    public class HighlightString(string text, List<HighlightSpan> highlights)
    {
        public string Text { get; } = text;
        protected List<HighlightSpan> Highlights { get; } = highlights;

        public override string ToString() => Text;
    }

    public record HighlightSpan(string HighlightMetadata, int Start, int Length)
    {
        public int End => Start + Length;
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
        public override J? PreVisit(Tree? tree, Dictionary<Guid, OutputLine> p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
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

    internal class OutputLine
    {
        public required string Node { get; init; }
        public string NodeTreeRender { get; set; } = "";
        public MarkupString Render { get; set; } = new("",new());
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
    
    private static readonly Regex RepeatingSpaceRegex = new (@"\s{2,}", RegexOptions.Compiled);
    private static readonly Regex LineBreaksRegex = new (@"[\r\n]+", RegexOptions.Compiled | RegexOptions.Multiline);
    /// <summary>
    /// Reduce all repeating whitespace to a single space character
    /// </summary>
    /// <returns></returns>
    public static string SingularSpace(this string s) => RepeatingSpaceRegex.Replace(s, " ");
    public static string LineBreaksAsSpaces(this string s) => LineBreaksRegex.Replace(s, " ");
    
    public static LstTextRender.MarkupString Truncate(this LstTextRender.MarkupString source, int maxVisibleLength)
    {
        var newSegments = new List<LstTextRender.MarkupString.Segment>();
        int currentLength = 0;
        bool done = false;

        foreach (var segment in source.Segments)
        {
            if (segment.SegmentType == LstTextRender.MarkupString.SegmentType.Markup)
            {
                // Always preserve markup (including closing tags after truncation)
                newSegments.Add(new(segment.SegmentType, segment.Value));
                continue;
            }

            if (done)
            {
                // We've already hit the limit, skip remaining code segments
                continue;
            }

            var segmentLength = segment.Value.Length;

            if (currentLength + segmentLength <= maxVisibleLength)
            {
                // Include full code segment
                newSegments.Add(new(segment.SegmentType, segment.Value));
                currentLength += segmentLength;
            }
            else
            {
                // Truncate this segment
                int remaining = maxVisibleLength - currentLength;
                if (remaining > 0)
                {
                    newSegments.Add(new(LstTextRender.MarkupString.SegmentType.Code, segment.Value.Substring(0, remaining)));
                }
                done = true;
            }
        }

        var result = new LstTextRender.MarkupString(string.Empty, []);
        result.Segments.Clear();
        result.Segments.AddRange(newSegments);
        return result;
    }
}
