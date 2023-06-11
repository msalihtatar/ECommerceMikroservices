using ECommerceWeb.Models;

namespace ECommerceWeb.Services.Abstract
{
    public interface ICatalogService
    {
        Task<List<ProductViewModel>> GetAllProductAsync();
        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<List<ProductViewModel>> GetAllProductByUserIdAsync(string userId);
        Task<ProductViewModel> GetByProductId(string productId);
        Task<bool> CreateProductAsync(ProductCreateInputModel productCreateInput);
        Task<bool> UpdateProductAsync(ProductUpdateInputModel productUpdateInput);
        Task<bool> DeleteProductAsync(string productId);
    }
}
