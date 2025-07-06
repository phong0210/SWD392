using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Services.User;
using FluentValidation;

namespace DiamondShopSystem.BLL.Handlers.User.Commands.Update
{
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, UserUpdateResponseDto>
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserUpdateDto> _validator;

        public UserUpdateCommandHandler(IUserService userService, IValidator<UserUpdateDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        public async Task<UserUpdateResponseDto> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate the request
                var validationResult = await _validator.ValidateAsync(request.UpdateData, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new UserUpdateResponseDto
                    {
                        Success = false,
                        Error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                    };
                }

                // Update user account
                var result = await _userService.UpdateUserAccountAsync(request.UserId, request.UpdateData);
                
                return result;
            }
            catch (Exception)
            {
                return new UserUpdateResponseDto
                {
                    Success = false,
                    Error = "An error occurred while updating user account information."
                };
            }
        }
    }
} 