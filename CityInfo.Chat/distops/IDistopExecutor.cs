namespace CityInfo.Parking.distops;

public interface IDistopExecutor
{
    Task<object?> ExecuteDistop(DistopContext distopContext);
}