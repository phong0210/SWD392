using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("username")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string Username { get; set; }

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [Required]
        [Column("phone_number")]
        [Range(1000000000, 9999999999, ErrorMessage = "Phone number must be a 10-digit number.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be a 10-digit number without any special characters or spaces.")]
        [StringLength(10, ErrorMessage = "Phone number cannot be longer than 10 digits.")]
        public string PhoneNumber { get; set; }

        [Column("google_id")]
        public string GoogleId { get; set; }

        [Column("created_at")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("status")]
        [Required]
        public bool IsActive { get; set; } = true;
        public virtual StaffProfiles StaffProfiles { get; set; }
        public virtual VIP VIP { get; set; }
        public virtual LoyaltyPoints LoyaltyPoints { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
