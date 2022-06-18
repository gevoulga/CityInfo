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

    public Task FireAndForget(DistopContext distopContext)
    {
        throw new NotImplementedException();
    }
}