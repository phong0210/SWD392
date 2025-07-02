using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class RegisterCustomerForVipCommand : IRequest<bool>
    {
        public RegisterCustomerForVipRequest Request { get; set; }
    }

    public class RegisterCustomerForVipCommandHandler : IRequestHandler<RegisterCustomerForVipCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerForVipCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RegisterCustomerForVipCommand request, CancellationToken cancellationToken)
        {
            var customerProfile = await _unitOfWork.CustomerProfiles.GetByIdAsync(request.Request.CustomerId);

            if (customerProfile == null)
            {
                return false; // Or throw a NotFoundException
            }

            var vipStatus = await _unitOfWork.VipStatuses.GetByIdAsync(request.Request.VipStatusId);

            if (vipStatus == null)
            {
                throw new Exception("VIP Status not found.");
            }

            customerProfile.VipStatusId = request.Request.VipStatusId;
            await _unitOfWork.CustomerProfiles.UpdateAsync(customerProfile);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}