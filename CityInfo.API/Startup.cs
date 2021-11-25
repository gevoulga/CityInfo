using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CityInfo.API.Options;
using CityInfo.API.Services;
using CityInfo.API.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CityInfo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //These 2 do pretty much the same
            // services.Configure<MailServerOptions>(Configuration.GetSection(MailServerOptions.Section));
            services.AddOptions<MailServerOptions>()
                .Bind(Configuration.GetSection(MailServerOptions.Section))
                .ValidateDataAnnotations();
            
            services
                .AddControllers(
                    options => { options.SuppressAsyncSuffixInActionNames = false; }
                )
                // .AddXmlDataContractSerializerFormatters()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<PointOfInterestValidator>();
                    fv.ImplicitlyValidateChildProperties = true;
                })
                // .AddJsonOptions(jsonOptions =>
                // {
                //     jsonOptions.JsonSerializerOptions.PropertyNamingPolicy =JsonNamingPolicy.CamelCase;
                //     jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                // });
                .AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CityInfo.API", Version = "v1"});
            });

            //Register local services
            //Using compiler directives
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CityInfo.API v1"));
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                // .UseAuthorization()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); })
                .UseStatusCodePages();
        }
    }
}