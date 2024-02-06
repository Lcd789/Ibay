using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public enum UserRole
    {
        [Display(Name = "Standard User")]
        [Description("Standard User")]
        StandardUser,

        [Display(Name = "Admin")]
        [Description("Admin")]
        Admin
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [RegularExpression(@"^[a-zA-Z0-9_-]+$")]
        [DataType(DataType.Text)]
        [Column(TypeName = "varchar(50)")]
        [Required, MaxLength(50), MinLength(3)]
        public string UserPseudo { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required, MaxLength(80), MinLength(3)]
        public string UserEmail { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,255}$")]
        [DataType(DataType.Password)]
        [Required, MaxLength(255), MinLength(8)]
        public string UserPassword { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(0)]
        public double UserMoney { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        [DefaultValue(UserRole.StandardUser)]
        public UserRole UserRole { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
