﻿using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Rewrite.Core.Config;

namespace Rewrite.Core;


public record RecipeIdentity
{
    public RecipeIdentity(string type, string nugetPackageName, string nugetPackageVersion) : this(InferTypeQualifiedName(type, nugetPackageName), nugetPackageName, nugetPackageVersion ) { }

    public static TypeName InferTypeQualifiedName(string name, string packageName)
    {
        if (name.IndexOf(',') >= 0)
            return TypeName.Parse(name);
        return new TypeName(name, packageName);
    }
    public RecipeIdentity(TypeName type, string nugetPackageName, string nugetPackageVersion)
    {
        if (type.AssemblyName == null)
            throw new InvalidOperationException("TypeName must be assembly qualified");
        this.Type = type;
        this.NugetPackageName = nugetPackageName;
        this.NugetPackageVersion = nugetPackageVersion;
    }

    public TypeName Type { get; init; }
    public string NugetPackageName { get; init; }
    public string NugetPackageVersion { get; init; }

    public void Deconstruct(out TypeName Type, out string NugetPackageName, out string NugetPackageVersion)
    {
        Type = this.Type;
        NugetPackageName = this.NugetPackageName;
        NugetPackageVersion = this.NugetPackageVersion;
    }

    public override string ToString()
    {
        return $"{Type.FullName}, {NugetPackageName}.{NugetPackageVersion}";
    }
    
    public string ToUri() => $"recipe://{NugetPackageName}/{NugetPackageVersion}/{Type.AssemblyName}/{Type.FullName}";

    public static RecipeIdentity ParseUri(Uri uri)
    {
        var segments = uri.PathAndQuery.Trim('/').Split("/");
        var packageName = uri.Host;
        var version = segments[0];
        var assemblyName = segments[1];
        var recipeName = segments[2];
        var typeName = new TypeName(recipeName, assemblyName);
        return new RecipeIdentity(typeName, packageName, version);
    }
}

