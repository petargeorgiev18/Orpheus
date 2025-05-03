using System.ComponentModel.DataAnnotations;

namespace Orpheus.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; } = null!;
        public ICollection<Item> Items { get; set; } 
            = new List<Item>();
    }
}
