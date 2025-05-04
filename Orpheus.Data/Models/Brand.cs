using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Data.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? LogoUrl { get; set; }
        public ICollection<Item> Items { get; set; }
            = new List<Item>();
    }
}
