﻿using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace CityInfo.Parking.distops.Samples;

public class SyncDistop : ISyncDistop
{
    private readonly ILogger<SyncDistop> _logger;

    public SyncDistop(ILogger<SyncDistop> logger)
    {
        this._logger = logger;
    }

    public long SyncCallReturns()
    {
        _logger.LogInformation("SyncCallReturns started");
        var ret = 111;
        _logger.LogInformation("SyncCallReturns finished");
        return ret;
    }

    public void SyncFireAndForget()
    {
        _logger.LogInformation("SyncFireAndForget started");
        Task.Delay(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
        _logger.LogInformation("SyncFireAndForget finished");
    }
}