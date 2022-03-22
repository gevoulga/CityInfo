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
            Field<ListGraphType<ProductInterfaceType>>(
                "products", 
                resolve: context => productRepository.GetAll()
            );
            Field<ProductInterfaceType>(
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

// {
//     products {
//         id,
//         name,
//         ... on ProductType {     --> This is an inline fragment in our graphQL query! (polymorphism)
//             description,
//         }
//     },
//     p5: product(id: 5){          --> we need this to be renamed otherwise it will conflict with p6 (alias)
//         name,
//         ... on ProductType {
//             descr: description,  --> renaming of a field (alias)
//         }
//     },
//     p6: product(id: 6){
//         descr: name,
//     }
// }
