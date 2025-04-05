using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Rewrite.Analyzers;

public class SymbolEqualityComparer<T> : IEqualityComparer<T> where T : ISymbol
{
    public static readonly SymbolEqualityComparer<T> Default = new();
    public bool Equals(T? x, T? y) => SymbolEqualityComparer.Default.Equals(x, y);

    public int GetHashCode(T obj) => SymbolEqualityComparer.Default.GetHashCode(obj);
}