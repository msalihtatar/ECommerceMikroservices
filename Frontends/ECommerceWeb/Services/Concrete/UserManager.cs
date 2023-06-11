using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;

namespace ECommerceWeb.Services.Concrete
{
    public class UserManager : IUserService
    {
        HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser"); 
        }
    }
}
