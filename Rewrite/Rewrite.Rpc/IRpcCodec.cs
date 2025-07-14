// /*
//  * Copyright 2025 the original author or authors.
//  *
//  * Licensed under the Apache License, Version 2.0 (the "License");
//  * you may not use this file except in compliance with the License.
//  * You may obtain a copy of the License at
//  *
//  * https://www.apache.org/licenses/LICENSE-2.0
//  *
//  * Unless required by applicable law or agreed to in writing, software
//  * distributed under the License is distributed on an "AS IS" BASIS,
//  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  * See the License for the specific language governing permissions and
//  * limitations under the License.
//  */
//
// namespace Rewrite.Rpc;
//
// /// <summary>
// /// A codec decomposes a value into multiple RPC <see cref="RpcObjectData"/> events, and
// /// on the receiving side reconstitutes the value from those events.
// /// </summary>
// /// <typeparam name="T">The type of the value being sent and received.</typeparam>
// public interface IRpcCodec<T>
// {
//     /// <summary>
//     /// When the value has been determined to have been changed, this method is called
//     /// to send the values that comprise it.
//     /// </summary>
//     /// <param name="after">The value that has been either added or changed.</param>
//     /// <param name="q">The send queue that is collecting <see cref="RpcObjectData"/> to send.</param>
//      Task RpcSend(T after, RpcSendQueue q);
//
//     /// <summary>
//     /// When the value has been determined to have been changed, this method is called
//     /// to receive the values that comprise it.
//     /// </summary>
//     /// <param name="before">The value that has been either added or changed. In the case where it is added,
//     /// the before state will be non-null, but will be an initialized object with
//     /// all null fields that are expecting to be populated by this method.</param>
//     /// <param name="q">The queue that is receiving <see cref="RpcObjectData"/> from a remote.</param>
//     /// <returns>The received value.</returns>
//     Task<T> RpcReceive(T before, RpcReceiveQueue q);
// }