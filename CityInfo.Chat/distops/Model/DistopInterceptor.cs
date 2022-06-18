﻿namespace CityInfo.Parking.distops.Model;

public class DistopInterceptor : BaseDistopInterceptor
{
    private readonly IDistopService _distopService;

    internal DistopInterceptor(IServiceProvider sp)
        : base(sp.GetRequiredService<ILogger<DistopInterceptor>>())
    {
        _distopService = sp.GetRequiredService<IDistopService>();
    }

    protected override object? ExecuteRemote(DistopContext distopContext, Type methodReturnType)
    {
        // Check for the flag fire and forget
        bool IsTask() => methodReturnType.IsAssignableFrom(typeof(Task));
        bool IsGenericTask() => (methodReturnType?.IsGenericType ?? false) && (methodReturnType?.GetGenericTypeDefinition().IsAssignableFrom(typeof(Task<>)) ?? false);

        var returnedValue = _distopService.Call(distopContext);

        // Replace the return value so that it only completes when the post-interception code is complete.
        // invocation.ReturnValue = InterceptAsync(returnedValue);

        if (IsTask())
        {
            return returnedValue;
        }
        else if (IsGenericTask())
        {
            return returnedValue.Result;
        }
        else
        {
            return returnedValue.Result;
        }



        // var args = JsonSerializer.Serialize(invocation.Arguments);
        // var genArgs = JsonSerializer.Serialize(invocation.GenericArguments);
        // var meth = JsonSerializer.Serialize(invocation.Method);
        // invocation.Proceed();
    }
}