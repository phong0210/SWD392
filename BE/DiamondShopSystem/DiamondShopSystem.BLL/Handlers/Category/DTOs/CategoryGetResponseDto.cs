using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using System;

namespace DiamondShopSystem.BLL.Handlers.Category.DTOs
{
    public class CategoryGetResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public CategoryInfoDto? Category { get; set; }
    }
    public class CategoryInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}