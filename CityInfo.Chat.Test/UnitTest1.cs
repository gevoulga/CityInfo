using NUnit.Framework;

namespace CityInfo.Chat.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
public class Tests
{
    //https://www.lambdatest.com/blog/nunit-vs-xunit-vs-mstest/
    [OneTimeSetUp]
    public static void TestFixtureSetup()
    {
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task RxIAsyncEnumerable()
    {
        var received = await generate().ToObservable()
            .Take(5)
            .RunAsync(CancellationToken.None);
        TestContext.Progress.WriteLine($"received in the end {received}");

        var enums = generate();
        enums.ToObservable()
            .Take(5)
            .Subscribe(i => TestContext.Progress.WriteLine($"#1Got {i}"));
        await Task.Delay(TimeSpan.FromSeconds(2));
        enums.ToObservable()
            .Take(5)
            .Subscribe(i => TestContext.Progress.WriteLine($"#2Got {i}"));
        await Task.Delay(TimeSpan.FromSeconds(10));
    }

    private async IAsyncEnumerable<int> generate()
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(500);
            TestContext.Progress.WriteLine($"sending {i}");

            yield return i;
        }
    }
}