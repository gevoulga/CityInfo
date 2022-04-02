using CarvedRock.Web.Models;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Options;

namespace CarvedRock.Web.Clients
{
    public class ProductGraphClient
    {
        private readonly GraphQLHttpClient _client;

        public ProductGraphClient(IOptions<CarvedRockApiOptions> options)
        {
            var carvedRockApiOptions = options.Value;
            _client = new GraphQLHttpClient(carvedRockApiOptions.CarvedRockApiUri, new SystemTextJsonSerializer());
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var query = new GraphQLRequest
            {
                Query = @" 
                query productQuery($productId: ID!)
                { product(id: $productId) 
                    { id name price rating photoFileName description stock introducedAt 
                      reviews { title review }
                    }
                }",
                Variables = new {productId = id}
            };
            var response = await _client.SendQueryAsync<ProductModel>(query);
            return response.Data; //. GetDataFieldAs<ProductModel>("product");
        }

        public async Task<ProductReviewModel> AddReview(ProductReviewInputModel review)
        {
            var query = new GraphQLRequest
            {
                Query = @" 
                mutation($review: reviewInput!)
                {
                    createReview(review: $review)
                    {
                        id
                    }
                }",
                Variables = new { review }
            };
            var response = await _client.SendQueryAsync<ProductReviewModel>(query);
            // return response.GetDataFieldAs<ProductReviewModel>("createReview");
            return response.Data;
        }
    }
}
