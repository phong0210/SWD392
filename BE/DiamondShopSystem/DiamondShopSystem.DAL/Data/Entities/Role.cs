using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Data.Entities
{
    public enum RoleType
    {
        Head_Official_Admin,
        Store_Manager,
        Sale_Staff,
        Delivery_Staff
    }

    [Table("Roles")]
    public class Role
    {
        [Key]
        [Required]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }

        [Required]
        [Column("role_name")]
        public RoleType RoleName { get; set; }
    }
}
