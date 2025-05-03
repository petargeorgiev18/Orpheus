using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orpheus.Data.Models;

namespace Orpheus.Data
{
    public class OrpheusDbContext : IdentityDbContext<OrpheusAppUser, IdentityRole<Guid>, Guid>
    {
        public OrpheusDbContext() { }
        public OrpheusDbContext(DbContextOptions<OrpheusDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<ItemImage> Images { get; set; } = null!;
    }
}
