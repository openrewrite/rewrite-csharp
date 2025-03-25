using Rewrite.Test.Engine.Remote;
using Xunit;

namespace Rewrite.CSharp.Tests;

[CollectionDefinition("C# remoting")]
public class RemotingCollection : ICollectionFixture<RemotingFixture>;
