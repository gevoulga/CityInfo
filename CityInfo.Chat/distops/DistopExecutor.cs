using System.Diagnostics;

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

        Type targetType = distopContext.MethodDeclaringObject;
        var target = _serviceProvider.GetRequiredService(targetType);

        var genericParameterCount = distopContext.GenericArguments?.Length ?? 0;
        var parameterTypes = distopContext.ArgumentTypes
            .Select((t, i) =>t.GetType(i))
            .ToArray();
        var parameters = distopContext.Arguments
            .Select(tuple =>tuple.Item2)
            .ToArray();
        Type methodReturnType = distopContext.MethodReturnType;
        var methodInfo = target.GetType().GetMethod(distopContext.MethodName, genericParameterCount, parameterTypes)
                         ?? throw new InvalidOperationException($"Method not found for {distopContext}");

        try
        {
            var returnedValue = methodInfo.Invoke(target, parameters);
            if (methodReturnType.IsAssignableFrom(returnedValue?.GetType()))
            {
                return returnedValue;
            }

            throw new InvalidCastException($"Cannot cast {returnedValue} to {methodReturnType}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            watch.Stop();
            _logger.LogInformation($"After target call {distopContext.MethodName}, elapsed {watch.Elapsed}");
        }

        // string jsonString = JsonSerializer.Serialize(weatherForecast);
    }
}