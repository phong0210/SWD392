using MediatR;
using Microsoft.EntityFrameworkCore;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using System.Threading;
using System.Threading.Tasks;
using DiamondShopSystem.DAL.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.PromoteUserToStaff
{
    public class PromoteUserToStaffCommandHandler : IRequestHandler<PromoteUserToStaffCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromoteUserToStaffCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(PromoteUserToStaffCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<DiamondShopSystem.DAL.Entities.User>();
            var roleRepo = _unitOfWork.Repository<Role>();
            var staffProfileRepo = _unitOfWork.Repository<StaffProfile>();

            var users = await userRepo.FindAsync(u => u.Email == request.Email);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return false; // User not found.
            }

            // Check if user already has a staff profile
            var staffProfiles = await staffProfileRepo.FindAsync(sp => sp.UserId == user.Id);
            if (staffProfiles.Any())
            {
                return false; // User is already a staff member.
            }

            var roles = await roleRepo.FindAsync(r => r.Name == request.RoleName);
            var role = roles.FirstOrDefault();
            if (role == null)
            {
                return false; // Role not found.
            }

            var staffProfile = new StaffProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = role.Id,
                Salary = request.Salary,
                HireDate = request.HireDate
            };

            await staffProfileRepo.AddAsync(staffProfile);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
