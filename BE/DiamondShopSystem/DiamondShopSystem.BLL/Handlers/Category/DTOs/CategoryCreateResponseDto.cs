using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Category.DTOs
{
    public class CategoryCreateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? CategoryId { get; set; }
       
    }
}
