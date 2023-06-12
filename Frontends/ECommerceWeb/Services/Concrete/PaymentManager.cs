using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;

namespace ECommerceWeb.Services.Concrete
{
    public class PaymentManager : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInfoModel paymentInfoModel)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoModel>("payments", paymentInfoModel);

            return response.IsSuccessStatusCode;
        }
    }
}
