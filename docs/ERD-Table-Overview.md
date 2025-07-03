# Entity-Relationship Diagram (ERD) Table Overview

This document summarizes the current backend data model, including all entities, their properties, data types, and key constraints/relationships.

---

## Product
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| SKU              | string      |                                  |
| Name             | string      |                                  |
| Description      | string      |                                  |
| BasePrice        | decimal     |                                  |
| CategoryId       | Guid        | FK → Category.Id                 |
| Category         | Category    | Navigation                       |
| DiamondProperties| DiamondProperties | Value Object/Owned         |
| IsActive         | bool        | Default: true                    |
| Quantity         | int         |                                  |
| Promotions       | ICollection<Promotion> | Navigation            |
| Warranty         | Warranty    | 1:1 Navigation                   |

---

## Category
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| Name             | string      |                                  |
| Products         | ICollection<Product> | Navigation              |

---

## Warranty
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| ProductId        | Guid        | FK → Product.Id                  |
| Product          | Product     | Navigation                       |
| Duration         | int         |                                  |
| Terms            | string      |                                  |

---

## Promotion
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| Code             | string      |                                  |
| Description      | string      |                                  |
| DiscountPercentage | decimal   |                                  |
| StartDate        | DateTime    |                                  |
| EndDate          | DateTime    |                                  |
| ProductId        | Guid        | FK → Product.Id                  |
| Product          | Product     | Navigation                       |

---

## User
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| FullName         | string      |                                  |
| Email            | string      |                                  |
| Phone            | string      |                                  |
| PasswordHash     | string      |                                  |
| RoleId           | Guid        | FK → Role.Id                     |
| Role             | Role        | Navigation                       |
| IsActive         | bool        |                                  |
| CustomerProfile  | CustomerProfile | 1:1 Navigation               |
| StaffProfile     | StaffProfile    | 1:1 Navigation               |
| LoyaltyPointTransactions | ICollection<LoyaltyPointTransaction> | Navigation |
| Orders           | ICollection<Order> | Navigation                 |

---

## Role
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| Name             | string      | e.g., "Customer", "SalesStaff"   |

---

## CustomerProfile
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| UserId           | Guid        | PK, FK → User.Id                 |
| User             | User        | Navigation                       |
| Address          | Address     | Value Object/Owned               |
| VipStatusId      | Guid?       | FK → VipStatus.Id (nullable)     |
| VipStatus        | VipStatus   | Navigation                       |
| LoyaltyPoints    | int         |                                  |

---

## StaffProfile
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| UserId           | Guid        | PK, FK → User.Id                 |
| User             | User        | Navigation                       |
| HireDate         | DateTime    |                                  |
| Salary           | decimal     |                                  |

---

## Order
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| CustomerId       | Guid        | FK → User.Id                     |
| Customer         | User        | Navigation                       |
| OrderDate        | DateTime    |                                  |
| Status           | OrderStatus | Enum                             |
| TotalAmount      | decimal     |                                  |
| ShippingAddress  | Address     | Value Object/Owned               |
| OrderDetails     | ICollection<OrderDetail> | Navigation          |
| Payments         | ICollection<Payment> | Navigation              |
| Delivery         | Delivery    | 1:1 Navigation                   |

---

## OrderDetail
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| OrderId          | Guid        | FK → Order.Id                    |
| Order            | Order       | Navigation                       |
| ProductId        | Guid        | FK → Product.Id                  |
| Product          | Product     | Navigation                       |
| Quantity         | int         |                                  |
| PriceAtTimeOfPurchase | decimal|                                  |

---

## Payment
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| OrderId          | Guid        | FK → Order.Id                    |
| Order            | Order       | Navigation                       |
| Amount           | decimal     |                                  |
| PaymentMethod    | string      | e.g., "VNPay"                    |
| Status           | PaymentStatus | Enum                           |
| TransactionDate  | DateTime    |                                  |

---

## Delivery
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| OrderId          | Guid        | FK → Order.Id                    |
| Order            | Order       | Navigation                       |
| DeliveryStaffId  | Guid?       | FK → User.Id (nullable)          |
| DeliveryStaff    | User        | Navigation                       |
| Status           | DeliveryStatus | Enum                          |
| DispatchedDate   | DateTime?   | Nullable                        |
| DeliveredDate    | DateTime?   | Nullable                        |

---

## VipStatus
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| Name             | string      |                                  |
| RequiredPoints   | int         |                                  |
| DiscountMultiplier | decimal   |                                  |
| CustomerProfiles | ICollection<CustomerProfile> | Navigation      |

---

## LoyaltyPointTransaction
| Property         | Type        | Constraints/Notes                |
|------------------|-------------|----------------------------------|
| Id               | Guid        | PK                               |
| CustomerId       | Guid        | FK → User.Id                     |
| Customer         | User        | Navigation                       |
| Points           | int         |                                  |
| TransactionType  | TransactionType | Enum                         |
| Date             | DateTime    |                                  |

---

**PK** = Primary Key  
**FK** = Foreign Key  
**Navigation** = Navigation property (relationship)  
**Value Object/Owned** = Complex type, not a separate table (unless configured otherwise)  
**Enum** = Enum type, stored as int or string depending on EF config 