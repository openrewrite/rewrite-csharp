namespace Rewrite.Remote;

public interface TreeSender
{
    void SendNode(DiffEvent diffEvent, Action<TreeSender> visitor);
    void SendValue<T>(DiffEvent diffEvent);
    void Flush();
}