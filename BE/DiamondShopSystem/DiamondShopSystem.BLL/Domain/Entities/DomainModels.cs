using DiamondShopSystem.BLL.Domain.Enums;
using DiamondShopSystem.BLL.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public DiamondProperties DiamondProperties { get; set; } // Owned entity/Value Object
        public bool IsActive { get; set; } = true;
        public int Quantity;

        public ICollection<Promotion> Promotions { get; set; }
        public Warranty Warranty { get; set; } // One-to-one relationship

        public decimal CalculateFinalPrice(IEnumerable<Promotion> activePromotions)
        {
            decimal finalPrice = BasePrice;
            foreach (var promotion in activePromotions)
            {
                finalPrice -= finalPrice * (promotion.DiscountPercentage / 100);
            }
            return finalPrice;
        }
    }

    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    public class Warranty
    {
        public Guid Id { set; get; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Duration { get; set; } // Duration in months or years
        public string Terms { get; set; }
    }

    public class Promotion
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

    

    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }

        public CustomerProfile CustomerProfile { get; set; }
        public StaffProfile StaffProfile { get; set; }
        public ICollection<LoyaltyPointTransaction> LoyaltyPointTransactions { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // e.g., "Customer", "SalesStaff", "StoreManager", "HeadOfficeAdmin", "DeliveryStaff"
    }

    public class CustomerProfile
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Address Address { get; set; } // Value Object
        public Guid? VipStatusId { get; set; }
        public VipStatus VipStatus { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class StaffProfile
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public User Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } // Enum: Pending, Confirmed, Shipped, Delivered, Canceled
        public decimal TotalAmount { get; set; }
        public Address ShippingAddress { get; set; } // Value Object

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public Delivery Delivery { get; set; }
    }

    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTimeOfPurchase { get; set; }
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // e.g., "VNPay"
        public PaymentStatus Status { get; set; } // Enum: Pending, Completed, Failed, Refunded
        public DateTime TransactionDate { get; set; }
    }

    public class Delivery
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid? DeliveryStaffId { get; set; }
        public User DeliveryStaff { get; set; } // Assuming DeliveryStaff is a User with a StaffProfile
        public DeliveryStatus Status { get; set; } // Enum: Pending, Dispatched, InTransit, Delivered, Failed
        public DateTime? DispatchedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
    }

    public class VipStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RequiredPoints { get; set; }
        public decimal DiscountMultiplier { get; set; }
        public ICollection<CustomerProfile> CustomerProfiles { get; set; }
    }

    public class LoyaltyPointTransaction
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public User Customer { get; set; }
        public int Points { get; set; }
        public TransactionType TransactionType { get; set; } // Enum: Earned, Redeemed
        public DateTime Date { get; set; }
    }
}
