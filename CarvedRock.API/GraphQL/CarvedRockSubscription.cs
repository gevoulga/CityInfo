using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Services;
using GraphQL.Builders;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL;

public class CarvedRockSubscription : ObjectGraphType
{
    public CarvedRockSubscription(ReviewMessageService reviewMessageService)
    {
        Name = "Subscription";
        //We need to use Addfield because we need a specilized field type for stream
        AddField(FieldBuilder
            .Create<ReviewAddedMessage, ReviewAddedMessage>(typeof(ReviewAddedMessageType))
            .Name("reviewAdded")
            .Argument<NonNullGraphType<IdGraphType>>(
                "productId", "The product Id to subscribe to its reviews")
            // .Resolve(context =>
            // {
            //     return context.Source as ReviewAddedMessage;
            // })
            .ResolveStream(context =>
            {
                return reviewMessageService.GetMessages();
            })
            .FieldType);
    }
}