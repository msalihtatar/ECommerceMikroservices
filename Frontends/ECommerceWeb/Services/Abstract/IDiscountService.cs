using ECommerceWeb.Models;

namespace ECommerceWeb.Services.Abstract
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}
