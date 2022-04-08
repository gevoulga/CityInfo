using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types;

// This object doesn't use an entity as its template
public sealed class ProductReviewInputType : InputObjectGraphType
{
    public ProductReviewInputType()
    {
        Name = "reviewInput";
        //Mandatory
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<NonNullGraphType<IntGraphType>>("productId");
        //Can be empty
        Field<StringGraphType>("review");
    }
}