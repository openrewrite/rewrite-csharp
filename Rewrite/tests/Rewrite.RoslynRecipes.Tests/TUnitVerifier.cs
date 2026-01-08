// using System;
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Diagnostics.CodeAnalysis;
// using System.Linq;
// using FluentAssertions;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.Testing;
//
// namespace Analyzers1.Tests;
//
// public class TUnitVerifier : IVerifier
// {
//     public TUnitVerifier()
//         : this(ImmutableStack<string>.Empty)
//     {
//     }
//
//     protected TUnitVerifier(ImmutableStack<string> context)
//     {
//         Context = context ?? throw new ArgumentNullException(nameof(context));
//     }
//
//     protected ImmutableStack<string> Context { get; }
//
//     public virtual void Empty<T>(string collectionName, IEnumerable<T> collection)
//     {
//         collection.Should().BeEmpty();
//     }
//
//     public virtual void Equal<T>(T expected, T actual, string? message = null)
//     {
//         actual.Should().Be(expected, message);
//     }
//
//     public virtual void True([DoesNotReturnIf(false)] bool assert, string? message = null)
//     {
//         assert.Should().BeTrue(message);
//     }
//
//     public virtual void False([DoesNotReturnIf(true)] bool assert, string? message = null)
//     {
//         assert.Should().BeFalse(message);
//     }
//
// #pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return.
//     [DoesNotReturn]
//     public virtual void Fail(string? message = null)
//     {
//         FluentAssertions.Execution.Execute.Assertion.FailWith(message);
//     }
// #pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return.
//
//     public virtual void LanguageIsSupported(string language)
//     {
//         language.Should().BeOneOf(LanguageNames.CSharp, LanguageNames.VisualBasic);
//     }
//
//     public virtual void NotEmpty<T>(string collectionName, IEnumerable<T> collection)
//     {
//         collection.Should().NotBeEmpty();
//     }
//
//     public virtual void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? equalityComparer = null, string? message = null)
//     {
//         var comparer = new SequenceEqualEnumerableEqualityComparer<T>(equalityComparer);
//         var areEqual = comparer.Equals(expected, actual);
//         if (!areEqual)
//         {
//             throw new InvalidOperationException("Sequences are not equal");
//         }
//     }
//
//     public virtual IVerifier PushContext(string context)
//     {
//         this.Should().BeOfType<TUnitVerifier>();
//         return new TUnitVerifier(Context.Push(context));
//     }
//
//
//     private sealed class SequenceEqualEnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>?>
//     {
//         private readonly IEqualityComparer<T> _itemEqualityComparer;
//
//         public SequenceEqualEnumerableEqualityComparer(IEqualityComparer<T>? itemEqualityComparer)
//         {
//             _itemEqualityComparer = itemEqualityComparer ?? EqualityComparer<T>.Default;
//         }
//
//         public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
//         {
//             if (ReferenceEquals(x, y))
//             {
//                 return true;
//             }
//
//             if (x is null || y is null)
//             {
//                 return false;
//             }
//
//             return x.SequenceEqual(y, _itemEqualityComparer);
//         }
//
//         public int GetHashCode(IEnumerable<T>? obj)
//         {
//             if (obj is null)
//             {
//                 return 0;
//             }
//
//             // From System.Tuple
//             //
//             // The suppression is required due to an invalid contract in IEqualityComparer<T>
//             // https://github.com/dotnet/runtime/issues/30998
//             return obj
//                 .Select(item => _itemEqualityComparer.GetHashCode(item!))
//                 .Aggregate(
//                     0,
//                     (aggHash, nextHash) => ((aggHash << 5) + aggHash) ^ nextHash);
//         }
//     }
// }