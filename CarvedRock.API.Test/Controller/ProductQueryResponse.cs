namespace CarvedRock.API.Test.Controller;

public class ProductQueryResponse
{
    private QueryProduct Product { get; set; }

    public class QueryProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}