using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerceWeb.Models
{
    public class ProductUpdateInputModel
    {
        public string? Id { get; set; }
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        public string? UserId { get; set; }
        public string? Picture { get; set; }
        //public FeatureViewModel Feature { get; set; }
        [Display(Name = "Kategori")]
        public string CategoryId { get; set; }
        [Display(Name = "Resim")]
        public IFormFile? PhotoFormFile { get; set; }
    }
}
