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
        public int CartId { get; set; }

        [ForeignKey("Product")]
        public int FK_ProductId { get; set; }
        public virtual Product Product { get; set; }

        // Clé étrangère vers l'utilisateur
        [ForeignKey("User")]
        public int FK_UserId { get; set; }
        public virtual User User { get; set; }

        

        [Required]
        [Column(TypeName = "int")]
        public int Quantity { get; set; }
    }
}
