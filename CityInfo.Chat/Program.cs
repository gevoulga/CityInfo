using NLog.Web;

namespace CityInfo.Parking
{
    public partial class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseContentRoot(Directory.GetCurrentDirectory() + @"\Configuration")
                        .UseNLog();
                });
    }
}