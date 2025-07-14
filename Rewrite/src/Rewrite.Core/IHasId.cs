namespace Rewrite.Core;

public interface IHasId<out TKey>
{
    TKey Id { get; }
}