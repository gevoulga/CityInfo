namespace CityInfo.Parking.distops;

public interface IDistop
{
    long SyncCallReturns();
    void SyncFireAndForget();
    Task FireAndForget();
    Task Throws();
    Task DoSomething<T>(DistopDto distopDto, T t, CancellationToken cancellationToken);
    Task<long> CurrentTick(DistopDto distopDto, CancellationToken cancellationToken);
    IObservable<long> Ticks(DistopDto distopDto);
}