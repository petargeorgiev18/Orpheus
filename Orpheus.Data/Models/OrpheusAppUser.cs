using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace Orpheus.Data.Models
{
    public class OrpheusAppUser : IdentityUser<Guid>
    {
        public OrpheusAppUser()
        {
            Id = Guid.NewGuid();
        }
        [Required]
        public string PhoneNum { get; set; } = null!;
        public string? Street { get; set; }
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        public ICollection<Order> Orders { get; set; }
            = new List<Order>();
        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();
    }
}
