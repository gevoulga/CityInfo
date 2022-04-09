namespace CarvedRock.API.Test.Controller;

public class SuscribeReviewResponse
{
    public ReviewAddedMessage reviewAdded { get; set; }
    
    public class ReviewAddedMessage
    {
        public int ProductId { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return $"{nameof(ProductId)}: {ProductId}, {nameof(Title)}: {Title}";
        }
    }
}