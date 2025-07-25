namespace Rewrite.Rpc.Serialization;

public class RefTracker
{
    private Dictionary<object, long> _objToRef = new();
    private Dictionary<long, object> _refToObj = new();
    private long _refCounter;

    public bool AddRef(object obj, long refValue)
    {
        var a = _objToRef.TryAdd(obj, refValue);
        var b = _refToObj.TryAdd(refValue, obj);
        var isSuccess = a && b;
        return isSuccess;
    }
    public (bool IsNew, long? Id) GetOrAddRef(object? obj, long refValue = 0)
    {
        if (obj == null)
            return (false, null);
        if (refValue > 0)
        {
            // should probably be done with interlocks, but this is fine for now
            if(_refCounter > refValue)
                throw new InvalidOperationException("Attempt to assign ref value for one that was already used");
        }
        else
        {
            refValue = _refCounter + 1;
        }
        // var existing = new IsAdded();
        var isNew = _objToRef.TryAdd(obj, refValue);
        //
        // var refNum = _objToRef.GetOrAdd(obj, (objKey, existing2) =>
        // {
        //     existing2.IsNew = true;
        //     var refValue = Interlocked.Increment(ref _refCounter);
        //     return refValue;
        // }, existing);
        if (isNew)
        {
            _refToObj.TryAdd(refValue, obj);
            _refCounter = refValue;
        }
        else
        {
            refValue = _objToRef[obj];
        }
        return (isNew, refValue);
    }


    public bool TryGetObject(long refValue, out object? obj) => _refToObj.TryGetValue(refValue, out obj);
}