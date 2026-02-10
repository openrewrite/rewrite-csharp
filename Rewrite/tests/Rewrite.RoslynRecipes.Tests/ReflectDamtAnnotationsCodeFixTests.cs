using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipes.ReflectDamtAnnotationsAnalyzer,
    Rewrite.RoslynRecipes.ReflectDamtAnnotationsCodeFixProvider>;

namespace Rewrite.RoslynRecipes.Tests;

public class ReflectDamtAnnotationsCodeFixTests
{
    /// <summary>
    /// Tests that DynamicallyAccessedMemberTypes.All is replaced with the restricted
    /// member types on a class implementing IReflect.
    /// </summary>
    [Test]
    public async Task IReflectClass_AllReplacedWithRestrictedTypes()
    {
        const string source = """
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

        const string fixedSource = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Globalization;
            using System.Reflection;

            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
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

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that DynamicallyAccessedMemberTypes.All is replaced with the restricted
    /// member types on a class deriving from TypeDelegator.
    /// </summary>
    [Test]
    public async Task TypeDelegatorClass_AllReplacedWithRestrictedTypes()
    {
        const string source = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Reflection;

            [{|ORNETX0022:DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)|}]
            class MyType : TypeDelegator
            {
                public MyType() : base(typeof(object)) { }
            }
            """;

        const string fixedSource = """
            using System;
            using System.Diagnostics.CodeAnalysis;
            using System.Reflection;

            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
            class MyType : TypeDelegator
            {
                public MyType() : base(typeof(object)) { }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }
}
