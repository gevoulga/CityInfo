using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;

namespace CityInfo.Parking.distops;

public class DistopInterceptor : IInterceptor
{
    private readonly ILogger<DistopInterceptor> _logger;
    private readonly IDistopService _distopService;
    private readonly bool _fireAndForget;

    internal DistopInterceptor(ILogger<DistopInterceptor> logger, IDistopService distopService, bool fireAndForget)
    {
        _logger = logger;
        _distopService = distopService;
        this._fireAndForget = fireAndForget;
    }

    public void Intercept(IInvocation invocation)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"Before target call {invocation.Method.Name} with args: {invocation.Arguments}" );
        try
        {
            var distopContext = ResolveDistopContext(invocation);

            // Check for the flag fire and forget
            bool IsTask() => invocation.Method.ReturnType.IsAssignableFrom(typeof(Task));
            bool IsGenericTask() => invocation.Method.ReturnType.GetGenericTypeDefinition().IsAssignableFrom(typeof(Task<>));

            // Call the actual distop service to send the distop
            if (_fireAndForget)
            {
                // await _distopService.FireAndForget(distopContext);
                // invocation.ReturnValue = returnedValue;
            }
            else
            {
                var returnedValue = _distopService.Call(distopContext);

                // Replace the return value so that it only completes when the post-interception code is complete.
                // invocation.ReturnValue = InterceptAsync(returnedValue);

                if (IsTask())
                {
                    invocation.ReturnValue = returnedValue;
                }
                else if (IsGenericTask())
                {
                    invocation.ReturnValue = returnedValue.Result;
                }
                else
                {
                    invocation.ReturnValue = returnedValue.Result;
                }
            }

            // var args = JsonSerializer.Serialize(invocation.Arguments);
            // var genArgs = JsonSerializer.Serialize(invocation.GenericArguments);
            // var meth = JsonSerializer.Serialize(invocation.Method);
            // invocation.Proceed();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Target call threw exception");
            throw;
        }
        finally
        {
            watch.Stop();
            _logger.LogInformation($"After target call {invocation.Method.Name}, elapsed {watch.Elapsed}");
        }
    }

    // This method will complete when PostInterceptAsync completes.
    private async Task InterceptAsync(Task originalTask)
    {
        // Asynchronously wait for the original task to complete
        await originalTask;

        // Asynchronous post-execution
        // await PostInterceptAsync();
    }

    private DistopContext ResolveDistopContext(IInvocation invocation)
    {
        var arguments = invocation.Arguments?
            .Select<object, (SerializableType type, object obj)>(obj => (obj.GetType(), obj))
            .ToArray();
        var genericArguments = invocation.GenericArguments?
            .Select<Type, SerializableType>(genericType => genericType).ToArray();
        var argumentTypes = invocation.Method.GetParameters()
            .Select<ParameterInfo, GenericSerializableType>(parameterInfo => parameterInfo.ParameterType)
            .ToArray();

        return new DistopContext()
        {
            Arguments = arguments,
            ArgumentTypes = argumentTypes,
            GenericArguments = genericArguments,
            MethodDeclaringObject = invocation.Method.DeclaringType,
            MethodName = invocation.Method.Name,
            MethodReturnType = invocation.Method.ReturnType,
        };
    }
}