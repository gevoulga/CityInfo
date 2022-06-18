using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CityInfo.API.Utils
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Return a observable that will emit a single element, if the value is present, or empty if the provided value is null.
        /// </summary>
        /// <param name="value">The value which will be emitted as observable, if not null.</param>
        /// <typeparam name="TResult">The type of the elements in the sequence.</typeparam>
        /// <returns>An observble with a single value, or an empty observable.</returns>
        public static IObservable<TResult> FromNullable<TResult>(TResult value)
        {
            return value != null ? Observable.Return(value) : Observable.Empty<TResult>();
        }

        public static IObservable<T> SwitchIfEmpty<T>(this IObservable<T> @this, IObservable<T> switchTo)
        {
            // https://stackoverflow.com/questions/15209932/switch-to-a-different-iobservable-if-the-first-is-empty
            _ = @this ?? throw new ArgumentNullException(nameof(@this));
            _ = switchTo ?? throw new ArgumentNullException(nameof(switchTo));
            return Observable.Create<T>(obs =>
            {
                var source = @this.Replay(1);
                var switched = source.Any().SelectMany(any => any ? Observable.Empty<T>() : switchTo);

                var switchedSubscription = source.Concat(switched).Subscribe(obs);
                return new CompositeDisposable(switchedSubscription, source.Connect());
            });
        }
    }
}