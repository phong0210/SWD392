using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.DAL.Data.Entities
{
    [Table("VIPs")]
    public class VIP
    {
        [Required]
        [Key]
        [ForeignKey("User")]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("start_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Required]
        [Column("status")]
        public bool IsActive { get; set; } = true;

        public virtual User User { get; set; }

    }
