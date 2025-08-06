using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;

namespace Orpheus.Data
{
    public static class ItemSeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrpheusDbContext>();

            // Clear existing data
            dbContext.Images.RemoveRange(dbContext.Images);
            dbContext.Items.RemoveRange(dbContext.Items);
            dbContext.Categories.RemoveRange(dbContext.Categories);
            dbContext.Brands.RemoveRange(dbContext.Brands);
            await dbContext.SaveChangesAsync();

            // Seed fresh data
            await SeedBrands(dbContext);
            await SeedCategories(dbContext);
            await SeedItems(dbContext);
            await SeedImages(dbContext);
        }

        private static async Task SeedBrands(OrpheusDbContext dbContext)
        {
            var brands = new[]
            {
                new Brand
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "ESP LTD",
                    LogoUrl = "https://example.com/logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Fender",
                    LogoUrl = "https://example.com/fender-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Gibson",
                    LogoUrl = "https://example.com/gibson-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Dunlop",
                    LogoUrl = "https://example.com/dunlop-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Boss",
                    LogoUrl = "https://example.com/boss-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("3D34833B-4451-423C-83F8-5DB4FE4EA67A"),
                    Name = "Yamaha",
                    LogoUrl = null
                },
                new Brand
                {
                    Id = Guid.Parse("C255FDE3-8805-4813-B815-743048AC1FDF"),
                    Name = "Universal Music Group",
                    LogoUrl = null
                },
                new Brand
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Elektra Records",
                    LogoUrl = "/images/brands/elektra_records.png"
                },
                new Brand
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Island Records",
                    LogoUrl = "/images/brands/island_records.png"
                },
                new Brand
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    Name = "Harvest Records",
                    LogoUrl = "/images/brands/harvest_records.jpg"
                },
                new Brand
                {
                    Id = Guid.Parse("AAAAAAA1-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    Name = "Rammstein",
                    LogoUrl = "/images/brands/rammstein.png"
                },
                new Brand
                {
                    Id = Guid.Parse("BBBBBBB2-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    Name = "Slayer",
                    LogoUrl = "/images/brands/slayer.png"
                },
                new Brand
                {
                    Id = Guid.Parse("CCCCCCC3-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Name = "Metallica",
                    LogoUrl = "/images/brands/metallica.png"
                },
                new Brand
                {
                    Id = Guid.Parse("4A7EC03D-5A72-4265-8AA6-F893850B8EE9"),
                    Name = "Ibanez",
                    LogoUrl = null
                }
            };

            await dbContext.Brands.AddRangeAsync(brands);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedCategories(OrpheusDbContext dbContext)
        {
            var categories = new[]
            {
                new Category
                {
                    Id = Guid.Parse("BEB86BB8-F8F7-4844-B25C-2DE79D077F39"),
                    CategoryName = "Wooden"
                },
                new Category
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    CategoryName = "Guitars"
                },
                new Category
                {
                    Id = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    CategoryName = "Drums"
                },
                new Category
                {
                    Id = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    CategoryName = "Keyboards"
                },
                new Category
                {
                    Id = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    CategoryName = "Albums"
                },
                new Category
                {
                    Id = Guid.Parse("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"),
                    CategoryName = "Merch"
                },
                new Category
                {
                    Id = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
                    CategoryName = "Accessories"
                }
            };

            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedItems(OrpheusDbContext dbContext)
        {
            var items = new[]
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

            await dbContext.Items.AddRangeAsync(items);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedImages(OrpheusDbContext dbContext)
        {
            var images = new[]
            {
        // Albums
        new ItemImage
        {
            Id = Guid.Parse("6129E2C3-E051-4B60-990D-016CA1AD0BD2"),
            Url = "https://muzikercdn.com/uploads/products/3732/373292/thumb_large_d_gallery_base_af02fa57.jpg",
            ItemId = Guid.Parse("3B2EEDB0-E426-45AF-8F49-3169299A1F39"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("2B8F09DB-AD4C-4D15-B555-2A0124E4E0A9"),
            Url = "https://muzikercdn.com/uploads/product_gallery/19922/1992243/thumb_large_d_gallery_base_7094b0d5.jpg",
            ItemId = Guid.Parse("3B2EEDB0-E426-45AF-8F49-3169299A1F39"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            Url = "https://muzikercdn.com/uploads/products/20832/2083281/main_600bf18c.jpg",
            ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Url = "https://m.media-amazon.com/images/I/61KduVSVYlL._SX425_.jpg",
            ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            Url = "https://muzikercdn.com/uploads/products/4270/427019/thumb_large_d_gallery_base_e0067575.jpg",
            ItemId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Url = "/images/albums/thedarksideofthemoon.jpg",
            ItemId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("B4255949-C04B-4620-B7C7-8201736E4526"),
            Url = "https://muzikercdn.com/uploads/products/4301/430113/thumb_large_d_gallery_base_bbf86abb.jpg",
            ItemId = Guid.Parse("66F2BA23-232E-4573-86FF-EF04D7572F2C"),
            IsMain = true
        },

        // Instruments
        new ItemImage
        {
            Id = Guid.Parse("2E4952CF-05C2-42A8-B9DE-0F09CC321B37"),
            Url = "https://muzikercdn.com/uploads/products/19123/1912392/main_cf0f3076.jpg",
            ItemId = Guid.Parse("3E3C22C3-FD05-4D2C-932A-1841CA70A8C8"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("A42898D0-4799-4D3E-B78F-1D398F2C488B"),
            Url = "https://muzikercdn.com/uploads/products/2286/228604/thumb_large_d_gallery_base_96a1dc9a.png",
            ItemId = Guid.Parse("68F29CE0-6435-4CEE-BC5A-521C166F71E3"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("98BDDE14-41A1-45E3-B47F-A994B09A8E47"),
            Url = "https://muzikercdn.com/uploads/product_gallery/2286/228605/main_24ef7713.png",
            ItemId = Guid.Parse("68F29CE0-6435-4CEE-BC5A-521C166F71E3"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("750CBEAB-A9C6-4E80-8F07-7D44041E5543"),
            Url = "https://muzikercdn.com/uploads/products/11857/1185775/thumb_large_d_gallery_febb8645.jpg",
            ItemId = Guid.Parse("45B325C8-E2B3-4571-9223-F7FB06B8FD3A"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("FC582977-3F5B-4AC5-AF70-E1FE19EA5F7C"),
            Url = "https://muzikercdn.com/uploads/products/18261/1826120/thumb_large_d_gallery_base_083988c4.jpg",
            ItemId = Guid.Parse("264EAE50-18D3-4BFC-B26D-2F42F291A4E2"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("455FE7F2-59EF-400B-B994-CE30BDFB32F2"),
            Url = "https://muzikercdn.com/uploads/product_gallery/18261/1826121/main_f6c1716c.jpg",
            ItemId = Guid.Parse("264EAE50-18D3-4BFC-B26D-2F42F291A4E2"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("3BF64299-1644-4C0A-A3BC-E022B4985C86"),
            Url = "https://muzikercdn.com/uploads/products/19098/1909814/thumb_large_d_gallery_base_1aa098f0.jpg",
            ItemId = Guid.Parse("BDD5BD97-3826-42DB-B3F5-501BC2C58BC8"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("46101D5B-1988-4326-B450-EFB207180581"),
            Url = "https://muzikercdn.com/uploads/product_gallery/19098/1909812/main_8d028f74.jpg",
            ItemId = Guid.Parse("BDD5BD97-3826-42DB-B3F5-501BC2C58BC8"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("413501CE-202F-4B22-A44A-EDE80AADA039"),
            Url = "https://muzikercdn.com/uploads/products/2858/285815/thumb_large_d_gallery_base_c352c403.jpg",
            ItemId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("4C4127B6-E4E5-48D7-A4B0-D856A381DE4A"),
            Url = "https://muzikercdn.com/uploads/products/19432/1943248/thumb_large_d_gallery_base_95e6cb52.jpg",
            ItemId = Guid.Parse("C4690AE8-BBDF-4AA0-8140-FBFF27687865"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("84EA9BDE-785F-4D47-8874-2381123343FA"),
            Url = "https://muzikercdn.com/uploads/product_gallery/19432/1943249/main_a423b730.jpg",
            ItemId = Guid.Parse("C4690AE8-BBDF-4AA0-8140-FBFF27687865"),
            IsMain = false
        },

        // Accessories
        new ItemImage
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Url = "https://m.media-amazon.com/images/I/61lELUFnnkL._UF1000,1000_QL80_.jpg",
            ItemId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Url = "https://m.media-amazon.com/images/I/61Rx712UAbL._UF894,1000_QL80_.jpg",
            ItemId = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("4446400B-6B89-4356-A93E-4A851EC198AE"),
            Url = "https://muzikercdn.com/uploads/products/12334/1233487/thumb_large_d_gallery_base_9b60b202.jpg",
            ItemId = Guid.Parse("9953F144-647E-45A0-AEE8-A394674B5B06"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("F173CDFE-A0B2-48B9-997A-4285B15F12B6"),
            Url = "https://muzikercdn.com/uploads/products/20698/2069856/thumb_large_d_gallery_base_3d1f0fb7.jpg",
            ItemId = Guid.Parse("6B2343B9-BE7B-47DF-B0B5-FB8BD41798CF"),
            IsMain = true
        },

        // Merch
        new ItemImage
        {
            Id = Guid.Parse("D0A6186F-ACED-4A4B-BDA1-7F95FEA6B036"),
            Url = "https://muzikercdn.com/uploads/products/3876/387696/main_044ecb08.jpg",
            ItemId = Guid.Parse("1F261899-F152-4B34-BD1B-51D21B077F57"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("B74D660B-CF1F-44D7-8F93-1417A335071F"),
            Url = "https://muzikercdn.com/uploads/product_gallery/3876/387698/main_d2251751.jpg",
            ItemId = Guid.Parse("1F261899-F152-4B34-BD1B-51D21B077F57"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("BB8EEF12-C02F-4C70-A779-37ACD3AA6EF4"),
            Url = "https://muzikercdn.com/uploads/products/610/61003/main_1d3ca44c.jpg",
            ItemId = Guid.Parse("D67363D7-622E-4659-8D64-7FC0F7B02FCC"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("CCCCCCC1-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            Url = "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwa9ad3e79/images/1/0/5/8/105863a.jpg?sfrm=png",
            ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0001"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("CCCCCCC2-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            Url = "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwe630cd96/images/1/0/5/8/105863b.jpg?sfrm=png",
            ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0001"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("CCCCCCC3-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            Url = "https://muzikercdn.com/uploads/products/3972/397246/thumb_large_d_gallery_base_dc6883f7.jpg",
            ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0002"),
            IsMain = true
        },
        new ItemImage
        {
            Id = Guid.Parse("CCCCCCC4-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            Url = "https://muzikercdn.com/uploads/product_gallery/3972/397247/main_692dc8f6.jpg",
            ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0002"),
            IsMain = false
        },
        new ItemImage
        {
            Id = Guid.Parse("CCCCCCC5-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            Url = "https://i.ebayimg.com/00/s/MTUzN1gxNjAw/z/RFMAAOSwntxiBu1x/$_57.JPG?set_id=8800005007",
            ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0003"),
            IsMain = true
        }
    };

            await dbContext.Images.AddRangeAsync(images);
            await dbContext.SaveChangesAsync();
        }
    }
}