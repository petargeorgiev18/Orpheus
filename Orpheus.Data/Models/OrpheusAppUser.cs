using Microsoft.AspNetCore.Identity;

namespace Orpheus.Data.Models
{
    public class OrpheusAppUser : IdentityUser<Guid>
    {
        public OrpheusAppUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
