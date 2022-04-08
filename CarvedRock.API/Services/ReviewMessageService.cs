using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CarvedRock.Api.Data.Entities;

namespace CarvedRock.Api.Services;

public class ReviewMessageService
{
    private readonly ISubject<ReviewAddedMessage> messageStream = new ReplaySubject<ReviewAddedMessage>();

    public void AddReviewMessage(ProductReview productReview)
    {
        messageStream.OnNext(new ReviewAddedMessage()
        {
            ProductId = productReview.ProductId,
            Title = productReview.Title
        });
    }

    public IObservable<ReviewAddedMessage> GetMessages()
    {
        return messageStream.AsObservable();
    }
}