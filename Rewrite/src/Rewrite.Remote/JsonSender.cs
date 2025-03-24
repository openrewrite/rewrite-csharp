using System.Formats.Cbor;
using System.Numerics;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

public class JsonSender(Stream stream, SerializationContext context) : TreeSender
{
    private const bool Debug = false;
    private readonly CborWriter _writer = new(CborConformanceMode.Lax, allowMultipleRootLevelValues: true);

    public void SendNode(DiffEvent diffEvent, Action<TreeSender> visitor)
    {
        switch (diffEvent.EventType)
        {
            case EventType.Add:
            case EventType.Update:
                _writer.WriteStartArray(diffEvent.ConcreteType != null ? 2 : 1);

                _writer.WriteInt32((int)diffEvent.EventType);

                if (diffEvent.ConcreteType != null)
                {
                    _writer.WriteTextString(diffEvent.ConcreteType);
                }
#if(DEBUG)
                //todo: switch to proper logging infrastructure
                Console.WriteLine("SEND: " + diffEvent);
#endif
                _writer.WriteEndArray();

                stream.Write(_writer.Encode());
                _writer.Reset(); // reset the writer for the next frames

                visitor(this);
                break;
            case EventType.Delete:
            case EventType.NoChange:
                _writer.WriteStartArray(1);
                _writer.WriteInt32((int)diffEvent.EventType);

#if(DEBUG)
                //todo: switch to proper logging infrastructure
                Console.WriteLine("SEND: " + diffEvent);
#endif
                _writer.WriteEndArray();

                stream.Write(_writer.Encode());
                _writer.Reset(); // reset the writer for the next frames
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void SendValue<T>(DiffEvent diffEvent)
    {
        switch (diffEvent.EventType)
        {
            case EventType.Add:
            case EventType.Update:
                _writer.WriteStartArray(diffEvent.ConcreteType != null ? 3 : 2);
                _writer.WriteInt32((int)diffEvent.EventType);
                if (diffEvent.ConcreteType != null)
                {
                    _writer.WriteTextString(diffEvent.ConcreteType);
                }

                context.Serialize(diffEvent.Msg, diffEvent.ConcreteType, _writer);
                _writer.WriteEndArray();
                break;
            case EventType.Delete:
            case EventType.NoChange:
            case EventType.StartList:
            case EventType.EndList:
                _writer.WriteStartArray(1);
                _writer.WriteInt32((int)diffEvent.EventType);
                _writer.WriteEndArray();
                break;
            default:
                throw new NotImplementedException();
        }
#if(DEBUG)
        //todo: switch to proper logging infrastructure
        Console.WriteLine("SEND: " + diffEvent);
#endif
        stream.Write(_writer.Encode(), 0, _writer.Encode().Length);
        _writer.Reset();
    }

    public void Flush()
    {
        stream.Flush();
    }
}
