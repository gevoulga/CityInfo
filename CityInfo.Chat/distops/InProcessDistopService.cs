﻿using CityInfo.Parking.distops.Model;

namespace CityInfo.Parking.distops;

public class InProcessDistopService : IDistopService
{
    private readonly IDistopExecutor _distopExecutor;

    public InProcessDistopService(IDistopExecutor distopExecutor)
    {
        _distopExecutor = distopExecutor;
    }

    public async Task<object?> Call(DistopContext distopContext)
    {
        return await _distopExecutor.ExecuteDistop(distopContext);
    }

    public Task FireAndForget(DistopContext distopContext)
    {
        Task.Factory.StartNew(async () => await _distopExecutor.ExecuteDistop(distopContext));
        return Task.CompletedTask;
    }
}