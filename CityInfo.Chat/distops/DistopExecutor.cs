using System.Diagnostics;
using System.Reflection;

namespace CityInfo.Parking.distops;

public class DistopExecutor
{
    private readonly ILogger<DistopExecutor> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DistopExecutor(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<DistopExecutor>>();
        _serviceProvider = serviceProvider;
    }

    // public static TInterface CreateProxy<TInterface>(ILogger<Interceptor> logger)
    //     where TInterface : class
    // {
    //
    //     // var proxy = new ProxyGenerator()
    //     //     .CreateInterfaceProxyWithTarget(typeof(IDistop), distopImpl, interceptor) as IDistop;
    //     var interceptor = new Interceptor(logger);
    //     return new ProxyGenerator()
    //         .CreateInterfaceProxyWithoutTarget<TInterface>(interceptor);
    // }

    public object? Do(DistopContext distopContext)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"Before target call {distopContext.MethodDeclaringObject}.{distopContext.MethodName} with args: {distopContext.Arguments}" );

        var target = ResolveTarget(distopContext);
        var methodInfo = ResolveMethod(distopContext, target);
        Type methodReturnType = distopContext.MethodReturnType;
        var parameters = distopContext.Arguments
            .Select(tuple => tuple.Item2)
            .ToArray();

        try
        {
            var returnedValue = methodInfo.Invoke(target, parameters);
            if (IsValidType(methodReturnType, returnedValue))
            {
                return returnedValue;
            }

            throw new InvalidCastException($"Cannot cast {returnedValue} to {methodReturnType}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,$"Target call {distopContext.MethodName} threw exception");
            throw;
        }
        finally
        {
            watch.Stop();
            _logger.LogInformation($"After target call {distopContext.MethodName}, elapsed {watch.Elapsed}");
        }
    }

    private object ResolveTarget(DistopContext distopContext)
    {
        Type targetType = distopContext.MethodDeclaringObject;
        var target = _serviceProvider.GetRequiredService(targetType);
        return target;
    }

    private static MethodInfo ResolveMethod(DistopContext distopContext, object target)
    {
        var genericParameterCount = distopContext.GenericArguments?.Length ?? 0;
        var parameterTypes = distopContext.ArgumentTypes
            .Select((t, i) => t.GetType(i))
            .ToArray();
        var methodInfo = target.GetType().GetMethod(distopContext.MethodName, genericParameterCount, parameterTypes)
                         ?? throw new InvalidOperationException($"Method not found for {distopContext}");
        return methodInfo;
    }

    private static bool IsValidType(Type? methodReturnType, object? returnedValue)
    {
        var type = returnedValue?.GetType();
        bool IsAssignableFrom() => methodReturnType?.IsAssignableFrom(type) ?? false;
        bool IsVoid() => methodReturnType?.IsAssignableFrom(typeof(void)) ?? false;
        bool IsTask() => methodReturnType?.IsAssignableFrom(typeof(Task<>)) ?? false;
        return IsAssignableFrom() || IsVoid() || IsTask();
    }
}