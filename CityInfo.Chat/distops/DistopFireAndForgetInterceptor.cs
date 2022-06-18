using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;

namespace CityInfo.Parking.distops;

public class DistopFireAndForgetInterceptor : BaseDistopInterceptor
{
    private readonly IDistopService _distopService;

    internal DistopFireAndForgetInterceptor(ILogger<BaseDistopInterceptor> logger, IDistopService distopService)
        : base(logger)
    {
        _distopService = distopService;
    }

    protected override object? ExecuteRemote(DistopContext distopContext, Type methodReturnType)
    {
        var task = _distopService.FireAndForget(distopContext);
        return null;
    }
}