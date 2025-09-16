using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace Rewrite.MSBuild;

public static class DiffHelper
{
    /// <summary>
    /// Compares two strings and returns line numbers (1-based) where differences occur.
    /// </summary>
    /// <param name="oldText">The original text.</param>
    /// <param name="newText">The modified text.</param>
    /// <returns>A list of line numbers that differ between the two texts.</returns>
    public static List<int> GetDifferentLineNumbers(string oldText, string newText)
    {
        var differ = new SideBySideDiffBuilder(new DiffPlex.Differ());
        var diff = differ.BuildDiffModel(oldText, newText);

        var differingLines = new List<int>();

        for (int i = 0; i < diff.OldText.Lines.Count; i++)
        {
            var oldLine = diff.OldText.Lines[i];
            var newLine = diff.NewText.Lines[i];

            if (oldLine.Type != ChangeType.Unchanged || newLine.Type != ChangeType.Unchanged)
            {
                differingLines.Add(i + 1); // +1 for 1-based line numbers
            }
        }

        return differingLines;
    }
}