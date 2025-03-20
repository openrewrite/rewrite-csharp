namespace Rewrite.RewriteCSharp.Format;

internal record FormatStyle(string Indentation, string NewLine)
    {
        const string DefaultNewLine = "\r\n";
        public bool UseTabs => Indentation == "\t";
        public int IdentationSize => Indentation.Length;

        public static FormatStyle DetectStyle(string code)
        {

            if (string.IsNullOrWhiteSpace(code))
                return new FormatStyle(" ", DefaultNewLine); // Default to space and LF

            // Detect line ending type
            string endOfLine = code.Contains("\n") ? (code.Contains("\r\n") ? "\r\n" : "\n") : DefaultNewLine ;

            // Split lines
            var lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var spaceIndentLevels = new List<int>();
            bool usesTabs = false;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                int leadingSpaces = line.TakeWhile(c => c == ' ').Count();
                int leadingTabs = line.TakeWhile(c => c == '\t').Count();

                if (leadingTabs > 0)
                {
                    usesTabs = true;
                    break; // Found a tab, so the file likely uses tabs
                }

                if (leadingSpaces > 0)
                    spaceIndentLevels.Add(leadingSpaces);
            }

            if (usesTabs)
                return new FormatStyle("\t", endOfLine);

            if (spaceIndentLevels.Count < 2)
                return new FormatStyle("    ", endOfLine); // Default to 4 spaces if not enough data

            // Find the most common spacing step
            var distinctSteps = spaceIndentLevels.Distinct().OrderBy(x => x).ToList();
            var spacingDifferences = new List<int>();

            for (int i = 1; i < distinctSteps.Count; i++)
                spacingDifferences.Add(distinctSteps[i] - distinctSteps[i - 1]);

            int detectedTabSize = spacingDifferences.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault(4); // Default to 4 spaces if unclear

            return new FormatStyle(new string(' ', detectedTabSize), endOfLine);
        }
    }
