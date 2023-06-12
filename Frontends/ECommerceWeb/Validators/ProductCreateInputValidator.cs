using ECommerceWeb.Models;
using FluentValidation;

namespace ECommerceWeb.Validators
{
    public class ProductCreateInputValidator : AbstractValidator<ProductCreateInputModel>
    {
        public ProductCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün adı giriniz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama giriniz.");
            
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat Giriniz.").ScalePrecision(2, 6).WithMessage("hatalı para formatı");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("kategori alanı seçiniz");
        }
    }
}
