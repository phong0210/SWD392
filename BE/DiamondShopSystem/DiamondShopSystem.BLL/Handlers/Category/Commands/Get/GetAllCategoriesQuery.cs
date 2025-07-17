using System.Collections.Generic;
using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using MediatR;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Get
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryInfoDto>>
    {
    }
}