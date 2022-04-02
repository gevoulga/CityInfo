

using CarvedRock.Web.Clients;
using CarvedRock.Web.HttpClients;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CarvedRock.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CarvedRockApiOptions>(_config.GetSection(CarvedRockApiOptions.Section));
            
            services.AddSingleton<ProductGraphClient>();
            
            //MVC is needed if we're using plain httpclient
            services.AddMvc(); //(options => options.EnableEndpointRouting = false);
            services.AddSingleton<ProductGraphClient>();
            services.AddHttpClient<ProductHttpClient>(o => o.BaseAddress = new Uri(_config["CarvedRockApiUri"]));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
        }
    }
}
