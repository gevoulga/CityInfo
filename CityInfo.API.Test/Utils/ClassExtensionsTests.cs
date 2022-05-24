using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
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
}