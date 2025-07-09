using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Product.Commands.Update
{
    public class ProductUpdateCommand : IRequest<ProductUpdateResponseDto>
    {
        public Guid ProductId { get; }
        public ProductUpdateDto UpdateData { get; }

        public ProductUpdateCommand(Guid productId, ProductUpdateDto updateData)
        {
           ProductId = productId;
           UpdateData = updateData;
        }
    }
}
