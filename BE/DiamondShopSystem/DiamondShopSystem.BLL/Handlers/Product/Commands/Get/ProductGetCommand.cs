using System;
using MediatR;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Get
{
    public class ProductGetCommand : IRequest<ProductGetResponseDto>
    {
        public Guid ProductId { get; }

        public ProductGetCommand(Guid productId)
        {
            ProductId = productId;
        }
    }
}