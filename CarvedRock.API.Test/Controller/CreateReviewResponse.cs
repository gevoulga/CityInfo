namespace CarvedRock.API.Test.Controller;

public class CreateReviewResponse
{
    public ProductReviewModel createReview { get; set; }
    
    public class ProductReviewModel
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}