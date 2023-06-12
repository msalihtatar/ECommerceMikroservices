using ECommerceWeb.Models;
using FluentValidation;

namespace ECommerceWeb.Validators
{
    public class ProductUpdateInputValidator : AbstractValidator<ProductUpdateInputModel>
    {
        public ProductUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün Adı Giriniz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Giriniz");
            
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat Giriniz").ScalePrecision(2, 6).WithMessage("Hatalı Format: 1234.56 şeklinde fiyat giriniz.");
        }
    }
}
