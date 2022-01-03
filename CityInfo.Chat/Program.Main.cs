using Microsoft.Extensions.Hosting;

namespace CityInfo.Parking
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            // Logger logger = NLogBuilder.ConfigureNLog("nlog.config")
            //     .GetCurrentClassLogger();
            Program.CreateHostBuilder(args).Build().Run();
        }
    }
}