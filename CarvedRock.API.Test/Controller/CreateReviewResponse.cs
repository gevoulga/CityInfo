namespace CarvedRock.API.Test.Controller;

public class CreateReviewResponse
{
    public ProductReviewModel createReview { get; set; }
    
    public class ProductReviewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ProductId)}: {ProductId}, {nameof(Title)}: {Title}, {nameof(Review)}: {Review}";
        }
    }
}