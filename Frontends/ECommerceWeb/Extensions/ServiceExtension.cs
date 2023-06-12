using ECommerceWeb.Handlers;
using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;
using ECommerceWeb.Services.Concrete;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECommerceWeb.Extensions
{
    public static class ServiceExtension
    {
        public static void AddHttpClientServices(this IServiceCollection serviceCollection, IConfiguration configuration) 
        {
            var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            serviceCollection.AddHttpClient<IIdentityService, IdentityManager>();
            serviceCollection.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            serviceCollection.AddHttpClient<ICatalogService, CatalogManager>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseURL}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            serviceCollection.AddHttpClient<IPhotoStockService, PhotoStockManager>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseURL}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            serviceCollection.AddHttpClient<IBasketService, BasketManager>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseURL}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            serviceCollection.AddHttpClient<IDiscountService, DiscountManager>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseURL}/{serviceApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            serviceCollection.AddHttpClient<IPaymentService, PaymentManager>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseURL}/{serviceApiSettings.Payment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            serviceCollection.AddHttpClient<IOrderService, OrderManager>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseURL}/{serviceApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            serviceCollection.AddHttpClient<IUserService, UserManager>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseURL);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
