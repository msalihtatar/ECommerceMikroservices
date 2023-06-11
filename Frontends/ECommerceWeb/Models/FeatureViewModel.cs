using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerceWeb.Models
{
    public class FeatureViewModel
    {
        [Display(Name = "Süre")]
        public int Duration { get; set; }

    }
}
