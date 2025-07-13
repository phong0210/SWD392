using FluentValidation;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Product.Validators
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.");
        }
    }
}