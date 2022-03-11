using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockQuery: ObjectGraphType
    {
        public CarvedRockQuery(ProductRepository productRepository)
        {
            Field<ListGraphType<ProductType>>(
                "products", 
                resolve: context => productRepository.GetAll()
            );
            Field<ProductType>(
                "product",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "id",
                        Description = "The id of the product to lookup"
                    }),
                resolve: context =>
                {
                    var productId = context.GetArgument<int>("id");
                    return productRepository.Get(productId);
                });
        }
    }
}
