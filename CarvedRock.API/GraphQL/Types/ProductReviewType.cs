using CarvedRock.Api.Data.Entities;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types;

public sealed class ProductReviewType : ObjectGraphType<ProductReview>
{
    public ProductReviewType()
    {
        Field(r => r.Id);
        Field(r => r.ProductId);
        Field(r => r.Title);
        Field(r => r.Review);
    }
}