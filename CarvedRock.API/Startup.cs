using CarvedRock.Api.Data;
using CarvedRock.Api.GraphQL;
using CarvedRock.Api.Repositories;
using CarvedRock.Api.Services;
using GraphQL.Authorization.AspNetCore.Identity.Helpers;
// using GraphQL.MicrosoftDI;
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
            //The review message service needs to be singleton so there's one unique stream
            services.AddSingleton<ReviewMessageService>();

            services.AddScoped<CarvedRockSchema>();

            // GraphQL.MicrosoftDI.GraphQLBuilderExtensions.AddGraphQL(services)
            //     .AddServer(true, options => options.EnableMetrics = true)
            //     .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
            //     .AddSystemTextJson()
            //     .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            //     .AddSchema<StarWarsSchema>()
            //     .AddGraphTypes(typeof(StarWarsSchema).Assembly);
            // GraphQLBuilderExtensions.AddGraphQL(services)
            //         .AddSystemTextJson()
            //         .AddGraphTypes(Assembly.GetCallingAssembly())
            //         .AddSelfActivatingSchema<CarvedRockSchema>()
            //         .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
            //         .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = _env.IsDevelopment())
            //         .AddDataLoader()
            //         .AddWebSockets();
            // GraphQL.MicrosoftDI.GraphQLBuilderExtensions.AddGraphQL(services)
            //     .AddSelfActivatingSchema<CarvedRockSchema>()
            //     .AddSystemTextJson();
            services.AddGraphQL(o =>
                {
                    // o.ExposeExceptions = false;
                })
                .AddSystemTextJson()
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = _env.IsDevelopment())
                .AddDataLoader()
                .AddWebSockets();
        }

        public void Configure(IApplicationBuilder app, CarvedRockDbContext dbContext)
        {
            if (_env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //Enable websocket and add the subscription schema
            app.UseWebSockets();
            app.UseGraphQLWebSockets<CarvedRockSchema>();
            
            app.UseGraphQL<CarvedRockSchema>(); //adds the api at /graphql
            app.UseGraphQLPlayground(new PlaygroundOptions()); //adds the playground ui to play around with the api
            dbContext.Seed();
        }
    }
}