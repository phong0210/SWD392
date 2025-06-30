using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Orders
{
    public class AddToCartCommand : IRequest<CartItemDto>
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartItemDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            // Get the product to validate it exists and check stock
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException("Product is not available");
            }

            if (product.Quantity < request.Quantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }

            // Use positional constructor for CartItemDto
            var cartItem = new CartItemDto(
                request.ProductId,
                product.Name,
                request.Quantity,
                product.BasePrice,
                product.BasePrice * request.Quantity
            );

            return cartItem;
        }
    }
}