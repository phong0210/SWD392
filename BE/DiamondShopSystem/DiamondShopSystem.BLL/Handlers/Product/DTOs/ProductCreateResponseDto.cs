using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Product.DTOs
{
    public class ProductCreateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? ProductId { get; set; }
       
    }
}
