using CatalogAPI.Dtos;
using CatalogAPI.Models;
using Core.Dtos;

namespace CatalogAPI.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> GetByIdAsync(string id);
        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto category);
        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
