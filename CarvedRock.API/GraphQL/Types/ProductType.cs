using CarvedRock.Api.Data.Entities;
using CarvedRock.Api.Repositories;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types
{
    public sealed class ProductType: ObjectGraphType<Product>
    {
        public ProductType(
            ProductReviewRepository reviewRepository,
            IDataLoaderContextAccessor dataLoaderContextAccessor)
        {
            Field(t => t.Id);
            Field(t => t.Name).Description("The name of the product");
            Field(t => t.Description);
            Field(t => t.IntroducedAt).Description("When the product was first introduced in the catalog");
            Field(t => t.PhotoFileName).Description("The file name of the photo so the client can render it");
            Field(t => t.Price);
            Field(t => t.Rating).Description("The (max 5) star customer rating");
            Field(t => t.Stock);
            Field<ProductTypeEnumType>()
                .Name("Type")
                .Description("The type of product");
            // Field<ProductTypeEnumType>("Type", "The type of product");

            Field<ListGraphType<ProductReviewType>>(
                "reviews",
                resolve: context =>
                {
                    var productId = context.Source.Id;
                    
                    //Using DataLoader (acts like a cache)
                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddCollectionBatchLoader<int, ProductReview>(
                            "GetReviewsByProductId", reviewRepository.GetForProducts);
                    return loader.LoadAsync(productId);

                    //Using the repository directly
                    // return reviewRepository.GetForProduct(productId);
                });
        }
    }
}
