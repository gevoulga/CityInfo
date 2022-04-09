using System.Reactive.Linq;
using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Services;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL;

public class CarvedRockSubscription : ObjectGraphType
{
    public CarvedRockSubscription(ReviewMessageService reviewMessageService)
    {
        Name = "Subscription";
        AddField(new EventStreamFieldType()
        {
            Name = "reviewAdded",
            Type = typeof(ReviewAddedMessageType),
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>()
                {
                    Name = "productId",
                    Description = "The id of the product who's reviews to subscribe to"
                }),
            Resolver = new FuncFieldResolver<ReviewAddedMessage>(
                context => context.Source as ReviewAddedMessage),
            Subscriber = new EventStreamResolver<ReviewAddedMessage>(
                context =>
                {
                    var productId = context.GetArgument<int>("productId");
                    return reviewMessageService.GetMessages()
                        .Where(reviewAddedMessage => productId == reviewAddedMessage.ProductId);
                })
        });
        
        // //We need to use Addfield because we need a specilized field type for stream
        // AddField(FieldBuilder
        //     .Create<ReviewAddedMessage, ReviewAddedMessage>(typeof(ReviewAddedMessageType))
        //     .Name("reviewAdded")
        //     .Argument<NonNullGraphType<IdGraphType>>(
        //         "productId", "The product Id to subscribe to its reviews")
        //     // .Resolve(context =>
        //     // {
        //     //     return context.Source as ReviewAddedMessage;
        //     // })
        //     .ResolveStream(context =>
        //     {
        //         var productId = context.GetArgument<int>("productId");
        //         return reviewMessageService.GetMessages()
        //             .Where(reviewAddedMessage => productId == reviewAddedMessage.ProductId);
        //     })
        //     .FieldType);
    }
}