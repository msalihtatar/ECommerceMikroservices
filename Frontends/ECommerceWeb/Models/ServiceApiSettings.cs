namespace ECommerceWeb.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseURL { get; set; }
        public string GatewayBaseURL { get; set; }
        public string PhotoStockURL { get; set; }
        public ServiceAPI Catalog { get; set; }
        public ServiceAPI PhotoStock { get; set; }
        public ServiceAPI Basket { get; set; }
        public ServiceAPI Discount { get; set; }
        public ServiceAPI Payment { get; set; }
        public ServiceAPI Order { get; set; }
    }

    public class ServiceAPI 
    {
        public string Path { get; set; }
    }
}
