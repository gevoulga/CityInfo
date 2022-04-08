using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using NUnit.Framework;

namespace CarvedRock.API.Test.Controller;

public class MutationTests
{

    [TestCaseSource(nameof(ReviewMutationCases))]
    public async Task<int> CreateReviewTest(ProductReviewInputModel productReview)
    {
        var query = new GraphQLRequest
        {
            Query = @"
mutation createReview($review: reviewInput!){
    createReview(review:$review){
        id
    }
}",
            Variables = new {review = productReview}
        };
        var response = await ControllerTestSetup.GraphQlHttpClient.SendQueryAsync<CreateReviewResponse>(query);
        TestContext.Progress.WriteLine($"Received: {response.Data}");
        return  response.Data.createReview.Id; //. GetDataFieldAs<ProductModel>("product");
    }

    public static IEnumerable<TestCaseData> ReviewMutationCases()
    {
        yield return new TestCaseData(new ProductReviewInputModel()
            {
                ProductId = 10,
                Title = "An awesome title.",
                Review = "This is great!"
            })
            .Returns(10);
        
        yield return new TestCaseData(new ProductReviewInputModel()
            {
                ProductId = 10,
                Title = "A great title.",
            })
            .Returns(11);
    }
}