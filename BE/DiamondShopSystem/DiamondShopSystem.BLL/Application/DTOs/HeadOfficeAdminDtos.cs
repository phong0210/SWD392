using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public record ManageUserAccountRequest(
        Guid UserId,
        string FullName,
        string Email,
        string Phone,
        Guid RoleId,
        bool IsActive
    );

    public record CreatePromotionRequest(
        string Code,
        string Description,
        decimal DiscountPercentage,
        DateTime StartDate,
        DateTime EndDate
    );

    public record UpdatePricingParametersRequest(
        // Define parameters for updating pricing, e.g., global markup percentage, category-specific markups
        decimal GlobalMarkupPercentage
        // Dictionary<Guid, decimal> CategoryMarkups
    );
}