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
    public enum UserRole
    {
        StandardUser,
        Banned,
        Moderator,
        Admin
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$")]
        [DataType(DataType.Text)]
        [Column(TypeName = "varchar(50)")]
        public string UserPseudo { get; set; }

        [Required]
        [MaxLength(80)]
        [MinLength(3)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,255}$")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        [DefaultValue(UserRole.StandardUser)]
        public UserRole UserRole { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        // Navigation property for one-to-one relationship with ShoppingCart
        public Cart UserCart { get; set; }

    }
}
