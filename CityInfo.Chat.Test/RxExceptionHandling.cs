using NUnit.Framework;

namespace CityInfo.Chat.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
public class RxExceptionHandling
{
    [Test]
    public async Task RxExceptionHandlingOnSubscriber()
    {

        try
        {
            Observable.Range(1, 10)
                .Do(
                    i => TestContext.Progress.WriteLine("On next"),
                    exception => TestContext.Progress.WriteLine("On error"),
                    () => TestContext.Progress.WriteLine("On completed"))
                .Finally(() => TestContext.Progress.WriteLine("finally"))
                .Subscribe(i =>
                    {
                        TestContext.Progress.WriteLine("at the end");
                        if (i > 3)
                        {
                            throw new Exception("CRAP");
                        }
                    },
                    ex =>
                    {
                        TestContext.Error.WriteLine($"On error in subscriber");
                    });

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
        catch (Exception e)
        {
            TestContext.Error.WriteLine($"Outside of stream catch");
        }
    }
}
