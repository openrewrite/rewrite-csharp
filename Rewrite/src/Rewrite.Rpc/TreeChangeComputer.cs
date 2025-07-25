// using System.Runtime.CompilerServices;
//
// namespace Rewrite.Rpc;
//
// public class TreeChangeComputer
// {
//     public List<RpcObjectData> GetChanges(object before, object after)
//     {
//         var result = new List<RpcObjectData>();
//         if (before == after)
//         {
//             result.Add(new RpcObjectData { State = RpcObjectData.ObjectState.NO_CHANGE });
//         }
//         // else if (beforeVal == null) {
//     }
// }
//
// public class SessionCache
// {
//     private readonly Dictionary<string, object?> _remoteObjects = new();
//     private readonly Dictionary<string, object> _localObjects = new();
//     // private readonly Dictionary<string, Recipe> _preparedRecipes = new();
//     public void Test()
//     {
//         _localObjects.GetValue()
//     }
// }