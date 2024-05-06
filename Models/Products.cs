using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Image { get; set; }

        [Display(Name="Product Color")]
        public string ProductColor { get; set; }
        
        [Display(Name = "Avaiable")]
        public bool isAvailable { get; set; }
        [Required]
        [Display(Name="Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes? ProductTypes { get; set; }
        [Required]
        [Display(Name="Special Tag")]
        public int SpecialTagId { get; set; }

        [ForeignKey("SpecialTagId")]
        public virtual SpecialTags? SpecialTags { get; set; }

    }
}
