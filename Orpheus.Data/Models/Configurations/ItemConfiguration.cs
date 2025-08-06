using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;

namespace Orpheus.Data.Models.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            //builder.HasData(CreateItems());
        }

        public IEnumerable<Item> CreateItems()
        {
            return new List<Item>
            {
                new Item
                {
                    Id = Guid.Parse("3E3C22C3-FD05-4D2C-932A-1841CA70A8C8"),
                    Name = "Fender Squier Sonic Stratocaster HSS MN Black",
                    Description = "Guitar...",
                    Price = 350.00m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                },
                new Item
                {
                    Id = Guid.Parse("264EAE50-18D3-4BFC-B26D-2F42F291A4E2"),
                    Name = "Ibanez GRG121DX-BKF Black Flat Electric Guitar",
                    Description = "Premium guitar!",
                    Price = 450.00m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("4A7EC03D-5A72-4265-8AA6-F893850B8EE9"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                },
                new Item
                {
                    Id = Guid.Parse("3B2EEDB0-E426-45AF-8F49-3169299A1F39"),
                    Name = "Michael Jackson - Thriller (LP)",
                    Description = @"Michael Jackson - Thriller (LP)
Tracklist

A1        Wanna Be Startin' Somethin'
A2        Baby Be Mine
A3        The Girl Is Mine
A4        Thriller
B1        Beat It
B2        Billie Jean
B3        Human Nature
B4        P.Y.T. (Pretty Young Thing)
B5        The Lady In My Life",
                    Price = 50.00m,
                    CategoryId = null,
                    BrandId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    IsAvailable = true,
                    ItemType = ItemType.Album
                },
                new Item
                {
                    Id = Guid.Parse("BDD5BD97-3826-42DB-B3F5-501BC2C58BC8"),
                    Name = "Yamaha Pacifica 012 Black",
                    Description = "Electric guitar",
                    Price = 406.00m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("3D34833B-4451-423C-83F8-5DB4FE4EA67A"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                },
                new Item
                {
                    Id = Guid.Parse("1F261899-F152-4B34-BD1B-51D21B077F57"),
                    Name = "Fender Amp Keychain Holder",
                    Description = "Jack Rack Fender wall-mounted amp keychain holder made with the texture of a classic retro amplifier in an elegant design.",
                    Price = 57.90m,
                    CategoryId = null,
                    BrandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    IsAvailable = true,
                    ItemType = ItemType.Merch
                },
                new Item
                {
                    Id = Guid.Parse("68F29CE0-6435-4CEE-BC5A-521C166F71E3"),
                    Name = "Fender FA-125 WN Black",
                    Description = "Acoustic guitar",
                    Price = 368.00m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                },
                new Item
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Gibson Les Paul Standard",
                    Description = "Classic Gibson Les Paul electric guitar with mahogany body, maple top, and humbucker pickups, perfect for rock and blues.",
                    Price = 1499.99m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                },
                new Item
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Born This Way",
                    Description = "Lady Gaga's 2011 album blending pop, dance, and electronic music, celebrated for its themes of empowerment and individuality.",
                    Price = 19.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    IsAvailable = true,
                    ItemType = ItemType.Album
                },
                new Item
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Master of Puppets",
                    Description = "Metallica's iconic thrash metal album released in 1986.",
                    Price = 24.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    IsAvailable = true,
                    ItemType = ItemType.Album
                },
                new Item
                {
                    Id = Guid.Parse("D67363D7-622E-4659-8D64-7FC0F7B02FCC"),
                    Name = "Pantera T-Shirt 101 Proof Skull Unisex Black S",
                    Description = @"Gender: Unisex
Original Colour by Producer: Black
Size: S
Material: Cotton",
                    Price = 48.00m,
                    CategoryId = null,
                    BrandId = Guid.Parse("BBBBBBB2-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    IsAvailable = true,
                    ItemType = ItemType.Merch
                },
                new Item
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "The Dark Side of the Moon",
                    Description = "Pink Floyd's 1973 album, a landmark in progressive rock.",
                    Price = 29.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    IsAvailable = true,
                    ItemType = ItemType.Album
                },
                new Item
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Fender Deluxe Instrument Cable",
                    Description = "High-quality 10ft cable with durable connectors for guitars and other instruments.",
                    Price = 24.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    IsAvailable = true,
                    ItemType = ItemType.Accessory
                },
                new Item
                {
                    Id = Guid.Parse("9953F144-647E-45A0-AEE8-A394674B5B06"),
                    Name = "Dunlop MXR Trigger Fly Capo Satin Chrome Satin Chrome Acoustic Guitar Capo",
                    Description = @"The Trigger Fly Capo builds on our most popular capo design with a streamlined ergonomic grip and a custom spring mechanism for easy placement and precise intonation.",
                    Price = 56.00m,
                    CategoryId = null,
                    BrandId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    IsAvailable = true,
                    ItemType = ItemType.Accessory
                },
                new Item
                {
                    Id = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Name = "Boss TU-3 Chromatic Tuner Pedal",
                    Description = "Compact pedal tuner with bright LED display, suitable for stage or studio use.",
                    Price = 99.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    IsAvailable = true,
                    ItemType = ItemType.Accessory
                },
                new Item
                {
                    Id = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0001"),
                    Name = "Rammstein T-Shirt - Logo Edition",
                    Description = "Official Rammstein band t-shirt with iconic logo. 100% cotton, black.",
                    Price = 29.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("AAAAAAA1-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    IsAvailable = true,
                    ItemType = ItemType.Merch
                },
                new Item
                {
                    Id = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0002"),
                    Name = "Metallica Hoodie - Master of Puppets",
                    Description = "Comfortable hoodie featuring 'Master of Puppets' album artwork.",
                    Price = 49.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("CCCCCCC3-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    IsAvailable = true,
                    ItemType = ItemType.Merch
                },
                new Item
                {
                    Id = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0003"),
                    Name = "Slayer Cap - Eagle Logo",
                    Description = "Adjustable Slayer cap with embroidered eagle logo.",
                    Price = 19.99m,
                    CategoryId = null,
                    BrandId = Guid.Parse("BBBBBBB2-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    IsAvailable = true,
                    ItemType = ItemType.Merch
                },
                new Item
                {
                    Id = Guid.Parse("66F2BA23-232E-4573-86FF-EF04D7572F2C"),
                    Name = "Bob Marley - Legend",
                    Description = "Legendary album",
                    Price = 35.00m,
                    CategoryId = null,
                    BrandId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    IsAvailable = true,
                    ItemType = ItemType.Album
                },
                new Item
                {
                    Id = Guid.Parse("45B325C8-E2B3-4571-9223-F7FB06B8FD3A"),
                    Name = "ESP LTD EC-100",
                    Description = "This is a sample electric guitar.",
                    Price = 900.00m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                },
                new Item
                {
                    Id = Guid.Parse("6B2343B9-BE7B-47DF-B0B5-FB8BD41798CF"),
                    Name = "Revoltage FR02 Guitar Foot Rest",
                    Description = "The Revoltage FR02 is a reliable guitar footrest built for optimal playing posture. Constructed from durable iron with rubber surfaces for secure grip and slip resistance, it provides stability whether you're practicing at home or performing on stage.",
                    Price = 13.00m,
                    CategoryId = null,
                    BrandId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    IsAvailable = true,
                    ItemType = ItemType.Accessory
                },
                new Item
                {
                    Id = Guid.Parse("C4690AE8-BBDF-4AA0-8140-FBFF27687865"),
                    Name = "Gibson SG Standard Heritage Cherry",
                    Description = "Guitar",
                    Price = 720.00m,
                    CategoryId = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    BrandId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    IsAvailable = true,
                    ItemType = ItemType.Instrument
                }
            };
        }
    }
}