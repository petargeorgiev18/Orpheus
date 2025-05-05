using System.ComponentModel.DataAnnotations;
using static Orpheus.Common.EntityClassesValidation.Category;

namespace Orpheus.Data.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string CategoryName { get; set; } = null!;
        public ICollection<Item> Items { get; set; } 
            = new List<Item>();
    }
}
