using FluentValidation;
using DiamondShopSystem.BLL.Application.DTOs;

namespace DiamondShopSystem.API.Validators
{
    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
    {
        public PlaceOrderRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least one item")
                .Must(items => items.Count > 0).WithMessage("Order must contain at least one item");

            RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());

            RuleFor(x => x.ShippingAddress)
                .NotNull().WithMessage("Shipping address is required");
        }
    }

    public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100");
        }
    }
} 