using Core.Dtos;
using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;

namespace ECommerceWeb.Services.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly Core.Services.IIdentityService _identityService;

        public OrderManager(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, Core.Services.IIdentityService identityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _identityService = identityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoModel checkoutInfoInput)
        {
            var basket = await _basketService.Get();

            var paymentInfoInput = new PaymentInfoModel()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Ödeme Alınamadı!", IsSuccessful = false };
            }

            var orderCreateInput = new OrderCreateInputModel()
            {
                BuyerId = _identityService.GetUserID,
                Address = new AddressCreateInputModel { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateModel { ProductId = x.ProductId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.ProductName };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInputModel>("orders", orderCreateInput);

            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Sipariş Oluşturulamadı!", IsSuccessful = false };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

            orderCreatedViewModel.Data.IsSuccessful = true;
            await _basketService.Delete();
            return orderCreatedViewModel.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckOutInfoModel checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            var orderCreateInput = new OrderCreateInputModel()
            {
                BuyerId = _identityService.GetUserID,
                Address = new AddressCreateInputModel { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateModel { ProductId = x.ProductId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.ProductName };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var paymentInfoInput = new PaymentInfoModel()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderSuspendViewModel() { Error = "Ödeme Alınamadı!", IsSuccessful = false };
            }

            await _basketService.Delete();
            return new OrderSuspendViewModel() { IsSuccessful = true };
        }
    }
}
