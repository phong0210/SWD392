using System;

namespace DiamondShopSystem.BLL.Handlers.Category.DTOs
{
    public class CategoryUpdateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public CategoryInfoDto? category { get; set; }
    }
}