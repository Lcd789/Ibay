using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public enum ProductType
    {
        [Display(Name = "Electronics")]
        [Description("Electronics")]
        Electronics,

        [Display(Name = "Clothing")]
        [Description("Clothing")]
        Clothing,

        [Display(Name = "Furniture")]
        [Description("Furniture")]
        Furniture,

        [Display(Name = "Books")]
        [Description("Books")]
        Books,

        [Display(Name = "Other")]
        [Description("Other")]
        Other
    }

    public class Product
    {
        [Key]
        public int product_id { get; set; }

        [Required, MaxLength(255)]
        public string product_name { get; set; }

        [Required, MaxLength(1000)]
        public string product_description { get; set; }

        [Required]
        [EnumDataType(typeof(ProductType))]
        [DefaultValue(ProductType.Other)]
        public ProductType product_type { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(0.01)]
        public double product_price { get; set; }

        [Required]
        [DefaultValue(0)]
        public int product_stock { get; set; }

        [Required]
        public bool available { get; set; }

        [Required]
        public DateTime added_time { get; set; }

        public DateTime? updated_time { get; set; }
        
        [Required]
        [ForeignKey("Seller")]
        public int fk_user_id { get; set; }
        public virtual User seller { get; set; }
    }
}
