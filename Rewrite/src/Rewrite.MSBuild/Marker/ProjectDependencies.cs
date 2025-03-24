namespace Rewrite.MSBuild.Marker;

public record ProjectDependencies(Guid Id, string ProjectFile, IList<Dependency> Dependencies) : Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is ProjectDependencies && other.Id.Equals(Id);
    }
}
