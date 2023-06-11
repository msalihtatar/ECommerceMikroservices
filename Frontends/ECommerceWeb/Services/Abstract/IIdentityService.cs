using Core.Dtos;
using ECommerceWeb.Models;
using IdentityModel.Client;

namespace ECommerceWeb.Services.Abstract
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInInputModel signInModel);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
