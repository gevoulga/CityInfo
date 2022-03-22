﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarvedRock.Api.Data;
using CarvedRock.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarvedRock.Api.Repositories
{
    public class ProductRepository
    {
        private readonly CarvedRockDbContext _dbContext;

        public ProductRepository(CarvedRockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<IProduct>> GetAll()
        {
            return _dbContext.Products
                .Select(product => product as IProduct)
                .ToListAsync();
        }

        public Task<IProduct?> Get(int productId)
        {
            return _dbContext.Products
                .Select(product => product as IProduct)
                .FirstOrDefaultAsync(product => product.Id == productId);
        }
    }
}