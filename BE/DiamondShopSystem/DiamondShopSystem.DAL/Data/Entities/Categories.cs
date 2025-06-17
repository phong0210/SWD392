using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("Categories")]
    public class Categories
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Column("description")]
        [StringLength(500, ErrorMessage = "Category description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        public virtual ICollection<Products> Products { get; set; } = new List<Products>();

    }
}
