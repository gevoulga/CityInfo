﻿namespace CityInfo.Parking.distops;

public class InProcessDistopService : IDistopService
{
    private readonly DistopExecutor _distopExecutor;

    public InProcessDistopService(DistopExecutor distopExecutor)
    {
        _distopExecutor = distopExecutor;
    }

    public object? Call(DistopContext distopContext)
    {
        return _distopExecutor.Do(distopContext);
    }
}