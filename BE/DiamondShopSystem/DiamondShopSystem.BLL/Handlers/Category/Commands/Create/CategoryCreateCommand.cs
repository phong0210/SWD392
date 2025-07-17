using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Create
{
    public class CategoryCreateCommand : IRequest<CategoryInfoDto>
    {
        public CategoryCreateDto Dto { get; }

        public CategoryCreateCommand(CategoryCreateDto dto)
        {
            Dto = dto;
        }
    }
}