﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarvedRock.Api.Data;
using CarvedRock.Api.Data.Entities;

namespace CarvedRock.Api.Repositories
{
    public class ProductReviewRepository
    {
        private readonly CarvedRockDbContext _dbContext;

        public ProductReviewRepository(CarvedRockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ProductReview> GetForProduct(int productId)
        {
            return _dbContext.ProductReviews
                .Where(review => review.ProductId == productId);
        }
        
        public Task<ILookup<int, ProductReview>> GetForProducts(IEnumerable<int> productIds)
        {
            var productReviews = _dbContext.ProductReviews
                .Where(review => productIds.Any(productId => review.ProductId == productId));
            return Task.FromResult(productReviews.ToLookup(review => review.Id));
        }

        public async Task<ProductReview> AddReview(ProductReview review)
        {
            _dbContext.ProductReviews.Add(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }
    }
}
