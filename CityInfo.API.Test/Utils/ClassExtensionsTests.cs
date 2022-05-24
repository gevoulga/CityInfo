using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using CityInfo.API.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityInfo.API.Test.Utils;

[TestClass]
public class ClassExtensionsTests
{

    [TestMethod]
    public void FromDictionaryTest()
    {
        var dictionary = new Dictionary<string, object>()
        {
            {"Prop1", "hi"},
            {"Prop2", 3}
        };

        var a = dictionary.ToObject<A>();

        a.Should().BeEquivalentTo(new A()
        {
            Prop1 = "hi",
            Prop2 = 3,
        });
    }

    [TestMethod]
    public void ToDictionaryTest()
    {
        var a = new A()
        {
            Prop1 = "hi",
            Prop2 = 3,
        };

        var dictionary = a.AsDictionary();

        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            {"Prop1", "hi"},
            {"Prop2", 3}
        });
    }

    [TestMethod]
    public void ToAnonymousTest()
    {
        var dictionary = new Dictionary<string, object>()
        {
            {"Prop1", "hi"},
            {"Prop2", 3}
        };

        var anonymous = dictionary.ToAnonymous();

        dynamic dyn = new
        {
            Prop1 = "hi",
            Prop2 = 3
        };

        var expected = ToExpandoObject(dyn);
        var actual = anonymous as ExpandoObject;
        actual.Should().BeEquivalentTo(expected);
    }

    private ExpandoObject ToExpandoObject(object obj)
    {
        var expando = new ExpandoObject();

        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
        {
            expando.TryAdd(property.Name, property.GetValue(obj));
        }

        return expando;
    }


    class A
    {
        public string Prop1 { get; set; }
        public int Prop2 { get; set; }
    }

    [TestMethod]
    public void ToAnonymousTestt()
    {
        var f1 = Observable.Return(3);
        var f2 = Observable.Return(new A()
        {
            Prop1 = "hi",
            Prop2 = 3
        });
        var f3 = Observable.Return("test");


        var aa = new AA()
        {
            First = Observable.Return(3),
            Second = Observable.Return(new A()
            {
                Prop1 = "hi",
                Prop2 = 3
            }),
            Third = Observable.Return("test"),
        };

        BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
        var observables = aa.GetType().GetProperties(bindingAttr).ToDictionary(
            propInfo => propInfo.Name,
            propInfo =>
            {
                var value = propInfo.GetValue(aa, null);
                var genericType = propInfo.PropertyType.GetGenericTypeDefinition();
                var isIt = typeof(IObservable<>).IsAssignableFrom(genericType);
                Console.WriteLine($"Is: {isIt}");
                if (value is IObservable<object> observable)
                {
                    return observable;
                }

                throw new InvalidOperationException($"{propInfo.Name} is not an Observable");
            });

        observables
            .Select(kv => kv.Value.Select(o => (kv.Key, o)))
            .Merge()
            .Subscribe(tuple => Console.WriteLine(tuple), () => Console.WriteLine("Max: "));
    }

    class AA
    {
        public IObservable<A> Second { get; set; }
        public IObservable<int> First { get; set; }
        public IObservable<string> Third { get; set; }
    }
}