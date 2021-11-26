//Use the namespace of the Dependency injection thus we get this added automatically
// ReSharper disable once CheckNamespace

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Extensions.DependencyInjection
{
    interface IMyInterface1
    {
        Guid Id { get; }
    }

    interface IMyInterface2
    {
        Guid Id { get; }
    }

    internal class MyClass : IMyInterface1, IMyInterface2
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    [TestClass()]
    public class MultiInterfaceRegistrationExtensionTests
    {
        [TestMethod()]
        public void AddSingletonTest()
        {
            var service = new ServiceCollection()
                .AddSingleton<IMyInterface1, IMyInterface2, MyClass>()
                .BuildServiceProvider();

            var foo1 = service.GetRequiredService<IMyInterface1>();
            var foo2 = service.GetRequiredService<IMyInterface2>();
            Assert.AreEqual(foo1.Id, foo2.Id);
            Assert.AreSame(foo1, foo2);
        }
    }
}