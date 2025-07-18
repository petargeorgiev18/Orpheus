using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orpheus.Data.Models.Enums;

namespace Orpheus.Data.Models.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasData(CreateItems());
        }
        public IEnumerable<Item> CreateItems()
        {
            return new List<Item>
            {
                new Item
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "ESP LTD EC-256",
                    Description = "This is a sample electric guitar.",
                    Price = 499.99m,
                    CategoryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    BrandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ItemType = ItemType.Instrument,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Gibson Les Paul Standard",
                    Description = "Classic Gibson Les Paul electric guitar with mahogany body, maple top, and humbucker pickups, perfect for rock and blues.",
                    Price = 1499.99m,
                    CategoryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    BrandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ItemType = ItemType.Instrument,
                    IsAvailable = true
                }
            };
        }
    }
}
