using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static Orpheus.Common.EntityClassesValidation.Brand;

namespace Orpheus.Data.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(BrandNameMaxLength)]
        public string Name { get; set; } = null!;       
        [MaxLength(BrandLogoUrlMaxLength)]
        public string? LogoUrl { get; set; }
        public ICollection<Item> Items { get; set; }
            = new List<Item>();
    }
}
