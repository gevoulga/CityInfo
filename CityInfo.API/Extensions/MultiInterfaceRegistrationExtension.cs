//Use the namespace of the Dependency injection thus we get this added automatically
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiInterfaceRegistrationExtension
    {
        public static IServiceCollection AddSingleton<TI1, TI2, T>(this IServiceCollection services) 
            where T : class, TI1, TI2
            where TI1 : class
            where TI2 : class
        {
            services.AddSingleton<TI1, T>();
            services.AddSingleton<TI2, T>(x => (T) x.GetRequiredService<TI1>());
            return services;
        }
    }
}