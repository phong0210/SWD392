
using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;
using FluentValidation;

namespace DiamondShopSystem.BLL.Handlers.Delivery.Validators
{
    public class UpdateDeliveryDtoValidator : AbstractValidator<UpdateDeliveryDto>
    {
        public UpdateDeliveryDtoValidator()
        {
            RuleFor(x => x.DispatchTime).NotEmpty();
            RuleFor(x => x.DeliveryTime).NotEmpty();
            RuleFor(x => x.ShippingAddress).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
