namespace CityInfo.Parking.distops.Samples;

public class ThrowsDistop : IThrowsDistop
{
    public Task Throws()
    {
        throw new NotImplementedException();
    }
}