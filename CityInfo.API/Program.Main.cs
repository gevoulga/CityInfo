using Microsoft.Extensions.Hosting;

namespace CityInfo.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            // Logger logger = NLogBuilder.ConfigureNLog("nlog.config")
            //     .GetCurrentClassLogger();
            CreateHostBuilder(args).Build().Run();
        }
    }
}