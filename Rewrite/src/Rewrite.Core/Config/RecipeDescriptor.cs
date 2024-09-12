namespace Rewrite.Core.Config;

using System;
using System.Collections.Generic;
using System.Linq;

public class RecipeDescriptor
{
    // Properties with public getters and private setters to maintain immutability
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public HashSet<string> Tags { get; }
    public TimeSpan? EstimatedEffortPerOccurrence { get; }
    public List<OptionDescriptor> Options { get; }
    public List<RecipeDescriptor> RecipeList { get; }
    // public List<DataTableDescriptor> DataTables { get; }
    // public List<Maintainer> Maintainers { get; }
    // public List<Contributor> Contributors { get; }
    // public List<RecipeExample> Examples { get; }
    public Uri Source { get; }

    // Constructor to initialize the properties
    public RecipeDescriptor(string name, string displayName, string description, HashSet<string> tags,
        TimeSpan? estimatedEffortPerOccurrence, List<OptionDescriptor> options, List<RecipeDescriptor> recipeList,
        /*List<DataTableDescriptor> dataTables, List<Maintainer> maintainers, List<Contributor> contributors,
        List<RecipeExample> examples,*/ Uri source)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        Tags = tags;
        EstimatedEffortPerOccurrence = estimatedEffortPerOccurrence;
        Options = options;
        RecipeList = recipeList;
        // DataTables = dataTables;
        // Maintainers = maintainers;
        // Contributors = contributors;
        // Examples = examples;
        Source = source;
    }

    // Override Equals and GetHashCode to handle equality based on Name and Options
    public override bool Equals(object? obj)
    {
        if (obj is RecipeDescriptor other)
        {
            return Name == other.Name && Options.SequenceEqual(other.Options);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return (Name, Options).GetHashCode();
    }

    // With pattern for immutability
    public RecipeDescriptor With(
        string? name = null,
        string? displayName = null,
        string? description = null,
        HashSet<string>? tags = null,
        TimeSpan? estimatedEffortPerOccurrence = null,
        List<OptionDescriptor>? options = null,
        List<RecipeDescriptor>? recipeList = null,
        // List<DataTableDescriptor> dataTables = null,
        // List<Maintainer> maintainers = null,
        // List<Contributor> contributors = null,
        // List<RecipeExample> examples = null,
        Uri? source = null)
    {
        return new RecipeDescriptor(
            name ?? this.Name,
            displayName ?? this.DisplayName,
            description ?? this.Description,
            tags ?? this.Tags,
            estimatedEffortPerOccurrence ?? this.EstimatedEffortPerOccurrence,
            options ?? this.Options,
            recipeList ?? this.RecipeList,
            // dataTables ?? this.DataTables,
            // maintainers ?? this.Maintainers,
            // contributors ?? this.Contributors,
            // examples ?? this.Examples,
            source ?? this.Source
        );
    }
}