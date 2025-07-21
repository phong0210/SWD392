using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using MediatR;
using System;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Update
{
    public class CategoryUpdateCommand : IRequest<CategoryInfoDto>
    {
        public Guid CategoryId { get; }
        public CategoryUpdateDto Dto { get; }

        public CategoryUpdateCommand(Guid categoryId, CategoryUpdateDto dto)
        {
            CategoryId = categoryId;
            Dto = dto;
        }
    }
}