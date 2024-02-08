using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }

        [ForeignKey("Product")]
<<<<<<< Updated upstream
        public int FK_ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("User")]
        public int FK_UserId { get; set; }
        public virtual User User { get; set; }

=======
        public int fk_product_id { get; set; }
        public Product product { get; set; }

        [ForeignKey("User")]
        public int fk_user_id { get; set; }
        public User user { get; set; }
>>>>>>> Stashed changes
        

        [Required]
        [Column(TypeName = "int")]
        public int quantity { get; set; }
    }
}
