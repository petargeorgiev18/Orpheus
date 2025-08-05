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
                },
                new Item
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Born This Way",
                    Description = "Lady Gaga's 2011 album blending pop, dance, and electronic music, celebrated for its themes of empowerment and individuality.",
                    Price = 19.99m,
                    BrandId = Guid.Parse("99999999-9999-9999-9999-999999999999"), // Pop label or brand Guid
                    ItemType = ItemType.Album,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Master of Puppets",
                    Description = "Metallica's iconic thrash metal album released in 1986.",
                    Price = 24.99m,
                    BrandId = Guid.Parse("88888888-8888-8888-8888-888888888888"), // Metallica label or brand Guid
                    ItemType = ItemType.Album,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "The Dark Side of the Moon",
                    Description = "Pink Floyd's 1973 album, a landmark in progressive rock.",
                    Price = 29.99m,
                    BrandId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    ItemType = ItemType.Album,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Fender Deluxe Instrument Cable",
                    Description = "High-quality 10ft cable with durable connectors for guitars and other instruments.",
                    Price = 24.99m,
                    BrandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ItemType = ItemType.Accessory,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb"),
                    Name = "Dunlop Jazz III Picks (6-pack)",
                    Description = "Set of 6 precision guitar picks, ideal for fast and articulate playing.",
                    Price = 5.99m,
                    BrandId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ItemType = ItemType.Accessory,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Boss TU-3 Chromatic Tuner Pedal",
                    Description = "Compact pedal tuner with bright LED display, suitable for stage or studio use.",
                    Price = 99.99m,
                    BrandId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ItemType = ItemType.Accessory,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0001"),
                    Name = "Rammstein T-Shirt - Logo Edition",
                    Description = "Official Rammstein band t-shirt with iconic logo. 100% cotton, black.",
                    Price = 29.99m,
                    BrandId = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    ItemType = ItemType.Merch,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0002"),
                    Name = "Metallica Hoodie - Master of Puppets",
                    Description = "Comfortable hoodie featuring 'Master of Puppets' album artwork.",
                    Price = 49.99m,
                    BrandId = Guid.Parse("ccccccc3-cccc-cccc-cccc-cccccccccccc"),
                    ItemType = ItemType.Merch,
                    IsAvailable = true
                },
                new Item
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0003"),
                    Name = "Slayer Cap - Eagle Logo",
                    Description = "Adjustable Slayer cap with embroidered eagle logo.",
                    Price = 19.99m,
                    BrandId = Guid.Parse("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    ItemType = ItemType.Merch,
                    IsAvailable = true
                }
            };
        }
    }
}
