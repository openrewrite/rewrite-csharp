using System.Collections.Immutable;
using Rewrite.Core;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

internal static class RemoteUtils
{
    private static readonly byte[] CopyBuffer = new byte[16 * 1024];
    private static readonly byte[] CommandEnd = [0x81, 0x17];

    internal static MemoryStream ReadToCommandEnd(Stream stream)
    {
        var memoryStream = new MemoryStream();

        int bytesRead;
        while ((bytesRead = stream.Read(CopyBuffer, 0, CopyBuffer.Length)) > 0)
        {
            memoryStream.Write(CopyBuffer, 0, bytesRead);
            if (bytesRead > 1 && CopyBuffer[bytesRead - 2] == CommandEnd[0] &&
                CopyBuffer[bytesRead - 1] == CommandEnd[1])
            {
                break;
            }
            else if (bytesRead == 1)
            {
                var originalPosition = memoryStream.Position;
                memoryStream.Position = memoryStream.Length - 2;
                if (memoryStream.ReadByte() == CommandEnd[0] && memoryStream.ReadByte() == CommandEnd[1])
                {
                    memoryStream.Position = originalPosition;
                    break;
                }
            }
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    internal static IList<T>? ReceiveNodes<T>(IList<T>? before, ReceiverContext.DetailsReceiver<T> details,
        ReceiverContext ctx)
    {
        var listEvent = ctx.Receiver.ReceiveValue(typeof(IList<>));
        switch (listEvent.EventType)
        {
            case EventType.NoChange:
                return before;
            case EventType.Delete:
                return null;
            case EventType.Add:
                var afterSize = (int)listEvent.Msg!;
                var after = new List<T>(afterSize);
                for (var i = 0; i < afterSize; i++)
                {
                    var diffEvent = ctx.Receiver.ReceiveNode();
                    if (diffEvent.EventType == EventType.Add)
                    {
                        after.Add(details(default, diffEvent.ConcreteType, ctx));
                    }
                    else if (diffEvent.EventType == EventType.NoChange)
                    {
                        after.Add(default!);
                    }
                    else
                    {
                        throw new NotImplementedException("Unexpected operation: " + diffEvent.EventType);
                    }
                }

                return after;
            case EventType.Update:
                return ReceiveUpdatedNodes<T>(before!, (int)listEvent.Msg!, details, ctx);
            default:
                throw new NotImplementedException("Unexpected operation: " + listEvent.EventType);
        }
    }

    private static IList<T> ReceiveUpdatedNodes<T>(IList<T> before, int afterSize,
        ReceiverContext.DetailsReceiver<T> details, ReceiverContext ctx)
    {
        var modified = false;
        var afterList = before;
        var evt = ctx.Receiver.ReceiveNode();
        if (evt.EventType != EventType.StartList)
        {
            throw new InvalidOperationException("Expected start list event: " + evt.EventType);
        }

        var beforeIdx = 0;
        do
        {
            evt = ctx.Receiver.ReceiveNode();
            switch (evt.EventType)
            {
                case EventType.NoChange:
                case EventType.EndList:
                    break;
                case EventType.Delete:
                case EventType.Update:
                case EventType.Add:
                    if (!modified)
                    {
                        afterList = CopyRange(before, beforeIdx);
                        modified = true;
                    }

                    break;
                default:
                    throw new NotSupportedException("Unexpected operation: " + evt.EventType);
            }

            switch (evt.EventType)
            {
                case EventType.NoChange:
                    if (modified)
                    {
                        afterList.Add(before[beforeIdx]);
                    }

                    beforeIdx++;
                    break;
                case EventType.Delete:
                    beforeIdx++;
                    break;
                case EventType.Update:
                    afterList.Add(details(before[beforeIdx], evt.ConcreteType, ctx)!);
                    beforeIdx++;
                    break;
                case EventType.Add:
                    afterList.Add(details(default, evt.ConcreteType, ctx));
                    break;
            }
        } while (evt.EventType != EventType.EndList);

        return afterList.Count > afterSize ? ((List<T>)afterList).GetRange(0, afterSize) : afterList;
    }

    internal static IList<T>? ReceiveValues<T>(IList<T>? before, ReceiverContext ctx)
    {
        var listEvent = ctx.Receiver.ReceiveValue(typeof(IList<>));
        switch (listEvent.EventType)
        {
            case EventType.NoChange:
                return before;
            case EventType.Delete:
                return null;
            case EventType.Add:
                var afterSize = (int)listEvent.Msg!;
                var after = new List<T>(afterSize);
                for (var i = 0; i < afterSize; i++)
                {
                    var diffEvent = ctx.Receiver.ReceiveValue(typeof(T));
                    if (diffEvent.EventType == EventType.Add)
                    {
                        after.Add((T)diffEvent.Msg!);
                    }
                    else if (diffEvent.EventType == EventType.NoChange)
                    {
                        after.Add(default!);
                    }
                    else
                    {
                        throw new NotImplementedException("Unexpected operation: " + diffEvent.EventType);
                    }
                }

                return after;
            case EventType.Update:
                return ReceiveUpdatedValues<T>(before!, (int)listEvent.Msg!, ctx);
            default:
                throw new NotImplementedException("Unexpected operation: " + listEvent.EventType);
        }
    }

    private static IList<T> ReceiveUpdatedValues<T>(IList<T> before, int afterSize, ReceiverContext ctx)
    {
        var modified = false;
        var afterList = before;
        var evt = ctx.Receiver.ReceiveNode();
        if (evt.EventType != EventType.StartList)
        {
            throw new InvalidOperationException("Expected start list event: " + evt.EventType);
        }

        var beforeIdx = 0;
        do
        {
            evt = ctx.Receiver.ReceiveValue(typeof(T));
            switch (evt.EventType)
            {
                case EventType.NoChange:
                case EventType.EndList:
                    break;
                case EventType.Delete:
                case EventType.Update:
                case EventType.Add:
                    if (!modified)
                    {
                        afterList = CopyRange(before, beforeIdx);
                        modified = true;
                    }

                    break;
                default:
                    throw new NotSupportedException("Unexpected operation: " + evt.EventType);
            }

            switch (evt.EventType)
            {
                case EventType.NoChange:
                    if (modified)
                    {
                        afterList.Add(before[beforeIdx]);
                    }

                    beforeIdx++;
                    break;
                case EventType.Delete:
                    beforeIdx++;
                    break;
                case EventType.Update:
                    afterList.Add((T)evt.Msg!);
                    beforeIdx++;
                    break;
                case EventType.Add:
                    afterList.Add((T)evt.Msg!);
                    break;
            }
        } while (evt.EventType != EventType.EndList);

        return afterList.Count > afterSize ? ((List<T>)afterList).GetRange(0, afterSize) : afterList;
    }

    internal static void CalculateListDiff<T, I>(IList<T> before, IList<T> after, Func<T, I> idFunction,
        ListDiffConsumer<T> consumer) where T : class where I : notnull
    {
        int beforeIdx = 0, afterIdx = 0;
        int beforeSize = before.Count, afterSize = after.Count;
        Dictionary<I, int>? afterMap = null;

        while (beforeIdx < beforeSize || afterIdx < afterSize)
        {
            // Check if we've reached the end of either of the lists
            if (beforeIdx >= beforeSize)
            {
                consumer(Operation.Add, -1, afterIdx, default, after[afterIdx++]);
                continue;
            }
            else if (afterIdx >= afterSize)
            {
                consumer(Operation.Delete, beforeIdx, -1, before[beforeIdx++], default);
                continue;
            }

            if (before[beforeIdx] == after[afterIdx])
            {
                consumer(
                    Operation.NoChange,
                    beforeIdx,
                    afterIdx,
                    before[beforeIdx++],
                    after[afterIdx++]
                );
            }
            else
            {
                I beforeId = idFunction(before[beforeIdx]);
                I afterId = idFunction(after[afterIdx]);
                if (beforeId!.Equals(afterId))
                {
                    consumer(
                        Operation.Update,
                        beforeIdx,
                        afterIdx,
                        before[beforeIdx++],
                        after[afterIdx++]
                    );
                }
                else
                {
                    if (afterMap == null)
                    {
                        afterMap = CreateIndexMap(after, afterIdx, idFunction);
                    }

                    // If elements at current indices are not equal, figure out the operation
                    if (!afterMap.ContainsKey(beforeId))
                    {
                        consumer(Operation.Delete, beforeIdx, -1, before[beforeIdx++], null);
                    }
                    else
                    {
                        consumer(Operation.Add, -1, afterIdx, null, after[afterIdx++]);
                    }
                }
            }
        }
    }

    private static Dictionary<I, int> CreateIndexMap<I, T>(IList<T> list, int fromIndex, Func<T, I> idFunction) where I : notnull
    {
        var result = new Dictionary<I, int>(list.Count - fromIndex);
        for (var i = fromIndex; i < list.Count; i++)
        {
            result.Add(idFunction(list[i]), i);
        }

        return result;
    }


    private static List<T> CopyRange<T>(IEnumerable<T> before, int j)
    {
        return before switch
        {
            List<T> list => [..list.GetRange(0, j)],
            ImmutableList<T> immutableList => [..immutableList.GetRange(0, j)],
            _ => [..before.Take(j)]
        };
    }

    internal delegate void ListDiffConsumer<T>(Operation op, int beforeIndex, int afterIndex, T? beforeValue,
        T? afterValue);

    internal enum Operation
    {
        NoChange,
        Update,
        Add,
        Delete,
        Move
    }
}
