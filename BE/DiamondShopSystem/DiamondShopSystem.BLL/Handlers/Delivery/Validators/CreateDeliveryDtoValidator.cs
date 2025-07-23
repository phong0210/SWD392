
using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;
using FluentValidation;

namespace DiamondShopSystem.BLL.Handlers.Delivery.Validators
{
    public class CreateDeliveryDtoValidator : AbstractValidator<CreateDeliveryDto>
    {
        public CreateDeliveryDtoValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ShippingAddress).NotEmpty();
        }
    }
}
