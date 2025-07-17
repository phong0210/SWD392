using System;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Get
{
    public class GetCategoryByIdQuery : IRequest<CategoryInfoDto>
    {
        public Guid CategoryId { get; }

        public GetCategoryByIdQuery(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}