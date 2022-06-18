namespace CityInfo.Parking.distops;

public interface IDistopService
{
    object? Call(DistopContext distopContext, bool fireAndForget);
    object? FireAndForget(DistopContext distopContext, bool fireAndForget);
}