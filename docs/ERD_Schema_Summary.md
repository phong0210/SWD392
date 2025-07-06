# Database Schema - ERD Format

## Categories
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| Name | string | Required, MaxLength(100) |
| Description | string | MaxLength(255) |

## Deliveries
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| OrderId | Guid | FK → Orders.Id |
| DispatchTime | datetime | nullable |
| DeliveryTime | datetime | nullable |
| ShippingAddress | string | MaxLength(255) |
| Status | int | e.g., Pending, Delivered, etc. |

## LoyaltyPoints
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| UserId | Guid | FK → Users.Id |
| PointsEarned | int | |
| PointsRedeemed | int | |
| LastUpdated | datetime | |

## OrderDetails
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| OrderId | Guid | FK → Orders.Id |
| UnitPrice | double | |
| Quantity | int | |

## Orders
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| UserId | Guid | FK → Users.Id |
| TotalPrice | double | |
| OrderDate | datetime | |
| VipApplied | bool | |
| Status | int | |
| SaleStaff | string | MaxLength(100) |

## Payments
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| OrderId | Guid | FK → Orders.Id |
| Method | string | MaxLength(50) |
| Date | datetime | |
| Amount | double | |
| Status | int | |

## Products
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| Name | string | Required, MaxLength(100) |
| SKU | string | Required, MaxLength(50) |
| Description | string | MaxLength(255) |
| Price | double | |
| Carat | int | nullable |
| Color | string | MaxLength(50) |
| Clarity | string | MaxLength(50) |
| Cut | string | MaxLength(50) |
| StockQuantity | int | |
| GIACertNumber | string | MaxLength(100) |
| IsHidden | bool | |
| CategoryId | Guid | FK → Categories.Id |
| OrderDetailId | Guid | nullable, FK → OrderDetails.Id |

## Promotions
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| Name | string | Required, MaxLength(100) |
| Description | string | MaxLength(255) |
| StartDate | datetime | |
| EndDate | datetime | nullable |
| DiscountType | string | Required, MaxLength(50) |
| DiscountValue | string | Required, MaxLength(50) |
| AppliesToProductId | Guid | nullable, FK → Products.Id |

## Roles
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| Name | string | Required, MaxLength(50) |

## StaffProfiles
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| UserId | Guid | nullable, FK → Users.Id |
| RoleId | Guid | FK → Roles.Id |
| Salary | double | |
| HireDate | datetime | |

## Users
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| Name | string | Required, MaxLength(100) |
| Email | string | Required, MaxLength(100) |
| PasswordHash | string | Required, MaxLength(255) |
| Phone | string | MaxLength(20) |
| Address | string | MaxLength(255) |
| GoogleId | Guid | nullable |
| CreatedAt | datetime | |
| Status | bool | |

## Vips
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| VipId | Guid | PK |
| UserId | Guid | nullable, FK → Users.Id |
| StartDate | datetime | |
| EndDate | datetime | nullable |
| Tier | string | MaxLength(50) |

## Warranties
| Property | Type | Constraints/Notes |
|----------|------|-------------------|
| Id | Guid | PK |
| ProductId | Guid | FK → Products.Id |
| WarrantyStart | datetime | |
| WarrantyEnd | datetime | |
| Details | string | MaxLength(255) |
| IsActive | bool | |