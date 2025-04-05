using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

internal static class TypeUtils
{
    private static readonly Dictionary<string, Type> TypeCache = new();
    private static readonly Dictionary<Type, string> JavaTypeNameCache = new();

    private static readonly Regex DotNetTypeNameRegex =
        new Regex(@"Rewrite\.Rewrite(?<module>[^.]+?)\.Tree\.(?<interface>[^+]+)(?:\+(?<class>.+))?");

    private static readonly Regex JavaTypeNameRegex =
        new Regex(@"org\.openrewrite\.(?<module>[^.]+?)\.tree\.(?<interface>[^$]+)(?:\$(?<class>.+))*");

    private static string GetAssemblyName(string javaModule)
    {
        var shortName = javaModule switch
        {
            "csharp" => "CSharp",
            "java" => "CSharp",
            _ => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(javaModule)
        };
        var fullName = $"Rewrite.{shortName}";
        return fullName;
    }

    private static string GetPrimaryNamespaceSegment(string javaModule)
    {

        var shortName = javaModule switch
        {
            "csharp" => "CSharp",
            _ => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(javaModule)
        };
        return shortName;
    }
    internal static Type GetType(string typeName)
    {
        if (TypeCache.TryGetValue(typeName, out var type)) return type;

        Match match;
        if ((match = JavaTypeNameRegex.Match(typeName)).Success)
        {
            var module = match.Groups["module"].Value;
            var primaryNamespaceSegment = GetPrimaryNamespaceSegment(module);
            var assemblyName = GetAssemblyName(module);

            var result = "Rewrite.Rewrite" + primaryNamespaceSegment + ".Tree." + match.Groups["interface"].Value +
                         (match.Groups["class"].Success ? "+" + match.Groups["class"].Value.Replace('$', '+') : "");
            type = Assembly.Load(assemblyName).GetType(result);
        }
        else if ((match = DotNetTypeNameRegex.Match(typeName)).Success)
        {
            var module = match.Groups["module"].Value;
            if(module == "Java")
                module = "CSharp";
            type = Assembly.Load("Rewrite." + module).GetType(typeName);
        }
        else if (typeName == "org.openrewrite.tree.ParseError")
        {
            type = Assembly.Load("Rewrite.Core").GetType("Rewrite.Core.ParseError");
        }

        if (type == null)
        {
            throw new TypeLoadException($"Unable to find type {typeName}");
        }
        TypeCache.Add(typeName, type);
        return type;
    }

    private static string ModuleFromJavaPackage(string javaModule)
    {
        return javaModule == "csharp" ? "CSharp" : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(javaModule);
    }

    private static readonly Func<string, string> PackageMap = before =>
    {
        if (before.StartsWith("Rewrite"))
            return before.Substring(7).ToLower();
        else if (before.Equals("MSBuild"))
            return "csharp";
        else return before.ToLower();
    };

    public static string ToJavaTypeName(object o)
    {
        if (o is UnknownJavaMarker unknown)
            return unknown.Data["@c"] as string ?? throw new InvalidOperationException("Key not found");
        return ToJavaTypeName(o.GetType());
    }

    public static string ToJavaTypeName(Type type)
    {
        if (JavaTypeNameCache.TryGetValue(type, out var javaTypeName)) return javaTypeName;

        var typeName = type.FullName!;
        if (typeName.StartsWith("Rewrite.Core.Marker."))
        {
            javaTypeName = "org.openrewrite.marker." + typeName[20..].Replace('+', '$');
        }
        else if (typeName.StartsWith("Rewrite.Core."))
        {
            if (typeName == "Rewrite.Core.ParseError")
                return "org.openrewrite.tree.ParseError";

            javaTypeName = "org.openrewrite." + typeName[13..].Replace('+', '$');
        }
        else if (type.IsPrimitive)
        {
            if (type == typeof(bool))
            {
                javaTypeName = "java.lang.Boolean";
            }
            else if (type == typeof(int))
            {
                javaTypeName = "java.lang.Integer";
            }
            else if (type == typeof(long))
            {
                javaTypeName = "java.lang.Long";
            }
            else if (type == typeof(char))
            {
                javaTypeName = "java.lang.Character";
            }
            else if (type == typeof(void))
            {
                javaTypeName = "java.lang.Void";
            }
        }
        else if (type == typeof(string))
        {
            javaTypeName = "java.lang.String";
        }
        else
        {
            var match = Regex.Match(typeName, @"Rewrite.(\w+)\.(\w+\.)*([A-Za-z+0-9]+)");
            if (match.Success)
            {
                var package = PackageMap.Invoke(match.Groups[1].Value);
                var subpackage = match.Groups[2].Success ? match.Groups[2].Value.ToLower() : "";
                javaTypeName = $"org.openrewrite.{package}.{subpackage}{match.Groups[3].Value.Replace('+', '$')}";
            }
            else
            {
                // FIXME: IDK if that is right
                javaTypeName = "java.lang.Object";
            }
        }

        JavaTypeNameCache.Add(type, javaTypeName!);
        return javaTypeName!;
    }
}
