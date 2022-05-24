using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityInfo.API.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            TimeZoneInfo? tz = null;
            bool? ae = null;

            var zip = FromNullable(tz)
                .Zip(FromNullable(ae))
                .Do(tuple => Console.WriteLine($"Result: Success. Operation: Meeting Details: {tuple}."),
                    ex => Console.WriteLine($"Result: Error. Type: {ex.GetType().Name}. Operation: Meeting Details."))
                    .SingleOrDefaultAsync();

            Console.WriteLine(zip);

            var t = await zip.ToTask();
            Console.WriteLine(t);

            // var rx1 = Observable.Return("test");
            // var rx2 = Observable.Return(1);
            // Observable.Merge<>(rx1, rx2)
        }

        private static IObservable<TResult> FromNullable<TResult>(TResult? tz)
        {
            var fromNullable = Observable.Empty<TResult>();
            return tz == null ? fromNullable : Observable.Return(tz);
        }


        [TestMethod]
        public void TestMethod2()
        {
            A v = new C();
            Console.WriteLine(v.GetType().Name);
        }

        public class A {}
        public class B : A {}

        public class C : B {}

        [TestMethod]
        public void TestMethod3()
        {
            // Handle using Dynamic type
            // dynamic newtype= AnonymousReturn();
            // Console.WriteLine(newtype.Name + " " + newtype.EmailID);

            // Handle by creating the same anonymous type
            // object o = AnonymousReturn();
            // var obj = Cast(o, new { Name = "", EmailID = "" });
            // Console.WriteLine(obj.Name + " " + obj.EmailID);

            // Handle using Reflection
            dynamic refobj = AnonymousReturn();
            Type type = refobj.GetType();
            PropertyInfo[] fields = type.GetProperties();
            foreach (var field in fields)
            {
                string name = field.Name;
                var typee = field.PropertyType;
                var val = field.GetValue(refobj, null);
                Console.WriteLine($"{name}, {typee}, {val}");
            }
        }

        dynamic AnonymousReturn()
        {
            return new
            {
                Name = "Pranay",
                EmailID = "pranayamr@gmail.com",
                Tag = 10,
            };
        }
        // public static IObservable<TValue> ToObservable<TValue>(this Optional<TValue> optional)
        // {
        //     return optional.Select(Observable.Return)
        //         .OrElse(() => Observable.Empty<TValue>());
        // }
        //
        // public static async Task<Optional<TValue>> SingleOrOptionalAsync<TValue>(this IObservable<TValue> observable)
        // {
        //     var value = await observable.SingleOrDefaultAsync();
        //     return Optional.FromValue(value);
        // }
        // var chatThreadTask = Observable.Return(threadMri)
        //     .Zip(threadProperties.ToObservable())
        //     .Select(tuple => ChatThread.Create(tuple.First, tuple.Second))
        //     .SingleOrOptionalAsync();

        private static void Switch(){
            var test = ('A', 'B');
            var result = test switch
            {
                ('A', 'B') => "OK",
                ('A', _) => "First part OK",
                (_, 'B') => "Second part OK",
                _ => "Not OK",
            };
        }
    }
}