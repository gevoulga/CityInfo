using CarvedRock.Api.Data.Entities;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types;

public sealed class ProductInterfaceType : InterfaceGraphType<IProduct>
{
    public ProductInterfaceType()
    {
        // Name = "IProduct";
        Field(t => t.Id);
        Field(t => t.Name).Description("The name of the product");
    }
}