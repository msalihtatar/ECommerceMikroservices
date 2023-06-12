using Core.Dtos;
using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;

namespace ECommerceWeb.Services.Concrete
{
    public class DiscountManager : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            //[controller]/[action]/{code}

            var response = await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

            return discount.Data;
        }
    }
}
