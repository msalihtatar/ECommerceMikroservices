using ECommerceWeb.Models;

namespace ECommerceWeb.Services.Abstract
{
    public interface IOrderService
    {
        /// <summary>
        /// Senkron iletişim, direkt order mikroservisine istek yapar
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoModel checkoutInfoInput);

        /// <summary>
        /// Asenkron iletişim, sipariş bilgilerini rabbitMQ'ya gönderir
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<OrderSuspendViewModel> SuspendOrder(CheckOutInfoModel checkoutInfoInput);

        Task<List<OrderViewModel>> GetOrder();
    }
}
