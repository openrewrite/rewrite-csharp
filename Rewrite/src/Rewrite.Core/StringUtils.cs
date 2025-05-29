using System.Text;
using System.Text.RegularExpressions;

namespace Rewrite.Core;

public static class StringUtils
{
    private static readonly Regex LINE_BREAK = new Regex("\\r");

    public static string? TrimIndentPreserveCRLF(string? text)
    {
        if (text == null)
        {
            //noinspection DataFlowIssue
            return null;
        }
        return TrimIndent((text.EndsWith("\r\n") ? text.Substring(0, text.Length - 2) : text)
                .Replace('\r', '⏎'))
                .Replace('⏎', '\r');
    }

    /// <summary>
    /// Detects a common minimal indent of all the input lines and removes the indent from each line.
    /// <para>
    /// This is modeled after Kotlin's trimIndent and is useful for pruning the indent from multi-line text blocks.
    /// </para>
    /// <para>
    /// Note: Blank lines do not affect the detected indent level.
    /// </para>
    /// </summary>
    /// <param name="text">A string that have a common indention</param>
    /// <returns>A mutated version of the string that removed the common indention.</returns>
    public static string TrimIndent(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        int indentLevel = MinCommonIndentLevel(text);

        // The logic for trimming the start of the string is consistent with the functionality of Kotlin's trimIndent.
        char startChar = text[0];
        int start = 0;
        if (startChar == '\n' || startChar == '\r')
        {
            //If the string starts with a line break, always trim it.
            int i = 1;
            for (; i < text.Length; i++)
            {
                char c = text[i];
                if (!char.IsWhiteSpace(c))
                {
                    //If there is any non-whitespace on the first line, do not trim the line.
                    start = 1;
                    break;
                }
                else if (c == '\n' || c == '\r')
                {
                    if (i - 1 <= indentLevel)
                    {
                        //If the first line is only whitespace and the line size is less than indent size, trim it.
                        start = i;
                    }
                    else
                    {
                        //If the line size is equal or greater than indent, do not trim the line.
                        start = 1;
                    }
                    break;
                }
            }
        }

        //If the last line of the string is only whitespace, trim it.
        int end = text.Length - 1;
        while (end > start)
        {
            char endChar = text[end];
            if (!char.IsWhiteSpace(endChar))
            {
                end = text.Length;
                break;
            }
            else if (endChar == '\n' || endChar == '\r')
            {
                break;
            }
            end--;
        }
        if (end == start)
        {
            end++;
        }
        StringBuilder trimmed = new StringBuilder();
        for (int i = start; i < end; i++)
        {
            int j = i;
            for (; j < end; j++)
            {
                char c = text[j];
                if (c == '\r' || c == '\n')
                {
                    trimmed.Append(c);
                    break;
                }
                if (j - i >= indentLevel)
                {
                    trimmed.Append(c);
                }
            }
            i = j;
        }

        return trimmed.ToString();
    }

