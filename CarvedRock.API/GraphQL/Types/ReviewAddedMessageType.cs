using CarvedRock.Api.Services;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types;

public class ReviewAddedMessageType : ObjectGraphType<ReviewAddedMessage>
{
    public ReviewAddedMessageType()
    {
        Field(r => r.ProductId);
        Field(r => r.Title);
    }
}