using MediatR;
using System;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Delete
{
    public class CategoryDeleteCommand : IRequest<bool>
    {
        public Guid CategoryId { get; }

        public CategoryDeleteCommand(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}