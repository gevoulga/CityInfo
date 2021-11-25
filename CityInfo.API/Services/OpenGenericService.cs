using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    //T can be GuidAttribute, or...
    public interface IOpenGenericService<T>
    {
        Task<T?> GetAsync();
    }

    public class OpenGenericService<T> : IOpenGenericService<T?>
    {
        public Task<T?> GetAsync()
        {
            return Task.Run(Get);
        }

        private static T? Get()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var attributes = assembly.GetCustomAttributes(typeof(T), true);
            // var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute),true)[0];

            if (attributes.Length == 0) return default(T); //otherwise we can use null and define:  where T : class
            if (attributes.Length != 1) throw new ArgumentOutOfRangeException(nameof(attributes));
            return (T) attributes[0];
        }
    }
}