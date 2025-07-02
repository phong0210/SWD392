Create the domain model for the Diamond Shop System based on the conceptual data model. The system will use C# with .NET.

**Entities:**
- **Product:** The core entity for items sold. Properties: Id, SKU, Name, Description, BasePrice, CategoryId. It should have methods to calculate the final price by applying promotions.
- **Category:** For product organization. Properties: Id, Name.
- **Warranty:** Product warranty details. Properties: ProductId, Duration, Terms. This has a one-to-one relationship with Product.
- **Promotion:** For discounts. Properties: Id, Code, Description, DiscountPercentage, StartDate, EndDate.
- **ProductPromotion:** A join table for the many-to-many relationship between Product and Promotion.
- **User:** Represents all actors in the system. Properties: Id, FullName, Email, Phone, PasswordHash, RoleId, IsActive.
- **Role:** User roles. Properties: Id, Name (e.g., "Customer", "SalesStaff", "StoreManager", "HeadOfficeAdmin", "DeliveryStaff").
- **CustomerProfile:** Extends User for customer-specific data. Properties: UserId, Address (Value Object), VipStatusId, LoyaltyPoints.
- **StaffProfile:** Extends User for staff-specific data. Properties: UserId, HireDate, Salary.
- **Order:** Customer orders. Properties: Id, CustomerId, OrderDate, Status (Enum: Pending, Confirmed, Shipped, Delivered, Canceled), TotalAmount, ShippingAddress.
- **OrderDetail:** Line items for an order. Properties: Id, OrderId, ProductId, Quantity, PriceAtTimeOfPurchase.
- **Payment:** Transaction details. Properties: Id, OrderId, Amount, PaymentMethod (e.g., "VNPay"), Status, TransactionDate.
- **Delivery:** Shipment details. Properties: Id, OrderId, DeliveryStaffId, Status, DispatchedDate, DeliveredDate.
- **VipStatus:** VIP program levels. Properties: Id, Name, RequiredPoints, DiscountMultiplier.
- **LoyaltyPointTransaction:** History of points earned/spent. Properties: CustomerId, Points, TransactionType, Date.

**Value Objects:**
- **Address:** Street, City, State, PostalCode, Country.
- **Money:** Amount (decimal), Currency (string, e.g., "VND").
- **DiamondProperties:** Encapsulates the 4Cs: Carat, Color, Clarity, Cut. This should be an owned entity or complex type within the Product entity.

**Business Rules:**
- Pricing engine in a Domain Service that calculates product prices based on base cost, markups, and active promotions.
- Loyalty point calculation logic (e.g., 1 point per 100,000 VND spent).
- Inventory management logic: An order placement should decrease stock.