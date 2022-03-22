using CarvedRock.Api.Data.Entities;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types;

public class UsedProductType : AutoRegisteringObjectGraphType<UsedProduct>
{
    public UsedProductType()
    {
        // Using the AutoRegisteringObjectGraphType allows us to avoid having to define the fields explicitly
        Interface<ProductInterfaceType>();
    }
}