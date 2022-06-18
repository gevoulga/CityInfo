namespace CityInfo.Parking.distops;

public interface IFireAndForgetDistop
{
    void SyncFireAndForget();

    Task FireAndForget();
}