using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DAL.Model
{
    public class Product
    {
        // Product is at least {id, name, image, price, available, added_time}
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ProductDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(0.01)]
        public double ProductPrice { get; set; }

        // ProductStock is at least 0 and cannot be negative
        [Required]
        [DefaultValue(0)]
        public int ProductStock { get; set; }

        [Required]
        public bool Available { get; set; }
        /* Arguments for later, for now we only need the above
        public string? ProductImage { get; set; }

        public string? Category { get; set; }

        public string? SubCategory { get; set; }

        public string? Brand { get; set; }
        */
        [Required]
        public DateTime AddedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        // Foreign key for the seller (User)
        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        // Navigation property for the seller
        public User Seller { get; set; }

    }
}
