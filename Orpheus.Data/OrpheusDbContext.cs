﻿using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Configurations;

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
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartsItems { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;
        public DbSet<WishlistItem> WishlistsItems { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new BrandConfiguration());
            builder.ApplyConfiguration(new ItemConfiguration());
            builder.ApplyConfiguration(new ItemImagesConfiguration());
        }
    }
}
