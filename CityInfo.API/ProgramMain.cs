using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;

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