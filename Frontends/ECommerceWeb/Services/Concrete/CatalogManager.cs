using Core.Dtos;
using ECommerceWeb.Helpers;
using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;
using System.Text.Json;

namespace ECommerceWeb.Services.Concrete
{
    public class CatalogManager : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogManager(HttpClient client, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _client = client;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateProductAsync(ProductCreateInputModel productCreateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(productCreateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                productCreateInput.Picture = resultPhotoService.Url;
            }
            
            var response = await _client.PostAsJsonAsync<ProductCreateInputModel>("products", productCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(string ProductId)
        {
            var response = await _client.DeleteAsync($"products/{ProductId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _client.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<List<ProductViewModel>> GetAllProductAsync()
        {
            //http:localhost:5000/services/catalog/products
            var response = await _client.GetAsync("products");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture.Split("/").Last());
            });

            return responseSuccess.Data;
        }

        public async Task<List<ProductViewModel>> GetAllProductByUserIdAsync(string userId)
        {
            //[controller]/GetAllByUserId/{userId}

             var response = await _client.GetAsync($"products/getallbyuserid/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture.Split("/").Last());
            });

            return responseSuccess.Data;
        }

        public async Task<ProductViewModel> GetByProductId(string productId)
        {
            var response = await _client.GetAsync($"products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            //var responseSuccess = await response.Content.ReadFromJsonAsync<Response<ProductViewModel>>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Büyük-küçük harfi ignore etme ayarı
            };

            var responseSuccess = JsonSerializer.Deserialize<Response<ProductViewModel>>(json, options);

            responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture.Split("/").Last());

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateInputModel productUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(productUpdateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                await _photoStockService.DeletePhoto(productUpdateInput.Picture);
                productUpdateInput.Picture = resultPhotoService.Url;
            }

            var response = await _client.PutAsJsonAsync<ProductUpdateInputModel>("products", productUpdateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
