using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class StaffProfile
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
        [Required]
        public double Salary { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
    }
} 