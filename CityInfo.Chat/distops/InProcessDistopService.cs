namespace CityInfo.Parking.distops;

public class InProcessDistopService : IDistopService
{
    private readonly DistopExecutor _distopExecutor;

    public InProcessDistopService(DistopExecutor distopExecutor)
    {
        _distopExecutor = distopExecutor;
    }

    public async Task<object?> Call(DistopContext distopContext)
    {
        return await _distopExecutor.Do(distopContext);
    }

    public async Task FireAndForget(DistopContext distopContext)
    {
        await _distopExecutor.Do(distopContext);
    }
}