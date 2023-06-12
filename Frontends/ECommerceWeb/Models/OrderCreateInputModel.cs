namespace ECommerceWeb.Models
{
    public class OrderCreateInputModel
    {
        public OrderCreateInputModel()
        {
            OrderItems = new List<OrderItemCreateModel>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemCreateModel> OrderItems { get; set; }
        public AddressCreateInputModel Address { get; set; }
    }
}
