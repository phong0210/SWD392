using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("StaffProfiles")]
    public class StaffProfiles
    {
        [Key]
        [ForeignKey("User")]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Role")]
        [Column("role_id")]
        public Guid RoleId { get; set; }

        [Required]
        [Column("hire_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}