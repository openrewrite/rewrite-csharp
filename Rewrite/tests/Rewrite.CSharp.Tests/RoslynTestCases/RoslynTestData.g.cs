using System.Collections;
namespace Rewrite.CSharp.Tests.RoslynTestCases;
public class CSharpSyntaxFragments  : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
                yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single",@"
class C<T> where T : allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single_MissingRef",@"
class C<T> where T : allows struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single_MissingStruct",@"
class C<T> where T : allows ref
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single_MissingRefAndStruct",@"
class C<T> where T : allows
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single_EscapedAllows",@"
class C<T> where T : @allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single_EscapedRef",@"
class C<T> where T : allows @ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_Single_EscapedStruct",@"
class C<T> where T : allows ref @struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_TwoInARow",@"
class C<T> where T : allows ref struct, ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_TwoInARow_MissingRef",@"
class C<T> where T : allows ref struct, struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_TwoInARow_MissingStruct",@"
class C<T> where T : allows ref struct, ref
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_TwoAllowsInARow",@"
class C<T> where T : allows ref struct, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_FollowedByAComma_01",@"
class C<T> where T : allows ref struct, 
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_FollowedByAComma_02",@"
class C<T> where T : struct, allows ref struct, 
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_FollowedByACommaAndWhere_01",@"
class C<T, S> where T : allows ref struct, where S : class
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_FollowedByACommaAndWhere_02",@"
class C<T, S> where T : struct, allows ref struct, where S : class
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_FollowedByWhere_01",@"
class C<T, S> where T : allows ref struct where S : class
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_FollowedByWhere_02",@"
class C<T, S> where T : struct, allows ref struct where S : class
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterStruct",@"
class C<T> where T : struct, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterStructAndMissingComma",@"
class C<T> where T : struct allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterClass",@"
class C<T> where T : class, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterDefault",@"
class C<T> where T : default, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterUnmanaged",@"
class C<T> where T : unmanaged, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterNotNull",@"
class C<T> where T : notnull, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterTypeConstraint",@"
class C<T> where T : SomeType, allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterNew",@"
class C<T> where T : new(), allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_AfterMultiple",@"
class C<T> where T : struct, SomeType, new(), allows ref struct
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_BeforeClass",@"
class C<T> where T : allows ref struct, class
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_BeforeDefault",@"
class C<T> where T : allows ref struct, default
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_BeforeUnmanaged",@"
class C<T> where T : allows ref struct, unmanaged
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_BeforeNotNull",@"
class C<T> where T : allows ref struct, notnull
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_BeforeTypeConstraint",@"
class C<T> where T : allows ref struct, SomeType
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AllowsConstraintParsing.RefStruct_BeforeNew",@"
class C<T> where T : allows ref struct, new()
{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("AsyncParsingTests.AsyncAsType_Indexer_ExpressionBody_ErrorCase","interface async { async this[async i] => null; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestExternAlias","extern alias a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsing","using a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingStatic","using static a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingStaticInWrongOrder","static using a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDuplicateStatic","using static static a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingNamespace","using namespace a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingDottedName","using a.b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingStaticDottedName","using static a.b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingStaticGenericName","using static a<int?>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingAliasName","using a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestUsingAliasGenericName","using a = b<c>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttribute","[assembly:a]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttribute_Verbatim","[@assembly:a]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttribute_Escape",@"[as\u0073embly:a]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalModuleAttribute","[module:a]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalModuleAttribute_Verbatim","[@module:a]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttributeWithParentheses","[assembly:a()]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttributeWithMultipleArguments","[assembly:a(b, c)]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttributeWithNamedArguments","[assembly:a(b = c)]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGlobalAttributeWithMultipleAttributes","[assembly:a, b]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestMultipleGlobalAttributeDeclarations","[assembly:a] [assembly:b]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespace","namespace a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestFileScopedNamespace","namespace a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithDottedName","namespace a.b.c { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithUsing","namespace a { using b.c; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestFileScopedNamespaceWithUsing","namespace a; using b.c;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithExternAlias","namespace a { extern alias b; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestFileScopedNamespaceWithExternAlias","namespace a; extern alias b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithExternAliasFollowingUsingBad","namespace a { using b; extern alias c; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithNestedNamespace","namespace a { namespace b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClass","class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithPublic","public class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithInternal","internal class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithStatic","static class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithSealed","sealed class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithAbstract","abstract class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithPartial","partial class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithAttribute","[attr] class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleAttributes","[attr1] [attr2] class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleAttributesInAList","[attr1, attr2] class a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithBaseType","class a : b { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleBases","class a : b, c { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithTypeConstraintBound","class a<b> where b : c { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNonGenericClassWithTypeConstraintBound","class a where b : c { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNonGenericMethodWithTypeConstraintBound","class a { void M() where b : c { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithNewConstraintBound","class a<b> where b : new() { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithClassConstraintBound","class a<b> where b : class { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithStructConstraintBound","class a<b> where b : struct { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleConstraintBounds","class a<b> where b : class, c, new() { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleConstraints","class a<b> where b : c where b : new() { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleConstraints001","class a<b> where b : c where b { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleConstraints002","class a<b> where b : c where { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassWithMultipleBasesAndConstraints","class a<b> : c, d where b : class, e, new() { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestInterface","interface a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGenericInterface","interface A<B> { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGenericInterfaceWithAttributesAndVariance","interface A<[B] out C> { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestStruct","struct a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedClass","class a { class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedPrivateClass","class a { private class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedProtectedClass","class a { protected class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedProtectedInternalClass","class a { protected internal class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedInternalProtectedClass","class a { internal protected class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedPublicClass","class a { public class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedInternalClass","class a { internal class b { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegate","delegate a b();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithRefReturnType","delegate ref a b();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithRefReadonlyReturnType","delegate ref readonly a b();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithParameter","delegate a b(c d);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithMultipleParameters","delegate a b(c d, e f);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithRefParameter","delegate a b(ref c d);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithOutParameter","delegate a b(out c d);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithParamsParameter","delegate a b(params c d);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithArgListParameter","delegate a b(__arglist);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestDelegateWithParameterAttribute","delegate a b([attr] c d);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNestedDelegate","class a { delegate b c(); }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethod","class a { b X() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithRefReturn","class a { ref b X() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithRefReadonlyReturn","class a { ref readonly b X() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithRef","class a { ref }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithRefReadonly","class a { ref readonly }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithPartial","class a { partial void M() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestStructMethodWithReadonly","struct a { readonly void M() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestReadOnlyRefReturning","struct a { readonly ref readonly int M() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestStructExpressionPropertyWithReadonly","struct a { readonly int M => 42; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestStructGetterPropertyWithReadonly","struct a { int P { readonly get { return 42; } } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestStructBadExpressionProperty",@"public struct S
{
    public int P readonly => 0;
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithParameter","class a { b X(c d) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithMultipleParameters","class a { b X(c d, e f) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassMethodWithArgListParameter","class a { b X(__arglist) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGenericClassMethod","class a { b<c> M() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGenericClassMethodWithTypeConstraintBound","class a { b X<c>() where b : d { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestGenericClassConstructor",@"
class Class1<T>{
    public Class1() { }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassConstructor","class a { a() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassDestructor","class a { ~a() { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassField","class a { b c; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassConstField","class a { const b c = d; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassFieldWithInitializer","class a { b c = e; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassFieldWithArrayInitializer","class a { b c = { }; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassFieldWithMultipleVariables","class a { b c, d, e; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassFieldWithMultipleVariablesAndInitializers","class a { b c = x, d = y, e = z; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassFixedField","class a { fixed b c[10]; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassProperty","class a { b c { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassPropertyWithRefReturn","class a { ref b c { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassPropertyWithRefReadonlyReturn","class a { ref readonly b c { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassPropertyWithBodies","class a { b c { get { } set { } } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassAutoPropertyWithInitializer","class a { b c { get; set; } = d; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.InitializerOnNonAutoProp","class C { int P { set {} } = 0; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassPropertyExplicit","class a { b I.c { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassEventProperty","class a { event b c { add { } remove { } } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassEventPropertyExplicit","class a { event b I.c { add { } remove { } } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassIndexer","class a { b this[c d] { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassIndexerWithRefReturn","class a { ref b this[c d] { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassIndexerWithRefReadonlyReturn","class a { ref readonly b this[c d] { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassIndexerWithMultipleParameters","class a { b this[c d, e f] { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassIndexerExplicit","class a { b I.this[c d] { get; set; } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassRightShiftOperatorMethod","class a { b operator >> (c d, e f) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassUnsignedRightShiftOperatorMethod","class a { b operator >>> (c d, e f) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassImplicitConversionOperatorMethod","class a { implicit operator b (c d) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestClassExplicitConversionOperatorMethod","class a { explicit operator b (c d) { } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceDeclarationsBadNames","namespace A::B { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceDeclarationsBadNames1",@"namespace A::B { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceDeclarationsBadNames2",@"namespace A<B> { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceDeclarationsBadNames3",@"namespace A<,> { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestMissingSemicolonAfterListInitializer",@"using System;
using System.Linq;
class Program {
  static void Main() {
    var r = new List<int>() { 3, 3 }
    var s = 2;
  }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestPartialPartial",@"
partial class PartialPartial
{
    int i = 1;
    partial partial void PM();
    partial partial void PM()
    {
        i = 0;
    }
    static int Main()
    {
        PartialPartial t = new PartialPartial();
        t.PM();
        return t.i;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestPartialEnum",@"partial enum E{}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestEscapedConstructor",@"
class @class
{
    public @class()
    {
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestAnonymousMethodWithDefaultParameter",@"
delegate void F(int x);
class C {
   void M() {
     F f = delegate (int x = 0) { };
   }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.RegressIfDevTrueUnicode",@"
class P
{
static void Main()
{
#if tru\u0065
System.Console.WriteLine(""Good, backwards compatible"");
#else
System.Console.WriteLine(""Bad, breaking change"");
#endif
}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.RegressLongDirectiveIdentifierDefn",@"
//130 chars (max is 128)
#define A234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
class P
{
static void Main()
{
//first 128 chars of defined value
#if A2345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678
System.Console.WriteLine(""Good, backwards compatible"");
#else
System.Console.WriteLine(""Bad, breaking change"");
#endif
}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.RegressLongDirectiveIdentifierUse",@"
//128 chars (max)
#define A2345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678
class P
{
static void Main()
{
//defined value + two chars (larger than max)
#if A234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
System.Console.WriteLine(""Good, backwards compatible"");
#else
System.Console.WriteLine(""Bad, breaking change"");
#endif
}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.ValidFixedBufferTypes",@"
unsafe struct s
{
    public fixed bool _Type1[10];
    internal fixed int _Type3[10];
    private fixed short _Type4[10];
    unsafe fixed long _Type5[10];
    new fixed char _Type6[10];    
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.ValidFixedBufferTypesMultipleDeclarationsOnSameLine",@"
unsafe struct s
{
    public fixed bool _Type1[10], _Type2[10], _Type3[20];
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.ValidFixedBufferTypesWithCountFromConstantOrLiteral",@"
unsafe struct s
{
    public const int abc = 10;
    public fixed bool _Type1[abc];
    public fixed bool _Type2[20];
    }
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.ValidFixedBufferTypesAllValidTypes",@"
unsafe struct s
{
    public fixed bool _Type1[10]; 
    public fixed byte _Type12[10]; 
    public fixed int _Type2[10]; 
    public fixed short _Type3[10]; 
    public fixed long _Type4[10]; 
    public fixed char _Type5[10]; 
    public fixed sbyte _Type6[10]; 
    public fixed ushort _Type7[10]; 
    public fixed uint _Type8[10]; 
    public fixed ulong _Type9[10]; 
    public fixed float _Type10[10]; 
    public fixed double _Type11[10];     
 }


")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TupleArgument01",@"
class C1
{
    static (T, T) Test1<T>(int a, (byte, byte) arg0)
    {
        return default((T, T));
    }

    static (T, T) Test2<T>(ref (byte, byte) arg0)
    {
        return default((T, T));
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TupleArgument02",@"
class C1
{
    static (T, T) Test3<T>((byte, byte) arg0)
    {
        return default((T, T));
    }

    (T, T) Test3<T>((byte a, byte b)[] arg0)
    {
        return default((T, T));
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithDotDot1",@"namespace a..b { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestExtraneousColonInBaseList",@"
class A : B : C
{
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithDotDot2",@"namespace a
                    ..b { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithDotDot3",@"namespace a..
b { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.TestNamespaceWithDotDot4",@"namespace a
                    ..
b { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.Interface_NoBody",@"
interface C")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.Interface_SemicolonBody",@"
interface C
;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.Interface_SemicolonBodyAfterBase_01",@"
interface C : I1
;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.Interface_SemicolonBodyAfterBase_02",@"
interface C : I1, I2
;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.Interface_SemicolonBodyAfterConstraint_01",@"
interface C where T1 : U1
;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeclarationParsingTests.Interface_SemicolonBodyAfterConstraint_02",@"
interface C where T1 : U1 where T2 : U2
;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.TupleArray","(T, T)[] id;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.ParenthesizedExpression","(x).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.TupleLiteralStatement","(x, x).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.Statement4","((x)).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.Statement5","((x, y) = M()).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.CastWithTupleType","(((x, y))z).Goo();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.NotACast","((Int32.MaxValue, Int32.MaxValue)).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.AlsoNotACast","((x, y)).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.StillNotACast","((((x, y)))).ToString();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.LambdaInExpressionStatement","(a) => a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.LambdaWithBodyInExpressionStatement","(a, b) => { };")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.InvalidStatement","(x, y)? = M();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("DeconstructionTests.NullableTuple","(x, y)? z = M();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestAltInterpolatedVerbatimString_CSharp73",@"@$""hello""")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestParenthesizedExpression","(goo)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestStringLiteralExpression","\"stuff\"")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestVerbatimLiteralExpression","@\"\"\"stuff\"\"\"")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestCharacterLiteralExpression","'c'")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNumericLiteralExpression","0")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestRefValue","(a, b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestConditional","a ? b : c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestConditional02","a ? b=c : d=e")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestCast","(a) b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestCall","a(b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestCallWithRef","a(ref b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestCallWithOut","a(out b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestCallWithNamedArgument","a(B: b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestIndex","a[b]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestIndexWithRef","a[ref b]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestIndexWithOut","a[out b]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestIndexWithNamedArgument","a[B: b]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNew","new a()")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithArgument","new a(b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithNamedArgument","new a(B: b)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithEmptyInitializer","new a() { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithNoArgumentsAndEmptyInitializer","new a { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithNoArgumentsAndInitializer","new a { b }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithNoArgumentsAndInitializers","new a { b, c, d }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithNoArgumentsAndAssignmentInitializer","new a { B = b }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewWithNoArgumentsAndNestedAssignmentInitializer","new a { B = { X = x } }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewArray","new a[1]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewArrayWithInitializer","new a[] {b}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewArrayWithInitializers","new a[] {b, c, d}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestNewMultiDimensionalArrayWithInitializer","new a[][,][,,] {b}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestImplicitArrayCreation","new [] {b}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestAnonymousObjectCreation","new {a, b}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestAnonymousMethod","delegate (int a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestAnonymousMethodWithNoArguments","delegate () { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestAnonymousMethodWithNoArgumentList","delegate { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestSimpleLambda","a => b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestSimpleLambdaWithRefReturn","a => ref b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestSimpleLambdaWithBlock","a => { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithNoParameters","() => b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithNoParametersAndRefReturn","() => ref b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithNoParametersAndBlock","() => { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithOneParameter","(a) => b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithTwoParameters","(a, a2) => b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithOneTypedParameter","(T a) => b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestLambdaWithOneRefParameter","(ref T a) => b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestTupleWithTwoArguments","(a, a2)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestTupleWithTwoNamedArguments","(arg1: (a, a2), arg2: a2)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromSelect","from a in A select b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromWithType","from T a in A select b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromSelectIntoSelect","from a in A select b into c select d")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromWhereSelect","from a in A where b select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromFromSelect","from a in A from b in B select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromLetSelect","from a in A let b = B select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromOrderBySelect","from a in A orderby b select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromOrderBy2Select","from a in A orderby b, b2 select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromOrderByAscendingSelect","from a in A orderby b ascending select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromOrderByDescendingSelect","from a in A orderby b descending select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromGroupBy","from a in A group b by c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromGroupByIntoSelect","from a in A group b by c into d select e")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromJoinSelect","from a in A join b in B on a equals b select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromJoinWithTypesSelect","from Ta a in A join Tb b in B on a equals b select c")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromJoinIntoSelect","from a in A join b in B on a equals b into c select d")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestFromGroupBy1","from it in goo group x by y")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.UnterminatedRankSpecifier","new int[")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.UnterminatedTypeArgumentList","new C<")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.QueryKeywordInObjectInitializer","from elem in aRay select new Result { A = on = true }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.IndexingExpressionInParens","(aRay[i,j])")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.ParseBigExpression",@"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WB.Core.SharedKernels.DataCollection.Generated
{
   internal partial class QuestionnaireTopLevel
   {   
      private bool IsValid_a()
      {
            return (stackDepth == 100) || ((stackDepth == 200) || ((stackDepth == 300) || ((stackDepth == 400) || ((stackDepth == 501) || ((stackDepth == 502) || ((stackDepth == 600) || ((stackDepth == 701) || ((stackDepth == 702) || ((stackDepth == 801) || ((stackDepth == 802) || ((stackDepth == 901) || ((stackDepth == 902) || ((stackDepth == 903) || ((stackDepth == 1001) || ((stackDepth == 1002) || ((stackDepth == 1101) || ((stackDepth == 1102) || ((stackDepth == 1201) || ((stackDepth == 1202) || ((stackDepth == 1301) || ((stackDepth == 1302) || ((stackDepth == 1401) || ((stackDepth == 1402) || ((stackDepth == 1403) || ((stackDepth == 1404) || ((stackDepth == 1405) || ((stackDepth == 1406) || ((stackDepth == 1407) || ((stackDepth == 1408) || ((stackDepth == 1409) || ((stackDepth == 1410) || ((stackDepth == 1411) || ((stackDepth == 1412) || ((stackDepth == 1413) || ((stackDepth == 1500) || ((stackDepth == 1601) || ((stackDepth == 1602) || ((stackDepth == 1701) || ((stackDepth == 1702) || ((stackDepth == 1703) || ((stackDepth == 1800) || ((stackDepth == 1901) || ((stackDepth == 1902) || ((stackDepth == 1903) || ((stackDepth == 1904) || ((stackDepth == 2000) || ((stackDepth == 2101) || ((stackDepth == 2102) || ((stackDepth == 2103) || ((stackDepth == 2104) || ((stackDepth == 2105) || ((stackDepth == 2106) || ((stackDepth == 2107) || ((stackDepth == 2201) || ((stackDepth == 2202) || ((stackDepth == 2203) || ((stackDepth == 2301) || ((stackDepth == 2302) || ((stackDepth == 2303) || ((stackDepth == 2304) || ((stackDepth == 2305) || ((stackDepth == 2401) || ((stackDepth == 2402) || ((stackDepth == 2403) || ((stackDepth == 2404) || ((stackDepth == 2501) || ((stackDepth == 2502) || ((stackDepth == 2503) || ((stackDepth == 2504) || ((stackDepth == 2505) || ((stackDepth == 2601) || ((stackDepth == 2602) || ((stackDepth == 2603) || ((stackDepth == 2604) || ((stackDepth == 2605) || ((stackDepth == 2606) || ((stackDepth == 2607) || ((stackDepth == 2608) || ((stackDepth == 2701) || ((stackDepth == 2702) || ((stackDepth == 2703) || ((stackDepth == 2704) || ((stackDepth == 2705) || ((stackDepth == 2706) || ((stackDepth == 2801) || ((stackDepth == 2802) || ((stackDepth == 2803) || ((stackDepth == 2804) || ((stackDepth == 2805) || ((stackDepth == 2806) || ((stackDepth == 2807) || ((stackDepth == 2808) || ((stackDepth == 2809) || ((stackDepth == 2810) || ((stackDepth == 2901) || ((stackDepth == 2902) || ((stackDepth == 3001) || ((stackDepth == 3002) || ((stackDepth == 3101) || ((stackDepth == 3102) || ((stackDepth == 3103) || ((stackDepth == 3104) || ((stackDepth == 3105) || ((stackDepth == 3201) || ((stackDepth == 3202) || ((stackDepth == 3203) || ((stackDepth == 3301) || ((stackDepth == 3302) || ((stackDepth == 3401) || ((stackDepth == 3402) || ((stackDepth == 3403) || ((stackDepth == 3404) || ((stackDepth == 3405) || ((stackDepth == 3406) || ((stackDepth == 3407) || ((stackDepth == 3408) || ((stackDepth == 3409) || ((stackDepth == 3410) || ((stackDepth == 3501) || ((stackDepth == 3502) || ((stackDepth == 3503) || ((stackDepth == 3504) || ((stackDepth == 3505) || ((stackDepth == 3506) || ((stackDepth == 3507) || ((stackDepth == 3508) || ((stackDepth == 3509) || ((stackDepth == 3601) || ((stackDepth == 3602) || ((stackDepth == 3701) || ((stackDepth == 3702) || ((stackDepth == 3703) || ((stackDepth == 3704) || ((stackDepth == 3705) || ((stackDepth == 3706) || ((stackDepth == 3801) || ((stackDepth == 3802) || ((stackDepth == 3803) || ((stackDepth == 3804) || ((stackDepth == 3805) || ((stackDepth == 3901) || ((stackDepth == 3902) || ((stackDepth == 3903) || ((stackDepth == 3904) || ((stackDepth == 3905) || ((stackDepth == 4001) || ((stackDepth == 4002) || ((stackDepth == 4003) || ((stackDepth == 4004) || ((stackDepth == 4005) || ((stackDepth == 4006) || ((stackDepth == 4007) || ((stackDepth == 4100) || ((stackDepth == 4201) || ((stackDepth == 4202) || ((stackDepth == 4203) || ((stackDepth == 4204) || ((stackDepth == 4301) || ((stackDepth == 4302) || ((stackDepth == 4304) || ((stackDepth == 4401) || ((stackDepth == 4402) || ((stackDepth == 4403) || ((stackDepth == 4404) || ((stackDepth == 4501) || ((stackDepth == 4502) || ((stackDepth == 4503) || ((stackDepth == 4504) || ((stackDepth == 4600) || ((stackDepth == 4701) || ((stackDepth == 4702) || ((stackDepth == 4801) || ((stackDepth == 4802) || ((stackDepth == 4803) || ((stackDepth == 4804) || ((stackDepth == 4805) || ((stackDepth == 4806) || ((stackDepth == 4807) || ((stackDepth == 4808) || ((stackDepth == 4809) || ((stackDepth == 4811) || ((stackDepth == 4901) || ((stackDepth == 4902) || ((stackDepth == 4903) || ((stackDepth == 4904) || ((stackDepth == 4905) || ((stackDepth == 4906) || ((stackDepth == 4907) || ((stackDepth == 4908) || ((stackDepth == 4909) || ((stackDepth == 4910) || ((stackDepth == 4911) || ((stackDepth == 4912) || ((stackDepth == 4913) || ((stackDepth == 4914) || ((stackDepth == 4915) || ((stackDepth == 4916) || ((stackDepth == 4917) || ((stackDepth == 4918) || ((stackDepth == 4919) || ((stackDepth == 4920) || ((stackDepth == 4921) || ((stackDepth == 4922) || ((stackDepth == 4923) || ((stackDepth == 5001) || ((stackDepth == 5002) || ((stackDepth == 5003) || ((stackDepth == 5004) || ((stackDepth == 5005) || ((stackDepth == 5006) || ((stackDepth == 5100) || ((stackDepth == 5200) || ((stackDepth == 5301) || ((stackDepth == 5302) || ((stackDepth == 5400) || ((stackDepth == 5500) || ((stackDepth == 5600) || ((stackDepth == 5700) || ((stackDepth == 5801) || ((stackDepth == 5802) || ((stackDepth == 5901) || ((stackDepth == 5902) || ((stackDepth == 6001) || ((stackDepth == 6002) || ((stackDepth == 6101) || ((stackDepth == 6102) || ((stackDepth == 6201) || ((stackDepth == 6202) || ((stackDepth == 6203) || ((stackDepth == 6204) || ((stackDepth == 6205) || ((stackDepth == 6301) || ((stackDepth == 6302) || ((stackDepth == 6401) || ((stackDepth == 6402) || ((stackDepth == 6501) || ((stackDepth == 6502) || ((stackDepth == 6503) || ((stackDepth == 6504) || ((stackDepth == 6601) || ((stackDepth == 6602) || ((stackDepth == 6701) || ((stackDepth == 6702) || ((stackDepth == 6703) || ((stackDepth == 6704) || ((stackDepth == 6801) || ((stackDepth == 6802) || ((stackDepth == 6901) || ((stackDepth == 6902) || ((stackDepth == 6903) || ((stackDepth == 6904) || ((stackDepth == 7001) || ((stackDepth == 7002) || ((stackDepth == 7101) || ((stackDepth == 7102) || ((stackDepth == 7103) || ((stackDepth == 7200) || ((stackDepth == 7301) || ((stackDepth == 7302) || ((stackDepth == 7400) || ((stackDepth == 7501) || ((stackDepth == 7502) || ((stackDepth == 7503) || ((stackDepth == 7600) || ((stackDepth == 7700) || ((stackDepth == 7800) || ((stackDepth == 7900) || ((stackDepth == 8001) || ((stackDepth == 8002) || ((stackDepth == 8101) || ((stackDepth == 8102) || ((stackDepth == 8103) || ((stackDepth == 8200) || ((stackDepth == 8300) || ((stackDepth == 8400) || ((stackDepth == 8501) || ((stackDepth == 8502) || ((stackDepth == 8601) || ((stackDepth == 8602) || ((stackDepth == 8700) || ((stackDepth == 8801) || ((stackDepth == 8802) || ((stackDepth == 8901) || ((stackDepth == 8902) || ((stackDepth == 8903) || ((stackDepth == 9001) || ((stackDepth == 9002) || ((stackDepth == 9003) || ((stackDepth == 9004) || ((stackDepth == 9005) || ((stackDepth == 9101) || ((stackDepth == 9102) || ((stackDepth == 9200) || ((stackDepth == 9300) || ((stackDepth == 9401) || ((stackDepth == 9402) || ((stackDepth == 9403) || ((stackDepth == 9500) || ((stackDepth == 9601) || ((stackDepth == 9602) || ((stackDepth == 9701) || ((stackDepth == 9702) || ((stackDepth == 9801) || ((stackDepth == 9802) || ((stackDepth == 9900) || ((stackDepth == 10000) || ((stackDepth == 10100) || ((stackDepth == 10201) || ((stackDepth == 10202) || ((stackDepth == 10301) || ((stackDepth == 10302) || ((stackDepth == 10401) || ((stackDepth == 10402) || ((stackDepth == 10403) || ((stackDepth == 10501) || ((stackDepth == 10502) || ((stackDepth == 10601) || ((stackDepth == 10602) || ((stackDepth == 10701) || ((stackDepth == 10702) || ((stackDepth == 10703) || ((stackDepth == 10704) || ((stackDepth == 10705) || ((stackDepth == 10706) || ((stackDepth == 10801) || ((stackDepth == 10802) || ((stackDepth == 10803) || ((stackDepth == 10804) || ((stackDepth == 10805) || ((stackDepth == 10806) || ((stackDepth == 10807) || ((stackDepth == 10808) || ((stackDepth == 10809) || ((stackDepth == 10900) || ((stackDepth == 11000) || ((stackDepth == 11100) || ((stackDepth == 11201) || ((stackDepth == 11202) || ((stackDepth == 11203) || ((stackDepth == 11204) || ((stackDepth == 11205) || ((stackDepth == 11206) || ((stackDepth == 11207) || ((stackDepth == 11208) || ((stackDepth == 11209) || ((stackDepth == 11210) || ((stackDepth == 11211) || ((stackDepth == 11212) || ((stackDepth == 11213) || ((stackDepth == 11214) || ((stackDepth == 11301) || ((stackDepth == 11302) || ((stackDepth == 11303) || ((stackDepth == 11304) || ((stackDepth == 11305) || ((stackDepth == 11306) || ((stackDepth == 11307) || ((stackDepth == 11308) || ((stackDepth == 11309) || ((stackDepth == 11401) || ((stackDepth == 11402) || ((stackDepth == 11403) || ((stackDepth == 11404) || ((stackDepth == 11501) || ((stackDepth == 11502) || ((stackDepth == 11503) || ((stackDepth == 11504) || ((stackDepth == 11505) || ((stackDepth == 11601) || ((stackDepth == 11602) || ((stackDepth == 11603) || ((stackDepth == 11604) || ((stackDepth == 11605) || ((stackDepth == 11606) || ((stackDepth == 11701) || ((stackDepth == 11702) || ((stackDepth == 11800) || ((stackDepth == 11901) || ((stackDepth == 11902) || ((stackDepth == 11903) || ((stackDepth == 11904) || ((stackDepth == 11905) || ((stackDepth == 12001) || ((stackDepth == 12002) || ((stackDepth == 12003) || ((stackDepth == 12004) || ((stackDepth == 12101) || ((stackDepth == 12102) || ((stackDepth == 12103) || ((stackDepth == 12104) || ((stackDepth == 12105) || ((stackDepth == 12106) || ((stackDepth == 12107) || ((stackDepth == 12108) || ((stackDepth == 12109) || ((stackDepth == 12110) || ((stackDepth == 12111) || ((stackDepth == 12112) || ((stackDepth == 12113) || ((stackDepth == 12114) || ((stackDepth == 12115) || ((stackDepth == 12116) || ((stackDepth == 12201) || ((stackDepth == 12202) || ((stackDepth == 12203) || ((stackDepth == 12204) || ((stackDepth == 12205) || ((stackDepth == 12301) || ((stackDepth == 12302) || ((stackDepth == 12401) || ((stackDepth == 12402) || ((stackDepth == 12403) || ((stackDepth == 12404) || ((stackDepth == 12405) || ((stackDepth == 12406) || ((stackDepth == 12501) || ((stackDepth == 12502) || ((stackDepth == 12601) || ((stackDepth == 12602) || ((stackDepth == 12603) || ((stackDepth == 12700) || ((stackDepth == 12800) || ((stackDepth == 12900) || ((stackDepth == 13001) || ((stackDepth == 13002) || ((stackDepth == 13003) || ((stackDepth == 13004) || ((stackDepth == 13005) || ((stackDepth == 13101) || ((stackDepth == 13102) || ((stackDepth == 13103) || ((stackDepth == 13201) || ((stackDepth == 13202) || ((stackDepth == 13203) || ((stackDepth == 13301) || ((stackDepth == 13302) || ((stackDepth == 13303) || ((stackDepth == 13304) || ((stackDepth == 13401) || ((stackDepth == 13402) || ((stackDepth == 13403) || ((stackDepth == 13404) || ((stackDepth == 13405) || ((stackDepth == 13501) || ((stackDepth == 13502) || ((stackDepth == 13600) || ((stackDepth == 13701) || ((stackDepth == 13702) || ((stackDepth == 13703) || ((stackDepth == 13800) || ((stackDepth == 13901) || ((stackDepth == 13902) || ((stackDepth == 13903) || ((stackDepth == 14001) || ((stackDepth == 14002) || ((stackDepth == 14100) || ((stackDepth == 14200) || ((stackDepth == 14301) || ((stackDepth == 14302) || ((stackDepth == 14400) || ((stackDepth == 14501) || ((stackDepth == 14502) || ((stackDepth == 14601) || ((stackDepth == 14602) || ((stackDepth == 14603) || ((stackDepth == 14604) || ((stackDepth == 14605) || ((stackDepth == 14606) || ((stackDepth == 14607) || ((stackDepth == 14701) || ((stackDepth == 14702) || ((stackDepth == 14703) || ((stackDepth == 14704) || ((stackDepth == 14705) || ((stackDepth == 14706) || ((stackDepth == 14707) || ((stackDepth == 14708) || ((stackDepth == 14709) || ((stackDepth == 14710) || ((stackDepth == 14711) || ((stackDepth == 14712) || ((stackDepth == 14713) || ((stackDepth == 14714) || ((stackDepth == 14715) || ((stackDepth == 14716) || ((stackDepth == 14717) || ((stackDepth == 14718) || ((stackDepth == 14719) || ((stackDepth == 14720 || ((stackDepth == 14717 || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717 || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717 || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717) || ((stackDepth == 14717))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))));
      }      
   }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration1",@"
class C
{
    async void M()
    {
        Task.
        await Task.Delay();
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration2",@"
class C
{
    async void M()
    {
        Task.await Task.Delay();
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration3",@"
class C
{
    async void M()
    {
        Task.
        await Task;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration4",@"
class C
{
    async void M()
    {
        Task.
        await Task = 1;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration5",@"
class C
{
    async void M()
    {
        Task.
        await Task, Task2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration6",@"
class C
{
    async void M()
    {
        Task.
        await Task();
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration7",@"
class C
{
    async void M()
    {
        Task.
        await Task<T>();
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.InProgressLocalDeclaration8",@"
class C
{
    async void M()
    {
        Task.
        await Task[1];
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_01",@"
class C
{
    void M()
    {
        //int a = 1;
        //int i = 1;
        var j = a < i >> 2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_02",@"
class C
{
    void M()
    {
        //const int a = 1;
        //const int i = 2;
        switch (false)
        {
            case a < i >> 2: break;
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_03",@"
class C
{
    void M()
    {
        M(out a < i >> 2);
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_04",@"
class C
{
    void M()
    {
        // (e is a<i>) > 2
        var j = e is a < i >> 2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_05",@"
class C
{
    void M()
    {
        var j = e is a < i >>> 2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_06",@"
class C
{
    void M()
    {
        // syntax error
        var j = e is a < i > << 2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_07",@"
class C
{
    void M()
    {
        // syntax error
        var j = e is a < i >>>> 2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_08",@"
class C
{
    void M()
    {
        M(out a < i >>> 2);
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_09",@"
class C
{
    void M()
    {
        //const int a = 1;
        //const int i = 2;
        switch (false)
        {
            case a < i >>> 2: break;
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TypeArgumentShiftAmbiguity_10",@"
class C
{
    void M()
    {
        //int a = 1;
        //int i = 1;
        var j = a < i >>> 2;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.TestTargetTypedDefaultWithCSharp7_1","default")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.MismatchedInterpolatedStringContents_01",@"class A
{
    void M()
    {
        if (b)
        {
            A B = new C($@""{D(.E}"");
            N.O("""", P.Q);
            R.S(T);
            U.V(W.X, Y.Z);
        }
    }

    string M() => """";
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.MismatchedInterpolatedStringContents_02",@"class A
{
    void M()
    {
        if (b)
        {
            A B = new C($@""{D(.E}\F\G{H}_{I.J.K(""L"")}.M"");
            N.O("""", P.Q);
            R.S(T);
            U.V(W.X, Y.Z);
        }
    }

    string M() => """";
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.LinqQueryInConditionalExpression1","x is X ? from item in collection select item : null")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.LinqQueryInConditionalExpression2","x is X.Y ? from item in collection select item : null")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ExpressionParsingTests.LinqQueryInConditionalExpression_Incomplete","x is X.Y ? from item")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("FunctionPointerTests.LangVersion8","delegate* unmanaged[cdecl]<string, Goo, int> ptr;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.DiagnosticsWithoutExperimental",@"
class c
{
    void m()
    {
        int local() => 0;
    }
    void m2()
    {
        int local() { return 0; }
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.StaticFunctions",@"class Program
{
    void M()
    {
        static void F() { }
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.AsyncStaticFunctions",@"class Program
{
    void M()
    {
        static async void F1() { }
        async static void F2() { }
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.DuplicateStatic",@"class Program
{
    void M()
    {
        static static void F1() { }
        static async static void F2() { }
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.DuplicateAsyncs1","""
                class Program
                {
                    void M()
                    {
                        #pragma warning disable 1998, 8321
                        async async void F() { }
                    }
                }
                """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.DuplicateAsyncs2","""
                class Program
                {
                    void M()
                    {
                        #pragma warning disable 1998, 8321
                        async async async void F() { }
                    }
                }
                """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.DuplicateAsyncs3","""
                class Program
                {
                    void M()
                    {
                        #pragma warning disable 1998, 8321
                        async async async async void F() { }
                    }
                }
                """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.DuplicateAsyncs4","""
                class Program
                {
                    void M()
                    {
                        #pragma warning disable 1998, 8321
                        async async async async async void F() { }
                    }
                }
                """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.ReturnTypeBeforeStatic",@"class Program
{
    void M()
    {
        void static F() { }
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("LocalFunctionParsingTests.ExtraInvocationClosingParentheses","""
                public class C
                {
                    public void M()
                    {
                        int sum0 = Sum(1, 2));

                        void Local()
                        {
                            AnotherLocal());

                            int sum1 = Sum(1, 2));
                            int sum2 = Sum(1, 3));

                            void AnotherLocal()
                            {
                                int x = sum2 + 2;
                            }
                        }
                    }

                    public static int Sum(int a, int b) => a + b;
                }
                """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_01","public int N.I.operator +(int x, int y) => x + y;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_02","public int N.I.implicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_03","public int N.I.explicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_04","public int N.I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_05","public int I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_11","public int N.I.operator +(int x, int y) => x + y;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_12","public int N.I.implicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_13","public int N.I.explicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_14","public int N.I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_15","public int I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_23","int N.I.operator +(int x, int y) => x + y;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_24","int N.I.implicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_25","int N.I.explicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_26","int N.I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_27","int I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_33","int N.I.operator +(int x, int y) => x + y;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_34","int N.I.implicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_35","int N.I.explicit (int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_36","int N.I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.OperatorDeclaration_ExplicitImplementation_37","int I operator +(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_01","implicit N.I.operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_02","N.I.operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_04","implicit N.I operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_05","explicit I operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_11","explicit N.I.operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_12","implicit N.I int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_13","explicit N.I. int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_14","implicit N.I operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.ConversionDeclaration_ExplicitImplementation_15","explicit I operator int(int x) => x;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.PropertyWithErrantSemicolon1",@"
public class Class
{
    public int MyProperty; { get; set; }

    // Pretty much anything here causes an error
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("MemberDeclarationParsingTests.PropertyWithErrantSemicolon2",@"
public class Class
{
    public int MyProperty; => 0;

    // Pretty much anything here causes an error
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestBasicName","goo")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestBasicNameWithTrash","/*comment*/goo/*comment2*/ bar")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestMissingNameDueToKeyword","class")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestMissingNameDueToPartialClassStart","partial class")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestMissingNameDueToPartialMethodStart","partial void Method()")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestAliasedName","goo::bar")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestGlobalAliasedName","global::bar")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestDottedName","goo.bar")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestAliasedDottedName","goo::bar.Zed")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestDoubleAliasName","goo::bar::baz")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestGenericName","goo<bar>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestGenericNameWithTwoArguments","goo<bar,zed>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestNestedGenericName_01","goo<bar<zed>>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestNestedGenericName_02","goo<bar<zed<U>>>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestOpenNameWithNoCommas","goo<>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestOpenNameWithAComma","goo<,>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestBasicTypeName","goo")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestDottedTypeName","goo.bar")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestGenericTypeName","goo<bar>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestNestedGenericTypeName_01","goo<bar<zed>>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestNestedGenericTypeName_02","goo<bar<zed<U>>>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestOpenTypeNameWithNoCommas","goo<>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestNullableTypeName","goo?")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestPointerTypeName","goo*")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestPointerTypeNameWithMultipleAsterisks","goo***")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestArrayTypeName","goo[]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestMultiDimensionalArrayTypeName","goo[,,]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestMultiRankedArrayTypeName","goo[][,][,,]")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestVarianceInNameBad","goo<in bar>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestAttributeInNameBad","goo<[My]bar>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestConstantInGenericNameBad","goo<0>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestConstantInGenericNamePartiallyBad","goo<0,bool>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestKeywordInGenericNameBad","goo<static>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestAttributeAndVarianceInNameBad","goo<[My]in bar>")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestFormattingCharacter","\u0915\u094d\u200d\u0937")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("NameParsingTests.TestSoftHyphen","x\u00ady")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1009ERR_IllegalEscape_Strings",@"
class Program
{
    static void Main()
    {
        string s;
        s = ""\u"";
        s = ""\u0"";
        s = ""\u00"";
        s = ""\u000"";
        
        s = ""a\uz"";
        s = ""a\u0z"";
        s = ""a\u00z"";
        s = ""a\u000z"";
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1009ERR_IllegalEscape_Identifiers",@"using System;
class Program
{
    static void Main()
    {
        int \u;
        int \u0;
        int \u00;
        int \u000;

        int a\uz;
        int a\u0z;
        int a\u00z;
        int a\u000z;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1031ERR_TypeExpected02",@"namespace x
{
    public class a
    {
        public static void Main()
        {
            e = new base;   // CS1031, not a type
            e = new this;   // CS1031, not a type
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1031ERR_TypeExpected02_Tuple",@"namespace x
{
    public class @a
    {
        public static void Main()
        {
            var e = new ();
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1031ERR_TypeExpected02WithCSharp6",@"namespace x
{
    public class a
    {
        public static void Main()
        {
            e = new base;   // CS1031, not a type
            e = new this;   // CS1031, not a type
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1031ERR_TypeExpected02WithCSharp6_Tuple",@"namespace x
{
    public class @a
    {
        public static void Main()
        {
            var e = new ();
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CS1031ERR_TypeExpected02WithCSharp7_Tuple",@"namespace x
{
    public class @a
    {
        public static void Main()
        {
            var e = new ();
        }
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.InvalidQueryExpression",@"
using System;
using System.Linq;

public class QueryExpressionTest
{
    public static void Main()
    {
        var expr1 = new[] { 1, 2, 3 };
        var expr2 = new[] { 1, 2, 3 };

        var query13 = from  const in expr1 join  i in expr2 on const equals i select new { const, i };
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.PartialTypesBeforeVersionTwo",@"
partial class C
{
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.PartialMethodsVersionThree",@"
class C
{
    partial int Goo() { }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.QueryBeforeVersionThree",@"
class C
{
    void Goo()
    {
        var q = from a in b
                select c;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AnonymousTypeBeforeVersionThree",@"
class C
{
    void Goo()
    {
        var q = new { };
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.ImplicitArrayBeforeVersionThree",@"
class C
{
    void Goo()
    {
        var q = new [] { };
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.ObjectInitializerBeforeVersionThree",@"
class C
{
    void Goo()
    {
        var q = new Goo { };
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.LambdaBeforeVersionThree",@"
class C
{
    void Goo()
    {
        var q = a => b;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.ExceptionFilterBeforeVersionSix",@"
public class C 
{
    public static int Main()
    {
        try { } catch when (true) {}
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.MissingCommaInAttribute",@"[One Two] // error: missing comma
class TestClass { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.InterpolatedStringBeforeCSharp6",@"
class C
{
    string M()
    {
        return $""hello"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.InterpolatedStringWithReplacementBeforeCSharp6",@"
class C
{
    string M()
    {
        string other = ""world"";
        return $""hello + {other}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AsyncBeforeCSharp5",@"
class C
{
    async void M() { }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AsyncWithOtherModifiersBeforeCSharp5",@"
class C
{
    async static void M() { }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AsyncLambdaBeforeCSharp5",@"
class C
{
    static void Main()
    {
        Func<int, Task<int>> f = async x => x;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AsyncDelegateBeforeCSharp5",@"
class C
{
    static void Main()
    {
        Func<int, Task<int>> f = async delegate (int x) { return x; };
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.NamedArgumentBeforeCSharp4",@"
[Attr(x:1)]
class C
{
    C()
    {
        M(y:2);
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.GlobalKeywordBeforeCSharp2",@"
class C : global::B
{
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AliasQualifiedNameBeforeCSharp2",@"
class C : A::B
{
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.OptionalParameterBeforeCSharp4",@"
class C
{
    void M(int x = 1) { }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.ObjectInitializerBeforeCSharp3",@"
class C
{
    void M() 
    {
        return new C { Goo = 1 }; 
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CollectionInitializerBeforeCSharp3",@"
class C
{
    void M() 
    {
        return new C { 1, 2, 3 }; 
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CrefGenericBeforeCSharp2",@"
/// <see cref='C{T}'/>
class C
{
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.CrefAliasQualifiedNameBeforeCSharp2",@"
/// <see cref='Alias::Goo'/>
/// <see cref='global::Goo'/>
class C { }
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.PragmaBeforeCSharp2",@"
#pragma warning disable 1584
#pragma checksum ""file.txt"" ""{00000000-0000-0000-0000-000000000000}"" ""2453""
class C { }
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.PragmaBeforeCSharp2_InDisabledCode",@"
#if UNDEF
#pragma warning disable 1584
#pragma checksum ""file.txt"" ""{00000000-0000-0000-0000-000000000000}"" ""2453""
#endif
class C { }
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ParserErrorMessageTests.AwaitAsIdentifierInAsyncContext",@"
class C
{
    async void f()
    {
        int await;
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("PatternParsingTests.ParenthesizedSwitchCase",@"
switch (e)
{
    case (0): break;
    case (-1): break;
    case (+2): break;
    case (~3): break;
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLine1",@"
class C
{
    void M()
    {
        var v = $"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineTooManyCloseQuotes1",@"
class C
{
    void M()
    {
        var v = $"""""" """""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineTooManyCloseQuotes2",@"
class C
{
    void M()
    {
        var v = $"""""" """""""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineSingleQuoteInside",@"
class C
{
    void M()
    {
        var v = $"""""" "" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineDoubleQuoteInside",@"
class C
{
    void M()
    {
        var v = $"""""" """" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationInside",@"
class C
{
    void M()
    {
        var v = $""""""{0}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationInsideSpacesOutside",@"
class C
{
    void M()
    {
        var v = $"""""" {0} """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationInsideSpacesInside",@"
class C
{
    void M()
    {
        var v = $""""""{ 0 }"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationInsideSpacesInsideAndOutside",@"
class C
{
    void M()
    {
        var v = $"""""" { 0 } """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesNotAllowed1",@"
class C
{
    void M()
    {
        var v = $""""""{{0}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesNotAllowed2",@"
class C
{
    void M()
    {
        var v = $$""""""{{{{0}}}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesNotAllowed3",@"
class C
{
    void M()
    {
        var v = $""""""{0}}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesNotAllowed4",@"
class C
{
    void M()
    {
        var v = $$""""""{{{0}}}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesNotAllowed5",@"
class C
{
    void M()
    {
        var v = $$""""""{0}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesNotAllowed6",@"
class C
{
    void M()
    {
        var v = $$""""""{{{0}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesAllowed1",@"
class C
{
    void M()
    {
        var v = $$""""""{{0}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesAllowed2",@"
class C
{
    void M()
    {
        var v = $$""""""{{{0}}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationMultipleCurliesAllowed4",@"
class C
{
    void M()
    {
        var v = $$""""""{{{0}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingNormalString",@"
class C
{
    void M()
    {
        var v = $""""""{""a""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingVerbatimString1",@"
class C
{
    void M()
    {
        var v = $""""""{@""a""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingVerbatimString2",@"
class C
{
    void M()
    {
        var v = $""""""{@""
a""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingInterpolatedString1",@"
class C
{
    void M()
    {
        var v = $""""""{$""a""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingInterpolatedString2",@"
class C
{
    void M()
    {
        var v = $""""""{$""{0}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingVerbatimInterpolatedString1",@"
class C
{
    void M()
    {
        var v = $""""""{$@""{0}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingVerbatimInterpolatedString2",@"
class C
{
    void M()
    {
        var v = $""""""{@$""{0}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingVerbatimInterpolatedString3",@"
class C
{
    void M()
    {
        var v = $""""""{$@""{
0}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingVerbatimInterpolatedString4",@"
class C
{
    void M()
    {
        var v = $""""""{
$@""{
0}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawStringLiteral1",@"
class C
{
    void M()
    {
        var v = $""""""{""""""a""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawStringLiteral2",@"
class C
{
    void M()
    {
        var v = $""""""{""""""
  a
  """"""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawStringLiteral3",@"
class C
{
    void M()
    {
        var v = $""""""{""""""
  a
    """"""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral1",@"
class C
{
    void M()
    {
        var v = $""""""{$"""""" """"""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral2",@"
class C
{
    void M()
    {
        var v = $""""""{$"""""""" """"""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral3",@"
class C
{
    void M()
    {
        var v = $""""""{$""""""{0}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral4",@"
class C
{
    void M()
    {
        var v = $""""""{$""""""{
0}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral5",@"
class C
{
    void M()
    {
        var v = $""""""{
$""""""{
0}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral6",@"
class C
{
    void M()
    {
        var v = $""""""{$$""""""{{0}}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral7",@"
class C
{
    void M()
    {
        var v = $""""""{$$""""""{{{0}}}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingRawInterpolatedStringLiteral8",@"
class C
{
    void M()
    {
        var v = $$""""""{{{$""""""{0}""""""}}}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingClosingBraceAsCharacterLiteral",@"
class C
{
    void M()
    {
        var v = $""""""{'}'}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingClosingBraceAsRegularStringLiteral",@"
class C
{
    void M()
    {
        var v = $""""""{""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingClosingBraceAsVerbatimStringLiteral",@"
class C
{
    void M()
    {
        var v = $""""""{@""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.SingleLineInterpolationContainingClosingBraceAsRawStringLiteral",@"
class C
{
    void M()
    {
        var v = $""""""{""""""}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleNormalInnerNormal",@"
class C
{
    void M()
    {
        var v = $""{$""{$""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleNormalInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $""{$""{$@""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleNormalInnerRaw",@"
class C
{
    void M()
    {
        var v = $""{$""{$""""""{0}""""""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleVerbatimInnerNormal",@"
class C
{
    void M()
    {
        var v = $""{@$""{$""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleVerbatimInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $""{@$""{@$""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleVerbatimInnerRaw",@"
class C
{
    void M()
    {
        var v = $""{@$""{$""""""{0}""""""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleRawInnerNormal",@"
class C
{
    void M()
    {
        var v = $""{$""""""{$""{0}""}""""""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleRawInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $""{$""""""{@$""{0}""}""""""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterNormalMiddleRawInnerRaw",@"
class C
{
    void M()
    {
        var v = $""{$""""""{$""""""{0}""""""}""""""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleNormalInnerNormal",@"
class C
{
    void M()
    {
        var v = $@""{$""{$""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleNormalInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $@""{$""{$@""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleNormalInnerRaw",@"
class C
{
    void M()
    {
        var v = $@""{$""{$""""""{0}""""""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleVerbatimInnerNormal",@"
class C
{
    void M()
    {
        var v = $@""{@$""{$""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleVerbatimInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $@""{@$""{@$""{0}""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleVerbatimInnerRaw",@"
class C
{
    void M()
    {
        var v = $@""{@$""{$""""""{0}""""""}""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleRawInnerNormal",@"
class C
{
    void M()
    {
        var v = $@""{$""""""{$""{0}""}""""""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleRawInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $@""{$""""""{@$""{0}""}""""""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterVerbatimMiddleRawInnerRaw",@"
class C
{
    void M()
    {
        var v = $@""{$""""""{$""""""{0}""""""}""""""}"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleNormalInnerNormal",@"
class C
{
    void M()
    {
        var v = $""""""{$""{$""{0}""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleNormalInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $""""""{$""{$@""{0}""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleNormalInnerRaw",@"
class C
{
    void M()
    {
        var v = $""""""{$""{$""""""{0}""""""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleVerbatimInnerNormal",@"
class C
{
    void M()
    {
        var v = $""""""{@$""{$""{0}""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleVerbatimInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $""""""{@$""{@$""{0}""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleVerbatimInnerRaw",@"
class C
{
    void M()
    {
        var v = $""""""{@$""{$""""""{0}""""""}""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleRawInnerNormal",@"
class C
{
    void M()
    {
        var v = $""""""{$""""""{$""{0}""}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleRawInnerVerbatim",@"
class C
{
    void M()
    {
        var v = $""""""{$""""""{@$""{0}""}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.OuterRawMiddleRawInnerRaw",@"
class C
{
    void M()
    {
        var v = $""""""{$""""""{$""""""{0}""""""}""""""}"""""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns1",@"
class C
{
    void M()
    {
        var v = @@;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns2",@"
class C
{
    void M()
    {
        var v = @@"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns3",@"
class C
{
    void M()
    {
        var v = @@"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns4",@"
class C
{
    void M()
    {
        var v = @@"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns5",@"
class C
{
    void M()
    {
        var v = @@@;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns6",@"
class C
{
    void M()
    {
        var v = @@@"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns7",@"
class C
{
    void M()
    {
        var v = @@@"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.MultipleAtSigns8",@"
class C
{
    void M()
    {
        var v = @@@"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt1",@"
class C
{
    void M()
    {
        var v = $@@;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt2",@"
class C
{
    void M()
    {
        var v = $@@"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt3",@"
class C
{
    void M()
    {
        var v = $@@"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt4",@"
class C
{
    void M()
    {
        var v = $@@"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt5",@"
class C
{
    void M()
    {
        var v = $@@@;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt6",@"
class C
{
    void M()
    {
        var v = $@@@"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt7",@"
class C
{
    void M()
    {
        var v = $@@@"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarThenAt8",@"
class C
{
    void M()
    {
        var v = $@@@"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar1",@"
class C
{
    void M()
    {
        var v = @@$;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar2",@"
class C
{
    void M()
    {
        var v = @@$"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar3",@"
class C
{
    void M()
    {
        var v = @@$"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar4",@"
class C
{
    void M()
    {
        var v = @@$"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar5",@"
class C
{
    void M()
    {
        var v = @@$$;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar6",@"
class C
{
    void M()
    {
        var v = @@$$"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar7",@"
class C
{
    void M()
    {
        var v = @@$$"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollar8",@"
class C
{
    void M()
    {
        var v = @@$$"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt1",@"
class C
{
    void M()
    {
        var v = @@$@;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt2",@"
class C
{
    void M()
    {
        var v = @@$@"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt3",@"
class C
{
    void M()
    {
        var v = @@$@"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt4",@"
class C
{
    void M()
    {
        var v = @@$@"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt5",@"
class C
{
    void M()
    {
        var v = @@$$@;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt6",@"
class C
{
    void M()
    {
        var v = @@$$@"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt7",@"
class C
{
    void M()
    {
        var v = @@$$@"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.AtThenDollarThenAt8",@"
class C
{
    void M()
    {
        var v = @@$$@"""""" """""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithoutQuotes0",@"
class C
{
    void M()
    {
        var v = $;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithoutQuotes1",@"
class C
{
    void M()
    {
        var v = $$;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithoutQuotes2",@"
class C
{
    void M()
    {
        var v = $$$;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithQuotes1",@"
class C
{
    void M()
    {
        var v = $$"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithQuotes2",@"
class C
{
    void M()
    {
        var v = $$"" "";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithQuotes3",@"
class C
{
    void M()
    {
        var v = $$"""" """";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithQuotes2_MultiLine",@"
class C
{
    void M()
    {
        var v = $$""

"";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RawInterpolatedStringLiteralParsingTests.DollarsWithQuotes3_MultiLine",@"
class C
{
    void M()
    {
        var v = $$""""

"""";
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ReadOnlyStructs.ReadOnlyStructSimple",@"
class Program
{
    readonly struct S1{}

    public readonly struct S2{}

    readonly public struct S3{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ReadOnlyStructs.ReadOnlyStructSimpleLangVer",@"
class Program
{
    readonly struct S1{}

    public readonly struct S2{}

    readonly public struct S3{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ReadOnlyStructs.ReadOnlyClassErr",@"
class Program
{
    readonly class S1{}

    public readonly delegate ref readonly int S2();

    readonly public interface S3{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ReadOnlyStructs.ReadOnlyRefStruct",@"
class Program
{
    readonly ref struct S1{}

    unsafe readonly public ref struct S2{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ReadOnlyStructs.ReadOnlyStructPartialMatchingModifiers",@"
class Program
{
    readonly partial struct S1{}

    readonly partial struct S1{}

    readonly ref partial struct S2{}

    readonly ref partial struct S2{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("ReadOnlyStructs.ReadOnlyStructPartialNotMatchingModifiers",@"
class Program
{
    readonly partial struct S1{}

    readonly ref partial struct S1{}

    readonly partial struct S2{}

    partial struct S2{}

    readonly ref partial struct S3{}

    partial struct S3{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.FieldNamedData",@"
class C
{
    int data;
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing01","record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing02","record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing03","record C;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing04","record C { public int record; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing05","record Point;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing07","interface P(int x, int y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsingAmbiguities",@"
record R1() { return null; }
abstract record D
{
    record R2() { return null; }
    abstract record R3();
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing_BlockBodyAndSemiColon","record C { };")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsingLangVer",@"
class C
{
    int x = 0 with {};
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing1",@"
class C
{
    with { };
    x with { };
    int x = with { };
    int x = 0 with { };
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing2",@"
class C
{
    int M()
    {
        int x = M() with { } + 3;
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing3","0 with {")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing4","0 with { X")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing5","0 with { X 3 =,")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing6",@"M() with { } switch { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing7",@"M() with { } + 3")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing8",@"M() with { }.ToString()")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing9",@"M() with { } with { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing12",@"M() switch { } with { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing13",@"M(out await with)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing14",@"x is int y with {}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing15",@"x with { X = ""2"" }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing16",@"x with { X = ""2"" };")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing17",@"x = x with { X = ""2"" };")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsing18",@"x with { A = e is T y B = y }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsingInConditionalExpression1","x is X ? record with { } : record with { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsingInConditionalExpression2","x is X.Y ? record with { } : record with { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.WithParsingInConditionalExpression_Incomplete","x is X ? record with")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.Base_03","interface C : B;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.Base_04","interface C(int X, int Y) : B;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.Base_05","interface C : B(X, Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_RecordNamedStruct","record struct(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing","record struct C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_WithBody","record struct C(int X, int Y) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordClassParsing","record class C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordInterfaceParsing","record interface C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordRecordParsing","record record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_WrongOrder_CSharp10","struct record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_WrongOrder_CSharp9","struct record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.StructNamedRecord_CSharp8","struct record { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.StructNamedRecord_CSharp9","struct record { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.StructNamedRecord_CSharp10","struct record { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordClassParsing_WrongOrder_CSharp10","class record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordClassParsing_WrongOrder_CSharp9","class record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordInterfaceParsing_WrongOrder","interface record C(int X, int Y);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Partial","partial record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordClassParsing_Partial","partial record class S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing_Partial","partial record S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Partial_WithParameterList","partial record struct S(int X);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Partial_WithParameterList_AndMembers","partial record struct S(int X) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Readonly","readonly record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_ReadonlyPartial","readonly partial record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_PartialReadonly","partial readonly record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_New","new record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Ref","ref record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordParsing_Ref","ref record R;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Const","const record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_Fixed","fixed record struct S;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_BaseListWithParens","record struct S : Base(1);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RecordParsingTests.RecordStructParsing_BaseListWithParens_WithPositionalParameterList","record struct S(int X) : Base(1);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefReadonlyTests.RefReadonlyReturn_CSharp7",@"
unsafe class Program
{
    delegate ref readonly int D1();

    static ref readonly T M<T>()
    {
        return ref (new T[1])[0];
    }

    public virtual ref readonly int* P1 => throw null;

    public ref readonly int[][] this[int i] => throw null;
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefReadonlyTests.InArgs_CSharp7",@"
class Program
{
    static void M(in int x)
    {
    }

    int this[in int x]
    {
        get
        {
            return 1;
        }
    }

    static void Test1()
    {
        int x = 1;
        M(in x);

        _ = (new Program())[in x];
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefReadonlyTests.RefReadonlyReturn_Unexpected",@"

class Program
{
    static void Main()
    {
    }

    ref readonly int Field;

    public static ref readonly Program  operator  +(Program x, Program y)
    {
        throw null;
    }

    // this parses fine
    static async ref readonly Task M<T>()
    {
        throw null;
    }

    public ref readonly virtual int* P1 => throw null;

}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefReadonlyTests.RefReadonlyReturn_UnexpectedBindTime",@"

class Program
{
    static void Main()
    {
        ref readonly int local = ref (new int[1])[0];

        (ref readonly int, ref readonly int Alice)? t = null;

        System.Collections.Generic.List<ref readonly int> x = null;

        Use(local);
        Use(t);
        Use(x);
    }

    static void Use<T>(T dummy)
    {
    }
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefStructs.RefStructSimple",@"
class Program
{
    ref struct S1{}

    public ref struct S2{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefStructs.RefStructSimpleLangVer",@"
class Program
{
    ref struct S1{}

    public ref struct S2{}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefStructs.RefStructErr",@"
class Program
{
    ref class S1{}

    public ref unsafe struct S2{}

    ref interface I1{};

    public ref delegate ref int D1();
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("RefStructs.PartialRefStruct",@"
class Program
{
    partial ref struct S {}
    partial ref struct S {}
}
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("SeparatedSyntaxListParsingTests.TypeArguments2WithCSharp6",@"
class C
{
    new C<>();
    new C<, >();
    C<C<>> a1;
    C<A<>> a1;
    object a1 = typeof(C<C<, >, int>);
    object a2 = Swap<>(1, 1);
}

class M<,> { }
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestDottedName","a.b();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestGenericName","a<b>();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestGenericDotName","a<b>.c();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestDotGenericName","a.b<c>();")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatement","T a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithVar","var a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithTuple","(int, int) a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithNamedTuple","(T x, (U k, V l, W m) y) a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithDynamic","dynamic a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithGenericType","T<a> b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithDottedType","T.X.Y a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithMixedType","T<t>.X<x>.Y<y> a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithArrayType","T[][,][,,] a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithPointerType","T* a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithNullableType","T? a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithMultipleVariables","T a, b, c;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithInitializer","T a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithMultipleVariablesAndInitializers","T a = va, b = vb, c = vc;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLocalDeclarationStatementWithArrayInitializer","T a = {b, c};")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestConstLocalDeclarationStatement","const T a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestStaticLocalDeclarationStatement","static T a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestReadOnlyLocalDeclarationStatement","readonly T a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestVolatileLocalDeclarationStatement","volatile T a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestRefLocalDeclarationStatement","ref T a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestRefLocalDeclarationStatementWithInitializer","ref T a = ref b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestRefLocalDeclarationStatementWithMultipleInitializers","ref T a = ref b, c = ref d;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestFixedStatement","fixed(T a = b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestFixedVarStatement","fixed(var a = b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestFixedStatementWithMultipleVariables","fixed(T a = b, c = d) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestEmptyStatement",";")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLabeledStatement","label: ;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestBreakStatement","break;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestContinueStatement","continue;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestGotoStatement","goto label;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestGotoCaseStatement","goto case label;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestGotoDefault","goto default;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestReturn","return;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestReturnExpression","return a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestYieldReturnExpression","yield return a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestYieldBreakExpression","yield break;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestThrow","throw;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestThrowExpression","throw a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestTryCatch","try { } catch(T e) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestTryCatchWithNoExceptionName","try { } catch(T) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestTryCatchWithNoExceptionDeclaration","try { } catch { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestTryCatchWithMultipleCatches","try { } catch(T e) { } catch(T2) { } catch { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestTryFinally","try { } finally { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestTryCatchWithMultipleCatchesAndFinally","try { } catch(T e) { } catch(T2) { } catch { } finally { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestChecked","checked { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUnchecked","unchecked { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUnsafe","unsafe { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestWhile","while(a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestDoWhile","do { } while (a);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestFor","for(;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithVariableDeclaration","for(T a = 0;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithVarDeclaration","for(var a = 0;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithMultipleVariableDeclarations","for(T a = 0, b = 1;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithRefVariableDeclaration","for(ref T a = ref b, c = ref d;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithVariableInitializer","for(a = 0;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithMultipleVariableInitializers","for(a = 0, b = 1;;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithCondition","for(; a;) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithIncrementor","for(; ; a++) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithMultipleIncrementors","for(; ; a++, b++) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForWithDeclarationConditionAndIncrementor","for(T a = 0; a < 10; a++) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForEach","foreach(T a in b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForAsForEach","for(T a in b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestForEachWithVar","foreach(var a in b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestIf","if (a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestIfElse","if (a) { } else { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestIfElseIf","if (a) { } else if (b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestLock","lock (a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestSwitch","switch (a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestSwitchWithCase","switch (a) { case b:; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestSwitchWithMultipleCases","switch (a) { case b:; case c:; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestSwitchWithDefaultCase","switch (a) { default:; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestSwitchWithMultipleLabelsOnOneCase","switch (a) { case b: case c:; }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestSwitchWithMultipleStatementsOnOneCase","switch (a) { case b: s1(); s2(); }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingWithExpression","using (a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingWithDeclaration","using (T a = b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingVarWithDeclaration","using T a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingWithVarDeclaration","using (var a = b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingVarWithVarDeclaration","using var a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestAwaitUsingWithVarDeclaration","await using var a = b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingWithDeclarationWithMultipleVariables","using (T a = b, c = d) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingVarWithDeclarationWithMultipleVariables","using T a = b, c = d;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingSpecialCase1","using (f ? x = a : x = b) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingVarSpecialCase1","using var x = f ? a : b;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingSpecialCase2","using (f ? x = a) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingVarSpecialCase2","using f ? x = a;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingSpecialCase3","using (f ? x, y) { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestUsingVarSpecialCase3","using f ? x, y;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.Bug862649",@"static char[] delimiter;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("StatementParsingTests.TestRunEmbeddedStatementNotFollowedBySemicolon",@"if (true)
System.Console.WriteLine(true)")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("SuppressNullableWarningExpressionParsingTests.ConditionalAccess_Suppression_LangVersion","x?.y!.z")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("TopLevelStatementsParsingTests.TestIncompleteGlobalMembers",@"
asas]
extern alias A;
asas
using System;
sadasdasd]

[assembly: goo]

class C
{
}


[a]fod;
[b")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("TopLevelStatementsParsingTests.IncompleteTopLevelOperator",@"
fg implicit//
class C { }
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("TopLevelStatementsParsingTests.TestGlobalNamespaceWithOpenBraceBeforeNamespace","{ namespace n { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("TopLevelStatementsParsingTests.EmptyLocalDeclaration",""" 
struct S { }
partial ext X
""")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.StaticUsingDirectiveRefType",@"using static x = ref int;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveNamePointer1",@"using x = A*;

struct A { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveNamePointer2",@"using unsafe x = A*;

struct A { }")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveFunctionPointer1",@"using x = delegate*<int, void>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveFunctionPointer2",@"using unsafe x = delegate*<int, void>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingUnsafeNonAlias",@"using unsafe System;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedType_CSharp11",@"using x = int;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedType_CSharp12",@"using x = int;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedType_Preview",@"using x = int;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveRefType",@"using x = ref int;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveRefReadonlyType",@"using x = ref readonly int;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer1",@"using x = int*;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer2",@"using unsafe x = int*;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer3",@"
using unsafe X = int*;

namespace N
{
    using Y = X;
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer4",@"
using unsafe X = int*;

namespace N
{
    using unsafe Y = X;
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer5",@"
using X = int*;

namespace N
{
    using unsafe Y = X;
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer6",@"
using unsafe X = int*;

namespace N
{
    using Y = X[];
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectivePredefinedTypePointer7",@"
using unsafe X = int*;

namespace N
{
    using unsafe Y = X[];
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveTuple1",@"using x = (int, int);")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveTuple2","""
            using X = (int, int);

            class C
            {
                X x = (0, 0);
            }
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveTuple3","""
            using X = (int, int);

            class C
            {
                X x = (true, false);
            }
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingNullableValueType",@"using x = int?;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingNullableReferenceType1",@"using x = string?;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingNullableReferenceType2","""
            #nullable enable
            using X = string?;
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingNullableReferenceType3","""
            using X = string;
            namespace N
            {
                using Y = X?;
            }
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingNullableReferenceType4","""
            #nullable enable
            using X = string;
            namespace N
            {
                using Y = X?;
            }
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingVoidPointer1",@"using unsafe VP = void*;

class C
{
    void M(VP vp) { }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingVoidPointer2",@"using unsafe VP = void*;

class C
{
    unsafe void M(VP vp) { }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingVoidPointer3",@"using VP = void*;

class C
{
    unsafe void M(VP vp) { }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingVoid1",@"using V = void;

class C
{
    void M(V v) { }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingVoid2",@"using V = void;

class C
{
    V M() { }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingVoid3",@"using V = void[];

class C
{
    V M() { }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingDirectiveDynamic1",@"
using dynamic;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveDynamic1",@"
using D = dynamic;

class C
{
    void M(D d)
    {
        d.Goo();
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveDynamic2",@"
using D = System.Collections.Generic.List<dynamic>;

class C
{
    void M(D d)
    {
        d[0].Goo();
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveDynamic3",@"
using D = dynamic[];

class C
{
    void M(D d)
    {
        d[0].Goo();
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveDynamic4",@"
using D = dynamic;

class dynamic
{
    void M(D d)
    {
        d.Goo();
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDirectiveDynamic5",@"
// Note: this is weird, but is supported by language.  It checks just that the ValueText is `dynamic`, not the raw text.
using D = @dynamic;

class C
{
    void M(D d)
    {
        d.Goo();
    }
}")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDuplicate1","""
            using X = int?;
            using X = System;
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDuplicate2","""
            using X = int?;
            using X = int;
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingDuplicate3","""
            using X = int?;
            using X = System.Int32;
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.AliasUsingNotDuplicate1","""
            using X = int?;
            namespace N;
            using X = int;
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestScopedType1",@"
using scoped int;
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestScopedType2",@"
using X = scoped int;
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestScopedType3",@"
using X = scoped System;
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestScopedType4",@"
using X = scoped System.AppDomain;
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestObsolete1","""
            using System;
            using X = C;

            [Obsolete("", error: true)]
            class C
            {
            }

            class D
            {
                X x;
                C c;
            }
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestObsolete2","""
            using System;
            using X = C[];

            [Obsolete("", error: true)]
            class C
            {
            }

            class D
            {
                X x1;
                C[] c1;
            }
            """)
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestArgList",@"
using X = __arglist;
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestMakeref",@"
using X = __makeref;
")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic1_CSharp11_NoUnsafeFlag",@"using unsafe static System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic1_CSharp11_UnsafeFlag",@"using unsafe static System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic1_CSharp12_NoUnsafeFlag",@"using unsafe static System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic1_CSharp12_UnsafeFlag",@"using unsafe static System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic2_CSharp11_NoUnsafeFlag",@"using unsafe static X = System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic2_CSharp11_UnsafeFlag",@"using unsafe static X = System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic2_CSharp12_NoUnsafeFlag",@"using unsafe static X = System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.TestUnsafeStatic2_CSharp12_UnsafeFlag",@"using unsafe static X = System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_SafeType_CSharp11_NoUnsafeFlag",@"using static unsafe System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_SafeType_CSharp11_UnsafeFlag",@"using static unsafe System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_SafeType_CSharp12_NoUnsafeFlag",@"using static unsafe System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_SafeType_CSharp12_UnsafeFlag",@"using static unsafe System.Console;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_UnsafeType_CSharp11_NoUnsafeFlag",@"using static unsafe System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_UnsafeType_CSharp11_UnsafeFlag",@"using static unsafe System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_UnsafeType_CSharp12_NoUnsafeFlag",@"using static unsafe System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStaticUnsafe_UnsafeType_CSharp12_UnsafeFlag",@"using static unsafe System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStatic_UnsafeType_CSharp11_NoUnsafeFlag",@"using static System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStatic_UnsafeType_CSharp11_UnsafeFlag",@"using static System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStatic_UnsafeType_CSharp12_NoUnsafeFlag",@"using static System.Collections.Generic.List<int*[]>;")
        };
        
        yield return new object[]
        {
            new CSharpSyntaxFragment("UsingDirectiveParsingTests.UsingStatic_UnsafeType_CSharp12_UnsafeFlag",@"using static System.Collections.Generic.List<int*[]>;")
        };
        
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}