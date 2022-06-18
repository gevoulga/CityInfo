namespace CityInfo.Parking.distops;

public class FireAndForgetDistop : IFireAndForgetDistop
{
    public void SyncFireAndForget()
    {
        Task.Delay(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
    }

    public async Task FireAndForget()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
    }
}