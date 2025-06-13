using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Rewrite.Core.Marker;
using static Rewrite.Rpc.Reference;
using static Rewrite.Rpc.RpcObjectData.ObjectState;

namespace Rewrite.Rpc;

public class RpcSendQueue
{
    private readonly int _batchSize;
    private readonly IList<RpcObjectData> _batch;
    private readonly Action<IList<RpcObjectData>> _drain;
    private readonly Dictionary<object, int> _refs;
    private readonly bool _trace;

    private object? _before;

    public RpcSendQueue(int batchSize, Action<IList<RpcObjectData>> drain, Dictionary<object, int> refs, bool trace)
    {
        _batchSize = batchSize;
        _batch = new List<RpcObjectData>(batchSize);
        _drain = drain;
        _refs = refs;
        _trace = trace;
    }

    public void Put(RpcObjectData rpcObjectData)
    {
        _batch.Add(rpcObjectData);
        if (_batch.Count == _batchSize)
        {
            Flush();
        }
    }

    /// <summary>
    /// Called whenever the batch size is reached or at the end of the tree.
    /// </summary>
    public void Flush()
    {
        if (_batch.Count == 0)
        {
            return;
        }
        _drain(new List<RpcObjectData>(_batch));
        _batch.Clear();
    }

    public async Task SendMarkersAsync<T>(T parent, Func<T, Markers> markersFn)
    {
        await GetAndSendAsync(parent, t2 => AsRef(markersFn(t2)), async markersRef =>
        {
            var markers = GetValue<Markers>(markersRef)!;
            await GetAndSendAsync(markers ?? throw new ArgumentNullException(), x => x.Id);
            await GetAndSendListAsync(markers, x => x?.MarkerList, x => x.Id, null);
        });
    }

    public async Task GetAndSendAsync<T, U>(T parent, Func<T, U?> value)
    {
        await GetAndSendAsync(parent, value, null);
    }

    public async Task GetAndSendAsync<T, U>(T parent, Func<T, U?> value, Func<U, Task>? onChange)
    {
        var after = value(parent);
        var before = _before == null ? default(U) : value((T)_before);
        await SendAsync(after, before, onChange == null || after == null ? null : () => onChange(after));
    }

    public async Task GetAndSendListAsync<T, U>(T? parent,
                                      Func<T?, IList<U>?> values,
                                      Func<U, object> id,
                                      Func<U, Task>? onChange)
    {
        var after = values(parent);
        var before = _before == null ? null : values((T)_before);
        await SendListAsync(after, before, id, onChange);
    }

    public async Task SendAsync<T>(T? after, T? before, Func<Task>? onChange)
    {
        object? afterVal = GetValue<object>(after);
        object? beforeVal = GetValue<object>(before);

        if (ReferenceEquals(beforeVal, afterVal))
        {
            Put(new RpcObjectData(NO_CHANGE, null, null, null, null));
        }
        else if (beforeVal == null)
        {
            await AddAsync(after, onChange);
        }
        else if (afterVal == null)
        {
            Put(new RpcObjectData(DELETE, null, null, null, null));
        }
        else
        {
            var afterCodec = after as IRpcCodec<object>;
            Put(new RpcObjectData(CHANGE, null, onChange == null && afterCodec == null ? afterVal : null, null, null));
            await DoChangeAsync(after, before, onChange, afterCodec);
        }
    }

    public async Task SendListAsync<T>(IList<T>? after,
                             IList<T>? before,
                             Func<T, object> id,
                             Func<T, Task>? onChange)
    {
        await SendAsync(after, before, async () =>
        {
            if (after == null)
                throw new InvalidOperationException("A DELETE event should have been sent.");

            var beforeIdx = PutListPositions(after, before, id);

            for (var i = 0; i < after.Count; i++)
            {
                var anAfter = after[i];
                var itemId = id(anAfter);
                beforeIdx.TryGetValue(itemId, out var beforePos);
                Func<Task>? onChangeRun = onChange == null ? null : () => onChange(anAfter);
                if (!beforeIdx.ContainsKey(itemId))
                {
                    await AddAsync(anAfter, onChangeRun);
                }
                else
                {
                    var aBefore = before == null ? default(T) : before[beforePos];
                    if (ReferenceEquals(aBefore, anAfter))
                    {
                        Put(new RpcObjectData(NO_CHANGE, null, null, null, null));
                    }
                    else
                    {
                        Put(new RpcObjectData(CHANGE, null, null, null, null));
                        await DoChangeAsync(anAfter, aBefore, onChangeRun, anAfter is IRpcCodec<object> codec ? codec : null);
                    }
                }
            }
        });
    }

    private Dictionary<object, int> PutListPositions<T>(IList<T> after, IList<T>? before, Func<T, object> id)
    {
        var beforeIdx = new Dictionary<object, int>();
        if (before != null)
        {
            for (var i = 0; i < before.Count; i++)
            {
                beforeIdx[id(before[i])!] = i;
            }
        }
        var positions = new List<int>();
        foreach (var t in after)
        {
            var itemId = id(t);
            beforeIdx.TryGetValue(itemId, out var beforePos);
            positions.Add(beforeIdx.ContainsKey(itemId) ? beforePos : RpcObjectData.ADDED_LIST_ITEM);
        }
        Put(new RpcObjectData(CHANGE, null, positions, null, null));
        return beforeIdx;
    }

    private async Task AddAsync(object? after, Func<Task>? onChange)
    {
        object? afterVal = Reference.GetValue<object?>(after);
        int? refValue = null;
        if (afterVal != null && !ReferenceEquals(after, afterVal)) /* Is a reference */
        {
            if (_refs.TryGetValue(afterVal, out var @ref))
            {
                Put(new RpcObjectData(ADD, GetValueType(afterVal), null, @ref, null));
                // No onChange call because the remote will be using an instance from its ref cache
                return;
            }
            refValue = _refs.Count + 1;
            _refs[afterVal] = refValue.Value;
        }
        IRpcCodec<object>? afterCodec = after is IRpcCodec<object> codec ? codec : null;
        Put(new RpcObjectData(ADD, GetValueType(afterVal), onChange == null && afterCodec == null ? afterVal : null, refValue, null));
        await DoChangeAsync(afterVal, null, onChange, afterCodec);
    }

    private async Task DoChangeAsync(object? after, object? before, Func<Task>? onChange, IRpcCodec<object>? afterCodec)
    {
        var lastBefore = _before;
        _before = before;
        try
        {
            if (onChange != null)
            {
                if (after != null)
                {
                    await onChange();
                }
            }
            else if (afterCodec != null && after != null)
            {
                await afterCodec.RpcSend(after, this);
            }
        }
        finally
        {
            _before = lastBefore;
        }
    }

    private static string? GetValueType(object? after)
    {
        if (after == null)
        {
            return null;
        }
        var type = after.GetType();
        if (type.IsPrimitive || type.Namespace?.StartsWith("System") == true ||
            type == typeof(Guid) || typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
        {
            return null;
        }
        return type.FullName;
    }
}