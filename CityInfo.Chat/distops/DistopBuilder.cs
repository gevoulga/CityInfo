﻿using Castle.DynamicProxy;

namespace CityInfo.Parking.distops;

public class DistopBuilder
{
    public static TInterface Create<TInterface>(ILogger<DistopInterceptor> logger, IDistopService distopService)
        where TInterface : class
    {
        var interceptor = new DistopInterceptor(logger, distopService, false);
        return new ProxyGenerator()
            .CreateInterfaceProxyWithoutTarget<TInterface>(interceptor);
    }

    public static TInterface FireAndForget<TInterface>(ILogger<DistopInterceptor> logger, IDistopService distopService)
        where TInterface : class
    {
        var interceptor = new DistopInterceptor(logger, distopService, true);
        ValidateFireAndForget(typeof(TInterface));
        return new ProxyGenerator()
            .CreateInterfaceProxyWithoutTarget<TInterface>(interceptor);
    }

    private static void ValidateFireAndForget(Type interfaceType)
    {
        var methodInfos = interfaceType.GetMethods();
        foreach (var methodInfo in methodInfos)
        {
            // Make sure that the return type is Task or void?
            if (methodInfo.ReturnType.IsAssignableFrom(typeof(Task)))
            {
                continue;
            }
            else if (methodInfo.ReturnType.IsAssignableFrom(typeof(void)))
            {
                continue;
            }

            throw new InvalidOperationException(
                $"Cannot use fire and forget configured distop for method '{interfaceType}.{methodInfo.Name}' with return type '{methodInfo.ReturnType}'");
            // string res = (methodInfo.ReturnType) switch
            // {
            //
            //     (Task) => "1 and A",
            //     (2, "B") => "b",
            //     _ => "default"
            // };
        }
    }
}