using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<StaffProfile> StaffProfiles { get; set; } = new List<StaffProfile>();
    }
} 