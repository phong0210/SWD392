using DiamondShopSystem.BLL.Domain.ValueObjects;
using System;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record ProductDto(
        Guid Id,
        string SKU,
        string Name,
        string Description,
        decimal BasePrice,
        Guid CategoryId,
        string CategoryName,
        DiamondProperties DiamondProperties
    );

    public record CreateProductDto(
        string SKU,
        string Name,
        string Description,
        decimal BasePrice,
        Guid CategoryId,
        DiamondProperties DiamondProperties
    );

    public record UpdateProductDto(
        Guid Id,
        string SKU,
        string Name,
        string Description,
        decimal BasePrice,
        Guid CategoryId,
        DiamondProperties DiamondProperties
    );
}