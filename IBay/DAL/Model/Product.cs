using System;
using System.Collections.Generic;
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
        public int ProductId { get; set; }

        [Required, MaxLength(255)]
        public string ProductName { get; set; }

        [Required, MaxLength(1000)]
        public string ProductDescription { get; set; }

        [Required]
        [EnumDataType(typeof(ProductType))]
        [DefaultValue(ProductType.Other)]
        public ProductType ProductType { get; set; }

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

        // Clé étrangère vers l'utilisateur (vendeur)
        
        [Required]
        [ForeignKey("Seller")]
        public int FK_UserId { get; set; }
        public virtual User Seller { get; set; }
    }
}
