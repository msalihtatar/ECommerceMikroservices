using ECommerceWeb.Models;

namespace ECommerceWeb.Services.Abstract
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
