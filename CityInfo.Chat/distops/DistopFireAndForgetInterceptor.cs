namespace CityInfo.Parking.distops;

public class DistopFireAndForgetInterceptor : BaseDistopInterceptor
{
    private readonly IDistopService _distopService;

    internal DistopFireAndForgetInterceptor(IServiceProvider sp)
        : base(sp.GetRequiredService<ILogger<DistopInterceptor>>())
    {
        _distopService = sp.GetRequiredService<IDistopService>();
    }

    protected override object? ExecuteRemote(DistopContext distopContext, Type methodReturnType)
    {
        bool IsTask() => methodReturnType.IsAssignableFrom(typeof(Task));
        var task = _distopService.FireAndForget(distopContext);
        return IsTask() ? task : null;
    }
}