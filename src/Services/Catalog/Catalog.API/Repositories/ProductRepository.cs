﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteResult = await _context
                                            .Products
                                            .DeleteOneAsync(p => p.Id == id);

            return deleteResult.IsAcknowledged
                    && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                            .Products
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            return await _context
                            .Products
                            .Find(p => p.Category == categoryName)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _context
                            .Products
                            .Find(p => p.Name == name)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResults = await _context
                                            .Products
                                            .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResults.IsAcknowledged
                    && updateResults.ModifiedCount > 0;
        }
    }
}
