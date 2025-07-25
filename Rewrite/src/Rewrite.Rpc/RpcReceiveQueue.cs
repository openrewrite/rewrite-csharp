// /*
//  * Copyright 2025 the original author or authors.
//  * <p>
//  * Licensed under the Apache License, Version 2.0 (the "License");
//  * you may not use this file except in compliance with the License.
//  * You may obtain a copy of the License at
//  * <p>
//  * https://www.apache.org/licenses/LICENSE-2.0
//  * <p>
//  * Unless required by applicable law or agreed to in writing, software
//  * distributed under the License is distributed on an "AS IS" BASIS,
//  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  * See the License for the specific language governing permissions and
//  * limitations under the License.
//  */
//
// using Rewrite.Core.Marker;
//
// namespace Rewrite.Rpc;
//
// public class RpcReceiveQueue
// {
//     private readonly IList<RpcObjectData> batch;
//     private readonly IDictionary<int, object> refs;
//     private readonly TextWriter? logFile;
//     private readonly Func<Task<IList<RpcObjectData>>> pull;
//
//     public RpcReceiveQueue(IDictionary<int, object> refs, TextWriter? logFile,
//         Func<Task<IList<RpcObjectData>>> pull)
//     {
//         this.refs = refs;
//         this.batch = new List<RpcObjectData>();
//         this.logFile = logFile;
//         this.pull = pull;
//     }
//
//     public async Task<RpcObjectData> TakeAsync()
//     {
//         if (batch.Count == 0)
//         {
//             var data = await pull();
//             foreach (var item in data)
//             {
//                 batch.Add(item);
//             }
//         }
//         var result = batch[0];
//         batch.RemoveAt(0);
//         return result;
//     }
//
//     /// <summary>
//     /// Receive a value from the queue and apply a function to it to convert it to the required
//     /// type.
//     /// </summary>
//     /// <param name="before">The value to apply the function to, which may be null.</param>
//     /// <param name="mapping">Function to apply in case of ADD or CHANGE to convert the received value to
//     ///                the desired type. Only applied when the value from the queue is not null.</param>
//     /// <typeparam name="T">The property value type of the before and returned value.</typeparam>
//     /// <typeparam name="U">The type of the value as encoded by the data item read from the queue.</typeparam>
//     /// <returns>The received and converted value. When the received state is NO_CHANGE then the
//     /// before value will be returned.</returns>
//     public async Task<T?> ReceiveAndGetAsync<T, U>(T? before, Func<U, T?> mapping)
//     {
//         var after = await ReceiveAsync(before, null);
//         return after != null && !ReferenceEquals(after, before) ? mapping((U)(object)after) : after;
//     }
//
//     public async Task<Markers?> ReceiveMarkersAsync(Markers markers)
//     {
//         return await ReceiveAsync(markers,  async m => m
//             .WithId(await ReceiveAndGetAsync(m.Id, (string s) => Guid.Parse(s)))
//             .WithMarkers(await ReceiveListAsync(m.MarkerList, null)));
//         
//         
//     }
//
//     /// <summary>
//     /// Receive a simple value from the remote.
//     /// </summary>
//     /// <param name="before">The before state.</param>
//     /// <typeparam name="T">The type of the value being received.</typeparam>
//     /// <returns>The received value.</returns>
//     public async Task<T?> ReceiveAsync<T>(T? before)
//     {
//         return await ReceiveAsync(before, null);
//     }
//
//     /// <summary>
//     /// Receive a value from the remote and, when it is an ADD or CHANGE, invoke a callback
//     /// to receive its constituent parts.
//     /// </summary>
//     /// <param name="before">The before state.</param>
//     /// <param name="onChange">When the state is ADD or CHANGE, this function is called to receive the
//     ///                 pieces of this value. If the callback is null, the value is assumed to
//     ///                 be in the value part of the message and is deserialized directly.</param>
//     /// <typeparam name="T">The type of the value being received.</typeparam>
//     /// <returns>The received value.</returns>
//     public async Task<T?> ReceiveAsync<T>(T? before, Func<T, Task<T>>? onChange)
//     {
//         var message = await TakeAsync();
//         // todo: figure out this tracing thing
//         // if (logFile != null && message.Trace != null)
//         // {
//         //     logFile.WriteLine(message.WithTrace(null));
//         //     logFile.WriteLine("  " + message.Trace);
//         //     logFile.WriteLine("  " + Trace.TraceReceiver());
//         //     logFile.Flush();
//         // }
//         int? refValue = null;
//         switch (message.State)
//         {
//             case RpcObjectData.ObjectState.NO_CHANGE:
//                 return before;
//             case RpcObjectData.ObjectState.DELETE:
//                 return default(T);
//             case RpcObjectData.ObjectState.ADD:
//                 refValue = message.Ref;
//                 if (refValue != null && refs.TryGetValue(refValue.Value, out var @ref))
//                 {
//                     return (T?)@ref;
//                 }
//                 before = message.ValueType == null ? (T?)message.Value : NewObj<T>(message.ValueType);
//                 goto case RpcObjectData.ObjectState.CHANGE;
//             case RpcObjectData.ObjectState.CHANGE:
//                 T? after;
//
//                 // TODO handle enums here
//
//                 if (onChange != null)
//                 {
//                     after = await onChange(before!);
//                 }
//                 else if (before is IRpcCodec<T>)
//                 {
//                     after = await ((IRpcCodec<T>)before).RpcReceive(before, this);
//                 }
//                 else if (message.ValueType == null)
//                 {
//                     after = message.GetValue<T>();
//                 }
//                 else
//                 {
//                     after = before;
//                 }
//                 if (refValue != null)
//                 {
//                     refs[refValue.Value] = after!;
//                 }
//                 return after;
//             default:
//                 throw new NotSupportedException("Unknown state type " + message.State);
//         }
//     }
//
//     public async Task<IList<T>> ReceiveListAsync<T>(IList<T>? before, Func<T, Task<T>>? onChange)
//     {
//         var msg = await TakeAsync();
//         switch (msg.State)
//         {
//             case RpcObjectData.ObjectState.NO_CHANGE:
//                 return before!;
//             case RpcObjectData.ObjectState.DELETE:
//                 return null!;
//             case RpcObjectData.ObjectState.ADD:
//                 before = new List<T>();
//                 goto case RpcObjectData.ObjectState.CHANGE;
//             case RpcObjectData.ObjectState.CHANGE:
//                 msg = await TakeAsync(); // the next message should be a CHANGE with a list of positions
//                 if (msg.State != RpcObjectData.ObjectState.CHANGE)
//                     throw new InvalidOperationException("Expected CHANGE state");
//                 var positions = msg.GetValue<IList<int>>() ?? throw new InvalidOperationException("Positions cannot be null");
//                 IList<T> after = new List<T>(positions.Count);
//                 foreach (var beforeIdx in positions)
//                 {
//                     after.Add((await ReceiveAsync(beforeIdx >= 0 ? before![beforeIdx] : default, onChange))!);
//                 }
//                 return after;
//             default:
//                 throw new NotSupportedException(msg.State + " is not supported for lists.");
//         }
//     }
//
//     private T NewObj<T>(string type)
//     {
//         try
//         {
//             var clazz = Type.GetType(type) ?? throw new TypeLoadException($"Type {type} not found");
//             return (T)Activator.CreateInstance(clazz)!;
//         }
//         catch (Exception e)
//         {
//             throw new InvalidOperationException("Failed to create instance of type: " + type, e);
//         }
//     }
//
//     /// <summary>
//     /// </summary>
//     /// <param name="enumType">The enumeration that we are creating or updating</param>
//     /// <typeparam name="T">The enum type.</typeparam>
//     /// <returns>An enum mapping function that can be used when receiving a string to convert
//     /// it to an enum value.</returns>
//     public static Func<object, T> ToEnum<T>(Type enumType) where T : Enum
//     {
//         return value => (T)Enum.Parse(enumType, (string)value);
//     }
// }