﻿using AutoMapper;
using CatalogAPI.Dtos;
using CatalogAPI.Models;
using CatalogAPI.Settings;
using Core.Dtos;
using Core.Messages;
using MassTransit;
using MongoDB.Driver;

namespace CatalogAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _ProductCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _ProductCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;

            _publishEndpoint = publishEndpoint;
        }

        public async Task<Core.Dtos.Response<List<ProductDto>>> GetAllAsync()
        {
            try
            {
                var Products = await _ProductCollection.Find(Product => true).ToListAsync();

                if (Products.Any())
                {
                    foreach (var Product in Products)
                    {
                        Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();
                    }
                }
                else
                {
                    Products = new List<Product>();
                }

                return Core.Dtos.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(Products), 200);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Core.Dtos.Response<ProductDto>> GetByIdAsync(string id)
        {
            var Product = await _ProductCollection.Find<Product>(x => x.Id == id).FirstOrDefaultAsync();

            if (Product == null)
            {
                return Core.Dtos.Response<ProductDto>.Fail("Product not found", 404);
            }
            Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();

            return Core.Dtos.Response<ProductDto>.Success(_mapper.Map<ProductDto>(Product), 200);
        }

        public async Task<Core.Dtos.Response<List<ProductDto>>> GetAllByUserIdAsync(string userId)
        {
            var Products = await _ProductCollection.Find<Product>(x => x.UserId == userId).ToListAsync();

            if (Products.Any())
            {
                foreach (var Product in Products)
                {
                    Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();
                }
            }
            else
            {
                Products = new List<Product>();
            }

            return Core.Dtos.Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(Products), 200);
        }

        public async Task<Core.Dtos.Response<ProductDto>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var newProduct = _mapper.Map<Product>(productCreateDto);

            newProduct.CreatedTime = DateTime.Now;
            await _ProductCollection.InsertOneAsync(newProduct);

            return Core.Dtos.Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 200);
        }

        public async Task<Core.Dtos.Response<NoContent>> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(productUpdateDto);

            var result = await _ProductCollection.FindOneAndReplaceAsync(x => x.Id == productUpdateDto.Id, updateProduct);

            if (result == null)
            {
                return Core.Dtos.Response<NoContent>.Fail("Product not found", 404);
            }

            await _publishEndpoint.Publish<ProductNameChangedEvent>(new ProductNameChangedEvent 
            { 
                ProductId = updateProduct.Id, 
                UpdatedProductName = productUpdateDto.Name 
            });

            await _publishEndpoint.Publish<ProductNameChangedBasketEvent>(new ProductNameChangedBasketEvent
            {
                UserId = updateProduct.UserId,
                UpdatedProductName = productUpdateDto.Name
            });

            return Core.Dtos.Response<NoContent>.Success(204);
        }

        public async Task<Core.Dtos.Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _ProductCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Core.Dtos.Response<NoContent>.Success(204);
            }
            else
            {
                return Core.Dtos.Response<NoContent>.Fail("Product not found", 404);
            }
        }
    }
}
