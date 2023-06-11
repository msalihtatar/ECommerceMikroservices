namespace ECommerceWeb.Services.Abstract
{
    public interface IClientCredentialTokenService
    {
        Task<String> GetToken();
    }
}
