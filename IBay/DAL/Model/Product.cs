using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DAL.Model
{
    public class Product
    {
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

        [Required]
        [DefaultValue(0)]
        public int ProductStock { get; set; }

        [Required]
        public bool Available { get; set; }

        [Required]
        public DateTime AddedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        [Required]
        public User Seller { get; set; }
    }
}
