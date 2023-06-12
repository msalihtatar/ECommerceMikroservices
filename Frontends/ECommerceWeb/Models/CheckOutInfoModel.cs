using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerceWeb.Models
{
    public class CheckOutInfoModel
    {
        [Display(Name = "İl")]
        public string Province { get; set; }

        [Display(Name = "İlçe")]
        public string District { get; set; }

        [Display(Name = "Cadde")]
        public string Street { get; set; }

        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }

        [Display(Name = "Adres")]
        public string Line { get; set; }

        [Display(Name = "Kartın Üzerinde Yazan İsim")]
        public string CardName { get; set; }

        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Display(Name = "Son Kullanma Tarih(Ay/Yıl)")]
        public string Expiration { get; set; }

        [Display(Name = "CVV/CVC2 numarası")]
        public string CVV { get; set; }
    }
}
