using System;
using CarvedRock.Api.GraphQL.Types;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockSchema: Schema
    {
        public CarvedRockSchema(IServiceProvider sp): base(sp)
        {           
            Query = sp.GetRequiredService<CarvedRockQuery>();
            Mutation = sp.GetRequiredService<CarvedRockMutation>();
            RegisterType(typeof(ProductType));
            RegisterType(typeof(UsedProductType));
        }
    }
}
