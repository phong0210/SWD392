using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class ManageUserAccountCommand : IRequest<bool>
    {
        public ManageUserAccountRequest UserAccount { get; set; }
    }

    public class ManageUserAccountCommandHandler : IRequestHandler<ManageUserAccountCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageUserAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ManageUserAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserAccount.UserId);

            if (user == null)
            {
                return false; // Or throw a NotFoundException
            }

            user.FullName = request.UserAccount.FullName;
            user.Email = request.UserAccount.Email;
            user.Phone = request.UserAccount.Phone;
            user.RoleId = request.UserAccount.RoleId;
            user.IsActive = request.UserAccount.IsActive;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}