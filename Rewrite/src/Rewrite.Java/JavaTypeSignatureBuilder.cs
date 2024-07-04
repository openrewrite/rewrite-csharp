namespace Rewrite.RewriteJava;

using System;

using System;

/// <summary>
/// In addition to the signature formats described below, implementations should provide a way of retrieving method
/// and variable signatures, but they may have different numbers of input arguments depending on the implementation.
/// <para>
/// Method signatures should be formatted like com.MyThing{name=add,return=void,parameters=[Integer]},
/// where MyThing is the declaring type, void is the return type, and Integer is a parameter.
/// </para>
/// <para>
/// Variable signatures should be formatted like com.MyThing{name=MY_FIELD}.
/// </para>
/// </summary>
public interface JavaTypeSignatureBuilder<T>
{
    /// <summary>
    /// Returns the type signature.
    /// </summary>
    /// <param name="type">A type object.</param>
    /// <returns>The type signature. If <paramref name="type"/> is null, the signature is {undefined}.</returns>
    string Signature(T type);
    
    /// <summary>
    /// Formats an array type.
    /// </summary>
    /// <param name="type">An array type.</param>
    /// <returns>Formatted like Integer[].</returns>
    string ArraySignature(T type);

    /// <summary>
    /// Formats a class type.
    /// </summary>
    /// <param name="type">A class type.</param>
    /// <returns>Formatted like java.util.List.</returns>
    string ClassSignature(T type);

    /// <summary>
    /// When generic type variables are cyclic, like U extends Cyclic{? extends U}, represent the cycle with the
    /// bound name, like Generic{U extends Cyclic{? extends U}}.
    /// <para>
    /// When the bound is Object (regardless of whether
    /// that bound is implicit or explicit in the source code), the type variable is considered invariant and the
    /// bound is omitted. So Generic{List{?}} is favored over Generic{List{? extends java.lang.Object}}.
    /// </para>
    /// </summary>
    /// <param name="type">A generic type.</param>
    /// <returns>Formatted like: Generic{U extends java.lang.Comparable} or Generic{U super java.lang.Comparable}.</returns>
    string GenericSignature(T type);

    /// <summary>
    /// Formats a parameterized type.
    /// </summary>
    /// <param name="type">A parameterized type.</param>
    /// <returns>Formatted like java.util.List{java.util.List{Integer}}.</returns>
    string ParameterizedSignature(T type);

    /// <summary>
    /// Formats a primitive type.
    /// </summary>
    /// <param name="type">A primitive type.</param>
    /// <returns>Formatted like Integer.</returns>
    string PrimitiveSignature(T type);
}