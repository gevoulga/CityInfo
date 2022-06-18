using System;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using NUnit.Framework;

namespace CarvedRock.API.Test.Controller;

public class SubscriptionTests
{

    [Test]
    public async Task SubscribeToReviewStreamTest()
    {
        Mutate();
        var response = SubscribeToReviews();
        
        await response
            .Select(qlResponse => qlResponse.Data.reviewAdded.Title)
            .Do(title => TestContext.Progress.WriteLine($"Received: {title}"))
            .ObserveOn(Scheduler.Immediate)
            .RunAsync(CancellationToken.None);
        
        // Mutate();

        // var t = response
        //     .Select(qlResponse => qlResponse.Data.reviewAdded.Title)
        //     .Do(title => TestContext.Progress.WriteLine($"Received: {title}"))
        //     .ObserveOn(Scheduler.CurrentThread)
        //     .ToListObservable();
        // TestContext.Progress.WriteLine($"Finally at the end! Received: {t.Value}");
    }

    private IObservable<GraphQLResponse<SuscribeReviewResponse>> SubscribeToReviews()
    {
        var query = new GraphQLRequest
        {
            Query = @"
subscription reviewStream($productId: ID!){
  reviewAdded(productId:$productId){
    productId,
    title
  }
}",
            Variables = new {productId = 10}
        };

        return ControllerTestSetup.GraphQlHttpClient
            .CreateSubscriptionStream<SuscribeReviewResponse>(query, 
                exception => TestContext.Error.WriteLine(exception));
    }

    private void Mutate()
    {
        var runFor = Observable.Range(0,2)
            .ObserveOn(Scheduler.Default);
        
        runFor
            .SelectMany(i => SendReview(i).ToObservable())
            .Subscribe();

        //Maintain order -at cost of efficiency
        // runFor
        //     .Select(i => SendReview(i).ToObservable())
        //     .Concat()
        //     .Subscribe();
        //
        // //Limit the amount of max-parallelism
        // runFor
        //     .Select(i => SendReview(i).ToObservable())
        //     .Merge(3)
        //     .Subscribe();
    }

    private async Task SendReview(int i)
    {
        var query = new GraphQLRequest
        {
            Query = @"
mutation createReview($review: reviewInput!){
    createReview(review:$review){
        id
    }
}",
            Variables = new
            {
                review = new ProductReviewInputModel()
                {
                    ProductId = 10,
                    Title = $"Title #{i}"
                }
            }
        };
        var response = await ControllerTestSetup.GraphQlHttpClient.SendQueryAsync<CreateReviewResponse>(query);
        TestContext.Progress.WriteLine($"Review sent: {response.Data.createReview}");
        if (response?.Errors?.Any()?? false)
        {
            throw new HttpRequestException("Failed to get response from graphql: " + response.Errors);
        }
    }
}