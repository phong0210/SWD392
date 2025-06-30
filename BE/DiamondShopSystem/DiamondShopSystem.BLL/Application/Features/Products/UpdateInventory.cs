using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Products
{
    public class UpdateInventoryCommand : IRequest<bool>
    {
        public UpdateInventoryRequest Request { get; set; }
    }

    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInventoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Request.ProductId);

            if (product == null)
            {
                return false; // Or throw a NotFoundException
            }

            product.Quantity += request.Request.QuantityChange;

            if (product.Quantity < 0)
            {
                throw new Exception("Inventory cannot be negative.");
            }

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}