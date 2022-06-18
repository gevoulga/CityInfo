using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace CityInfo.Parking.distops;

public class Distop : IDistop
{
    private readonly ILogger<Distop> _logger;
    private readonly IObservable<long> _tickStream;

    public Distop(ILogger<Distop> logger)
    {
        this._logger = logger;
        _tickStream = Observable.Interval(TimeSpan.FromSeconds(1))
            .Publish()
            .RefCount();
    }

    public long SyncCallReturns()
    {
        return 111;
    }

    public void SyncFireAndForget()
    {
        Task.Delay(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
    }

    public async Task FireAndForget()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
    }

    public Task Throws()
    {
        throw new NotImplementedException();
    }

    public virtual async Task DoSomething<T>(DistopDto distopDto, T t, CancellationToken cancellationToken)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"DoSomething started with args: {nameof(distopDto)}: {distopDto}, {nameof(t)}: {t}");

        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

        watch.Stop();
        _logger.LogInformation($"DoSomething finished in {watch.Elapsed}");
    }

    public virtual async Task<long> CurrentTick(DistopDto distopDto, CancellationToken cancellationToken)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"CurrentTick started with args: {nameof(distopDto)}: {distopDto}");

        try
        {
            var tick = await _tickStream.Take(1).ToTask(cancellationToken);
            _logger.LogInformation($"CurrentTick: {tick}");
            return tick;
        }
        finally
        {
            watch.Stop();
            _logger.LogInformation($"CurrentTick finished in {watch.Elapsed}");
        }
    }

    public virtual IObservable<long> Ticks(DistopDto distopDto)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"Ticks started with args: {nameof(distopDto)}: {distopDto}");

        try
        {
            return _tickStream;
        }
        finally
        {
            watch.Stop();
            _logger.LogInformation($"Ticks finished in {watch.Elapsed}");
        }
    }
}