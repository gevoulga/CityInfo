using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityInfo.API.Controllers
{
    //https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    //https://www.automatetheplanet.com/mstest-cheat-sheet/
    [TestClass]
    public class LoremIpsumControllerTest
    {
        private static TestContext testContext;
        private static WebApplicationFactory<Program> application;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            application = new TestWebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        // ... Configure test services
                    });
                });
            testContext = context;
        }

        [TestMethod]
        public async Task LoremIpsumTest()
        {
            var httpClient = application.CreateClient();
            // using HttpClient httpClient = new();

            testContext.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Receving weather forecasts . . .");


            using HttpResponseMessage response = await httpClient.GetAsync(
                "/api/LoremIpsum",
                HttpCompletionOption.ResponseHeadersRead
            ).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            //Read Async still takes the entire response before returning the words array
            // IAsyncEnumerable<string> words = await response.Content
            //     .ReadFromJsonAsync<IAsyncEnumerable<string>>()
            //     .ConfigureAwait(false);
            var responseStream = await response.Content
                .ReadAsStreamAsync()
                .ConfigureAwait(false);
            IAsyncEnumerable<string> words = JsonSerializer.DeserializeAsyncEnumerable<string>(responseStream,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultBufferSize = 16 //Use a small number otherwise the entire response will be returned at one!
                });

            await foreach (var word in words)
            {
                testContext.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] {word}");
            }

            testContext.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.fff}] Weather forecasts has been received.");
        }
    }
}