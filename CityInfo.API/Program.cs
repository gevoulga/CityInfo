using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace CityInfo.API
{
    public partial class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        // .ConfigureLogging(builder =>
                        // {
                        //     builder.AddL
                        // })
                        .UseContentRoot(Directory.GetCurrentDirectory() + @"\Configuration")

                        //This is not necessary cuz by setting content root the settings are scanned!
                        // .ConfigureAppConfiguration((hostingContext, config) =>
                        // {
                        //     var env = hostingContext.HostingEnvironment;
                        //     config
                        //         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        //         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false,
                        //             reloadOnChange: true)
                        //         .AddEnvironmentVariables();
                        // })
                        .UseNLog();
                });
    }
}