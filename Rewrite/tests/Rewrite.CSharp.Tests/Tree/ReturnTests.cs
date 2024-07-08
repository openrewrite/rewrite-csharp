using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class ReturnTests : RewriteTest
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo 
                {
                    public int M() 
                    {
                        return 1;
                    }    
                }
                "
            )
        );
    }

    [Fact]
    public void Void()
    {
        RewriteRun(
            CSharp(
                @"
                public class T 
                {
                    public void M()
                    {
                        return;
                    }
                }
                "
            )
        );
    }
}