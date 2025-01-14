using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;


public class AsyncTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    private void Simple()
    {
        RewriteRun(
          CSharp(
              """
              class T
              {
                  async System.Threading.Tasks.Task<int> M()
                  {
                      return await M();
                  }
              }
              """
          )
        );
    }
    [Fact]
    private void AwaitStatement()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    async System.Threading.Tasks.Task M()
                    {
                        await M();
                    }
                }
                var requestAction =  async () => await sutProvider.Sut.RemoveDomain(orgId, id);
                """
            )
        );
    }

    [Fact]
    private void AsyncLambda()
    {
        RewriteRun(
          CSharp(
              """
              var requestAction = async () => await sutProvider.Sut.RemoveDomain(orgId, id);
              """
          )
        );
    }

}
