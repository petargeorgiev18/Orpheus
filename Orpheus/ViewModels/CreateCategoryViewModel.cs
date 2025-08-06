using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Orpheus.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;
    }
}
