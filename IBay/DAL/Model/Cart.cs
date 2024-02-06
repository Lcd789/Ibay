using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public sealed class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("Product")]
        public int FK_ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("User")]
        public int FK_UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }
    }
}
