using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public sealed class Cart
    {
        [Key]
        public int cart_id { get; set; }

        [ForeignKey("Product")]
        public int fk_produc_id { get; set; }
        public Product product { get; set; }

        [ForeignKey("User")]
        public int fk_user_id { get; set; }
        public User user { get; set; }
        
        [Required]
        [Column(TypeName = "int")]
        public int quantity { get; set; }
    }
}
