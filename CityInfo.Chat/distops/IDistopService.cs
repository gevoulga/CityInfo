namespace CityInfo.Parking.distops;

public interface IDistopService
{
    object? Call(DistopContext distopContext);
}