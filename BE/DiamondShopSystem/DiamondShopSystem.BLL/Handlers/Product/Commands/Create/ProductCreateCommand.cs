using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Create
{
    public class ProductCreateCommand : IRequest<ProductCreateResponseDto>
    {
        public ProductCreateDto Dto { get; }

        public ProductCreateCommand(ProductCreateDto dto)
        {
            Dto = dto ?? throw new ArgumentNullException(nameof(dto), "ProductCreateDto cannot be null");
        }
    }
}
