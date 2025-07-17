using FluentValidation;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Product.Validators
{
    public class WarrantyUpdateValidator : AbstractValidator<WarrantyUpdateDto>
    {
        public WarrantyUpdateValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Product ID cannot be an empty GUID.");
            RuleFor(x => x.WarrantyStart)
                .NotEmpty().WithMessage("Warranty start date is required.")
                .LessThanOrEqualTo(x => x.WarrantyEnd).WithMessage("Warranty start date must be before or equal to the warranty end date.");
            RuleFor(x => x.WarrantyEnd)
                .NotEmpty().WithMessage("Warranty end date is required.")
                .GreaterThanOrEqualTo(x => x.WarrantyStart).WithMessage("Warranty end date must be after or equal to the warranty start date.");
            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Warranty details are required.")
                .MaximumLength(255).WithMessage("Warranty details cannot exceed 255 characters.");
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive status is required.");
        }
    }
}