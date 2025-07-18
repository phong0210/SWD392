using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.BLL.Services.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Category.Commands.Get
{
    public class CategoryGetCommandHandler : IRequestHandler<CategoryGetCommand, CategoryGetResponseDto>
    {
        private readonly ICategoryService _categoryService;
        public CategoryGetCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public Task<CategoryGetResponseDto> Handle(CategoryGetCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
