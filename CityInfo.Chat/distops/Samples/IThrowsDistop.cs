namespace CityInfo.Parking.distops.Samples;

public interface IThrowsDistop
{

    void ThrowsSync();
    Task ThrowsAsync();
    Task<long> ThrowsAsyncLong();
}