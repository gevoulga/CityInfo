using NUnit.Framework;

namespace CityInfo.Chat.Test;

using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
public class ObservableSinks
{
    // https://gist.github.com/omnibs/6b2cbdba2685693448ee6779736a00c2#use-subjectt-as-backend-for-iobservablet
    [Test]
    public void AsyncLocalTest()
    {
        //The sink
        var sink = new Subject<int>();
        var observable = sink.AsObservable();
        
        //Our subscriber
        observable
            .Subscribe(i =>
                {
                    TestContext.Progress.WriteLine($"at the end {i}");
                    // if (i > 3)
                    // {
                    //     throw new Exception("CRAP");
                    // }
                },
                ex =>
                {
                    TestContext.Error.WriteLine($"On error in subscriber");
                });

        sink.OnNext(1);
        sink.OnNext(2);
        sink.OnNext(3);
        sink.OnError(new Exception("Exc"));

        TestContext.Progress.WriteLine($"Done");
    }

    [Test]
    public async Task CreateObservable()
    {
        var observable = Observable.Create<int>(observer =>
        {
            observer.OnNext(1);
            observer.OnNext(2);
            observer.OnNext(3);
            observer.OnNext(4);
            observer.OnCompleted();
            
            return Disposable.Empty;
        });

        await foreach (var i in observable.ToAsyncEnumerable())
        {
            TestContext.Progress.WriteLine($"Got at the end {i}");
        }
    }

}
