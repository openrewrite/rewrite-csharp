using System.Text.RegularExpressions;

namespace Rewrite.Analyzers.Authoring
{
    /// <summary>
    /// Provides a thread-local indentation level tracker.
    /// Useful for scenarios like structured logging or tracing nested operations.
    /// </summary>
    public static class IndentationContext
    {
        [ThreadStatic]
        private static int _indentLevel;

        private const int IdentSize = 4;

        [ThreadStatic]
        private static string? _currentIndent;

        /// <summary>
        /// Gets the current indentation level for the executing thread.
        /// </summary>
        public static int CurrentLevel
        {
            get => _indentLevel;
            set
            {
                _indentLevel = value;
                ComputeIndent();
            }
        }

        public static void Reset()
        {
             _indentLevel = 0;
             ComputeIndent();
        }

        /// <summary>
        /// Gets the current indentation string for the executing thread.
        /// </summary>
        public static string CurrentIndent => _currentIndent ??= ComputeIndent();

        /// <summary>
        /// Increases the current indentation level by one and returns a disposable that will decrease it on dispose.
        /// </summary> 
        /// <returns>An <see cref="IDisposable"/> that will decrease the indentation level when disposed.</returns>
        public static IDisposable Increase()
        {
            _indentLevel++;
            _currentIndent = ComputeIndent();
            return new IndentScope();
        }
        private static Regex _lineBreakSplitRegex = new Regex("\r\n|\r|\n");
        public static string RenderIndented(IEnumerable<string> lines) => RenderIndented(lines, CurrentIndent);
        public static string RenderIndented(IEnumerable<string> lines, string ident) => string.Join("\n", lines.Select(line => $"{ident}{line.TrimStart()}"));
        public static string RenderIndented(string input) => RenderIndented(input, CurrentIndent);
        
        public static string RenderIndented(string input, string ident)
        {
            return RenderIndented(_lineBreakSplitRegex.Split(input), ident);
        }

        private static string ComputeIndent()
        {
            return _currentIndent = new string(' ', _indentLevel * IdentSize);
        }
        
        private sealed class IndentScope : IDisposable
        {
            private bool _disposed;

            public void Dispose()
            {
                if (_disposed)
                    return;

                if (_indentLevel > 0)
                {
                    _indentLevel--;
                    _currentIndent = ComputeIndent();
                }

                _disposed = true;
            }
        }
    }
}