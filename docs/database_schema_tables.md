# Database Schema - Entities, Properties, and Relationships

## Categories

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| product_id | guid | Foreign Key → Products.id | Yes | No |
| description | nvarchar | | No | No |
| name | nvarchar | | Yes | No |

**Relationships:**
- Many-to-One with Products (via product_id)
- One-to-Many with Products (via inverse relationship)

---

## Products

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| category_id | guid | Foreign Key → Categories.id | Yes | No |
| name | nvarchar | | Yes | No |
| sku | nvarchar | | Yes | No |
| description | nvarchar | | No | No |
| price | float | | Yes | No |
| carat | int | | No | No |
| color | nvarchar | | No | No |
| clarity | nvarchar | | No | No |
| cut | nvarchar | | No | No |
| stock_quantity | int | | Yes | No |
| GIA_cert_number | nvarchar | | No | No |
| is_hidden | bool | | Yes | No |

**Relationships:**
- Many-to-One with Categories (via category_id)
- One-to-Many with OrderDetail
- One-to-Many with Warranties
- One-to-Many with Promotions

---

## OrderDetail

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| product_id | guid | Foreign Key 1 → Products.id | Yes | No |
| order_id | guid | Foreign Key 2 → Order.id | Yes | No |
| unit_price | nvarchar | | Yes | No |
| quantity | int | | Yes | No |

**Relationships:**
- Many-to-One with Products (via product_id)
- Many-to-One with Order (via order_id)

---

## Order

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| user_id | guid | Foreign Key → User.id | Yes | No |
| total_price | nvarchar | | Yes | No |
| order_date | DATE | | Yes | No |
| vip_applied | bool | | Yes | No |
| status | int | | Yes | No |
| sale_staff | nvarchar | | No | No |

**Relationships:**
- Many-to-One with User (via user_id)
- One-to-Many with OrderDetail
- One-to-Many with Deliveries
- One-to-Many with Payment

---

## Deliveries

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| order_id | guid | Foreign Key 1 → Order.id | Yes | No |
| user_id | guid | Foreign Key 2 → User.id | Yes | No |
| delivery_staff_id | guid | Foreign Key 3 → StaffProfiles.id | Yes | No |
| dispatch_time | DATE | | No | No |
| delivery_time | DATE | | No | No |
| shipping_address | nvarchar | | Yes | No |
| status | int | | Yes | No |

**Relationships:**
- Many-to-One with Order (via order_id)
- Many-to-One with User (via user_id)
- Many-to-One with StaffProfiles (via delivery_staff_id)

---

## Payment

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| order_id | guid | Foreign Key → Order.id | Yes | No |
| method | nvarchar | | Yes | No |
| date | DATE | | Yes | No |
| amount | nvarchar | | Yes | No |
| status | int | | Yes | No |

**Relationships:**
- Many-to-One with Order (via order_id)

---

## User

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| name | nvarchar | | Yes | No |
| email | nvarchar | | Yes | No |
| password_hash | nvarchar | | No | No |
| phone | nvarchar | | No | No |
| address | nvarchar | | No | No |
| google_id | guid | | No | No |
| created_at | DATE | | Yes | Yes |
| status | bool | | Yes | No |

**Relationships:**
- One-to-Many with Order
- One-to-Many with LoyaltyPoints
- One-to-Many with Vip
- One-to-Many with Deliveries

---

## LoyaltyPoints

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| user_id | guid | Foreign Key → User.id | Yes | No |
| points_earned | int | | Yes | No |
| points_redeemed | int | | Yes | No |
| last_updated | DATE | | Yes | Yes |

**Relationships:**
- Many-to-One with User (via user_id)

---

## Vip

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| vip_id | guid | Primary Key | Yes | Yes |
| user_id | guid | Foreign Key → User.id | Yes | No |
| start_date | DATE | | Yes | No |
| end_date | DATE | | No | No |
| tier | nvarchar | | Yes | No |

**Relationships:**
- Many-to-One with User (via user_id)

---

## StaffProfiles

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| role_id | guid | Foreign Key → Role.id | Yes | No |
| salary | float | | Yes | No |
| hire_date | DATE | | Yes | No |

**Relationships:**
- Many-to-One with Role (via role_id)
- One-to-Many with Deliveries

---

## Role

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| name | nvarchar | | Yes | No |

**Relationships:**
- One-to-Many with StaffProfiles

---

## Warranties

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| product_id | guid | Foreign Key → Products.id | Yes | No |
| warranty_start | DATE | | Yes | No |
| warranty_end | DATE | | Yes | No |
| details | nvarchar | | No | No |
| is_active | bool | | Yes | No |

**Relationships:**
- Many-to-One with Products (via product_id)

---

## Promotions

| Property | Data Type | Constraints | Required | Auto-Generated |
|----------|-----------|-------------|----------|----------------|
| id | guid | Primary Key | Yes | Yes |
| name | nvarchar | | Yes | No |
| description | nvarchar | | No | No |
| start_date | DATE | | Yes | No |
| end_date | DATE | | No | No |
| discount_type | nvarchar | | Yes | No |
| discount_value | nvarchar | | Yes | No |
| applies_to_product_id | guid | Foreign Key → Products.id | Yes | No |

**Relationships:**
- Many-to-One with Products (via applies_to_product_id)

---

## Summary of Key Relationships

- **Products** is central to the schema, connected to Categories, OrderDetail, Warranties, and Promotions
- **Order** connects Users to Products through OrderDetail, and manages Deliveries and Payments
- **User** has associated LoyaltyPoints and Vip status
- **StaffProfiles** are linked to Roles and handle Deliveries
- The schema supports a jewelry e-commerce system with VIP programs, loyalty points, staff management, and comprehensive order tracking