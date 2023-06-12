using ECommerceWeb.Models;
using ECommerceWeb.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();

            ViewBag.basket = basket;
            return View(new CheckOutInfoModel());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutInfoModel checkoutInfoInput)
        {
            //1. yol senkron iletişim
            //var resultSync = await CheckOutSync(checkoutInfoInput);
            //if (!resultSync.IsSuccessful)
            //    return View();
            //else
            //    return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = resultSync.OrderId });

            // 2.yol asenkron iletişim
            var resultAsync = await CheckOutAsync(checkoutInfoInput);
            if (!resultAsync)
                return View();
            else
                return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrder());
        }

        private async Task<bool> CheckOutAsync(CheckOutInfoModel checkOutInfoModel)
        {
            var orderSuspend = await _orderService.SuspendOrder(checkOutInfoModel);
            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();

                ViewBag.basket = basket;

                ViewBag.error = orderSuspend.Error;
            }
            return orderSuspend.IsSuccessful;
        }

        private async Task<OrderCreatedViewModel> CheckOutSync(CheckOutInfoModel checkOutInfoModel)
        {
            var orderStatus = await _orderService.CreateOrder(checkOutInfoModel);

            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.Get();

                ViewBag.basket = basket;

                ViewBag.error = orderStatus.Error;
            }

            return orderStatus;
        }
    }
}
