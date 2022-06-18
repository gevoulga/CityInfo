using System;
using System.Threading;
using System.Threading.Tasks;
using CityInfo.Parking.distops;
using CityInfo.Parking.distops.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace CityInfo.Chat.Test
{
    public class DistopTests
    {
        private ILogger<DistopInterceptor> interceptorLogger;
        private InProcessDistopService distopService;

        public DistopTests()
        {
            var loggerFactory = new NLogLoggerFactory();
            interceptorLogger = loggerFactory.CreateLogger<DistopInterceptor>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<NLogLoggerFactory>();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<IAsyncDistop, AsyncDistop>();
            serviceCollection.AddSingleton<ISyncDistop, SyncDistop>();
            serviceCollection.AddSingleton<IThrowsDistop, ThrowsDistop>();
            serviceCollection.AddSingleton<IFireAndForgetDistop, FireAndForgetDistop>();
            var sp = serviceCollection.BuildServiceProvider();

            var distopExecutor = new DistopExecutor(sp);
            distopService = new InProcessDistopService(distopExecutor);
        }

        [Test]
        public void InProcessSyncDistop()
        {
            var proxy = DistopBuilder.Create<ISyncDistop>(interceptorLogger, distopService);
            TestContext.Progress.WriteLine("Starting calls to proxy");

            proxy.SyncFireAndForget();
            TestContext.Progress.WriteLine($"After fire and forget");

            var tick = proxy.SyncCallReturns();
            TestContext.Progress.WriteLine($"got tick {tick}");
        }

        [Test]
        public async Task InProcessAsyncDistop()
        {
            var proxy = DistopBuilder.Create<IAsyncDistop>(interceptorLogger, distopService);

            TestContext.Progress.WriteLine("Starting calls to proxy");
            // await proxy.DoSomething<bool>(new DistopDto(), true, CancellationToken.None);
            // TestContext.Progress.WriteLine("Done Something");

            await proxy.FireAndForget();
            TestContext.Progress.WriteLine($"After fire and forget");

            var justALong = await proxy.JustALong();
            TestContext.Progress.WriteLine($"just a long{justALong}");

            var tick1 = await proxy.CurrentTick(new DistopDto(), CancellationToken.None);
            TestContext.Progress.WriteLine($"got tick {tick1}");

            await Task.Delay(TimeSpan.FromSeconds(2));
            var tick2 = await proxy.CurrentTick(new DistopDto(), CancellationToken.None);
            TestContext.Progress.WriteLine($"got tick {tick2}");
        }

        [Test]
        public async Task InProcessDistopFireAndForget()
        {
            var proxy = DistopBuilder.FireAndForget<IFireAndForgetDistop>(interceptorLogger, distopService);

            TestContext.Progress.WriteLine("Starting calls to proxy");
            // await proxy.DoSomething<bool>(new DistopDto(), true, CancellationToken.None);
            // TestContext.Progress.WriteLine("Done Something");

            // await proxy.Throws();
            // TestContext.Progress.WriteLine($"Throws");

            proxy.SyncFireAndForget();
            TestContext.Progress.WriteLine($"Should return immediately after sync fire and forget");

            await proxy.FireAndForget();
            TestContext.Progress.WriteLine($"Should return immediately after fire and forget");
        }

        [Test]
        public async Task Throws()
        {
            var proxy = DistopBuilder.Create<IThrowsDistop>(interceptorLogger, distopService);
            FluentActions.Invoking(() => proxy.ThrowsSync())
                .Should().Throw<InvalidOperationException>();
            await FluentActions.Invoking(async () => await proxy.ThrowsAsync())
                .Should().ThrowAsync<NotImplementedException>();
            await FluentActions.Invoking(async () => await proxy.ThrowsAsyncLong())
                .Should().ThrowAsync<NotImplementedException>();

        }


        [Test]
        public async Task TestMethod1()
        {


            var proxy = DistopBuilder.Create<IAsyncDistop>(interceptorLogger, null);
            // var proxy = new ProxyGenerator()
            //     .CreateInterfaceProxyWithoutTarget<IDistop>(interceptor);
            // var proxy = new ProxyGenerator()
            //     .CreateInterfaceProxyWithTarget(typeof(IDistop), distopImpl, interceptor) as IDistop;

            // var proxy = new ProxyGenerator().CreateClassProxyWithTarget(distopImpl.GetType(),
            //     distopImpl, new IInterceptor[] { interceptor }) as Distop;
            // var proxy = new ProxyGenerator()
            //     .CreateClassProxy<DistopImpl>(
            //         interceptor);

            TestContext.Progress.WriteLine("Starting calls to proxy");
            // await proxy.DoSomething<bool>(new DistopDto(), true, CancellationToken.None);
            // TestContext.Progress.WriteLine("Done Something");

            var tick1 = await proxy.CurrentTick(new DistopDto(), CancellationToken.None);
            TestContext.Progress.WriteLine($"got tick {tick1}");

            await Task.Delay(TimeSpan.FromSeconds(2));
            var tick2 = await proxy.CurrentTick(new DistopDto(), CancellationToken.None);
            TestContext.Progress.WriteLine($"got tick {tick2}");
        }
    }
}