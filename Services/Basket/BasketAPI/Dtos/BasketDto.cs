namespace BasketAPI.Dtos
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDto> BasketItemList { get; set; }
        public decimal TotalPrice 
        { get => BasketItemList.Sum(x => x.Price * x.Quantity); }
    }
}
 