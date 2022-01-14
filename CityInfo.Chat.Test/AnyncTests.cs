using NUnit.Framework;

namespace CityInfo.Chat.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
public class AsyncTests
{

    private readonly AsyncLocal<string> _currentContext = new();

    [Test]
    public async Task AsyncLocalTest()
    {
        _currentContext.Value = "Root";

        var tasks = Enumerable.Range(0, 10)
            .Select(TestAsyncLocal);
        await Task.WhenAll(tasks);

        TestContext.Progress.WriteLine($"async local from  root is {_currentContext.Value}");
    }

    private async Task TestAsyncLocal(int i)
    {
        _currentContext.Value = $"{i}";
        var tasks = Enumerable.Range(0, 10)
            .Select(ii => Task.Run(async () =>
            {
                await Task.Delay(100);
                TestContext.Progress.WriteLine($"internal from  {i},{ii} is {_currentContext.Value}");
                Assert.AreEqual($"{i}", _currentContext.Value);
            }));
        await Task.WhenAll(tasks);
        TestContext.Progress.WriteLine($"async local from  {i} is {_currentContext.Value}");
    }
}
