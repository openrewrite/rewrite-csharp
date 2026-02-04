using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NullableTypeTests : RewriteTest
{
    [Test]
    public void NullableFieldDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    object? nullableVal;
                }
                """
            )
        );
    }

    [Test]
    public void CommentsOnNullableFieldDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    private /*test*/ object/*test*/? /*test*/ nullableVal;
                }
                """
            )
        );
    }

    [Test]
    public void NullableValueType()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int? nullableInt;
                    bool? nullableBool;
                    double? nullableDouble;
                }
                """
            )
        );
    }

    [Test]
    public void NullableReferenceType()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    string? nullableString;
                    object? nullableObject;
                }
                """
            )
        );
    }

    [Test]
    public void NullableMethodParameter()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M(int? value) { }
                }
                """
            )
        );
    }

    [Test]
    public void NullableMethodReturn()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int? M() { return null; }
                }
                """
            )
        );
    }

    [Test]
    public void NullableInGeneric()
    {
        RewriteRun(
            CSharp(
                """
                using System.Collections.Generic;
                class Test {
                    List<int?> nullableList;
                }
                """
            )
        );
    }

    [Test]
    public void NullableArray()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int?[] nullableArray;
                }
                """
            )
        );
    }

    [Test]
    public void NullableLocalVariable()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M() {
                        int? x = null;
                        string? s = null;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void NullableProperty()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int? Value { get; set; }
                }
                """
            )
        );
    }

    [Test]
    public void NullableWithWhitespace()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int ? nullableWithSpace;
                }
                """
            )
        );
    }

    [Test]
    public void NullableInCast()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M(object o) {
                        var x = (int?)o;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void NullableInTypeOf()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M() {
                        var t = typeof(int?);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void NullableInDefault()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M() {
                        int? x = default(int?);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void NullableStructType()
    {
        RewriteRun(
            CSharp(
                """
                using System;
                class Test {
                    DateTime? nullableDateTime;
                    Guid? nullableGuid;
                }
                """
            )
        );
    }

    [Test]
    public void NullableInTernary()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M(bool condition) {
                        int? result = condition ? 1 : null;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void NullableCoalescing()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M(int? x) {
                        int y = x ?? 0;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void NullableHasValue()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M(int? x) {
                        bool hasValue = x.HasValue;
                        int value = x.Value;
                    }
                }
                """
            )
        );
    }
}
