namespace CityInfo.Parking.distops.Model;

public interface IDistopExecutor
{
    Task<object?> ExecuteDistop(DistopContext distopContext);
}