using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DAL.Model
{
    public enum UserRole
    {
        StandardUser,
        Admin
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(40)]
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

        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(0)]
        public double UserMoney { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        [DefaultValue(UserRole.StandardUser)]
        public UserRole UserRole { get; set; }

        public List<Product> AddedProducts { get; set; }

        public List<Product> UserCart { get; set; }

        public List<Product> UserProducts { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
