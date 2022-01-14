using NUnit.Framework;

namespace CityInfo.Chat.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
public class AsyncErrorHandling
{

    [Test]
    public async Task AsyncErrorHandlingTest()
    {
        await process(GenerateWithPostAction());
    }
    
    private async IAsyncEnumerable<int> GenerateWithPostAction()
    {
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(500);
            TestContext.Progress.WriteLine($"sending {i}");
            yield return i;
            TestContext.Progress.WriteLine($"cleanup after {i}");
        }
    }

    private async Task process(IAsyncEnumerable<int> ints)
    {
        await foreach (var i in ints)
        {
            if (i > 3)
            {
                TestContext.Error.WriteLine($"Throwing error {i}");
                throw new Exception("Processing error");
            }
            TestContext.Progress.WriteLine($"processing {i}");
        }
    }
}
