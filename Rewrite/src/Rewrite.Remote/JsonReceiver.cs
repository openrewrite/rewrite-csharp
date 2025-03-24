using System.Formats.Cbor;

namespace Rewrite.Remote;

public class JsonReceiver(MemoryStream stream, DeserializationContext context)
    : TreeReceiver
{
    private const bool Debug = false;

    private readonly CborReader _reader = new(stream.ToArray(), CborConformanceMode.Lax, true);
#if(DEBUG)
    private int _count;
#endif

    public DiffEvent ReceiveNode()
    {
        switch (_reader.PeekState())
        {
            case CborReaderState.StartArray:
                _reader.ReadStartArray();
                var eventType = (EventType)_reader.ReadInt32();
                object? msg = null;
                string? concreteType = null;
                switch (eventType)
                {
                    case EventType.Add:
                    case EventType.Update:
                        concreteType = eventType == EventType.Add && _reader.PeekState() == CborReaderState.TextString
                            ? _reader.ReadTextString()
                            : null;
                        break;
                    case EventType.Delete:
                    case EventType.NoChange:
                    case EventType.StartList:
                    case EventType.EndList:
                        break;
                    default:
                        throw new NotImplementedException(eventType.ToString());
                }

                _reader.ReadEndArray();
#if(DEBUG)
                //todo: switch to proper logging infrastructure
                Console.WriteLine("[" + _count++ + "] " + new DiffEvent(eventType, concreteType, msg));
#endif
                return new DiffEvent(eventType, concreteType, msg);
            default:
                throw new NotImplementedException();
        }
    }

    public DiffEvent ReceiveValue(Type expectedType)
    {
        switch (_reader.PeekState())
        {
            case CborReaderState.StartArray:
                _reader.ReadStartArray();
                var eventType = (EventType)_reader.ReadInt32();
                object? msg = null;
                string? concreteType = null;
                switch (eventType)
                {
                    case EventType.Add:
                    case EventType.Update:
                        if (expectedType.Name != "Nullable`1" && expectedType.IsGenericType &&
                                 (expectedType.GetGenericTypeDefinition().IsAssignableTo(typeof(IList<>)) ||
                                  expectedType.GetGenericTypeDefinition().IsAssignableTo(typeof(List<>))))
                            // this is a special case for list events
                            msg = _reader.ReadInt32();
                        else
                            msg = context.Deserialize(expectedType, _reader);
                        break;
                    case EventType.Delete:
                    case EventType.NoChange:
                    case EventType.StartList:
                    case EventType.EndList:
                        break;
                    default:
                        throw new NotImplementedException(eventType.ToString());
                }
#if(DEBUG)
                //todo: switch to proper logging infrastructure
                Console.WriteLine("[" + _count++ + "] " + new DiffEvent(eventType, concreteType, msg));
#endif
                _reader.ReadEndArray();
                return new DiffEvent(eventType, concreteType, msg);
            default:
                throw new NotImplementedException("Unexpected state: " + _reader.PeekState());
        }
    }
}
