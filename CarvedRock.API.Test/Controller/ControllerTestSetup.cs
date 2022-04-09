using System;
using System.Threading.Tasks;
using CarvedRock.Api;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace CarvedRock.API.Test.Controller;

//https://dev.to/mattjhosking/graphql-net-with-asp-net-integration-tests-55a5
[SetUpFixture]
public class ControllerTestSetup
{
    private WebApplicationFactory<Program> _webApplicationFactory;
    public static TestServer TestServer { get; private set; }
    public static GraphQLHttpClient GraphQlHttpClient { get; private set; }

    [OneTimeSetUp]
    public void SpinUpWebService()
    {
        TestContext.Progress.WriteLine("Spinning up a local webserver");
        _webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // Custom service setup for testing...
                builder.ConfigureServices(services =>
                {
                    // We could replace the services
                    // services.Replace(ServiceDescriptor.Singleton<IDocumentExecuter, OptimisedDocumentExecuter>());
                });
            });
        TestServer = _webApplicationFactory.Server;
        var httpClient = _webApplicationFactory.CreateClient();


        //Create a graphql client using the underlying httpClient provided for Integration tests
        GraphQlHttpClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions()
            {
                EndPoint = new Uri(TestServer.BaseAddress, "graphql"),
                ConfigureWebsocketOptions = options => 
                    options.UseDefaultCredentials = true
            },
            new SystemTextJsonSerializer(),
            httpClient);

        // We could have special configuration based on runsettings
        // https://get-testy.com/nunit-runsettings-file/
        //TestContext.Parameters["webAppUrl"];
        //var propertyBagAdapter = TestContext.CurrentContext.Test.Properties;

        TestContext.Progress.WriteLine("Local webserver ready");
    }

    [OneTimeTearDown]
    public async Task Destroy()
    {
        await TestContext.Progress.WriteLineAsync("Destroying webserver");
        TestServer.Dispose();
        await _webApplicationFactory.DisposeAsync();
        GraphQlHttpClient.Dispose();
        await TestContext.Progress.WriteLineAsync("Webserver destroyed");
    }
}