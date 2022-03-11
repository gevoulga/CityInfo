using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockSchema: Schema
    {
        public CarvedRockSchema(IServiceProvider sp): base(sp)
        {           
            Query = sp.GetRequiredService<CarvedRockQuery>();
        }
    }
}
