namespace CityInfo.Parking.distops.Samples;

public interface IFireAndForgetDistop
{
    void SyncFireAndForget();

    Task FireAndForget();
}