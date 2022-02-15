using NUnit.Framework;

namespace CityInfo.Chat.Test;

using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
public class RxParallelism
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
    public async Task RxTaskParallelismTest()
    {
        await Observable.Range(1, 10)
            .Select(i => Observable.Defer(() =>
            {
                return TaskCreator(i).ToObservable();
            }))
            //Executed out-of-order tasks
            .Merge(3) //If no args, no max-parallelism is specified. 
            //.Concat() //Concat will execute the stream in-order
            //.Do(x => TestContext.Progress.WriteLine($"{x} concatenated"))
            .RunAsync(CancellationToken.None);
        // .ForEachAsync(i => TestContext.Progress.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Got {i}"));

        //Printed at the end of the run
        TestContext.Out.WriteLine("at the end");
        // Immediately display error
        TestContext.Error.WriteLine("immediate error");
        // Immedaitely display message
        TestContext.Progress.WriteLine("immediate text");
    }

    private Task<int> TaskCreator(int i)
    {
        return Task.Run(async () =>
        {
            TestContext.Progress.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Task{i}: Started {new string('.', i)}");
            await Task.Delay(TimeSpan.FromSeconds(i));
            //Thread.Sleep(TimeSpan.FromSeconds(i));
            TestContext.Progress.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Task{i}: Complete {new string('.', i)}");

            return i;
        });
    }
}
