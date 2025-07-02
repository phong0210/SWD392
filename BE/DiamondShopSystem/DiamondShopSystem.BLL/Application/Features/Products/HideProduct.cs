using MediatR;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Products
{
    public class HideProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public bool Hide { get; set; }
    }

    public class HideProductCommandHandler : IRequestHandler<HideProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HideProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(HideProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
            {
                return false; // Or throw a NotFoundException
            }

            product.IsActive = !request.Hide;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}