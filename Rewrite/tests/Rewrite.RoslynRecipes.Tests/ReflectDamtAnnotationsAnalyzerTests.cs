using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.ReflectDamtAnnotationsAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class ReflectDamtAnnotationsAnalyzerTests
{
    // ========================================================================
    // Positive cases
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when a class implementing IReflect
    /// uses DynamicallyAccessedMemberTypes.All on the class declaration.
    /// </summary>
    [Test]
    public async Task IReflectClass_WithAll_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            [{|ORNETX0022:DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)|}]
            class MyReflect : IReflect
            {
                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when a class deriving from TypeDelegator
    /// (which derives from TypeInfo, which derives from Type) uses DynamicallyAccessedMemberTypes.All.
    /// </summary>
    [Test]
    public async Task TypeDelegatorClass_WithAll_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Reflection;

            [{|ORNETX0022:DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)|}]
            class MyType : TypeDelegator
            {
                public MyType() : base(typeof(object)) { }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when a struct implementing IReflect
    /// uses DynamicallyAccessedMemberTypes.All on the struct declaration.
    /// </summary>
    [Test]
    public async Task IReflectStruct_WithAll_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            [{|ORNETX0022:DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)|}]
            struct MyReflect : IReflect
            {
                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when DynamicallyAccessedMemberTypes.All
    /// is used with the fully qualified attribute name DynamicallyAccessedMembersAttribute.
    /// </summary>
    [Test]
    public async Task IReflectClass_WithFullyQualifiedAttributeName_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            [{|ORNETX0022:DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes.All)|}]
            class MyReflect : IReflect
            {
                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when DynamicallyAccessedMemberTypes.All is combined
    /// with other flags using bitwise OR (All is redundant but still detected).
    /// </summary>
    [Test]
    public async Task IReflectClass_WithAllCombinedWithOtherFlags_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            [{|ORNETX0022:DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All | DynamicallyAccessedMemberTypes.PublicFields)|}]
            class MyReflect : IReflect
            {
                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported for a class implementing IReflect
    /// without any DynamicallyAccessedMembers attribute.
    /// </summary>
    [Test]
    public async Task IReflectClass_WithoutAttribute_NoDiagnostic()
    {
        const string text = """
            using System;
            using System.Globalization;
            using System.Reflection;

            class MyReflect : IReflect
            {
                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a class implementing IReflect
    /// that already uses restricted DynamicallyAccessedMemberTypes (not All).
    /// </summary>
    [Test]
    public async Task IReflectClass_WithRestrictedTypes_NoDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
            class MyReflect : IReflect
            {
                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a class that uses
    /// DynamicallyAccessedMemberTypes.All but does not implement IReflect
    /// or derive from Type/TypeInfo.
    /// </summary>
    [Test]
    public async Task RegularClass_WithAll_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics.CodeAnalysis;

            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
            class MyClass
            {
                public void DoWork() { }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when DynamicallyAccessedMemberTypes.All
    /// is on a field within an IReflect implementing class (not on the type declaration itself).
    /// </summary>
    [Test]
    public async Task IReflectClass_AllOnField_NoDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            class MyReflect : IReflect
            {
                [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
                public Type? SomeType = typeof(object);

                public FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public MethodInfo? GetMethod(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
                public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers) => throw new NotImplementedException();
                public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotImplementedException();
                public Type UnderlyingSystemType => throw new NotImplementedException();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a TypeDelegator subclass
    /// without DynamicallyAccessedMemberTypes.All.
    /// </summary>
    [Test]
    public async Task TypeDelegatorClass_WithoutAll_NoDiagnostic()
    {
        const string text = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Reflection;

            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
            class MyType : TypeDelegator
            {
                public MyType() : base(typeof(object)) { }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
