using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        public Guid? GoogleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public LoyaltyPoints? LoyaltyPoints { get; set; }
        public Vip? Vip { get; set; }
        public StaffProfile? StaffProfile { get; set; }
    }
} 