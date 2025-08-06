using System.ComponentModel.DataAnnotations;

namespace Orpheus.ViewModels
{
    public class CreateBrandViewModel
    {
        [Required]
        [Display(Name = "Brand Name")]
        public string Name { get; set; } = null!;
    }
}
