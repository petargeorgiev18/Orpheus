using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Orpheus.Common.EntityClassesValidation.OrpheusAppUser;
namespace Orpheus.Data.Models
{
    public class OrpheusAppUser : IdentityUser<Guid>
    {
        public OrpheusAppUser()
        {
            Id = Guid.NewGuid();
        }        
        [MaxLength(AddressMaxLength)]
        public string? Street { get; set; }
        [MaxLength(CityMaxLength)]
        public string? City { get; set; }
        [MaxLength(CountryMaxLength)]
        public string? Country { get; set; } = null!;
        [MaxLength(PostalCodeMaxLength)]
        public string? ZipCode { get; set; } = null!;
        public Cart? Cart { get; set; } = null!;
        public Wishlist? Wishlist { get; set; } = null!;
        public ICollection<Order> Orders { get; set; }
            = new List<Order>();
        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();
    }
}
