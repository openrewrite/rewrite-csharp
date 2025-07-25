// using System.Collections.Concurrent;
// using Newtonsoft.Json;
// using Rewrite.Core;
// using Rewrite.Core.Config;
// using Rewrite.MSBuild;
// using StreamJsonRpc;
// using static Rewrite.Rpc.RpcObjectData.ObjectState;
//
// namespace Rewrite.Rpc;
//
// public class LanguageServer(RecipeManager recipeManager, ILanguageServer counterparty) : ILanguageServer
// {
//     private readonly ILanguageServer _counterparty = counterparty;
//
//     /// <summary>
//     /// Keeps track of the local and remote state of objects that are used in
//     /// visits and other operations for which incremental state sharing is useful
//     /// between two processes.
//     /// </summary>
//     private readonly Dictionary<string, object?> _remoteObjects = new();
//     private readonly Dictionary<Recipe, Cursor> _recipeCursors = new();
//     private readonly Dictionary<string, object?> _localObjects = new();
//     private readonly Dictionary<string, Recipe> _preparedRecipes = new();
//     private readonly Dictionary<int, object> _remoteRefs = new();
//     
//     // private readonly int _batchSize = 10;
//     // private readonly bool _trace = true;
//     
//     private readonly ConcurrentDictionary<string, Queue<List<RpcObjectData>>> _inProgressGetRpcObjects = new();
//     /// <summary>
//     /// Keeps track of objects that need to be referentially deduplicated, and
//     /// the ref IDs to look them up by on the remote.
//     /// </summary>
//     private readonly Dictionary<object, int> _localRefs = new ();
//     
//     private RecipeManager _recipeManager = recipeManager;
//
//
//     public Task<string[]> Parse(ParseRequest request)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task<VisitResponse> Visit(VisitRequest request)
//     {
//         
//         var before = await GetObject<Tree>(request.TreeId);
//         var p = (IExecutionContext)(await GetVisitorP(request))!;
//
//         var visitor = InstantiateVisitor(request, p);
//
//         var after = visitor.Visit(before, p, GetCursor(request.Cursor));
//         if (after == null) 
//         {
//             _localObjects.Remove(before!.Id.ToString());
//         } 
//         else 
//         {
//             _localObjects[after.Id.ToString()] = after;
//         }
//
//         return new VisitResponse { Modified = !ReferenceEquals(before, after) };
//     }
//
//     public Task<string> Print(PrintRequest request)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task<PrepareRecipeResponse> PrepareRecipe(PrepareRecipeRequest request)
//     {
//         var installableRecipe = InstallableRecipe.ParseUri(request.Id);
//         var recipePackage = await _recipeManager.InstallRecipePackage(installableRecipe);
//         var recipeDescriptor = recipePackage.GetRecipeDescriptor(request.Id);
//         var recipeStartInfo = recipePackage.CreateRecipeStartInfo(recipeDescriptor, request.Options);
//         var recipe = _recipeManager.CreateRecipe(recipeStartInfo);
//         var recipeInstanceId = Guid.NewGuid().ToString();
//         _preparedRecipes.Add(recipeInstanceId, recipe);
//         var result = new PrepareRecipeResponse()
//         {
//             Id = recipeInstanceId,
//             Options = recipeDescriptor,
//             EditVisitor = $"edit:{recipeInstanceId}",
//             ScanVisitor = $"scan:{recipeInstanceId}",
//         };
//         return result;
//     }
//
//     public Task<string[]> Generate(GenerateRequest request)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<List<RecipeDescriptor>> GetRecipes()
//     {
//         throw new NotImplementedException();
//     }
//
//     private Task<object?> GetObject(string id) => GetObject<object?>(id);
//     private async Task<T?> GetObject<T>(string id)
//     {
//         var q = new RpcReceiveQueue(_remoteRefs, null, async () => await GetObject(new GetObjectRequest() { Id = id }));
//         var remoteObject = await q.ReceiveAsync(_localObjects[id], null);
//         var item = await q.TakeAsync();
//         if (item.State != END_OF_OBJECT) {
//             throw new InvalidOperationException("Expected END_OF_OBJECT");
//         }
//         // We are now in sync with the remote state of the object.
//         _remoteObjects[id] = remoteObject;
//         _localObjects[id] = remoteObject;
//         return (T?)remoteObject;
//     }
//     public async Task<List<RpcObjectData>> GetObject(GetObjectRequest request)
//     {
//         throw new NotImplementedException();
//         // var after = _localObjects[request.Id];
//         //
//         // if (after == null)
//         // {
//         //     var deleted = new List<RpcObjectData>(2);
//         //     deleted.Add(new RpcObjectData(DELETE, null, null, null, null));
//         //     deleted.Add(new RpcObjectData(END_OF_OBJECT, null, null, null, null));
//         //     return deleted;
//         // }
//         //
//         // var before = _remoteObjects[request.Id];
//         // var sendQueue = new RpcSendQueue(_batchSize, batch.Add, _localRefs, _trace);
//         // await sendQueue.SendAsync(after, before, null);
//         //
//         // var q = _inProgressGetRpcObjects.GetOrAdd(request.Id, id =>
//         // {
//         //     var batch = new BlockingCollection<IList<RpcObjectData>>(1);
//         //     var before = _remoteObjects[id];
//         //
//         //     var sendQueue = new RpcSendQueue(_batchSize, batch.Add, _localRefs, _trace);
//         //     await sendQueue.SendAsync(after, before, null);
//         //     Task.Run(() =>
//         //     {
//         //         try
//         //         {
//         //             sendQueue.Send(after, before, null);
//         //
//         //             // All the data has been sent, and the remote should have received
//         //             // the full tree, so update our understanding of the remote state
//         //             // of this tree.
//         //             _remoteObjects[id] = after;
//         //         }
//         //         catch (Exception ignored)
//         //         {
//         //             // TODO do something with this exception
//         //         }
//         //         finally
//         //         {
//         //             sendQueue.Put(new RpcObjectData(END_OF_OBJECT, null, null, null, null));
//         //             sendQueue.Flush();
//         //         }
//         //         return 0;
//         //     });
//         //     return batch;
//         // });
//         //
//         // var batch = q.Take();
//         // if (batch[batch.Count - 1].State == END_OF_OBJECT)
//         // {
//         //     _inProgressGetRpcObjects.TryRemove(request.Id, out _);
//         // }
//         //
//         // return batch;
//     }
//     
//     /// <summary>
//     /// Gets the visitor parameter from the request.
//     /// This is really probably particular to the Java implementation,
//     /// because we are carrying forward the legacy of cycles that are likely to be
//     /// removed from OpenRewrite in the future.
//     /// </summary>
//     /// <param name="request">The visit request</param>
//     /// <returns>The visitor parameter object</returns>
//     private async Task<object?> GetVisitorP(VisitRequest request)
//     {
//         var p = await GetObject(request.P);
//         // if (p is ExecutionContext context)
//         // {
//         //     string visitorName = request.Visitor;
//         //
//         //     if (visitorName.StartsWith("scan:") || visitorName.StartsWith("edit:"))
//         //     {
//         //         Recipe recipe = preparedRecipes[visitorName.Substring("edit:".Length /* 'scan:' has same length*/)];
//         //         // This is really probably particular to the Java implementation,
//         //         // because we are carrying forward the legacy of cycles that are likely to be
//         //         // removed from OpenRewrite in the future.
//         //         WatchableExecutionContext ctx = new WatchableExecutionContext(context);
//         //         ctx.PutCycle(new RecipeRunCycle<object>(recipe, 0, new Cursor(null, Cursor.ROOT_VALUE), ctx,
//         //             new RecipeRunStats(Recipe.Noop()), new SourcesFileResults(Recipe.Noop()),
//         //             new SourcesFileErrors(Recipe.Noop()), LargeSourceSet.Edit));
//         //         ctx.PutCurrentRecipe(recipe);
//         //         return ctx;
//         //     }
//         // }
//         return p;
//     }
//
//
//     /// <summary>
//     /// Instantiates a visitor based on the provided request and parameter.
//     /// </summary>
//     /// <param name="request">The visit request containing visitor information</param>
//     /// <param name="p">The parameter object</param>
//     /// <returns>A TreeVisitor instance</returns>
//     /// <exception cref="ArgumentException">Thrown when visitor class cannot be found</exception>
//     private ITreeVisitor<Tree, IExecutionContext> InstantiateVisitor(VisitRequest request, object p) 
//     {
//         var visitorName = request.Visitor;
//
//         if (visitorName.StartsWith("scan:")) 
//         {
//             System.Diagnostics.Debug.Assert(p is ExecutionContext);
//
//             var recipe = (ScanningRecipe<object>)_preparedRecipes[visitorName.Substring("scan:".Length)];
//             if (!_recipeCursors.TryGetValue(recipe, out var cursor))
//             {
//                 cursor = new Cursor(null, Cursor.ROOT_VALUE);
//                 _recipeCursors[recipe] = cursor;
//             }
//             
//             var acc = recipe.GetAccumulator(cursor, (IExecutionContext)p);
//             return recipe.GetScanner(acc);
//         } 
//         else if (visitorName.StartsWith("edit:")) 
//         {
//             var recipe = _preparedRecipes[visitorName.Substring("edit:".Length)];
//             return recipe.GetVisitor();
//         }
//
//         // var serializer = new JsonSerializer();
//         // serializer.Populate
//         throw new InvalidOperationException($"Can't find recipe {request.Visitor}");
//         // var withJsonType = request.VisitorOptions == null ?
//         //     new Dictionary<string, object>() :
//         //     new Dictionary<string, object>(request.VisitorOptions);
//         // withJsonType["@c"] = visitorName;
//         //
//         // try 
//         // {
//         //     Type visitorType = TypeFactory.DefaultInstance.FindClass(visitorName);
//         //     return (TreeVisitor<object, object>)mapper.ConvertValue(withJsonType, visitorType);
//         // } 
//         // catch (TypeLoadException e) 
//         // {
//         //     throw new RuntimeException(e);
//         // }
//     }
//     
//     /// <summary>
//     /// Gets a cursor based on the provided cursor IDs.
//     /// </summary>
//     /// <param name="cursorIds">The list of cursor IDs, can be null</param>
//     /// <returns>A cursor object</returns>
//     private Cursor GetCursor(IList<string>? cursorIds)
//     {
//         var cursor = new Cursor(null, Cursor.ROOT_VALUE);
//         if (cursorIds != null)
//         {
//             for (var i = cursorIds.Count - 1; i >= 0; i--)
//             {
//                 var cursorId = cursorIds[i];
//                 object cursorObject = GetObject(cursorId);
//                 _remoteObjects[cursorId] = cursorObject;
//                 cursor = new Cursor(cursor, cursorObject);
//             }
//         }
//         return cursor;
//     }
// }