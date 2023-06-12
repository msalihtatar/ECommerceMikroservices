using ECommerceWeb.Models;

namespace ECommerceWeb.Services.Abstract
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoModel paymentInfoInput);
    }
}
