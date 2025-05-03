using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Orpheus.Data
{
    public class OrpheusDbContext : IdentityDbContext
    {
        public OrpheusDbContext(DbContextOptions<OrpheusDbContext> options)
            : base(options)
        {
        }
    }
}