    /// <summary>
    /// This method will count the number of white space characters that precede any content for each line contained
    /// in string. It will not compute a white space count for a line, if the entire line is blank (only made up of white
    /// space characters).
    /// <para></para>
    /// <para>
    /// It will compute the minimum common number of white spaces across all lines and return that minimum.
    /// </para>
    /// </summary>
    /// <param name="text">A string with zero or more line breaks.</param>
    /// <returns>The minimum count of white space characters preceding each line of content.</returns>
    public static int MinCommonIndentLevel(string text)
    {
        int minIndent = int.MaxValue;
        int whiteSpaceCount = 0;
        bool contentEncountered = false;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '\n' || c == '\r')
            {
                if (contentEncountered)
                {
                    minIndent = Math.Min(whiteSpaceCount, minIndent);
                    if (minIndent == 0)
                    {
                        break;
                    }
                }
                whiteSpaceCount = 0;
                contentEncountered = false;
            }
            else if (!contentEncountered && char.IsWhiteSpace(c))
            {
                whiteSpaceCount++;
            }
            else
            {
                contentEncountered = true;
            }
        }
        if (contentEncountered)
        {
            minIndent = Math.Min(whiteSpaceCount, minIndent);
        }
        return minIndent;
    }

    /// <summary>
    /// Check if the String is null or has only whitespaces.
    /// <para>
    /// Modified from apache commons lang StringUtils.
    /// </para>
    /// </summary>
    /// <param name="string">String to check</param>
    /// <returns><c>true</c> if the String is null or has only whitespaces</returns>
    public static bool IsBlank(string @string)
    {
        if (@string == null || @string.Length == 0)
        {
            return true;
        }
        for (int i = 0; i < @string.Length; i++)
        {
            if (!char.IsWhiteSpace(@string[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Check if the String is empty string or null.
    /// </summary>
    /// <param name="string">String to check</param>
    /// <returns><c>true</c> if the String is null or empty string</returns>
    public static bool IsNullOrEmpty(string @string)
    {
        return @string == null || @string.Length == 0;
    }

    public static bool IsNotEmpty(string @string)
    {
        return @string != null && @string.Length > 0;
    }

    public static string ReadFully(Stream inputStream)
    {
        if (inputStream == null)
        {
            return "";
        }
        return ReadFully(inputStream, Encoding.UTF8);
    }

    /// <summary>
    /// If the input stream is coming from a stream with an unknown encoding, use
    /// EncodingDetectingInputStream.ReadFully() instead.
    /// </summary>
    /// <param name="inputStream">An input stream.</param>
    /// <param name="encoding">The encoding to use</param>
    /// <returns>the full contents of the input stream interpreted as a string of the specified encoding</returns>
    public static string ReadFully(Stream inputStream, Encoding encoding)
    {
        try
        {
            using (Stream is_ = inputStream)
            {
                using (MemoryStream bos = new MemoryStream())
                {
                    byte[] buffer = new byte[4096];
                    int n;
                    while ((n = is_.Read(buffer, 0, buffer.Length)) != -1)
                    {
                        bos.Write(buffer, 0, n);
                    }

                    byte[] bytes = bos.ToArray();
                    return encoding.GetString(bytes);
                }
            }
        }
        catch (IOException e)
        {
            throw new NotSupportedException(e.Message, e);
        }
    }

    public static string Capitalize(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        return char.ToUpper(value[0]) +
               value.Substring(1);
    }

    public static string Uncapitalize(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        return char.ToLower(value[0]) + value.Substring(1);
    }

    public static bool ContainsOnlyWhitespaceAndComments(string text)
    {
        int i = 0;
        char[] chars = text.ToCharArray();
        bool inSingleLineComment = false;
        bool inMultilineComment = false;
        while (i < chars.Length)
        {
            char c = chars[i];
            if (inSingleLineComment && c == '\n')
            {
                inSingleLineComment = false;
                continue;
            }
            if (i < chars.Length - 1)
            {
                string s = c.ToString() + chars[i + 1];
                switch (s)
                {
                    case "//":
                        {
                            inSingleLineComment = true;
                            i += 2;
                            continue;
                        }
                    case "/*":
                        {
                            inMultilineComment = true;
                            i += 2;
                            continue;
                        }
                    case "*/":
                        {
                            inMultilineComment = false;
                            i += 2;
                            continue;
                        }
                }
            }
            if (!inSingleLineComment && !inMultilineComment && !char.IsWhiteSpace(c))
            {
                return false;
            }
            i++;
        }
        return true;
    }

    public static int IndexOfNonWhitespace(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (!(c == ' ' || c == '\t' || c == '\n' || c == '\r'))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="text">Text to scan</param>
    /// <param name="test">The predicate to match</param>
    /// <returns>The index of the first character for which the predicate returns <c>true</c>,
    /// or <c>-1</c> if no character in the string matches the predicate.</returns>
    public static int IndexOf(string text, Predicate<char> test)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (test(text[i]))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Return the number of times a substring occurs within a target string.
    /// </summary>
    /// <param name="text">A target string</param>
    /// <param name="substring">The substring to search for</param>
    /// <returns>the number of times the substring is found in the target. 0 if no occurrences are found.</returns>
    public static int CountOccurrences(string text, string substring)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(substring))
        {
            return 0;
        }

        int count = 0;
        for (int index = text.IndexOf(substring); index >= 0; index = text.IndexOf(substring, index + substring.Length))
        {
            count++;
        }
        return count;
    }

    /// <summary>
    /// This method will search and replace the first occurrence of a matching substring. There is a a replaceFirst method
    /// on the String class but that version leverages regular expressions and is a magnitude slower than this simple
    /// replacement.
    /// </summary>
    /// <param name="text">The source string to search</param>
    /// <param name="match">The substring that is being searched for</param>
    /// <param name="replacement">The replacement.</param>
    /// <returns>The original string with the first occurrence replaced or the original text if a match is not found.</returns>
    public static string ReplaceFirst(string text, string match, string replacement)
    {
        int start = text.IndexOf(match);
        if (string.IsNullOrEmpty(match) || string.IsNullOrEmpty(text) || start == -1)
        {
            return text;
        }
        else
        {
            StringBuilder newValue = new StringBuilder(text.Length);
            newValue.Append(text, 0, start);
            newValue.Append(replacement);
            int end = start + match.Length;
            if (end < text.Length)
            {
                newValue.Append(text, end, text.Length - end);
            }
            return newValue.ToString();
        }
    }

    public static string Repeat(string s, int count)
    {
        if (count == 1)
        {
            return s;
        }

        byte[] value = Encoding.UTF8.GetBytes(s);
        int len = value.Length;
        if (len == 0 || count == 0)
        {
            return "";
        }
        if (len == 1)
        {
            byte[] single = new byte[count];
            for (int i = 0; i < count; i++)
            {
                single[i] = value[0];
            }
            return Encoding.UTF8.GetString(single);
        }
        int limit = len * count;
        byte[] multiple = new byte[limit];
        Buffer.BlockCopy(value, 0, multiple, 0, len);
        int copied = len;
        for (; copied < limit - copied; copied <<= 1)
        {
            Buffer.BlockCopy(multiple, 0, multiple, copied, copied);
        }
        Buffer.BlockCopy(multiple, 0, multiple, copied, limit - copied);
        return Encoding.UTF8.GetString(multiple);
    }


    private static readonly char wrongFileSeparatorChar = Path.DirectorySeparatorChar == '/' ? '\\' : '/';

    
    private static bool AllStars(string chars, int start, int end)
    {
        for (int i = start; i <= end; ++i)
        {
            if (chars[i] != '*')
            {
                return false;
            }
        }
        return true;
    }

    private static bool Different(bool caseSensitive, char ch, char other)
    {
        return caseSensitive ?
                ch != other :
                char.ToUpper(ch) != char.ToUpper(other);
    }

    public static string Indent(string text)
    {
        StringBuilder indent = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '\n' || c == '\r')
            {
                return indent.ToString();
            }
            else if (char.IsWhiteSpace(c))
            {
                indent.Append(c);
            }
            else
            {
                return indent.ToString();
            }
        }
        return indent.ToString();
    }

    /// <summary>
    /// Locate the greatest common margin of a multi-line string
    /// </summary>
    /// <param name="multiline">A string of one or more lines.</param>
    /// <returns>The greatest common margin consisting only of whitespace characters.</returns>
    public static string GreatestCommonMargin(string multiline)
    {
        string? gcm = null;
        StringBuilder margin = new StringBuilder();
        bool skipRestOfLine = false;
        char[] charArray = multiline.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];
            if (c == '\n')
            {
                if (i < charArray.Length - 1 && charArray[i + 1] == '\n')
                {
                    i++;
                    continue;
                }
                else if (i > 0)
                {
                    if (margin.Length == 0)
                    {
                        return "";
                    }
                    else
                    {
                        gcm = CommonMargin(gcm, margin);
                        margin = new StringBuilder();
                    }
                }
                skipRestOfLine = false;
            }
            else if (char.IsWhiteSpace(c) && !skipRestOfLine)
            {
                margin.Append(c);
            }
            else
            {
                skipRestOfLine = true;
            }
        }
        return gcm == null ? "" : gcm;
    }

    public static string CommonMargin(string? s1, StringBuilder s2)
    {
        if (s1 == null)
        {
            string s = s2.ToString();
            return s.Substring(s.LastIndexOf('\n') + 1);
        }
        for (int i = 0; i < s1.Length && i < s2.Length; i++)
        {
            if (s1[i] != s2[i] || !char.IsWhiteSpace(s1[i]))
            {
                return s1.ToString().Substring(0, i);
            }
        }
        return s2.Length < s1.Length ? s2.ToString() : s1.ToString();
    }

    public static bool IsNumeric(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        int sz = str.Length;
        for (int i = 0; i < sz; i++)
        {
            if (!char.IsDigit(str[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// See <a href="https://eclipse.org/aspectj/doc/next/progguide/semantics-pointcuts.html#type-patterns">https://eclipse.org/aspectj/doc/next/progguide/semantics-pointcuts.html#type-patterns</a>
    /// <para>
    /// An embedded * in an identifier matches any sequence of characters, but
    /// does not match the package (or inner-type) separator ".".
    /// </para>
    /// <para>
    /// The ".." wildcard matches any sequence of characters that start and end with a ".", so it can be used to pick out all
    /// types in any subpackage, or all inner types. e.g. <code>within(com.xerox..*)</code> picks out all join points where
    /// the code is in any declaration of a type whose name begins with "com.xerox.".
    /// </para>
    /// </summary>
    public static string AspectjNameToPattern(string name)
    {
        int length = name.Length;
        StringBuilder sb = new StringBuilder(length);
        char prev = (char)0;
        for (int i = 0; i < length; i++)
        {
            bool isLast = i == length - 1;
            char c = name[i];
            switch (c)
            {
                case '.':
                    if (prev != '.' && (isLast || name[i + 1] != '.'))
                    {
                        sb.Append("[.$]");
                    }
                    else if (prev == '.')
                    {
                        sb.Append("\\.(.+\\.)?");
                    }
                    break;
                case '*':
                    sb.Append("[^.]*");
                    break;
                case '$':
                case '[':
                case ']':
                    sb.Append('\\');
                    // fall-through
                    goto default;
                default:
                    sb.Append(c);
                    break;
            }
            prev = c;
        }
        return sb.ToString();
    }

    /// <summary>
    /// </summary>
    /// <param name="s1">first string</param>
    /// <param name="s2">second string</param>
    /// <returns>length of the longest substring common to both strings</returns>
    /// <see href="https://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Longest_common_substring#Java_-_O(n)_storage"/>
    public static int GreatestCommonSubstringLength(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
        {
            return 0;
        }

        int m = s1.Length;
        int n = s2.Length;
        int cost;
        int maxLen = 0;
        int[] p = new int[n];
        int[] d = new int[n];

        for (int i = 0; i < m; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                // calculate cost/score
                if (s1[i] != s2[j])
                {
                    cost = 0;
                }
                else
                {
                    if ((i == 0) || (j == 0))
                    {
                        cost = 1;
                    }
                    else
                    {
                        cost = p[j - 1] + 1;
                    }
                }
                d[j] = cost;

                if (cost > maxLen)
                {
                    maxLen = cost;
                }
            } // for {}

            int[] swap = p;
            p = d;
            d = swap;
        }

        return maxLen;
    }

    /// <summary>
    /// </summary>
    /// <returns>Considering C-style comments to be whitespace, return the index of the next non-whitespace character.</returns>
    public static int IndexOfNextNonWhitespace(int cursor, string source)
    {
        bool inMultiLineComment = false;
        bool inSingleLineComment = false;

        int length = source.Length;
        for (; cursor < length; cursor++)
        {
            char current = source[cursor];
            if (inSingleLineComment)
            {
                inSingleLineComment = current != '\n';
                continue;
            }
            else if (length > cursor + 1)
            {
                char next = source[cursor + 1];
                if (inMultiLineComment)
                {
                    if (current == '*' && next == '/')
                    {
                        inMultiLineComment = false;
                        cursor++;
                        continue;
                    }
                }
                else if (current == '/' && next == '/')
                {
                    inSingleLineComment = true;
                    cursor++;
                    continue;
                }
                else if (current == '/' && next == '*')
                {
                    inMultiLineComment = true;
                    cursor++;
                    continue;
                }
            }
            if (!inMultiLineComment && !char.IsWhiteSpace(current))
            {
                break; // found it!
            }
        }
        return cursor;
    }

    public static string FormatUriForPropertiesFile(string uri)
    {
        return Regex.Replace(uri, "(?<!\\\\)://", "\\\\://");
    }

    public static bool HasLineBreak(string s)
    {
        return s != null && LINE_BREAK.IsMatch(s);
    }

    public static bool ContainsWhitespace(string s)
    {
        for (int i = 0; i < s.Length; ++i)
        {
            if (char.IsWhiteSpace(s[i]))
            {
                return true;
            }
        }

        return false;
    }
}