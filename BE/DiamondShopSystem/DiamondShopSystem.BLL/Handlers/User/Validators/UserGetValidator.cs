using FluentValidation;
using System;
using DiamondShopSystem.BLL.Handlers.User.Commands.Get;

namespace DiamondShopSystem.BLL.Handlers.User.Validators
{
    public class UserGetValidator : AbstractValidator<UserGetCommand>
    {
        public UserGetValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty.");
        }
    }
} 