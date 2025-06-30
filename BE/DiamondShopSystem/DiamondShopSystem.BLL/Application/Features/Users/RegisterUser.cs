using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;
using BCrypt.Net;
using DiamondShopSystem.BLL.Domain.ValueObjects;

namespace DiamondShopSystem.BLL.Application.Features.Users
{
    public class RegisterUserCommand : IRequest<UserDto>
    {
        public RegisterUserRequest User { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user with email already exists
            var existingUser = (await _unitOfWork.Users.ListAllAsync()).FirstOrDefault(u => u.Email == request.User.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists."); // Or a custom exception
            }

            // Get Customer Role
            var customerRole = (await _unitOfWork.Roles.ListAllAsync()).FirstOrDefault(r => r.Name == "Customer");
            if (customerRole == null)
            {
                throw new Exception("Customer role not found.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.User.FullName,
                Email = request.User.Email,
                Phone = request.User.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.Password),
                RoleId = customerRole.Id,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            // Create CustomerProfile
            var customerProfile = new CustomerProfile
            {
                UserId = user.Id,
                Address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty), // Default empty address
                LoyaltyPoints = 0
            };
            await _unitOfWork.CustomerProfiles.AddAsync(customerProfile);
            await _unitOfWork.CommitAsync();

            return new UserDto(
                user.Id,
                user.FullName,
                user.Email,
                user.Phone,
                customerRole.Name,
                user.IsActive
            );
        }
    }
}