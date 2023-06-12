﻿namespace ECommerceWeb.Models
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; } = 1;
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        private decimal? DiscountAppliedPrice;
        public decimal GetCurrentPrice
        {
            get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;
        }
        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountAppliedPrice = discountPrice;
        }
    }
}