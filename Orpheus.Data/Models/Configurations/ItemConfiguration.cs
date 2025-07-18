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
                    CategoryId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), // Albums category Guid
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
                    CategoryId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), // Albums category Guid
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
                    CategoryId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), // Albums category Guid
                    BrandId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Pink Floyd label or brand Guid
                    ItemType = ItemType.Album,
                    IsAvailable = true
                }
            };
        }
    }
}
