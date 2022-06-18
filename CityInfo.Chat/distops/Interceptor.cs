using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;

namespace CityInfo.Parking.distops;

public class Interceptor : IInterceptor
{
    private readonly ILogger<Interceptor> _logger;
    private readonly IDistopService _distopService;

    protected Interceptor(ILogger<Interceptor> logger, IDistopService distopService)
    {
        _logger = logger;
        _distopService = distopService;
    }

    public static TInterface CreateProxy<TInterface>(ILogger<Interceptor> logger, IDistopService distopService)
        where TInterface : class
    {
        var interceptor = new Interceptor(logger, distopService);
        return new ProxyGenerator()
            .CreateInterfaceProxyWithoutTarget<TInterface>(interceptor);
    }

    public void Intercept(IInvocation invocation)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"Before target call {invocation.Method.Name} with args: {invocation.Arguments}" );
        try
        {
            var arguments = invocation.Arguments?
                .Select<object, (SerializableType type, object obj)>(obj => (obj.GetType(), obj))
                .ToArray();
            var genericArguments = invocation.GenericArguments?
                .Select<Type, SerializableType>(genericType => genericType).ToArray();
            var argumentTypes = invocation.Method.GetParameters()
                .Select<ParameterInfo, GenericSerializableType>(parameterInfo => parameterInfo.ParameterType)
                .ToArray();


            var distopContext = new DistopContext()
            {
                Arguments = arguments,
                ArgumentTypes = argumentTypes,
                GenericArguments = genericArguments,
                MethodDeclaringObject = invocation.Method.DeclaringType,
                MethodName = invocation.Method.Name,
                MethodReturnType = invocation.Method.ReturnType,
            };

            // Call the actual distop service to send the distop
            var returnedValue = _distopService.Call(distopContext);
            invocation.ReturnValue = returnedValue;

            // var args = JsonSerializer.Serialize(invocation.Arguments);
            // var genArgs = JsonSerializer.Serialize(invocation.GenericArguments);
            // var meth = JsonSerializer.Serialize(invocation.Method);
            // invocation.Proceed();
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Target exception {ex.Message}");
            throw;
        }
        finally
        {
            watch.Stop();
            _logger.LogInformation($"After target call {invocation.Method.Name}, elapsed {watch.Elapsed}");
        }
    }
}