using CarvedRock.Api.Data;
using CarvedRock.Api.GraphQL;
using CarvedRock.Api.Repositories;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarvedRock.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarvedRockDbContext>(options =>
            {
                options.UseInMemoryDatabase("GraphQLTestCarvedRock")
                    .EnableDetailedErrors();
                //options.UseSqlServer(_config["ConnectionStrings:CarvedRock"])
            });
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductReviewRepository>();

            services.AddScoped<CarvedRockSchema>();

            // GraphQL.MicrosoftDI.GraphQLBuilderExtensions.AddGraphQL(services)
            //     .AddServer(true, options => options.EnableMetrics = true)
            //     .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
            //     .AddSystemTextJson()
            //     .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            //     .AddSchema<StarWarsSchema>()
            //     .AddGraphTypes(typeof(StarWarsSchema).Assembly);
            // GraphQL.MicrosoftDI.GraphQLBuilderExtensions.AddGraphQL(services)
            //     .AddSelfActivatingSchema<CarvedRockSchema>()
            //     .AddSystemTextJson();
            services.AddGraphQL(o =>
                {
                    // o.ExposeExceptions = false;
                })
                .AddSystemTextJson()
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddDataLoader();
        }

        public void Configure(IApplicationBuilder app, CarvedRockDbContext dbContext)
        {
            if (_env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseGraphQL<CarvedRockSchema>(); //adds the api at /graphql
            app.UseGraphQLPlayground(new PlaygroundOptions()); //adds the playground ui to play around with the api
            dbContext.Seed();
        }
    }
}