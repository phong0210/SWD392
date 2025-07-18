using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Get
{
    public class CategoryGetCommand : IRequest<CategoryGetResponseDto>
    {
        public Guid CategoryId { get; }

        public CategoryGetCommand(Guid categoryId)
        {
            CategoryId = categoryId;
        }

    }
}
