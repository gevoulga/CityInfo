using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CityInfo.Parking.distops;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace CityInfo.Chat.Test
{
    public class DistopTests
    {
        [Test]
        public async Task InProcessDistop()
        {
            var loggerFactory = new NLogLoggerFactory();
            var interceptorLogger = loggerFactory.CreateLogger<Interceptor>();
            var distopLogger = loggerFactory.CreateLogger<Distop>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<NLogLoggerFactory>();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<IDistop, Distop>();
            var sp = serviceCollection.BuildServiceProvider();

            var distopExecutor = new DistopExecutor(sp);
            var distopService = new InProcessDistopService(distopExecutor);
            var proxy = Interceptor.CreateProxy<IDistop>(interceptorLogger, distopService);

            TestContext.Progress.WriteLine("Starting calls to proxy");
            // await proxy.DoSomething<bool>(new DistopDto(), true, CancellationToken.None);
            // TestContext.Progress.WriteLine("Done Something");

            var syncTick = proxy.CurrentTickk();
            TestContext.Progress.WriteLine($"got tick {syncTick}");K

            var tick1 = await proxy.CurrentTick(new DistopDto(), CancellationToken.None);
            TestContext.Progress.WriteLine($"got tick {tick1}");

            await Task.Delay(TimeSpan.FromSeconds(2));
            var tick2 = await proxy.CurrentTick(new DistopDto(), CancellationToken.None);
            TestContext.Progress.WriteLine($"got tick {tick2}");
        }


        [Test]
        public async Task TestMethod1()
        {
            var loggerFactory = new NLogLoggerFactory();
            var interceptorLogger = loggerFactory.CreateLogger<Interceptor>();
            var distopLogger = loggerFactory.CreateLogger<Distop>();

            var distopImpl = new Distop(distopLogger);
            // var interceptor = new Interceptor(interceptorLogger);

            var proxy = Interceptor.CreateProxy<IDistop>(interceptorLogger, null);
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