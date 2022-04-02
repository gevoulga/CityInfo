using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using CarvedRock.Api.Data.Entities;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using NUnit.Framework;

namespace CarvedRock.API.Test.Controller;

public class QueryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task QueryProductsUsingHttpClient()
    {
        var httpClient = ControllerTestSetup.TestServer.CreateClient();
        var responseMessage = await httpClient.GetAsync(@"/graphql?query=
{
  products
  { id, name }
}
");

// var requestBuilder = ControllerTestSetup.TestServer
//     .CreateRequest(@"/graphql?query=
// {
//   products
//   { id, name }
// }
// ").And(message =>
//     {
//         TestContext.Progress.Write("Confffff");
//     });
// var responseMessage = await requestBuilder
//             .GetAsync();

        responseMessage.StatusCode
            .Should().Be(HttpStatusCode.OK);
        
        var body = await responseMessage.Content.ReadAsStringAsync();
        var jsonNode = JsonNode.Parse(body);
        TestContext.Progress.WriteLine($"Received products: {body}");
        TestContext.Progress.WriteLine(jsonNode);
        var products = jsonNode["data"]["products"].AsArray();
        var productObjects = products
            .Should()
            .HaveCount(6)
            .And
            .AllBeOfType<JsonObject>()
            .Which;
        // productObjects.Select(o => o["id"].AsValue())
        //     .Should()
        //     .ContainEquivalentOf(Enumerable.Range(1, 7));
        
        
        var query = new GraphQLRequest
        {
            Query = @"
{
  products
  { id, name }
}"
        };
        var response = await ControllerTestSetup.GraphQlHttpClient.SendQueryAsync<ProductQueryResponse>(query);
        
    }

    [TestCaseSource(nameof(ProductQueryCases))]
    public async Task<ProductQueryResponse> QueryProductTest(int id)
    {
        var query = new GraphQLRequest
        {
            Query = @"
query productQuery($productId: ID!){
  product(id: $productId){
    id,
    name
    ... on ProductType{
      description
    }
  }
}",
            Variables = new {productId = id}
        };
        var response = await ControllerTestSetup.GraphQlHttpClient.SendQueryAsync<ProductQueryResponse>(query);
        return  response.Data; //. GetDataFieldAs<ProductModel>("product");
    }

    public static IEnumerable<TestCaseData> ProductQueryCases()
    {
        yield return new TestCaseData(2)
            .Returns(new ProductQueryResponse()
            {
                Product = new ProductQueryResponse.QueryProduct()
                {
                    Id = 2,
                    Name = "Army Slippers",
                    Description = "For your everyday marches in the army."
                }
            });
    }
}