using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orpheus.Data.Models;

namespace Orpheus.Data
{
    public class OrpheusDbContext : IdentityDbContext<OrpheusAppUser, IdentityRole<Guid>, Guid>
    {
        public OrpheusDbContext(DbContextOptions<OrpheusDbContext> options)
            : base(options)
        {
        }
    }
}
