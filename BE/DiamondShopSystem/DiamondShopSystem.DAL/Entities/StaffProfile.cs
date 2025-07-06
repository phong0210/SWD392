using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class StaffProfile
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
        public double Salary { get; set; }
        public DateTime HireDate { get; set; }
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
} 