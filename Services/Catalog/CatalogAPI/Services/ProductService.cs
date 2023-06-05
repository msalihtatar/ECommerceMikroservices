using AutoMapper;
using CatalogAPI.Dtos;
using CatalogAPI.Models;
using CatalogAPI.Settings;
using Core.Dtos;
using MongoDB.Driver;

namespace CatalogAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _ProductCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        //private readonly Mass.IPublishEndpoint _publishEndpoint;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings/*, Mass.IPublishEndpoint publishEndpoint*/)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _ProductCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;

            //_publishEndpoint = publishEndpoint;
        }

        public async Task<Response<List<ProductDto>>> GetAllAsync()
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

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(Products), 200);
        }

        public async Task<Response<ProductDto>> GetByIdAsync(string id)
        {
            var Product = await _ProductCollection.Find<Product>(x => x.Id == id).FirstOrDefaultAsync();

            if (Product == null)
            {
                return Response<ProductDto>.Fail("Product not found", 404);
            }
            Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(Product), 200);
        }

        public async Task<Response<List<ProductDto>>> GetAllByUserIdAsync(string userId)
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

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(Products), 200);
        }

        public async Task<Response<ProductDto>> CreateAsync(ProductCreateDto ProductCreateDto)
        {
            var newProduct = _mapper.Map<Product>(ProductCreateDto);

            newProduct.CreatedTime = DateTime.Now;
            await _ProductCollection.InsertOneAsync(newProduct);

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(ProductUpdateDto ProductUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(ProductUpdateDto);

            var result = await _ProductCollection.FindOneAndReplaceAsync(x => x.Id == ProductUpdateDto.Id, updateProduct);

            if (result == null)
            {
                return Response<NoContent>.Fail("Product not found", 404);
            }

            //await _publishEndpoint.Publish<ProductNameChangedEvent>(new ProductNameChangedEvent { ProductId = updateProduct.Id, UpdatedName = ProductUpdateDto.Name });

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _ProductCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Product not found", 404);
            }
        }
    }
}
