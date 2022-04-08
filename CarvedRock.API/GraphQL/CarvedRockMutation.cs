using CarvedRock.Api.Data.Entities;
using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockMutation: ObjectGraphType
    {
        public CarvedRockMutation(ProductReviewRepository repository)
        {
            //Using async signature to be able to await
            FieldAsync<ProductReviewType>(
                name: "createReview",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProductReviewInputType>>()
                    {
                        Name = "review"
                    }),
                resolve: async context =>
                {
                    // This will convert immediately to the entity product review! (from ProductReviewInputType)
                    // The properties need to match for this to work!
                    var review = context.GetArgument<ProductReview>("review");
                    var productReview = await repository.AddReview(review);
                    return productReview;
                });
        }
    }
}

// mutation createReview($review: reviewInput!){
//     createReview(review:$review){
//         id,
//         title
//     }
// }
// arguments:
// {
//     "review": {
//         "title": "this is awesome!",
//         "productId": 10
//     }
// }
