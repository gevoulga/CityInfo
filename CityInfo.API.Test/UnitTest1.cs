using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityInfo.API.Tests
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading;
    using System.Threading.Tasks;
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public async Task RxTaskParallelismTest()
        {
            await Observable.Range(1, 5)
                .Select(i => Observable.Defer(() =>
                {
                    return TaskCreator(i).ToObservable();
                }))
                //Executed out-of-order tasks
                .Merge(3) //If no args, no max-parallelism is specified. 
                //.Concat() //Concat will execute the stream in-order
                //.Do(x => Console.WriteLine($"{x} concatenated"))
                .RunAsync(CancellationToken.None);
                // .ForEachAsync(i => Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Got {i}"));
        }

        private Task<int> TaskCreator(int i)
        {
            return Task.Run(async () =>
            {
                Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Task{i}: Started {new string('.', i)}");
                await Task.Delay(TimeSpan.FromSeconds(i));
                //Thread.Sleep(TimeSpan.FromSeconds(i));
                Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Task{i}: Complete {new string('.', i)}");

                return i;
            });
        }

        async Task<int> LongProcessAsync(int n)
        {
            Console.WriteLine($"Job {n} started");
            await Task.Delay(TimeSpan.FromSeconds(5 - n));
            Console.WriteLine($"Job {n} done");

            return n;
        }
    }
}
