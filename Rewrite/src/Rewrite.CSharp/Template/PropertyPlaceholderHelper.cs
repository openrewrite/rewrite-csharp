namespace Rewrite.RewriteCSharp;

using System;
using System.Collections.Generic;
using System.Text;

public class PropertyPlaceholderHelper
{
    private static readonly Dictionary<string, string> WellKnownSimplePrefixes = new Dictionary<string, string>(4);

    static PropertyPlaceholderHelper()
    {
        WellKnownSimplePrefixes.Add("}", "{");
        WellKnownSimplePrefixes.Add("]", "[");
        WellKnownSimplePrefixes.Add(")", "(");
    }

    private readonly string _placeholderPrefix;
    private readonly string _placeholderSuffix;
    private readonly string _simplePrefix;
    private readonly string? _valueSeparator;

    /// <summary>
    /// Creates a new PropertyPlaceholderHelper with the specified placeholder prefix and suffix.
    /// </summary>
    /// <param name="placeholderPrefix">The prefix that denotes the start of a placeholder.</param>
    /// <param name="placeholderSuffix">The suffix that denotes the end of a placeholder.</param>
    /// <param name="valueSeparator">The separator that separates the placeholder name from the default value.</param>
    public PropertyPlaceholderHelper(string placeholderPrefix, string placeholderSuffix, 
                                     string? valueSeparator = null)
    {
        _placeholderPrefix = placeholderPrefix;
        _placeholderSuffix = placeholderSuffix;
        WellKnownSimplePrefixes.TryGetValue(_placeholderSuffix, out var simplePrefixForSuffix);
        
        if (simplePrefixForSuffix != null && _placeholderPrefix.EndsWith(simplePrefixForSuffix))
        {
            _simplePrefix = simplePrefixForSuffix;
        }
        else
        {
            _simplePrefix = _placeholderPrefix;
        }
        
        _valueSeparator = valueSeparator;
    }

    /// <summary>
    /// Checks if the given value contains placeholders.
    /// </summary>
    /// <param name="value">The value to check for placeholders.</param>
    /// <returns>True if the value contains placeholders, false otherwise.</returns>
    public bool HasPlaceholders(string? value)
    {
        if (value == null)
        {
            return false;
        }
        
        int startIndex = value.IndexOf(_placeholderPrefix, StringComparison.Ordinal);
        return startIndex > -1 && value.IndexOf(_placeholderSuffix, startIndex, StringComparison.Ordinal) > startIndex;
    }

    /// <summary>
    /// Replaces placeholders in the given value using properties.
    /// </summary>
    /// <param name="value">The value containing placeholders to be replaced.</param>
    /// <param name="properties">The properties used for replacement.</param>
    /// <returns>The value with placeholders replaced.</returns>
    public string ReplacePlaceholders(string value, System.Collections.Specialized.NameValueCollection properties)
    {
        return ReplacePlaceholders(value, key => properties[key]);
    }

    /// <summary>
    /// Replaces placeholders in the given value using a placeholder resolver function.
    /// </summary>
    /// <param name="value">The value containing placeholders to be replaced.</param>
    /// <param name="placeholderResolver">The function to resolve placeholders.</param>
    /// <returns>The value with placeholders replaced.</returns>
    public string ReplacePlaceholders(string value, Func<string, string?> placeholderResolver)
    {
        return ParseStringValue(value, placeholderResolver, null);
    }

    /// <summary>
    /// Parses the string value and resolves placeholders.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="placeholderResolver">The function to resolve placeholders.</param>
    /// <param name="visitedPlaceholders">Set of already visited placeholders to prevent circular references.</param>
    /// <returns>The parsed string with resolved placeholders.</returns>
    protected string ParseStringValue(string value, Func<string, string?> placeholderResolver, HashSet<string>? visitedPlaceholders)
    {
        int startIndex = value.IndexOf(_placeholderPrefix, StringComparison.Ordinal);
        if (startIndex == -1)
        {
            return value;
        }

        StringBuilder result = new StringBuilder(value);
        while (startIndex != -1)
        {
            int endIndex = FindPlaceholderEndIndex(result.ToString(), startIndex);
            if (endIndex != -1)
            {
                string placeholder = result.ToString(startIndex + _placeholderPrefix.Length, 
                                                    endIndex - startIndex - _placeholderPrefix.Length);
                string originalPlaceholder = placeholder;
                if (visitedPlaceholders == null)
                {
                    visitedPlaceholders = new HashSet<string>(4);
                }
                if (!visitedPlaceholders.Add(originalPlaceholder))
                {
                    throw new ArgumentException(
                            "Circular placeholder reference '" + originalPlaceholder + "' in property definitions");
                }
                // Recursive invocation, parsing placeholders contained in the placeholder key.
                placeholder = ParseStringValue(placeholder, placeholderResolver, visitedPlaceholders);
                // Now obtain the value for the fully resolved key...
                var propVal = placeholderResolver(placeholder);
                if (propVal == null && _valueSeparator != null)
                {
                    int separatorIndex = placeholder.IndexOf(_valueSeparator, StringComparison.Ordinal);
                    if (separatorIndex != -1)
                    {
                        string actualPlaceholder = placeholder.Substring(0, separatorIndex);
                        string defaultValue = placeholder.Substring(separatorIndex + _valueSeparator.Length);
                        propVal = placeholderResolver(actualPlaceholder);
                        if (propVal == null)
                        {
                            propVal = defaultValue;
                        }
                    }
                }
                if (propVal != null)
                {
                    // Recursive invocation, parsing placeholders contained in the
                    // previously resolved placeholder value.
                    propVal = ParseStringValue(propVal, placeholderResolver, visitedPlaceholders);
                    result.Remove(startIndex, endIndex + _placeholderSuffix.Length - startIndex);
                    result.Insert(startIndex, propVal);

                    if (propVal.Length < endIndex - startIndex + 1)
                    {
                        endIndex = startIndex + propVal.Length;
                    }
                }

                // Proceed with unprocessed value.
                startIndex = result.ToString().IndexOf(_placeholderPrefix, endIndex, StringComparison.Ordinal);
                visitedPlaceholders.Remove(originalPlaceholder);
            }
            else
            {
                startIndex = -1;
            }
        }
        return result.ToString();
    }

    private int FindPlaceholderEndIndex(string buf, int startIndex)
    {
        int index = startIndex + _placeholderPrefix.Length;
        int withinNestedPlaceholder = 0;
        while (index < buf.Length)
        {
            if (SubstringMatch(buf, index, _placeholderSuffix))
            {
                if (withinNestedPlaceholder > 0)
                {
                    withinNestedPlaceholder--;
                    index = index + _placeholderSuffix.Length;
                }
                else
                {
                    return index;
                }
            }
            else if (SubstringMatch(buf, index, _simplePrefix))
            {
                withinNestedPlaceholder++;
                index = index + _simplePrefix.Length;
            }
            else
            {
                index++;
            }
        }
        return -1;
    }

    private static bool SubstringMatch(string str, int index, string substring)
    {
        if (index + substring.Length > str.Length)
        {
            return false;
        }
        for (int i = 0; i < substring.Length; i++)
        {
            if (str[index + i] != substring[i])
            {
                return false;
            }
        }
        return true;
    }
}